using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.Models;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            string usersXml = File.ReadAllText("../../../Datasets/users.xml");
            string productsXml = File.ReadAllText("../../../Datasets/products.xml");
            string categoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
            string categoriesProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");

            string result = GetUsersWithProducts(context);

            Console.WriteLine(result);
        }

        //IMPORT DATA

        //Ex. 1
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XDocument doc = XDocument.Parse(inputXml);

            var root = doc.Root;

            var elements = root.Elements();

            List<User> users = new List<User>();
            foreach ( var element in elements ) 
            {
                users.Add(new User()
                {
                    FirstName = element.Element("firstName").Value,
                    LastName = element.Element("lastName").Value,
                    Age = int.Parse(element.Element("age").Value)
                });
            }

            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count}";
        }

        //Ex. 2
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XDocument doc = XDocument.Parse(inputXml);

            var root = doc.Root;

            var elements = root.Elements();

            List<Product> products = new List<Product>();
            foreach ( var element in elements )
            {
                XElement? buyerEl = element.Element("buyerId");
                string? value = buyerEl?.Value;

                int? buyerId = null;
                if(value != null)
                {
                    buyerId = int.Parse(value);
                }
                products.Add(new Product()
                {
                    Name = element.Element("name").Value,
                    Price = decimal.Parse(element.Element("price").Value, CultureInfo.InvariantCulture),
                    SellerId = int.Parse(element.Element("sellerId").Value),
                    BuyerId = buyerId
                });
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }
        
        //Ex. 3
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XDocument doc = XDocument.Parse(inputXml);

            var root = doc.Root;

            var elements = root?.Elements();

            List<Category> categories = new List<Category>();

            foreach (var element in elements)
            {
                string? name = element.Element("name")?.Value;

                if(name != null)
                {
                    categories.Add(new Category()
                    {
                        Name = name,
                    });
                }
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //Ex. 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XDocument doc = XDocument.Parse(inputXml);

            var root = doc.Root;

            var elements = root?.Elements();

            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (var element in elements)
            {
                int catId = int.Parse(element.Element("CategoryId").Value);
                int prodId = int.Parse(element.Element("ProductId").Value);

                if (!context.Categories.Any(c => c.Id == catId)
                    || !context.Products.Any(p => p.Id == prodId))
                {
                    continue;
                }

                categoryProducts.Add(new CategoryProduct()
                {
                    CategoryId = catId,
                    ProductId = prodId,
                });
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //EXPORT DATA

        //Ex. 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            List<ProductDto> products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Include(p => p.Buyer)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ProductDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerFullName = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                }).ToList();

            XDocument doc = new XDocument();

            doc.Add(new XElement("Products"));

            var root = doc.Root;

            foreach (var product in products)
            {
                var prod = new XElement("Product");

                prod.Add(
                            new XElement("name", product.Name),
                            new XElement("price", product.Price),
                            new XElement("buyer", product.BuyerFullName)
                        );

                root.Add(prod);
            }

            return doc.ToString();
        }

        //Ex. 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .Include(u => u.ProductsSold)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new UserDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Products = u.ProductsSold
                                .Select(ps => new ProductDto()
                                {
                                    Name = ps.Name,
                                    Price = ps.Price,
                                    BuyerFullName = "null"
                                }).ToList()
                });

            XDocument doc = new XDocument();

            var root = new XElement("Users");

            doc.Add(root);

            foreach (var user in users) 
            {
                var products = new XElement("soldProducts");
                foreach (var product in user.Products)
                {
                    var prod = new XElement("Product");

                    prod.Add
                        (
                            new XElement("name", product.Name),
                            new XElement("price", product.Price)
                        );

                    products.Add(prod);
                }

                var userEl = new XElement("User");

                userEl.Add
                    (
                        new XElement("firstName", user.FirstName),
                        new XElement("lastName", user.LastName),
                        products
                    );

                root.Add(userEl);
            }

            return doc.ToString();
        }

        //Ex. 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

            var categories = context.Categories
                .Include(c => c.CategoryProducts)
                .ThenInclude(cp => cp.Product)
                .Select(c => new CategoryDto()
                {
                    Name = c.Name,
                    ProductNumber = c.CategoryProducts.Count,
                    AvgPrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(cd => cd.ProductNumber)
                .ThenBy(cd => cd.TotalRevenue)
                .ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryDto>), new XmlRootAttribute("Categories"));
            string result = "";
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                serializer.Serialize(sw, categories, ns);

                result = sw.ToString();
            }

            return result;
        }

        //Ex. 8
        public static string GetUsersWithProducts(ProductShopContext context)
        {

            var users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(u => u.ProductsSold.Count)
                .Take(10)
                .Select(u => new UserDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Products = u.ProductsSold
                                .OrderByDescending(ps => ps.Price)
                                .Select(ps => new ProductDto
                                {
                                    Name = ps.Name,
                                    Price = ps.Price,
                                    BuyerFullName = "null"
                                }).ToList()
                }).ToList();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<UserDto>), new XmlRootAttribute("Users"));

            string result = "";
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                xmlSerializer.Serialize(sw, users , ns);
                result = sw.ToString();
            }

            return result;
        }
    }
}
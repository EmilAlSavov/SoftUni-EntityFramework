using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Text.Json.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            string userJson = File.ReadAllText(@"../../../Datasets/users.json");
            string productJson = File.ReadAllText(@"../../../Datasets/products.json");
            string catProdJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
            string categoryJson = File.ReadAllText(@"../../../Datasets/categories.json");
            ProductShopContext context = new ProductShopContext();

            string result = GetSoldProducts(context);

            Console.WriteLine(result);
        }

        //  IMPORT DATA 

        //Ex. 1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var userDtos = JsonConvert.DeserializeObject<List<UserDto>>(inputJson);

            List<User> users = new List<User>();
            foreach (var userDto in userDtos)
            {
                users.Add(new User()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Age = userDto.Age,
                });
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        //Ex. 2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<ProductDto> productDtos = JsonConvert.DeserializeObject<List<ProductDto>>(inputJson);
            
            List<Product> products = new List<Product>();

            foreach (var productDto in productDtos)
            {
                products.Add(new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId,
                    BuyerId = productDto.BuyerId,
                });
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //Ex. 3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<CategoryDto> categoryDtos = JsonConvert.DeserializeObject<List<CategoryDto>>(inputJson);
            List<Category> categories = new List<Category>();

            foreach (var catDto in categoryDtos)
            {
                if(catDto.Name != null)
                {
                    categories.Add(new Category()
                    {
                        Name = catDto.Name,
                    });
                }
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //Ex. 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProductDto> categoryProductDtos = JsonConvert.DeserializeObject<List<CategoryProductDto>>(inputJson);
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (var catprodDto in categoryProductDtos)
            {
                //if (!context.Categories.Any(c => c.Id == catprodDto.CategoryId) ||
                //    !context.Products.Any(p => p.Id == catprodDto.ProductId))
                //{
                //    continue;
                //}
                categoryProducts.Add(new CategoryProduct()
                {
                    CategoryId = catprodDto.CategoryId,
                    ProductId = catprodDto.ProductId,
                });
            }

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //  EXPORT DATA

        //Ex. 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Include(p => p.Seller)
                .Select(p => new ProductExportDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = p.Seller.FirstName + " " + p.Seller.LastName,
                });

            string Json = JsonConvert.SerializeObject(products, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return Json;
        }

        //Ex. 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            var sellers = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .Include(u => u.ProductsSold)
                .ThenInclude(p => p.Buyer)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new UserSoldProductsDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Products = u.ProductsSold.Select(p => new ProductExportDto()
                    {
                        Name = p.Name,
                        Price = p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName,
                    }).ToList()
                }).ToList();

            string Json = JsonConvert.SerializeObject(sellers, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return Json;
        }
    }
}
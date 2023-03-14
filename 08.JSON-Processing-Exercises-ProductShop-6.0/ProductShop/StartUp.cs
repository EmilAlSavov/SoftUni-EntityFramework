using Newtonsoft.Json;
using ProductShop.Data;
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

            string result = ImportProducts(context, productJson);

            Console.WriteLine(result);
        }

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
    }
}
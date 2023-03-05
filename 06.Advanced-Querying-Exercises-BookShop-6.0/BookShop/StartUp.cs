namespace BookShop
{
    using Data;
    using Initializer;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);


            Console.WriteLine(GetGoldenBooks(db));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder result = new StringBuilder();

            var books = context.Books
                .ToList()
                .Where(b => b.AgeRestriction.ToString().ToLower() == command)
                .OrderBy(b => b.Title);

            foreach ( var book in books )
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().Trim();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder result = new StringBuilder();

            var goldenBooks = context.Books.ToList()
                .Where(b => b.EditionType.ToString() == "Gold")
                .Where(b => b.Copies < 5000)
                .OrderBy(b => b.BookId);

            foreach ( var book in goldenBooks )
            {
                result.AppendLine(book.Title);
            }
            return result.ToString().Trim();
        }
    }
}



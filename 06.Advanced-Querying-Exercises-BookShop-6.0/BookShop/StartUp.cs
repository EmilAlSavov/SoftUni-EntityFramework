namespace BookShop
{
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            int year = int.Parse(Console.ReadLine());

            Console.WriteLine(GetBooksNotReleasedIn(db, year));
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

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder result = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .AsNoTracking();

            foreach (var b in books)
            {
                result.AppendLine($"{b.Title} - ${b.Price:f2}");
            }

            return result.ToString().Trim();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder result = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId);

            foreach (var b in books)
            {
                result.AppendLine(b.Title);
            }

            return result.ToString().Trim();
        }
    }
}



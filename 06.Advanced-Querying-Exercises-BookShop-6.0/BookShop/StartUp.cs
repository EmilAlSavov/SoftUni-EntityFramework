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

            string input = Console.ReadLine().ToLower();

            Console.WriteLine(GetBooksByAuthor(db, input));
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

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder result = new StringBuilder();

            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .Where(b => b.BookCategories.Any(bk => categories.Contains(bk.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .ToList();

            foreach (var b in books)
            {
                result.AppendLine(b.Title);
            }


            return result.ToString().Trim();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            int dd = int.Parse(date.Split('-')[0]);
            int MM = int.Parse(date.Split('-')[1]);
            int yy = int.Parse(date.Split('-')[2]);

            StringBuilder result = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate < new DateTime(yy, MM, dd))
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    Title = b.Title,
                    Edition = b.EditionType,
                    Price = b.Price
                }).ToList();

            foreach (var book in books)
            {
                result.AppendLine($"{book.Title} - {book.Edition.ToString()} - ${book.Price:F2}");
            }

            return result.ToString().Trim();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder result = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName);

            foreach (var author in authors)
            {
                result.AppendLine(author.FirstName + ' ' + author.LastName);
            }

            return result.ToString().Trim();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder result = new StringBuilder();

            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input))
                .OrderBy(b => b.Title);

            foreach (var book in books)
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().Trim();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder result = new StringBuilder();

            var books = context.Books
                .Include(b => b.Author)
                .Where(b => b.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(b => b.BookId);

            foreach (var b in books)
            {
                result.AppendLine($"{b.Title} ({b.Author.FirstName} {b.Author.LastName})");
            }

            return result.ToString().Trim();
        }
    }
}



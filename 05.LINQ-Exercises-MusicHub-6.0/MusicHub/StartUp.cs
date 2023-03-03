namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);

            //Test your solutions here

            Console.WriteLine(ExportSongsAboveDuration(context, 4));

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder result = new StringBuilder();

            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Include(a => a.Songs)
                .ThenInclude(s => s.Writer)
                .Include(a => a.Producer)
                .ToArray()
                .OrderByDescending(a => a.Price)
                .ToList();

            foreach (var album in albums) 
            {
                result.AppendLine($"-AlbumName: {album.Name}");
                result.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                result.AppendLine($"-ProducerName: {album.Producer?.Name}");
                result.AppendLine($"-Songs:");

                int count = 1;
                foreach (var song in album.Songs.OrderByDescending(s => s.Name).ThenBy(s => s.Writer.Name))
                {
                    result.AppendLine($"---#{count}");
                    result.AppendLine($"---SongName: {song.Name}");
                    result.AppendLine($"---Price: {song.Price:F2}"); 
                    result.AppendLine($"---Writer: {song.Writer.Name}");
                    count++;
                }
                result.AppendLine($"-AlbumPrice: {album.Price:F2}");
            }

            return result.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder result = new StringBuilder();
            var songs = context.Songs
                
                .Include(s => s.SongPerformers)
                .ThenInclude(sp => sp.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                .ThenInclude(a => a.Producer)
                .Select(s => new
                {
                    SongName = s.Name,
                    Performers = s.SongPerformers,
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration
                }).ToList()
                .Where(s => s.Duration.TotalSeconds > duration);

            int counter = 1;
            foreach (var song in songs.OrderBy(s => s.SongName).ThenBy(s => s.WriterName))
            {
                result.AppendLine($"-Song #{counter}");
                result.AppendLine($"---SongName: {song.SongName}");
                result.AppendLine($"---Writer: {song.WriterName}");

                if (song.Performers != null)
                {
                    foreach (var performer in song.Performers.OrderBy(p => p.Performer.FirstName + p.Performer.LastName))
                    {
                        result.AppendLine($"---Performer: {performer.Performer.FirstName} {performer.Performer.LastName}");
                    }
                }

                result.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                result.AppendLine($"---Duration: {song.Duration.ToString("c")}");

                counter++;
            }

            return result.ToString().Trim();
        }
    }
}

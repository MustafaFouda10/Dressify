using Dressify.API.Models;

namespace Dressify.API.Data
{
    public class DbInitializer
    {
        public static void Seed(DressifyDbContext context)
        {
            // If there are already dresses, do nothing
            if (context.Dresses.Any())
                return;

            var dresses = new List<Dress>
            {
                new Dress
                {
                    Name = "Elegant White Gown",
                    Description = "Classic white wedding dress with lace details.",
                    Price = 2500,
                    Size = "M",
                    Color = "White",
                    ImagePath = "images/image1.webp",
                    IsAvailable = true
                },
                new Dress
                {
                    Name = "Royal Blue Evening Dress",
                    Description = "Floor-length evening gown, perfect for formal occasions.",
                    Price = 1800,
                    Size = "L",
                    Color = "Blue",
                    ImagePath = "images/image2.jpg",
                    IsAvailable = true
                },
                new Dress
                {
                    Name = "Champagne Satin Dress",
                    Description = "Soft satin gown with a flowing silhouette.",
                    Price = 2200,
                    Size = "S",
                    Color = "Champagne",
                    ImagePath = "images/image3.webp",
                    IsAvailable = true
                }
            };

            context.Dresses.AddRange(dresses);
            context.SaveChanges();
        }
    }
}

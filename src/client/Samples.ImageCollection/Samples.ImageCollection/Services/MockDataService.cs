using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samples.ImageCollection.Model;

namespace Samples.ImageCollection.Services
{
    public class MockDataService : IDataService
    {
        private static Dictionary<string, IList<ImageReference>> _images = new Dictionary<string, IList<ImageReference>>();
        static MockDataService()
        {
            LoadImages();
        }

        private static void LoadImages()
        {
            var bicycles = new List<ImageReference>
            {
                new ImageReference { Id = "1", FileName = "http://assets.inhabitat.com/wp-content/blogs.dir/1/files/2015/12/Fortified-Bicycle-Invincible-Theft-Proof-Bike-10.jpg" },
                new ImageReference { Id = "2", FileName = "http://www.cpsc.gov/Global/Images/Recall/2016/16061/2015FocusIzalcoMAX4.jpg" },
                new ImageReference { Id = "3", FileName = "http://twentynineinches.com/wp-content/uploads/2010/06/Montare.jpg" },
                new ImageReference { Id = "11", FileName = "http://assets.inhabitat.com/wp-content/blogs.dir/1/files/2015/12/Fortified-Bicycle-Invincible-Theft-Proof-Bike-10.jpg" },
                new ImageReference { Id = "21", FileName = "http://www.cpsc.gov/Global/Images/Recall/2016/16061/2015FocusIzalcoMAX4.jpg" },
                new ImageReference { Id = "31", FileName = "http://twentynineinches.com/wp-content/uploads/2010/06/Montare.jpg" },
                new ImageReference { Id = "12", FileName = "http://assets.inhabitat.com/wp-content/blogs.dir/1/files/2015/12/Fortified-Bicycle-Invincible-Theft-Proof-Bike-10.jpg" },
                new ImageReference { Id = "22", FileName = "http://www.cpsc.gov/Global/Images/Recall/2016/16061/2015FocusIzalcoMAX4.jpg" },
                new ImageReference { Id = "32", FileName = "http://twentynineinches.com/wp-content/uploads/2010/06/Montare.jpg" }
            };

            _images.Add("1", bicycles);
        }

        public Task AddImage(ImageReference imageReference)
        {
            return Task.FromResult(0);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            // Simulate delay
            await Task.Delay(1000);

            return new List<Category>
            {
                new Category { Id = "1", Name = "Bicycles", Description = "Pictures of bicycles" },
                new Category { Id = "2", Name = "Cars", Description = "They go vroom" },
                new Category { Id = "3", Name = "Airplanes", Description = "Look up!" },
                new Category { Id = "4", Name = "Boats", Description = "They float (we hope)" }
            };
        }

        public async Task<IEnumerable<ImageReference>> GetImages(string categoryId)
        {
            await Task.Delay(300);
            IList<ImageReference> images;
            _images.TryGetValue(categoryId, out images);

            return images ?? Enumerable.Empty<ImageReference>();
        }

        public Task SyncAsync()
        {
            throw new NotImplementedException();
        }
    }
}

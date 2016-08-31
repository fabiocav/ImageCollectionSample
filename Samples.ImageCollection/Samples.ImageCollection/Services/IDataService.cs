using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samples.ImageCollection.Model;

namespace Samples.ImageCollection.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<IEnumerable<ImageReference>> GetImages(string categoryId);

        Task AddImage(ImageReference imageReference);
    }
}

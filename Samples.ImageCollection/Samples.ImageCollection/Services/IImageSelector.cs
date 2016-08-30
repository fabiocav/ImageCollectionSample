using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.ImageCollection.Services
{
    public interface IImageSelector
    {
        Task<string> SelectImageAsync();
    }
}

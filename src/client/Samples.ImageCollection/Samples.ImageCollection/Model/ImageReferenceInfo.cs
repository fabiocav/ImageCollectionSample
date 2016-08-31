using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.ImageCollection.Model
{
    public class ImageReferenceInfo
    {
        public string Id { get; set; }

        public string FileName { get; set; }

        public string CategoryId { get; set; }

        public string LocalFilePath { get; set; }

        public string Status { get; set; }
    }
}

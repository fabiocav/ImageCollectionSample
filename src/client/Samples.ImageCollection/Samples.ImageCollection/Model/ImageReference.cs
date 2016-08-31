﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.ImageCollection.Model
{
    public class ImageReference
    {
        public ImageReference()
        {
            Status = "Pending";
        }

        public string Id { get; set; }

        public string CategoryId { get; set; }

        public string FileName { get; set; }

        public string Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Io.Dom;
using Microsoft.AspNetCore.Http;
using NetParts.Models.ProductAggregator;

namespace NetParts.Models.ViewModels
{
    public class ProductImage
    {
        public Product product { get; set; }
        public List<IFormFile> ImageFile { get; set; }
    }
}

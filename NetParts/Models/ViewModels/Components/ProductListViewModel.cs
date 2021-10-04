using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace NetParts.Models.ViewModels.Components

{
    public class ProductListViewModel
    {
        public IPagedList<Advertisement> list { get; set; }
        public List<SelectListItem> ordination
        {
            get
            {
                if (isPortugues(CultureInfo.CurrentCulture.Name))
                {
                    return new List<SelectListItem>() {
                        new SelectListItem("Alfabética", "A"),
                        new SelectListItem("Menor preço", "ME"),
                        new SelectListItem("Maior preço", "MA")
                    };
                }
                else
                {
                    return new List<SelectListItem>() {
                        new SelectListItem("Alphabetical", "A"),
                        new SelectListItem("Lowest price", "ME"),
                        new SelectListItem("Biggest price", "MA")
                    };
                }
            }
            private set { }
        }
        public bool isPortugues(String cultura)
        {
            if (cultura == "pt-BR")
            {
                return true;
            }
            return false;
        }
    }

   
}

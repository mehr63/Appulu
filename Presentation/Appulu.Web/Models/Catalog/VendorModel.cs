﻿using System.Collections.Generic;
using Appulu.Web.Framework.Models;
using Appulu.Web.Models.Media;

namespace Appulu.Web.Models.Catalog
{
    public partial class VendorModel : BaseAppEntityModel
    {
        public VendorModel()
        {
            PictureModel = new PictureModel();
            Products = new List<ProductOverviewModel>();
            PagingFilteringContext = new CatalogPagingFilteringModel();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public bool AllowUsersToContactVendors { get; set; }

        public PictureModel PictureModel { get; set; }

        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

        public IList<ProductOverviewModel> Products { get; set; }
    }
}
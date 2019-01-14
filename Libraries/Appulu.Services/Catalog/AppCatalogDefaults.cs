namespace Appulu.Services.Catalog
{
    /// <summary>
    /// Represents default values related to catalog services
    /// </summary>
    public static partial class AppCatalogDefaults
    {
        #region Categories

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : category ID
        /// </remarks>
        public static string CategoriesByIdCacheKey => "App.category.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category ID
        /// {1} : show hidden records?
        /// {2} : current user ID
        /// {3} : store ID
        /// </remarks>
        public static string CategoriesByParentCategoryIdCacheKey => "App.category.byparent-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// {1} : comma separated list of user roles
        /// {2} : show hidden records?
        /// </remarks>
        public static string CategoriesAllCacheKey => "App.category.all-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category id
        /// {1} : comma separated list of user roles
        /// {2} : current store ID
        /// {3} : show hidden records?
        /// </remarks>
        public static string CategoriesChildIdentifiersCacheKey => "App.category.childidentifiers-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : category ID
        /// {2} : page index
        /// {3} : page size
        /// {4} : current user ID
        /// {5} : store ID
        /// </remarks>
        public static string ProductCategoriesAllByCategoryIdCacheKey => "App.productcategory.allbycategoryid-{0}-{1}-{2}-{3}-{4}-{5}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : product ID
        /// {2} : current user ID
        /// {3} : store ID
        /// </remarks>
        public static string ProductCategoriesAllByProductIdCacheKey => "App.productcategory.allbyproductid-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CategoriesPatternCacheKey => "App.category.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductCategoriesPatternCacheKey => "App.productcategory.";

        #endregion

        #region Manufacturers

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : manufacturer ID
        /// </remarks>
        public static string ManufacturersByIdCacheKey => "App.manufacturer.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : manufacturer ID
        /// {2} : page index
        /// {3} : page size
        /// {4} : current user ID
        /// {5} : store ID
        /// </remarks>
        public static string ProductManufacturersAllByManufacturerIdCacheKey => "App.productmanufacturer.allbymanufacturerid-{0}-{1}-{2}-{3}-{4}-{5}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : product ID
        /// {2} : current user ID
        /// {3} : store ID
        /// </remarks>
        public static string ProductManufacturersAllByProductIdCacheKey => "App.productmanufacturer.allbyproductid-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ManufacturersPatternCacheKey => "App.manufacturer.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductManufacturersPatternCacheKey => "App.productmanufacturer.";

        #endregion

        #region Products

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        public static string ProductsByIdCacheKey => "App.product.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductsPatternCacheKey => "App.product.";

        /// <summary>
        /// Gets a key for product prices
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : overridden product price
        /// {2} : additional charge
        /// {3} : include discounts (true, false)
        /// {4} : quantity
        /// {5} : roles of the current user
        /// {6} : current store ID
        /// </remarks>
        public static string ProductPriceModelCacheKey => "App.totals.productprice-{0}-{1}-{2}-{3}-{4}-{5}-{6}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductPricePatternCacheKey => "App.totals.productprice";

        /// <summary>
        /// Gets a key for category IDs of a product
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public static string ProductCategoryIdsModelCacheKey => "App.totals.product.categoryids-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductCategoryIdsPatternCacheKey => "App.totals.product.categoryids";

        /// <summary>
        /// Gets a key for manufacturer IDs of a product
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public static string ProductManufacturerIdsModelCacheKey => "App.totals.product.manufacturerids-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductManufacturerIdsPatternCacheKey => "App.totals.product.manufacturerids";

        /// <summary>
        /// Gets a template of product name on copying
        /// </summary>
        /// <remarks>
        /// {0} : product name
        /// </remarks>
        public static string ProductCopyNameTemplate => "Copy of {0}";

        #endregion

        #region Product attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : page index
        /// {1} : page size
        /// </remarks>
        public static string ProductAttributesAllCacheKey => "App.productattribute.all-{0}-{1}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product attribute ID
        /// </remarks>
        public static string ProductAttributesByIdCacheKey => "App.productattribute.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        public static string ProductAttributeMappingsAllCacheKey => "App.productattributemapping.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product attribute mapping ID
        /// </remarks>
        public static string ProductAttributeMappingsByIdCacheKey => "App.productattributemapping.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product attribute mapping ID
        /// </remarks>
        public static string ProductAttributeValuesAllCacheKey => "App.productattributevalue.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product attribute value ID
        /// </remarks>
        public static string ProductAttributeValuesByIdCacheKey => "App.productattributevalue.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        public static string ProductAttributeCombinationsAllCacheKey => "App.productattributecombination.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductAttributesPatternCacheKey => "App.productattribute.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductAttributeMappingsPatternCacheKey => "App.productattributemapping.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductAttributeValuesPatternCacheKey => "App.productattributevalue.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductAttributeCombinationsPatternCacheKey => "App.productattributecombination.";

        #endregion

        #region Product tags

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string ProductTagCountCacheKey => "App.producttag.count-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        public static string ProductTagAllByProductIdCacheKey => "App.producttag.allbyproductid-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductTagPatternCacheKey => "App.producttag.";

        #endregion

        #region Review type

        /// <summary>
        /// Key for caching all review types
        /// </summary>
        public static string ReviewTypeAllKey => "App.reviewType.all";

        /// <summary>
        /// Key for caching review type
        /// </summary>
        /// <remarks>
        /// {0} : review type ID
        /// </remarks>
        public static string ReviewTypeByIdKey => "App.reviewType.id-{0}";

        /// <summary>
        /// Key pattern to clear cache review types
        /// </summary>
        public static string ReviewTypeByPatternKey => "App.reviewType.";

        /// <summary>
        /// Key for caching product review and review type mapping
        /// </summary>
        /// <remarks>
        /// {0} : product review ID
        /// </remarks>
        public static string ProductReviewReviewTypeMappingAllKey => "App.productReviewReviewTypeMapping.all-{0}";

        #endregion

        #region Specification attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// {1} : specification attribute option ID
        /// {2} : allow filtering
        /// {3} : show on product page
        /// </remarks>
        public static string ProductSpecificationAttributeAllByProductIdCacheKey => "App.productspecificationattribute.allbyproductid-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductSpecificationAttributePatternCacheKey => "App.productspecificationattribute.";

        #endregion
    }
}
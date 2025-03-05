using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Helpers;
using K9.SharedLibrary.Models;
using K9.WebApplication.Models;
using Microsoft.Extensions.Caching.Memory;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace K9.WebApplication.Services
{
    public class ProductService : CacheableServiceBase<Product>, IProductService
    {
        private readonly ILogger _logger;
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<ProductIngredient> _productIngredientsRepository;
        private readonly IRepository<Ingredient> _ingredientsRepository;
        private readonly IRepository<ProductPackProduct> _productPackProductRepository;
        private readonly IRepository<ProductPack> _productPackRepository;
        private readonly IRepository<ProductIngredientSubstitute> _productIngredientSubstituteRepository;
        private readonly IIngredientService _ingredientService;
        private readonly IRepository<Activity> _activitiesRepository;
        private readonly IRepository<DietaryRecommendation> _dietaryRecommendationsRepository;
        private readonly IRepository<FoodItem> _foodItemsRepository;
        private readonly IRepository<ProductPackProduct> _productPackProductsRepository;
        private readonly UrlHelper _urlHelper;

        public ProductService(
            ILogger logger,
            IRepository<Product> productsRepository,
            IRepository<ProductIngredient> productIngredientsRepository,
            IRepository<Ingredient> ingredientsRepository,
            IRepository<ProductPackProduct> productPackProductRepository,
            IRepository<ProductPack> productPackRepository,
            IRepository<ProductIngredientSubstitute> productIngredientSubstituteRepository,
            IIngredientService ingredientService,
            IRepository<Protocol> protocolsRepository,
            IRepository<IngredientSubstitute> ingredientSubstitutesRepository,
            IRepository<Activity> activitiesRepository,
            IRepository<DietaryRecommendation> dietaryRecommendationsRepository,
            IRepository<FoodItem> foodItemsRepository,
            IRepository<ProductPackProduct> productPackProductsRepository) : base(productsRepository, productPackRepository, ingredientsRepository, protocolsRepository, ingredientSubstitutesRepository, productIngredientsRepository, productIngredientSubstituteRepository, activitiesRepository, dietaryRecommendationsRepository, productPackProductsRepository, foodItemsRepository)
        {
            _logger = logger;
            _productsRepository = productsRepository;
            _productIngredientsRepository = productIngredientsRepository;
            _ingredientsRepository = ingredientsRepository;
            _productPackProductRepository = productPackProductRepository;
            _productPackRepository = productPackRepository;
            _productIngredientSubstituteRepository = productIngredientSubstituteRepository;
            _ingredientService = ingredientService;
            _activitiesRepository = activitiesRepository;
            _dietaryRecommendationsRepository = dietaryRecommendationsRepository;
            _foodItemsRepository = foodItemsRepository;
            _productPackProductsRepository = productPackProductsRepository;
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public Product Find(int id)
        {
            return MemoryCacheHelper.Cache.GetOrCreate(GetCacheKey(id), entry =>
            {
                entry.SetOptions(GetMemoryCacheEntryOptions(Constants.Constants.OneWeek));

                var product = _productsRepository.Find(id);
                if (product != null)
                {
                    product = GetFullProduct(product);
                }

                return product;
            });
        }

        public Product FindNext(int id)
        {
            var product = _productsRepository.Find(e => e.Id > id).OrderBy(e => e.Id).FirstOrDefault() ?? _productsRepository.GetQuery("SELECT TOP 1 * FROM [Product] ORDER BY [Id]").FirstOrDefault();
            if (product != null)
            {
                product = GetFullProduct(product);
            }

            return product;
        }

        public Product FindPrevious(int id)
        {
            var product = _productsRepository.Find(e => e.Id < id).OrderByDescending(e => e.Id).FirstOrDefault() ?? _productsRepository.GetQuery("SELECT TOP 1 * FROM [Product] ORDER BY [Id] DESC").FirstOrDefault();
            if (product != null)
            {
                product = GetFullProduct(product);
            }

            return product;
        }

        public Product Find(string seoFriendlyId)
        {
            var product = _productsRepository.Find(e => e.SeoFriendlyId == seoFriendlyId).FirstOrDefault();
            if (product != null)
            {
                product = GetFullProduct(product);
            }

            return product;
        }

        public Product Find(Guid id)
        {
            var product = _productsRepository.Find(e => e.ExternalId == id).FirstOrDefault();
            if (product != null)
            {
                product = GetFullProduct(product);
            }

            return product;
        }

        public Product FindWithIngredientsSelectList(int id)
        {
            var model = Find(id);
            var existingIngredients = _productIngredientsRepository.Find(e => e.ProductId == id).ToList();
            var selectListItems = _ingredientsRepository.Find(e => !e.IsDeleted).OrderBy(e => e.Name).ToList();
            foreach (var ingredient in selectListItems)
            {
                ingredient.IsSelected = existingIngredients.Exists(e => e.IngredientId == ingredient.Id);
            }

            model.IngredientsSelectList = selectListItems.OrderByDescending(e => e.IsSelected)
                .ThenBy(e => e.Category)
                .ThenBy(e => e.Name).ToList();

            return model;
        }

        public ProductPack FindPack(Guid id)
        {
            var pack = _productPackRepository.Find(e => e.ExternalId == id).FirstOrDefault();
            if (pack != null)
            {
                pack = GetFullProductPack(pack);
            }
            return pack;
        }

        public ProductPack FindPack(int id)
        {
            var pack = _productPackRepository.Find(id);
            if (pack != null)
            {
                pack = GetFullProductPack(pack);
            }
            return pack;
        }

        public ProductPack FindPack(string seoFriendlyId)
        {
            var productPack = _productPackRepository.Find(e => e.SeoFriendlyId == seoFriendlyId).FirstOrDefault();
            if (productPack != null)
            {
                productPack = GetFullProductPack(productPack);
            }

            return productPack;
        }

        public Product GetFullProduct(Product product)
        {
            return MemoryCacheHelper.Cache.GetOrCreate(GetCacheKey(product.Id), entry =>
            {
                entry.SetOptions(GetMemoryCacheEntryOptions(Constants.Constants.OneWeek));

                product.Url = _urlHelper.AbsoluteAction("Details", "Product", new { seoFriendlyId = product.SeoFriendlyId });
                product.QrCodeUrl = _urlHelper.Action("GetQrCode", "Shared", new { code = product.Url, size = 111 });

                var productIngredients = GetProductIngredients().Where(e => e.ProductId == product.Id).ToList();
                var productIngredientsWithSubstitutes = new List<ProductIngredient>();
                var groupedProductIngredients = new List<ProductIngredient>();

                foreach (var productIngredient in productIngredients)
                {
                    productIngredient.Ingredient = _ingredientService.Find(productIngredient.IngredientId);
                    productIngredient.IngredientName = productIngredient.Ingredient.Name;
                    productIngredient.IngredientSubstitutes =
                        GetProductIngredientSubstitutes().Where(e => e.ProductIngredientId == productIngredient.Id).ToList();

                    foreach (var ingredientSubstitute in productIngredient.Ingredient.Substitutes)
                    {
                        var productIngredientSubstitute = productIngredient.IngredientSubstitutes.FirstOrDefault(e =>
                            e.SubstituteIngredientId == ingredientSubstitute.SubstituteIngredientId);

                        if (productIngredientSubstitute != null)
                        {
                            ingredientSubstitute.IsSelected = true;
                            ingredientSubstitute.Priority = productIngredientSubstitute.Priority;
                        }
                    }

                    foreach (var productIngredientIngredientSubstitute in productIngredient.IngredientSubstitutes)
                    {
                        productIngredientIngredientSubstitute.SubstituteIngredient =
                            GetIngredients().FirstOrDefault(e => e.Id == productIngredientIngredientSubstitute.SubstituteIngredientId);
                    }

                    productIngredient.Ingredient.Substitutes = productIngredient.Ingredient.Substitutes
                        .OrderByDescending(e => e.IsSelected)
                        .ThenBy(e => e.Priority).ToList();

                    if (productIngredient.Ingredient.GetIsInStock())
                    {
                        productIngredientsWithSubstitutes.Add(productIngredient);
                    }
                    else
                    {
                        var substitutes = productIngredient.IngredientSubstitutes
                            .Where(e => e.SubstituteIngredient != null && e.SubstituteIngredient.GetIsInStock())
                            .Take(productIngredient.NumberOfSubstitutesToUse)
                            .OrderBy(e => e.Priority).ToList();

                        if (substitutes.Any())
                        {
                            var splitAmount = productIngredient.Amount / substitutes.Count;

                            productIngredientsWithSubstitutes.AddRange(substitutes.Select(e => new ProductIngredient
                            {
                                BatchSize = productIngredient.BatchSize,
                                Ingredient = e.SubstituteIngredient,
                                IngredientId = e.SubstituteIngredientId,
                                Amount = splitAmount
                            }));
                        }
                        else
                        {
                            productIngredientsWithSubstitutes.Add(productIngredient);
                        }
                    }
                }

                groupedProductIngredients = productIngredientsWithSubstitutes
                    .GroupBy(e => e.IngredientId)
                    .Select(i =>
                    {
                        var productIngredient = productIngredientsWithSubstitutes.First(e => e.IngredientId == i.Key);
                        var item = new ProductIngredient
                        {
                            BatchSize = productIngredient.BatchSize,
                            Ingredient = productIngredient.Ingredient,
                            Amount = i.Sum(e => e.Amount)
                        };
                        return item;
                    }).ToList();

                product.ProductIngredients = productIngredients.OrderByDescending(e => e.Amount)
                                .ThenBy(e => e.Ingredient.Name);

                product.Ingredients = product.ProductIngredients.ToList();

                product.IngredientsWithSubstitutes = groupedProductIngredients.OrderBy(e => e.Ingredient.Category).ThenByDescending(e => e.Amount)
                    .ThenBy(e => e.Ingredient.Name).ToList();

                product.FormattedAmount = product.GetFormattedAmount();
                product.FormattedAmountPerServing = product.GetFormattedAmountPerServing();
                product.ProfitMargin = product.GetProfitMargin();
                product.ProfitMarginDiscount1 = product.GetProfitMarginDiscount1();
                product.ProfitMarginDiscount2 = product.GetProfitMarginDiscount2();
                product.SuggestedRetailPrice = product.GetSuggestedRetailPrice();
                product.ProfitMarginSmallPack = product.GetProfitMarginSmallPack();
                product.ProfitMarginSmallPackDiscount1 = product.GetProfitMarginSmallPackDiscount1();
                product.ProfitMarginSmallPackDiscount2 = product.GetProfitMarginSmallPackDiscount2();
                product.CostOfIngredients = product.GetCostOfIngredients();
                product.TotalCost = product.GetTotalCost();

                return product;
            });
        }

        public Product Duplicate(int id)
        {
            var product = Find(id);
            if (product == null)
            {
                return null;
            }

            var newProduct = new Product();
            var newProductExternalId = Guid.NewGuid();

            product.MapTo(newProduct);
            newProduct.Id = 0;
            newProduct.Name = $"{product.Name} (Copy)";
            newProduct.ExternalId = newProductExternalId;

            _productsRepository.Create(newProduct);

            // Get new Id
            newProduct = Find(newProductExternalId);

            if (newProduct == null)
            {
                throw new Exception("Error duplicating product");
            }

            // Copy Ingredients
            foreach (var ingredient in product.Ingredients)
            {
                var newIngredient = new ProductIngredient
                {
                    ProductId = newProduct.Id,
                    IngredientId = ingredient.IngredientId,
                    Amount = ingredient.Amount
                };

                _productIngredientsRepository.Create(newIngredient);
            }

            return newProduct;
        }

        public Product UpdateBatchSize(Product product, int batchSize)
        {
            if (batchSize > 1)
            {
                product.Ingredients.ForEach(e => e.BatchSize = batchSize);
                product.IngredientsWithSubstitutes.ForEach(e => e.BatchSize = batchSize);
            }

            product.BatchSize = batchSize;
            return product;
        }

        public void DeleteChildRecords(int id)
        {
            var product = Find(id);

            foreach (var productIngredient in product.Ingredients)
            {
                _productIngredientsRepository.Delete(productIngredient.Id);
            }
        }

        public List<Product> List(bool retrieveFullProduct = false, bool includeCustomProducts = false)
        {
            return MemoryCacheHelper.Cache.GetOrCreate(GetCacheKey(retrieveFullProduct, includeCustomProducts), entry =>
            {
                entry.SetOptions(GetMemoryCacheEntryOptions(SharedLibrary.Constants.OutputCacheConstants.TenMinutes));

                var products = _productsRepository.Find(e => !e.IsDeleted).OrderBy(e => e.Name).ToList();

                if (!includeCustomProducts)
                {
                    products = products.Where(e => e.ClientId <= 0 || !e.ClientId.HasValue).ToList();
                }

                if (retrieveFullProduct)
                {
                    var fullProducts = new List<Product>();
                    foreach (var product in products)
                    {
                        fullProducts.Add(GetFullProduct(product));
                    }

                    return fullProducts;
                }

                return products;
            });
        }

        public List<ProductItem> ListProductItemsAll()
        {
            return ListProductItems(List(true).ToList());
        }

        public List<ProductItem> ListProductItems100Capsules()
        {
            return ListProductItems(List(true).Where(e => e.ProductType == EProductType.Capsules && e.Amount == 100).ToList());
        }

        public List<ProductItem> ListProductItems200Capsules()
        {
            return ListProductItems(List(true).Where(e => e.ProductType == EProductType.Capsules && e.Amount == 200).ToList());
        }

        public List<ProductItem> ListProductItems400Capsules()
        {
            return ListProductItems(List(true).Where(e => e.ProductType == EProductType.Capsules && e.Amount == 400).ToList());
        }

        public List<ProductItem> ListProductItemsLiquids()
        {
            return ListProductItems(List(true).Where(e => e.ProductType == EProductType.Liquid).ToList());
        }

        public List<ProductItem> ListProductItemsPowders()
        {
            return ListProductItems(List(true).Where(e => e.ProductType == EProductType.Powder).ToList());
        }

        public List<ProductItem> ListProductItems(List<Product> products)
        {
            var productItems = new List<ProductItem>();

            foreach (var product in products)
            {
                var productItem = product.MapTo<ProductItem>();

                productItem.ProductName = product.GetProductName();
                productItem.ProductSubTitle = product.GetProductSubTitle();
                productItem.CapsulesDosageLabelText = product.GetCapsulesDosageLabelText();
                productItem.CapsulesDailyLabellext = product.GetAmountToTakeDailyText();
                productItem.FullDosageLabellext = product.GetFullDosageLabellext();
                productItem.BenefitsLabelText = product.GetBenefitsLabelText();
                productItem.IngredientsList = product.GetIngredientsList();
                productItem.QuantitiesList = product.GetQuantitiesList();
                productItem.DailyValues = product.GetDailyValues();
                productItem.RecommendationsText = product.GetRecommendationsText();

                productItems.Add(productItem);
            }

            return productItems;
        }

        public List<ProductPack> ListProductPacks(bool retrieveFullProduct = false)
        {
            return MemoryCacheHelper.Cache.GetOrCreate(GetCacheKey<ProductPack>(retrieveFullProduct), entry =>
            {
                entry.SetOptions(GetMemoryCacheEntryOptions(SharedLibrary.Constants.OutputCacheConstants.TenMinutes));

                var fullProductPacks = new List<ProductPack>();
                var productPacks = _productPackRepository.Find(e => !e.IsDeleted).OrderBy(e => e.Name).ToList();

                if (retrieveFullProduct)
                {
                    foreach (var pack in productPacks)
                    {
                        var fullPack = GetFullProductPack(pack);
                        foreach (var productPackProduct in fullPack.Products)
                        {
                            productPackProduct.Product = GetFullProduct(productPackProduct.Product);
                        }
                        fullProductPacks.Add(fullPack);
                    }
                }

                return fullProductPacks;
            });
        }

        public ProductPack GetFullProductPack(ProductPack productPack)
        {
            return MemoryCacheHelper.Cache.GetOrCreate(GetCacheKey<ProductPack>(productPack.Id), entry =>
            {
                entry.SetOptions(GetMemoryCacheEntryOptions(Constants.Constants.OneWeek));

                var products = _productPackProductRepository.Find(e => e.ProductPackId == productPack.Id).ToList();

                foreach (var product in products)
                {
                    product.Product = GetProducts().Where(e => e.Id == product.ProductId).FirstOrDefault();
                }

                productPack.Products = products.OrderBy(e => e.Product.Name).ToList();

                productPack.FormattedTotalProductsPrice = productPack.GetFormattedTotalProductsPrice();
                productPack.TotalProductsPrice = productPack.GetTotalProductsPrice();

                return productPack;
            });
        }

        public ProductPack DuplicateProductPack(int id)
        {
            ClearCache();

            var productPack = FindPack(id);
            if (productPack == null)
            {
                return null;
            }

            var newProductPack = new ProductPack();
            var newExternalId = Guid.NewGuid();

            productPack.MapTo(newProductPack);
            newProductPack.Id = 0;
            newProductPack.Name = $"{newProductPack.Name} (Copy)";
            newProductPack.ExternalId = newExternalId;

            _productPackRepository.Create(newProductPack);

            // Get new Id
            newProductPack = FindPack(newExternalId);

            if (newProductPack == null)
            {
                throw new Exception("Error duplicating product pack");
            }

            // Copy Products
            foreach (var product in productPack.Products)
            {
                var newProduct = new ProductPackProduct
                {
                    ProductId = product.ProductId,
                    ProductPackId = newProductPack.Id,
                    Amount = product.Amount
                };

                _productPackProductRepository.Create(newProduct);
            }

            return newProductPack;
        }

        public void EditIngredientSubstitutes(Product product)
        {
            foreach (var productIngredient in product.Ingredients.Where(e => e.Ingredient.Substitutes != null && e.Ingredient.Substitutes.Any()))
            {
                var existingSubstitutes = _productIngredientSubstituteRepository.Find(e => e.ProductIngredientId == productIngredient.Id).ToList();
                var newItems = productIngredient.Ingredient.Substitutes.Where(e => e.IsSelected).ToList();

                foreach (var item in existingSubstitutes)
                {
                    _productIngredientSubstituteRepository.Delete(item.Id);
                }

                foreach (var item in newItems)
                {
                    var newItem = new ProductIngredientSubstitute
                    {
                        ProductIngredientId = productIngredient.Id,
                        SubstituteIngredientId = item.SubstituteIngredientId,
                        Priority = item.Priority
                    };
                    _productIngredientSubstituteRepository.Create(newItem);
                }

                var productIngredientRecord = _productIngredientsRepository.Find(productIngredient.Id);
                productIngredientRecord.NumberOfSubstitutesToUse = productIngredient.NumberOfSubstitutesToUse;
                _productIngredientsRepository.Update(productIngredientRecord);
            }

            ClearCache();
        }

        public void EditIngredients(Product product)
        {
            var existingIngredients = _productIngredientsRepository.Find(e => e.ProductId == product.Id).ToList();
            var newItems = product.IngredientsSelectList.Where(e => e.IsSelected)
                .Where(i => !existingIngredients.Select(e => e.IngredientId).Contains(i.Id)).ToList();
            var itemsToDelete = existingIngredients.Where(i => !product.IngredientsSelectList.Where(e => e.IsSelected).Select(e => e.Id).Contains(i.IngredientId)).Select(e => e.Id).ToList();

            var substitutesToDelete = _productIngredientSubstituteRepository
                .Find(e => itemsToDelete.Contains(e.ProductIngredientId)).Select(e => e.Id).ToList();

            _productIngredientSubstituteRepository.DeleteBatch(substitutesToDelete);

            _productIngredientsRepository.DeleteBatch(itemsToDelete);

            foreach (var item in newItems)
            {
                var newItem = new ProductIngredient
                {
                    ProductId = product.Id,
                    IngredientId = item.Id
                };
                _productIngredientsRepository.Create(newItem);
            }
        }

        public void UpdateProductCategories()
        {
            var itemCode = (int)ECategory.DietarySupplement + Constants.Constants.ItemCodeGap;

            foreach (var product in _productsRepository.List().OrderBy(e => e.Name).ToList())
            {
                product.ItemCode = itemCode;
                _productsRepository.Update(product);
                itemCode += Constants.Constants.ItemCodeGap;
            }

            ClearCache();
        }
    }
}
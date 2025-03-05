using K9.DataAccessLayer.Models;
using K9.WebApplication.Models;
using System;
using System.Collections.Generic;

namespace K9.WebApplication.Services
{
    public interface IProductService : ICategorisableService, ICacheableService
    {
        Product Find(int id);
        Product FindPrevious(int id);
        Product FindNext(int id);
        Product Find(string seoFriendlyId);
        Product Find(Guid id);
        Product FindWithIngredientsSelectList(int id);
        Product GetFullProduct(Product product);
        Product Duplicate(int id);
        Product UpdateBatchSize(Product product, int batchSize);
        void DeleteChildRecords(int id);
        void EditIngredients(Product product);
        void EditIngredientSubstitutes(Product product);
        void UpdateProductCategories();
        List<Product> List(bool retrieveFullProduct = false, bool includeCustomProducts = false);
        List<ProductItem> ListProductItemsAll();
        List<ProductItem> ListProductItems100Capsules();
        List<ProductItem> ListProductItems200Capsules();
        List<ProductItem> ListProductItems400Capsules();
        List<ProductItem> ListProductItemsLiquids();
        List<ProductItem> ListProductItemsPowders();
        List<ProductPack> ListProductPacks(bool retrieveFullProduct = false);

        ProductPack FindPack(Guid id);
        ProductPack FindPack(int id);
        ProductPack FindPack(string seoFriendlyId);
        ProductPack GetFullProductPack(ProductPack productPack);
        ProductPack DuplicateProductPack(int id);
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.ViewModels
{

    public class ProtocolReviewItem
    {
        public Product Product { get; set; }
        public ProtocolProduct ProtocolProduct { get; set; }
        public ProductPackProduct ProductPackProduct { get; set; }
        
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProtocolRequiresLabel)]
        public int AmountRequired => (ProtocolProduct?.AmountRequired ?? 0) + (ProductPackProduct?.AmountRequired ?? 0);
        
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ActualAmountLabel)]
        public int ActualAmount => (ProtocolProduct?.Amount ?? 0) + (ProductPackProduct?.Amount ?? 0);

        public bool GetAmountsAreEqual() => AmountRequired == ActualAmount;
    }

    public class ProtocolReviewViewModel
    {
        public Protocol Protocol { get; set; }

        public List<ProtocolReviewItem> GetAllProducts() => Protocol.GetAllProducts().Select(e => new ProtocolReviewItem
        {
            Product = e,
            ProtocolProduct = Protocol.GetProtocolProductByProductId(e.Id),
            ProductPackProduct = Protocol.GetProtocolProductPackProductByProductId(e.Id)
        }).ToList();

      

    }
}
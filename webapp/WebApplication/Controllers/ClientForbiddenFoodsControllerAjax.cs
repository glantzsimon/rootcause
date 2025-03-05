using K9.DataAccessLayer.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    public partial class ClientForbiddenFoodsController
    {
        public JsonResult AddFoodItem(int clientId, int foodItemId)
        {
            try
            {
                var original = Repository.Find(e => e.ClientId == clientId && e.FoodItemId == foodItemId).FirstOrDefault();

                if (original == null)
                {
                    Repository.Create(new ClientForbiddenFood
                    {
                        ClientId = clientId,
                        FoodItemId = foodItemId
                    });
                    _protocolService.ClearCache();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public JsonResult PromoteFoodItem(int clientId, int foodItemId)
        {
            try
            {
                var original = Repository.Find(e => e.ClientId == clientId && e.FoodItemId == foodItemId).FirstOrDefault();
                if (original != null)
                {
                    Repository.Delete(original.Id);
                }
                else
                {
                    Repository.Create(new ClientForbiddenFood
                    {
                        ClientId = clientId,
                        FoodItemId = foodItemId,
                        IsPromotion = true
                    });
                }
                _protocolService.ClearCache();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}
using ServiceStack.Text;
using System;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    public partial class ProductsController
    {
        public JsonResult MarkIngredientAsOutOfStock(int ingredientId)
        {
            try
            {
                _ingredientService.MarkIngredientAsOutOfStock(ingredientId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
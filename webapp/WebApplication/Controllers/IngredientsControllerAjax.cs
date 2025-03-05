using K9.WebApplication.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    public partial class IngredientsController
    {
        public JsonResult UpdateIngredientPriorities(SortableItemsViewModel model)
        {
            try
            {
                _ingredientService.UpdateIngredientPriorities(model.Items.ToList());
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
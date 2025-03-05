﻿using K9.WebApplication.Packages;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    public class ErrorController : BaseRootController
	{
	    public ErrorController(IServicePackage servicePackage)
	        : base(servicePackage)
	    {
	    }

        public ActionResult Index(string errorMessage = "")
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Logger.Error(errorMessage);
            }
        	return View("FriendlyError");
		}

		public ActionResult NotFound()
		{
			return View("NotFound");
		}

		public ActionResult Unauthorized()
		{
			return View("Unauthorized");
		}

	    public override string GetObjectName()
	    {
	        return string.Empty;
	    }
    }
}

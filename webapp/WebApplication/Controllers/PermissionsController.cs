﻿using K9.Base.DataAccessLayer.Models;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Packages;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
	[RequirePermissions(Role = RoleNames.Administrators)]
	public class PermissionsController : BaseRootController<Permission>
	{
		public PermissionsController(IControllerPackage<Permission> controllerPackage, IServicePackage servicePackage) : base(controllerPackage, servicePackage)
		{
		}
	}
}

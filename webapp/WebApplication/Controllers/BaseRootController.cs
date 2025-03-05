﻿using K9.Base.DataAccessLayer.Attributes;
using K9.Base.WebApplication.Constants;
using K9.Base.WebApplication.Controllers;
using K9.Base.WebApplication.EventArgs;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Helpers.Html;
using K9.SharedLibrary.Models;
using K9.WebApplication.Enums;
using K9.WebApplication.Helpers;
using K9.WebApplication.Models;
using K9.WebApplication.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SessionHelper = K9.Base.WebApplication.Helpers.SessionHelper;

namespace K9.WebApplication.Controllers
{
    public class BaseRootController : BaseController
    {
        public BaseRootController(IServicePackage servicePackage)
            : base(servicePackage.Logger, servicePackage.DataSetsHelper,
                servicePackage.Roles, servicePackage.Authentication, servicePackage.FileSourceHelper)
        {
            My = servicePackage;
            UrlHelper = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            SetBetaWarningSessionVariable();
            SetSessionRoles(Current.UserId);
            
            ViewBag.DeviceType = GetDeviceType();
        }

        public IServicePackage My { get; }

        public UrlHelper UrlHelper { get; }

        public ActionResult SetLanguage(string languageCode, string cultureCode)
        {
            Session[SessionConstants.LanguageCode] = languageCode;
            Session[SessionConstants.CultureCode] = cultureCode;
            return Redirect(Request.UrlReferrer?.ToString());
        }

        public UserMembership GetActiveUserMembership()
        {
            if (Authentication.IsAuthenticated)
            {
                return My.MembershipService.GetActiveUserMembership(Authentication.CurrentUserId);
            }

            return null;
        }

        public ActionResult RedirectToLastSaved()
        {
            // Redirect to previous profile or compatibility reading if set
            var lastProfile = Helpers.SessionHelper.GetLastProfile(true, false);
            var lastCompatibility = Helpers.SessionHelper.GetLastCompatibility(true, false);
            var lastPrediction = Helpers.SessionHelper.GetLastPrediction(true, false);
            var lastBiorhythm = Helpers.SessionHelper.GetLastBiorhythm(true, false);
            var lastKnowledgeBase = Helpers.SessionHelper.GetLastKnowledgeBase(true, false);

            var lastItems = new List<RetrieveLastModel>
            {
                lastProfile,
                lastCompatibility,
                lastPrediction,
                lastBiorhythm,
                lastKnowledgeBase
            };

            var lastStoredItem = lastItems.OrderByDescending(e => e.StoredOn).FirstOrDefault();

            if (lastStoredItem != null && lastStoredItem.StoredOn.Value.Date == DateTime.Today)
            {
                switch (lastStoredItem.Section)
                {
                    case ESection.Profile:
                        return RedirectToAction("RetrieveLastProfile", "PersonalChart");

                    case ESection.Compatibility:
                        return RedirectToAction("RetrieveLastCompatibility", "PersonalChart");

                    case ESection.Predictions:
                        return RedirectToAction("RetrieveLastPrediction", "Predictions");

                    case ESection.Biorhythms:
                        return RedirectToAction("RetrieveLastBiorhythms", "Biorhythms");

                    case ESection.KnowledgeBase:
                        return RedirectToAction("RetrieveLastKnowledgeBaseSection", "KnowledgeBase");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public EDeviceType GetDeviceType()
        {
            return new BrowserInfo(Request.Headers["User-Agent"]).DeviceType;
        }

        public void SetSessionRoles(int userId)
        {
            Helpers.SessionHelper.SetCurrentUserRoles(My.RolesRepository, My.UserRolesRepository, userId);
        }

        public override string GetObjectName()
        {
            return string.Empty;
        }
        
        private static void SetBetaWarningSessionVariable()
        {
            var numberOfDisplays = Helpers.SessionHelper.GetIntValue(Constants.SessionConstants.BetaWarningDisplay);
            if (numberOfDisplays < 1)
            {
                numberOfDisplays++;
                SessionHelper.SetValue(Constants.SessionConstants.BetaWarningDisplay, numberOfDisplays);
            }
            else
            {
                SessionHelper.SetValue(Constants.SessionConstants.BetaWarningHide, true);
            }
        }
    }

    [EnableCaching]
    public class BaseRootController<T> : BaseController<T> where T : class, IObjectBase
    {
        public BaseRootController(IControllerPackage<T> controllerPackage, IServicePackage GetToTheRootPackage)
            : base(controllerPackage)
        {
            My = GetToTheRootPackage;
            UrlHelper = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);

            RecordBeforeCreated += BaseGetToTheRootKiController_RecordBeforeCreated;
            RecordBeforeUpdated += BaseGetToTheRootKiController_RecordBeforeUpdated;
            RecordBeforeUpdate += BaseGetToTheRootKiController_RecordBeforeUpdate;
        }

        public IServicePackage My { get; }

        public UrlHelper UrlHelper { get; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            SetSessionRoles(Current.UserId);
        }
        
        public void SetSessionRoles(int userId)
        {
            Helpers.SessionHelper.SetCurrentUserRoles(My.RolesRepository, My.UserRolesRepository, userId);
        }
        
        private void BaseGetToTheRootKiController_RecordBeforeUpdated(object sender, CrudEventArgs e)
        {
            var model = e.Item as T;
            HtmlParser.ParseHtml(ref model);
        }

        private void BaseGetToTheRootKiController_RecordBeforeCreated(object sender, CrudEventArgs e)
        {
            var model = e.Item as T;
            HtmlParser.ParseHtml(ref model);
        }

        private void BaseGetToTheRootKiController_RecordBeforeUpdate(object sender, CrudEventArgs e)
        {
            var model = e.Item as T;
            HtmlParser.ParseHtml(ref model);
        }
    }

}

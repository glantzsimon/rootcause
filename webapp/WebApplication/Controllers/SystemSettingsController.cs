﻿using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Packages;
using System;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
    public class SystemSettingsController : BaseRootController<SystemSetting>
    {
        public SystemSettingsController(IControllerPackage<SystemSetting> controllerPackage, IServicePackage servicePackage) : base(controllerPackage, servicePackage)
        {
            RecordBeforeCreated += SystemSettingsController_RecordBeforeCreated;
            RecordBeforeCreate += SystemSettingsController_RecordBeforeCreate;
            RecordBeforeIndex += SystemSettingsController_RecordBeforeIndex;
            RecordBeforeUpdate += SystemSettingsController_RecordBeforeUpdate;
        }

        private void SystemSettingsController_RecordBeforeUpdate(object sender, Base.WebApplication.EventArgs.CrudEventArgs e)
        {
            CreateDefaultSystemSettings();
        }

        private void SystemSettingsController_RecordBeforeIndex(object sender, Base.WebApplication.EventArgs.CrudEventArgs e)
        {
            CreateDefaultSystemSettings();
        }

        private void SystemSettingsController_RecordBeforeCreate(object sender, Base.WebApplication.EventArgs.CrudEventArgs e)
        {
            WarnAgainstMultipleSystemSettings();
        }

        private void SystemSettingsController_RecordBeforeCreated(object sender, Base.WebApplication.EventArgs.CrudEventArgs e)
        {
            WarnAgainstMultipleSystemSettings();
        }

        private void WarnAgainstMultipleSystemSettings()
        {
            if (Repository.GetCount() >= 1)
            {
                throw new Exception("Only one Systme Settings record is allowed");
            }
        }

        private void CreateDefaultSystemSettings()
        {
            if (Repository.GetCount() == 0)
            {
                Repository.Create(new SystemSetting
                {
                    Name = Guid.NewGuid().ToString(),
                    IsPausedEmailJobQueue = false,
                    IsSendMembershipUpgradeReminders = true
                });
            }
        }
    }
}

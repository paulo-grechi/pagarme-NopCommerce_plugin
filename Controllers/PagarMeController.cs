using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.PagarMe.Models;
using Nop.Services.Configuration;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Payments.PagarMe.Controllers
{
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class PagarMeController : BasePluginController
    {

        private IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly INotificationService _notificationService;

        public PagarMeController(IPermissionService permissionService, ISettingService settingService, IStoreContext storeContext, INotificationService notificationService)
        {
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Configure()
        {
            if (!AuthorizedPMAsync().Result)
            {
                return AccessDeniedView();
            }

            var storeScope = _storeContext.GetActiveStoreScopeConfigurationAsync().Result;
            var settings = _settingService.LoadSetting<PagarMeSettings>(storeScope);
            var model = new ConfigModel
            {
                SecKeyProd = settings.SecKeyProd,
                SecKeySand = settings.SecKeySand,
                PubKeyProd = settings.PubKeyProd,
                PubKeySand = settings.PubKeySand
            };
            return View("~/Plugins/Payments.PagarMe/Views/Configure.cshtml", model);
        }

        private Task<bool> AuthorizedPMAsync()
        {
            return _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods);
        }

        [HttpPost]
        public IActionResult Configure(ConfigModel model)
        {
            if (!AuthorizedPMAsync().Result)
            {
                return AccessDeniedView();
            }
            var storeScope = _storeContext.GetActiveStoreScopeConfigurationAsync().Result;
            var settings = _settingService.LoadSetting<PagarMeSettings>(storeScope);
            settings.SecKeySand = model.SecKeySand;
            settings.SecKeyProd = model.SecKeyProd;
            settings.PubKeySand = model.PubKeySand;
            settings.PubKeyProd = model.PubKeyProd;

            _settingService.SaveSetting(settings);

            _settingService.ClearCache();

            _notificationService.SuccessNotification("Settings saved successfully");

            return Configure();
        }
    }
}

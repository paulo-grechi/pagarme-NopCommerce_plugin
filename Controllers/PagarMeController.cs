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
                AppId = settings.AppId,
                Token = settings.Token,
                SecretKey = settings.SecretKey,
                ServerKey = settings.ServerKey,
                UserId = settings.UserId
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
            settings.AppId = model.AppId;
            settings.Token = model.Token;
            settings.SecretKey = model.SecretKey;
            settings.ServerKey = model.ServerKey;
            settings.UserId = model.UserId;

            _settingService.SaveSetting(settings);

            _settingService.ClearCache();

            _notificationService.SuccessNotification("Settings saved successfully");

            return Configure();
        }
    }
}

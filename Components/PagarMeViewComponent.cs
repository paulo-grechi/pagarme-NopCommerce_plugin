using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.PagarMe;
using Nop.Plugin.Payments.PagarMe.Models;
using Nop.Plugin.Payments.PagarMe.Services;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.MercadoPago.Components
{
    [ViewComponent(Name = "PaymentsPagarMe")]
    public class PagarMeViewComponent : NopViewComponent
    {
        private readonly ISettingService _setting;
        private readonly IStoreContext _storeContext;

        public PagarMeViewComponent(ISettingService setting, IStoreContext storeContext)
        {
            _setting = setting;
            _storeContext = storeContext;
        }
        public IViewComponentResult Invoke(string paymentZone, object additionalData)
        {
            var settings = _setting.LoadSetting<PagarMeSettings>(_storeContext.GetCurrentStore().Id);
            var model = new ConfigModel
            {
                SecKeyProd = settings.SecKeyProd,
                SecKeySand = settings.SecKeySand,
                PubKeyProd = settings.PubKeyProd,
                PubKeySand = settings.PubKeySand
            };
            //string.IsNullOrEmpty(model.ClientSecret)
            if (string.IsNullOrEmpty(model.SecKeyProd) || string.IsNullOrEmpty(model.PubKeyProd))
            {
                return Content("");
            }
            PagarMeServices mpService = new PagarMeServices(settings);
            var modelPayment = new PaymentInfoModel();
            return View("~/Plugins/Payments.PagarMe/Views/PaymentInfo.cshtml", modelPayment);
        }
    }
}

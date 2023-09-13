using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.MercadoPago.Enums;
using Nop.Plugin.Payments.MercadoPago.Models;
using Nop.Plugin.Payments.MercadoPago.Services;
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
                AppId = settings.AppId,
                SecretKey = settings.SecretKey,
                Token = settings.Token,
                ServerKey = settings.ServerKey,
                UserId = settings.UserId,
            };
            //string.IsNullOrEmpty(model.ClientSecret)
            if (string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.AppId))
            {
                return Content("");
            }
            PagarMeServices mpService = new PagarMeServices(settings, _setting);
            var payMethodList = mpService.GetPaymentMethodsAsync().Result;
            var modelPayment = new PaymentInfoModel(payMethodList)
            {
                OrderId = "",
                OrderTotal = 0,
                RequestInfo = new PaymentRequest
                {
                    Token = model.Token,
                    payer = new PayerForPR(),
                    Description = "",
                    ExternalReference = paymentZone,
                    PaymentMethod = "",
                },
            };
            return View("~/Plugins/Payments.PagarMe/Views/PaymentInfo.cshtml", modelPayment);
        }
    }
}

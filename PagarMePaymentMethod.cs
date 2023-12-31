﻿using Nop.Services.Plugins;
using Nop.Plugin.Payments.PagarMe.Services;
using Nop.Services.Customers;
using Nop.Plugin.Payments.PagarMe.Components;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Nop.Core.Http.Extensions;
using Nop.Services.Localization;
using Microsoft.Extensions.Primitives;
using Nop.Services.Payments;
using Nop.Services.Configuration;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Microsoft.AspNetCore.Http;
using Nop.Plugin.Payments.PagarMe.Models;
using PagarmeApiSDK.Standard.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using NUglify.Helpers;

namespace Nop.Plugin.Payments.PagarMe
{
    public class PagarMePaymentMethod : BasePlugin, IPaymentMethod
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ICustomerService _customerService;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        private readonly ICountryService _countryService;

        private PagarMeServices PMService { get; set; }

        public PagarMePaymentMethod(ISettingService settingService, IWebHelper webHelper, ICustomerService customerService, IActionContextAccessor actionContextAccessor, ILocalizationService localizationService, IProductService productService, ICountryService countryService)
        {
            _settingService = settingService;
            _webHelper = webHelper;
            _customerService = customerService;
            _actionContextAccessor = actionContextAccessor;
            _localizationService = localizationService;
            var settings = _settingService.LoadSetting<PagarMeSettings>();
            PMService = new PagarMeServices(settings);
            _productService = productService;
            _countryService = countryService;
        }

        public bool SupportCapture => false;

        public bool SupportPartiallyRefund => false;

        public bool SupportRefund => true;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.Manual;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;

        public bool SkipPaymentInfo => false;

        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return PMService.CancelRecurringPaymentResult(cancelPaymentRequest);
        }

        public Task<bool> CanRePostProcessPaymentAsync(Order order) => Task.FromResult(false);

        public Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest)
        {
            throw new NotImplementedException("CaptureAsync");
        }

        public Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult(decimal.Zero);
        }

        public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            //already set
            return _actionContextAccessor.ActionContext.HttpContext.Session.GetAsync<ProcessPaymentRequest>(PagarMeDefault.PaymentRequestSessionKey);
        }

        public async Task<string> GetPaymentMethodDescriptionAsync()
        {
            return await Task.FromResult("Pagar Me");
        }

        public Type GetPublicViewComponent()
        {
            return typeof(PaymentInfoViewComponent);
        }

        public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
        {
            var settings = _settingService.LoadSetting<PagarMeSettings>();
            var notConfigured = string.IsNullOrEmpty(settings.PubKeySand) || string.IsNullOrEmpty(settings.SecKeySand) || string.IsNullOrEmpty(settings.Username) || string.IsNullOrEmpty(settings.Pass);
            //var notConfigured = string.IsNullOrEmpty(settings.PubKeyProd) || string.IsNullOrEmpty(settings.SecKeyProd) || string.IsNullOrEmpty(settings.Username) || string.IsNullOrEmpty(settings.Pass);
            return Task.FromResult(notConfigured);
        }

        public override Task InstallAsync()
        {
            _settingService.SaveSetting(new ConfigPagarMe
            {
                PubKeyProd = "",
                PubKeySand = "",
                SecKeyProd = "",
                SecKeySand = ""
            });
            return base.InstallAsync();
        }

        public Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            try
            {
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            try
            {
                var orderId = await _localizationService.GetResourceAsync("PagarMe.OrderId");
                var orderInfo = PMService.GetOrderPagarMe(orderId.ToString()).Result;
                if (orderInfo.Status.Equals("pending"))
                {
                    throw new Exception("Pagamento pendente, aguarde alguns instantes e tente novamente");
                }
                var paymentReturn = new ProcessPaymentResult
                {
                    NewPaymentStatus = Core.Domain.Payments.PaymentStatus.Paid
                };
                return paymentReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            throw new NotImplementedException("ProcessRecurringPaymentAsync");
        }

        public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest)
        {
            throw new NotImplementedException("RefundAsync");
        }

        public override Task UninstallAsync() => base.UninstallAsync();

        public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
        {
            var errors = new List<string>();

            if (form.TryGetValue(nameof(PaymentInfoModel.OrderId), out var orderId) && !StringValues.IsNullOrEmpty(orderId))
            { 
                var orderInfo = PMService.GetOrderPagarMe(orderId.ToString()).Result;
                _localizationService.AddOrUpdateLocaleResourceAsync("PagarMe.OrderId", orderId.ToString());
                if (orderInfo.Status.Equals("pending"))
                {
                    errors.Add("Aguardande o processamento do pagamento para continuar");
                }
            }

            //try to get errors from the form parameters
            if (form.TryGetValue(nameof(PaymentInfoModel.Errors), out var errorValue) && !StringValues.IsNullOrEmpty(errorValue))
                errors.Add(errorValue.ToString());

            return Task.FromResult<IList<string>>(errors);
        }

        public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
        {
            return (Task<VoidPaymentResult>)Task.CompletedTask;
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PagarMe/Configure";
        }

        Task<decimal> IPaymentMethod.GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
        {
            decimal fee = 0;
            return Task.FromResult(fee);
        }

        Task<bool> IPaymentMethod.CanRePostProcessPaymentAsync(Order order)
        {
            throw new NotImplementedException("repost process payment");
        }

        Task<ProcessPaymentRequest> IPaymentMethod.GetPaymentInfoAsync(IFormCollection form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            //already set
            return _actionContextAccessor.ActionContext.HttpContext.Session.GetAsync<ProcessPaymentRequest>(PagarMeDefault.PaymentRequestSessionKey);
        }
    }
}
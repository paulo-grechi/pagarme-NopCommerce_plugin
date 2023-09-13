using Nop.Services.Plugins;
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

namespace Nop.Plugin.Payments.PagarMe
{
    public class PagarMePaymentMethod : BasePlugin, IPaymentMethod
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ICustomerService _customerService;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILocalizationService _localizationService;

        private PagarMeServices PMService { get; set; }

        public PagarMePaymentMethod(ISettingService settingService, IWebHelper webHelper, ICustomerService customerService, IActionContextAccessor actionContextAccessor, ILocalizationService localizationService)
        {
            _settingService = settingService;
            _webHelper = webHelper;
            _customerService = customerService;
            _actionContextAccessor = actionContextAccessor;
            _localizationService = localizationService;
            PMService = new PagarMeServices(_settingService.LoadSetting<PagarMeSettings>(), settingService);
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
            return Task.FromResult(_actionContextAccessor.ActionContext.HttpContext.Session
                .Get<ProcessPaymentRequest>(PagarMeDefault.PaymentRequestSessionKey));
        }

        public async Task<string> GetPaymentMethodDescriptionAsync()
        {
            return await Task.FromResult("Mercado Pago - PIX");
        }

        public Type GetPublicViewComponent()
        {
            return typeof(PaymentInfoViewComponent);
        }

        public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
        {
            var settings = _settingService.LoadSetting<PagarMeSettings>();
            var notConfigured = string.IsNullOrEmpty(settings.PubKeySand) || string.IsNullOrEmpty(settings.SecKeySand);
            //var notConfigured = string.IsNullOrEmpty(settings.PubKeyProd) || string.IsNullOrEmpty(settings.SecKeyProd);
            return Task.FromResult(notConfigured);
        }

        public override Task InstallAsync()
        {
            _settingService.SaveSetting(new ConfigPagarMe
            {
                PubKeyProd = null,
                PubKeySand = null,
                SecKeyProd = null,
                SecKeySand = null
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
                var settings = _settingService.LoadSetting<PagarMeSettings>();

                OrderCreation PayRequest = new OrderCreation();
                var customer = await _customerService.GetCustomerByIdAsync(processPaymentRequest.CustomerId);
                PayRequest.Description = "Pedido NopCommerce - InterplaceShop";
                PayRequest.ExternalReference = processPaymentRequest.OrderGuid.ToString();
                PayRequest.Total = (double)processPaymentRequest.OrderTotal;
                PayRequest.payer = new PayerForPR();
                PayRequest.payer.Email = customer.Email;
                PayRequest.payer.EntityType = "individual";
                PayRequest.payer.Identification = new Identification();
                PayRequest.PaymentMethod = "pix";
                PayRequest.Installments = 1;
                var PayReturn = await PMService.CreatePayment(PayRequest);
                var paymentReturn = new ProcessPaymentResult
                {
                    NewPaymentStatus = Core.Domain.Payments.PaymentStatus.Paid,
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
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            var errors = new List<string>();

            //try to get errors from the form parameters
            if (form.TryGetValue(nameof(PaymentInfoModel.Errors), out var errorValue) && !StringValues.IsNullOrEmpty(errorValue))
                errors.Add(errorValue.ToString());

            return Task.FromResult<IList<string>>(errors);
        }

        public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
        {
            throw new NotImplementedException("VoidAsync");
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PagarMe/Configure";
        }

        Task<bool> IPaymentMethod.HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
        {
            throw new NotImplementedException();
        }

        Task<decimal> IPaymentMethod.GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPaymentMethod.CanRePostProcessPaymentAsync(Order order)
        {
            throw new NotImplementedException();
        }

        Task<IList<string>> IPaymentMethod.ValidatePaymentFormAsync(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        Task<ProcessPaymentRequest> IPaymentMethod.GetPaymentInfoAsync(IFormCollection form)
        {
            throw new NotImplementedException();
        }
    }
}
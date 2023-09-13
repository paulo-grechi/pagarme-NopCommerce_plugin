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
using PagarmeApiSDK.Standard.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nop.Services.Catalog;
using Nop.Services.Directory;

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
            PMService = new PagarMeServices(_settingService.LoadSetting<PagarMeSettings>(), settingService);
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
                var settings = _settingService.LoadSetting<PagarMeSettings>();
                var customer = await _customerService.GetCustomerByIdAsync(processPaymentRequest.CustomerId);
                var country = _countryService.GetCountryByIdAsync(customer.CountryId).Result;
                var address = _customerService.GetAddressesByCustomerIdAsync(processPaymentRequest.CustomerId).Result;
                var splitAddress = address[0].Address1.Split(',');
                var paymentStr = _localizationService.GetResourceAsync("PagarMePaymentCreate").Result;
                var payment = JsonConvert.DeserializeObject<CreateOrderRequest>(paymentStr);
                payment.Code = processPaymentRequest.OrderGuid.ToString();
                var CustomerPagarMe = new CreateCustomerRequest
                {
                    Name = customer.FirstName + " " + customer.LastName,
                    Email = customer.Email,
                    Type = customer.Company == null ? "individual" : "company",
                    Address = new CreateAddressRequest
                    {
                        City = address[0].City,
                        Country = country == null ? "BR" : country.TwoLetterIsoCode,
                        Street = splitAddress[0],
                        ZipCode = address[0].ZipPostalCode,
                        Neighborhood = splitAddress[3],
                        Number = splitAddress[1],
                        Complement = splitAddress[2],
                        Line1 = address[0].Address1
                    },
                    Phones = new CreatePhonesRequest
                    {
                        MobilePhone = new CreatePhoneRequest
                        {
                            Number = customer.Phone
                        }
                    }
                };
                payment.Customer = CustomerPagarMe;
                JArray shoppingCartStr = JArray.Parse(_localizationService.GetResourceAsync("ShoppingCartInfo").Result);
                List<ShoppingCartItem> shoppingCart = new List<ShoppingCartItem>();
                foreach (var item in shoppingCartStr)
                {
                    ShoppingCartItem cartItem = JsonConvert.DeserializeObject<ShoppingCartItem>(item.ToString());
                    shoppingCart.Add(cartItem);
                }
                payment.Items = ParseCartItem(shoppingCart);
                //var PayReturn = await PMService.CreateOrderHttp(payment);
                var paymentReturn = new ProcessPaymentResult
                {
                    NewPaymentStatus = Core.Domain.Payments.PaymentStatus.Pending,
                };
                return paymentReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CreateOrderItemRequest> ParseCartItem(List<ShoppingCartItem> shoppingCartItems)
        {
            if (shoppingCartItems == null)
            {
                return new List<CreateOrderItemRequest>();
            }
            List<CreateOrderItemRequest> ItemsPagarMe = new List<CreateOrderItemRequest>();
            foreach (var cartItem in shoppingCartItems)
            {
                var product = _productService.GetProductByIdAsync(cartItem.ProductId).Result;
                CreateOrderItemRequest ItemPagarme = new CreateOrderItemRequest
                {
                    Amount = int.Parse((product.Price * 100).ToString().Split(',')[0]),
                    Quantity = cartItem.Quantity,
                    Description = product.Name
                };
                ItemsPagarMe.Add(ItemPagarme);
            }
            return ItemsPagarMe;
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
            var itemsList = new List<CreateOrderItemRequest>();
            var payments = new List<CreatePaymentRequest>();
            var payment = new CreatePaymentRequest
            {
                Checkout = new CreateCheckoutPaymentRequest
                {
                    ExpiresIn = 30,
                    DefaultPaymentMethod = form["SelectedMethod"],
                    SkipCheckoutSuccessPage = false,
                    CustomerEditable = true,
                    BillingAddressEditable = true
                }
            };
            payments.Add(payment);
            JArray shoppingCartStr = JArray.Parse(_localizationService.GetResourceAsync("ShoppingCartInfo").Result);
            List<ShoppingCartItem> shoppingCart = new List<ShoppingCartItem>();
            foreach (var item in shoppingCartStr)
            {
                ShoppingCartItem cartItem = JsonConvert.DeserializeObject<ShoppingCartItem>(item.ToString());
                shoppingCart.Add(cartItem);
            }
            var PayRequest = new CreateOrderRequest
            {
                Closed = false,
                Currency = "BRL",
                Payments = payments,
                Items = itemsList
            };
            _localizationService.AddOrUpdateLocaleResourceAsync("PagarMePaymentCreate", JsonConvert.SerializeObject(PayRequest));

            var errors = new List<string>();

            //try to get errors from the form parameters
            if (form.TryGetValue(nameof(PaymentInfoModel.Errors), out var errorValue) && !StringValues.IsNullOrEmpty(errorValue))
                errors.Add(errorValue.ToString());

            return Task.FromResult<IList<string>>(new List<string>());
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
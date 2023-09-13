using Nop.Core.Domain.Directory;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Payments.PagarMe.Models;
using Nop.Plugin.Payments.PagarMe.Services;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Nop.Core.Domain.Customers;
using Nop.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Configuration;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Web.Framework.Components;
using MaxMind.GeoIP2.Exceptions;
using StackExchange.Profiling.Internal;
using PagarmeApiSDK.Standard.Controllers;
using PagarmeApiSDK.Standard.Models;
using PagarmeApiSDK.Standard;
using Newtonsoft.Json.Linq;
using Nop.Services.Catalog;
using System.Xml.Linq;

namespace Nop.Plugin.Payments.PagarMe.Components
{
    public class PaymentInfoViewComponent : NopViewComponent
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPaymentService _paymentService;
        private readonly OrderSettings _orderSettings;
        private readonly ISettingService _settingService;
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IAddressService _addresService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IProductService _productService;
        //        private readonly ICheckoutAttributeParser _checkoutAttributeParser;

        public PaymentInfoViewComponent(
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPaymentService paymentService,
            OrderSettings orderSettings,
            ISettingService settingService,
            ILogger logger,
            IWorkContext workContext,
            IStoreContext storeContext,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IAddressService addressService,
            IShoppingCartService shoppingCartService,
            IStateProvinceService stateProvinceService,
            IOrderTotalCalculationService orderTotalCalculationService,
            IGenericAttributeService genericAttributeService,
            IProductService productService
         )
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _paymentService = paymentService;
            _orderSettings = orderSettings;
            _settingService = settingService;
            _logger = logger;
            _workContext = workContext;
            _storeContext = storeContext;
            _currencyService = currencyService;
            _currencySettings = currencySettings;
            _addresService = addressService;
            _shoppingCartService = shoppingCartService;
            _stateProvinceService = stateProvinceService;
            _orderTotalCalculationService = orderTotalCalculationService;
            _genericAttributeService = genericAttributeService;
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var pmSettings = _settingService.LoadSetting<PagarMeSettings>();
            PagarMeServices PMService = new PagarMeServices(pmSettings, _settingService);
            var model = new PaymentInfoModel();
            //prepare order GUID
            var paymentRequest = new ProcessPaymentRequest();
            _paymentService.GenerateOrderGuidAsync(paymentRequest);

            //try to create an order
            var (order, error, url, qrcode) = await CreateOrderAsync(pmSettings, paymentRequest.OrderGuid);
            if (order != null)
            {
                //save order details for future using
                var key = await _localizationService.GetResourceAsync("Plugins.Payments.PagarMe.OrderId");
            }
            else if (!string.IsNullOrEmpty(error))
            {
                if (_orderSettings.OnePageCheckoutEnabled)
                    ModelState.AddModelError(string.Empty, error);
                else
                    _notificationService.ErrorNotification(error);
            }
            string bytestring = model.ToJson();

            byte[] bytea = new byte[bytestring.ToCharArray().Length];
            int cont = 0;
            foreach (char c in bytestring.ToCharArray())
            {
                bytea[cont] = ((byte)c);
                cont++;
            }
            HttpContext.Session.Set(PagarMeDefault.PaymentRequestSessionKey, bytea);
            model.Errors = error;
            model.UrlPix = url.Replace("+55+55", "+55");
            model.QRCode = qrcode;
            string urlCheckout = "";
            foreach (var checkout in order.Checkouts)
            {
                if (checkout.PaymentUrl != null)
                {
                    urlCheckout = checkout.PaymentUrl;
                    break;
                }
            }
            model.UrlCheckout = urlCheckout;
            return View("~/Plugins/Payments.PagarMe/Views/PaymentInfo.cshtml", model);
        }

        public async Task<(GetOrderResponse Order, string Error, string UrlPix, string QRCode)> CreateOrderAsync(PagarMeSettings settings, Guid orderGuid)
        {
            PagarMeServices PMService = new PagarMeServices(settings, _settingService);
            if ((string.IsNullOrEmpty(settings.SecKeyProd)) || string.IsNullOrEmpty(settings.PubKeyProd))
                throw new NopException("Não Configurado");
            var customer = await _workContext.GetCurrentCustomerAsync();
            var store = await _storeContext.GetCurrentStoreAsync();
            var currency = (await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimaryStoreCurrencyId))?.CurrencyCode;

            if (string.IsNullOrEmpty(currency))
                throw new NopException("Moeda principal da loja não configurada");

            var billingAddress = await _addresService.GetAddressByIdAsync(customer.BillingAddressId ?? 0) ?? throw new NopException("Endereço de cobrança do cliente não configurado");

            var shoppingCart = (await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id)).ToList();

            var shippingAddress = await _addresService.GetAddressByIdAsync(customer.ShippingAddressId ?? 0);
            if (!await _shoppingCartService.ShoppingCartRequiresShippingAsync(shoppingCart))
                shippingAddress = null;

            var billStateProvince = await _stateProvinceService.GetStateProvinceByAddressAsync(billingAddress);
            var shipStateProvince = await _stateProvinceService.GetStateProvinceByAddressAsync(shippingAddress);

            //prepare order details
            //var orderDetails = new OrderCreation();
            //orderDetails.Payments = new PagarMePayment();
            //orderDetails.Payments.Amount = 0;
            //prepare some common properties


            //prepare purchase unit details
            var taxTotal = Math.Round((await _orderTotalCalculationService.GetTaxTotalAsync(shoppingCart, false)).taxTotal, 2);
            var (cartShippingTotal, _, _) = await _orderTotalCalculationService.GetShoppingCartShippingTotalAsync(shoppingCart, false);
            var shippingTotal = Math.Round(cartShippingTotal ?? decimal.Zero, 2);
            var (shoppingCartTotal, _, _, _, _, _) = await _orderTotalCalculationService
                .GetShoppingCartTotalAsync(shoppingCart, usePaymentMethodAdditionalFee: false);
            if (shoppingCart.Count > 0)
            {
                shoppingCart.First().CustomerEnteredPrice = Math.Round(shoppingCartTotal ?? decimal.Zero, 2);
            }
            _localizationService.AddOrUpdateLocaleResourceAsync("ShoppingCartInfo", JsonConvert.SerializeObject(shoppingCart));
            var orderTotal = Math.Round(shoppingCartTotal ?? decimal.Zero, 2);

            //orderDetails.Payments.Amount = orderTotal == null ? 0 : (double) orderTotal;
            //add checkout attributes as order items
            var checkoutAttributes = await _genericAttributeService
                .GetAttributeAsync<string>(customer, NopCustomerDefaults.CheckoutAttributes, store.Id);
            //var checkoutAttributeValues = _checkoutAttributeParser.ParseCheckoutAttributeValues(checkoutAttributes);
            (var url, var qr) = await PMService.GeraPayloadPix(customer, 0);
            var itemsList = new List<CreateOrderItemRequest>();
            var payments = new List<CreatePaymentRequest>();
            var payMethods = new List<string>();
            payMethods.Add("pix");
            payMethods.Add("credit_card");
            payMethods.Add("debit_card");
            var pixAddInfo = new List<PixAdditionalInformation>();
            var pixInfo = new PixAdditionalInformation
            {
                Name = "Lojas Pro4ce",
                MValue = "Lojas Pro4ce"
            };
            pixAddInfo.Add(pixInfo);
            var docxml = XDocument.Parse(customer.CustomCustomerAttributesXML);
            string doc = docxml.Root.Value;
            CreateCustomerRequest customerPagarme = new CreateCustomerRequest
            {
                Name = customer.FirstName + " " + customer.LastName,
                Email = customer.Email,
                Type = customer.Company != null && !customer.Company.Equals("") ? "company" : "individual",
                Address = new CreateAddressRequest
                {
                    ZipCode = customer.ZipPostalCode,
                    Line1 = customer.StreetAddress,
                    Line2 = customer.StreetAddress2,
                    State = customer.County,
                    City = customer.City,
                    Country = "BR"
                },
                Document = doc,
                Phones = new CreatePhonesRequest
                {
                    MobilePhone = new CreatePhoneRequest
                    {
                        CountryCode = customer.Phone.Substring(1, 2),
                        AreaCode = customer.Phone.Substring(customer.Phone.IndexOf("(") + 1, 2),
                        Number = customer.Phone.Substring(customer.Phone.IndexOf(")") + 1).Replace("-", "")
                    }
                }

            };
            var payment = new CreatePaymentRequest
            {
                PaymentMethod = "checkout",
                Checkout = new CreateCheckoutPaymentRequest
                {
                    AcceptedPaymentMethods = payMethods,
                    ExpiresIn = 120,
                    DefaultPaymentMethod = "",
                    SkipCheckoutSuccessPage = false,
                    //SuccessUrl = "/onepagecheckout#opc-confirm_order",
                    CustomerEditable = true,
                    BillingAddressEditable = true,
                    BillingAddress = customerPagarme.Address,
                    Pix = new CreateCheckoutPixPaymentRequest
                    {
                        ExpiresIn = 3600,
                        AdditionalInformation = pixAddInfo
                    }
                },
                Amount = int.Parse((orderTotal * 100).ToString().Split(',')[0])
            };
            payments.Add(payment);
            var PayRequest = new CreateOrderRequest
            {
                Closed = false,
                Code = orderGuid.ToString(),
                Currency = "BRL",
                Payments = payments,
                Items = ParseCartItem(shoppingCart),
                Customer = customerPagarme
            };
            GetOrderResponse model = PMService.CreateOrderHttp(PayRequest).Result;
            //GetOrderResponse model = PMService.CreateOrder(PayRequest).Result;
            //GetOrderResponse model = new GetOrderResponse();
            return (model, "", url, qr);
        }

        private async Task<(TResult Result, string Error)> HandleFunctionAsync<TResult>(Func<Task<TResult>> function)
        {
            try
            {
                return (await function(), default);
            }
            catch (Exception exception)
            {
                var message = exception.Message;
                if (exception is HttpException httpException)
                {
                    return (default, "ERRO");
                }

                var logMessage = $"error: {System.Environment.NewLine}{message}";
                await _logger.ErrorAsync(logMessage, exception, await _workContext.GetCurrentCustomerAsync());

                return (default, message);
            }
        }

        private static async Task<PaymentCreatedReturn> HandleCheckoutRequestAsync<TRequest, PaymentCreatedReturn>(PagarMeSettings settings, OrderCreation request)
        where TRequest : HttpRequest where PaymentCreatedReturn : class
        {
            try
            {
                if (!(string.IsNullOrEmpty(settings.SecKeyProd) || string.IsNullOrEmpty(settings.PubKeyProd)))
                {
                    var client = new HttpClient();
                    var url = "https://api.mercadopago.com/v1/payments/";
                    var json = JsonConvert.SerializeObject(request);
                    var reqCont = new StringContent(json);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + settings.SecKeyProd);
                    var response = (await client.PostAsync(url, reqCont)).Content;
                    var result = JsonConvert.DeserializeObject<PaymentCreatedReturn>(response.ToString());
                    return result;
                }
                else
                {
                    throw new Exception("Não Configurado");
                }
            }
            catch (Exception e)
            {
                throw e;
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
                    Description = product.ShortDescription,
                    Category = product.Name
                };
                ItemsPagarMe.Add(ItemPagarme);
            }
            return ItemsPagarMe;
        }
    }
}

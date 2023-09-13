using Nop.Core.Domain.Directory;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Payments.MercadoPago.Models;
using Nop.Plugin.Payments.MercadoPago.Services;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Web.Framework.Components;
using MaxMind.GeoIP2.Exceptions;
using Microsoft.Build.Framework;
using Nop.Plugin.Payments.MercadoPago.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Nop.Core.Domain.Customers;
using Nop.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Configuration;
using Nop.Services.Configuration;
using System.Net.Http;
using System.Net.Mail;
using Nop.Plugin.Payments.PagarMe.Services;

namespace Nop.Plugin.Payments.PagarMe.Components
{
    public class PaymentInfoViewComponent : NopViewComponent
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPaymentService _paymentService;
        private readonly OrderSettings _orderSettings;
        private readonly ISettingService _settingService;
        private readonly Nop.Services.Logging.ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IAddressService _addresService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICheckoutAttributeParser _checkoutAttributeParser;

        public PaymentInfoViewComponent(
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPaymentService paymentService,
            OrderSettings orderSettings,
            ISettingService settingService,
            Nop.Services.Logging.ILogger logger,
            IWorkContext workContext,
            IStoreContext storeContext,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IAddressService addressService,
            IShoppingCartService shoppingCartService,
            IStateProvinceService stateProvinceService,
            IOrderTotalCalculationService orderTotalCalculationService,
            IGenericAttributeService genericAttributeService,
            ICheckoutAttributeParser checkoutAttributeParser
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
            _checkoutAttributeParser = checkoutAttributeParser;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var pmSettings = _settingService.LoadSetting<PagarMeSettings>();
            PagarMeServices PMService = new PagarMeServices(pmSettings, _settingService);
            var payMethods = PMService.GetPaymentMethodsAsync().Result;
            var model = new PaymentInfoModel(payMethods);
            //prepare order GUID
            var paymentRequest = new ProcessPaymentRequest();
            _paymentService.GenerateOrderGuid(paymentRequest);

            //try to create an order
            var (order, error, url, qrcode) = await CreateOrderAsync(pmSettings, paymentRequest.OrderGuid);
            if (order != null)
            {
                model.OrderTotal = (double)order.Total;
                model.Errors = "";
                model.RequestInfo = order;
                model.UrlPix = url;
                model.QRCode = qrcode;

                //save order details for future using
                var key = await _localizationService.GetResourceAsync("Plugins.Payments.PagarMe.OrderId");
                paymentRequest.CustomValues.Add(key, model.OrderId);
            }
            else if (!string.IsNullOrEmpty(error))
            {
                model.Errors = error;
                if (_orderSettings.OnePageCheckoutEnabled)
                    ModelState.AddModelError(string.Empty, error);
                else
                    _notificationService.ErrorNotification(error);
            }

            HttpContext.Session.Set(PagarMeDefault.PaymentRequestSessionKey, model);

            return View("~/Plugins/Payments.PagarMe/Views/PaymentInfo.cshtml", model);
        }

        public async Task<(PaymentRequest Order, string Error, string UrlPix, string QRCode)> CreateOrderAsync(PagarMeSettings settings, Guid orderGuid)
        {
            PagarMeServices PMService = new PagarMeServices(settings, _settingService);
            if (string.IsNullOrEmpty(settings.Token))
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
            var orderDetails = new PaymentRequest();

            //prepare some common properties
            orderDetails.payer = new PayerForPR
            {
                Email = customer.Email,
            };

            //prepare purchase unit details
            var taxTotal = Math.Round((await _orderTotalCalculationService.GetTaxTotalAsync(shoppingCart, false)).taxTotal, 2);
            var (cartShippingTotal, _, _) = await _orderTotalCalculationService.GetShoppingCartShippingTotalAsync(shoppingCart, false);
            var shippingTotal = Math.Round(cartShippingTotal ?? decimal.Zero, 2);
            var (shoppingCartTotal, _, _, _, _, _) = await _orderTotalCalculationService
                .GetShoppingCartTotalAsync(shoppingCart, usePaymentMethodAdditionalFee: false);
            var orderTotal = Math.Round(shoppingCartTotal ?? decimal.Zero, 2);

            //add checkout attributes as order items
            var checkoutAttributes = await _genericAttributeService
                .GetAttributeAsync<string>(customer, NopCustomerDefaults.CheckoutAttributes, store.Id);
            var checkoutAttributeValues = _checkoutAttributeParser.ParseCheckoutAttributeValues(checkoutAttributes);
            orderDetails.Total = (double)orderTotal;
            orderDetails.Token = settings.Token;
            orderDetails.Installments = 1;
            orderDetails.PaymentMethod = "pix";
            orderDetails.ExternalReference = "pedido-nop";
            var orderMP = new CreateOrderReq();
            orderMP.Total = (double)orderTotal;
            orderMP.ExternalPosId = orderGuid.ToString();
            orderMP.UserId = settings.UserId;
            (var url, var qr) = await PMService.GeraPayloadPix(customer, orderDetails.Total);
            return (orderDetails, null, url, qr);
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

                var logMessage = $"{MercadoPagoDefault.SystemName} error: {System.Environment.NewLine}{message}";
                await _logger.ErrorAsync(logMessage, exception, await _workContext.GetCurrentCustomerAsync());

                return (default, message);
            }
        }

        private static async Task<PaymentCreatedReturn> HandleCheckoutRequestAsync<TRequest, PaymentCreatedReturn>(PagarMeSettings settings, PaymentRequest request)
        where TRequest : HttpRequest where PaymentCreatedReturn : class
        {
            try
            {
                if (!string.IsNullOrEmpty(settings.Token))
                {
                    var client = new HttpClient();
                    var url = "https://api.mercadopago.com/v1/payments/";
                    var json = JsonConvert.SerializeObject(request);
                    var reqCont = new StringContent(json);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + settings.Token);
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
    }
}

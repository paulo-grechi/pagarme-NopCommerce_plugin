using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Payments.PagarMe.Models;
using Nop.Services.Payments;
using Nop.Services.Configuration;

namespace Nop.Plugin.Payments.PagarMe.Services
{
    public class PagarMeServices
    {
        public static string URLBase = "https://api.mercadopago.com/";

        private PagarMeSettings _settings;
        private readonly ISettingService _settingService;

        public string UrlPix;
        public string QRCode;

        public PagarMeServices(PagarMeSettings settings, ISettingService settingService)
        {
            _settings = settings;
            _settingService = settingService;
        }

        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentResult(CancelRecurringPaymentRequest cancelPaymentRequest) => null;
        public Task<CapturePaymentResult> CapturePayment(CapturePaymentRequest capturePaymentRequest)
        {
            return null;
        }
        public async Task<PaymentCreatedReturn> CreatePayment(PaymentRequest payment)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePayment(string Id, PaymentUpdate UpdateInfo)
        {
            var uri = new Uri($"{URLBase}v1/payments/{Id}");
            var json = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(UpdateInfo));
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = uri;
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.Token);
                using (var response = httpClient.PutAsync(uri, json).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

        public async Task<(string Url, string QRCode)> GeraPayloadPix(Core.Domain.Customers.Customer cliente, double total)
        {
            var pixKey = "08829865000177";
            var merchantName = "PRO4CE PR";
            var merchantCity = "CURITIBA";
            using (var httpClient = new HttpClient())
            {
                var uri = new Uri($"https://gerarqrcodepix.com.br/api/v1?nome={merchantName.Replace(" ", "%20")}&cidade={merchantCity.Replace(" ", "%20")}&saida=br&chave={pixKey}&valor={total}");
                httpClient.BaseAddress = uri;
                using (var response = httpClient.GetAsync(uri).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var resposta = (System.Text.Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result));
                        var codigoReq = JObject.Parse(resposta);
                        UrlPix = $"https://wa.me/+55{cliente.Phone}?text={codigoReq.SelectToken("brcode")}";
                        QRCode = $"https://chart.googleapis.com/chart?chs=125x125&cht=qr&chl={codigoReq.SelectToken("brcode")}";
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            return (UrlPix, QRCode);
        }

        public void CreateOrder(Guid OrderGuid, CreateOrderReq Order)
        {
            var uri = new Uri($"https://api.mercadopago.com/instore/orders/qr/seller/collectors/{_settings.AppId}/pos/{OrderGuid}/qrs");
            var json = JsonConvert.SerializeObject(Order);
            var reqContent = new StringContent(json);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = uri;
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.Token);
                using (var response = httpClient.PutAsync(uri, reqContent).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var resposta = JObject.Parse(System.Text.Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result));
                        throw new Exception(resposta.SelectToken("message").ToString());
                    }
                }
            }
        }

        public async Task<(string Url, string QR)> CreatePixMP(Guid OrderGuid, CreateOrderReq createOrderReq, Customer customer)
        {
            var uri = new Uri($"https://api.mercadopago.com/instore/orders/qr/seller/collectors/{_settings.AppId}/pos/{OrderGuid}/qrs");
            var json = JsonConvert.SerializeObject(createOrderReq);
            var reqContent = new StringContent(json);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = uri;
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.Token);
                using (var response = httpClient.PostAsync(uri, reqContent).Result)
                {
                    var resposta = JObject.Parse(System.Text.Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result));
                    if (response.IsSuccessStatusCode)
                    {
                        var qr = resposta.SelectToken("qr_data").ToString();
                        var Url = $"https://wa.me/+55{customer.Phone.Replace("+55", "")}?text={qr}";
                        var QR = $"https://chart.googleapis.com/chart?chs=125x125&cht=qr&chl={qr}";
                        return (Url, QR);
                    }
                    throw new Exception(resposta.SelectToken("message").ToString());
                }
            }
        }

        public async Task<ResourcesList<PaymentMethod2>> GetPaymentMethodsAsync()
        {
            var client = new PaymentMethodClient();
            var paymentMethods = await client.ListAsync();
            return paymentMethods;
        }

        public async Task<PagarMeSettings> RegenToken(UpdateToken updateToken)
        {
            var uri = new Uri($"{URLBase}oauth/token");
            var reqContent = new StringContent(JsonConvert.SerializeObject(updateToken));
            using (var httpCli = new HttpClient())
            {
                httpCli.BaseAddress = uri;
                httpCli.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.Token);
                using (var response = httpCli.PostAsync(uri, reqContent).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var resposta = JObject.Parse(System.Text.Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result));
                        var newSettings = _settingService.LoadSetting<PagarMeSettings>();
                        newSettings.Token = resposta.SelectToken("access_token").ToString();
                        newSettings.ServerKey = resposta.SelectToken("refresh_token").ToString();
                        _settingService.SaveSetting(newSettings);
                        return newSettings;
                    }
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}

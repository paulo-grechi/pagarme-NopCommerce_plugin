using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Payments.PagarMe.Models;
using Nop.Services.Payments;
using Nop.Services.Configuration;
using PagarmeApiSDK.Standard.Controllers;
using PagarmeApiSDK.Standard.Models;
using PagarmeApiSDK.Standard;
using System.Net.Http.Headers;
using System.Text;

namespace Nop.Plugin.Payments.PagarMe.Services
{
    public class PagarMeServices
    {
        public static string URLBase = "https://api.pagar.me/core/v5";

        private PagarMeSettings _settings;

        public string UrlPix;
        public string QRCode;

        public PagarMeServices(PagarMeSettings settings)
        {
            _settings = settings;
        }

        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentResult(CancelRecurringPaymentRequest cancelPaymentRequest) => null;
        public Task<CapturePaymentResult> CapturePayment(CapturePaymentRequest capturePaymentRequest) => null;

        public async Task<GetOrderResponse> CreateOrder(CreateOrderRequest payment)
        {
            try
            {
                var client = new PagarmeApiSDKClient.Builder().BasicAuthCredentials(_settings.SecKeySand, "").ServiceRefererName("ServiceRefererName").Build();
                IOrdersController orController = client.OrdersController;
                GetOrderResponse result = await orController.CreateOrderAsync(payment);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<GetOrderResponse> CreateOrderHttp(CreateOrderRequest payment)
        {
            try
            {
                var client = new HttpClient();
                string authCredentials = _settings.SecKeySand + ":";
                //string authCredentials = _settings.SecKeyProd + ":";
                byte[] data = Encoding.ASCII.GetBytes(authCredentials);
                var auth = Convert.ToBase64String(data);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.pagar.me/core/v5/orders"),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", "Basic " + auth },
                    },
                    Content = new StringContent(JsonConvert.SerializeObject(payment, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, MissingMemberHandling = MissingMemberHandling.Ignore }))
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    var obj = JObject.Parse(System.Text.Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result));
                    if (obj.SelectToken("message") != null)
                    {
                        throw new Exception(obj.SelectToken("message").ToString());
                    }
                    var responseJson = System.Text.Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result);
                    GetOrderResponse retorno = JsonConvert.DeserializeObject<GetOrderResponse>(responseJson);
                    return retorno;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<(string Url, string QRCode)> GeraPayloadPix(Customer cliente, double total)
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

        public async Task<GetOrderResponse> GetOrderPagarMe(string orderId)
        {
            try
            {
                var client = new HttpClient();
                string authCredentials = _settings.SecKeySand + ":";
                //string authCredentials = _settings.SecKeyProd + ":";
                byte[] data = Encoding.ASCII.GetBytes(authCredentials);
                var auth = Convert.ToBase64String(data);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.pagar.me/core/v5/orders/{orderId}"),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", "Basic " + auth },
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    var obj = JObject.Parse(System.Text.Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result));
                    if (obj.SelectToken("message") != null)
                    {
                        throw new Exception(obj.SelectToken("message").ToString());
                    }
                    var responseJson = System.Text.Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result);
                    GetOrderResponse retorno = JsonConvert.DeserializeObject<GetOrderResponse>(responseJson);
                    return retorno;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

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
        public string authCredentials;

        public PagarMeServices(PagarMeSettings settings)
        {
            _settings = settings;
            authCredentials = _settings.SecKeySand;
            //authCredentials = _settings.SecKeyProd;
        }

        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentResult(CancelRecurringPaymentRequest cancelPaymentRequest) => null;
        public Task<CapturePaymentResult> CapturePayment(CapturePaymentRequest capturePaymentRequest) => null;

        public async Task<GetOrderResponse> CreateOrder(CreateOrderRequest payment)
        {
            try
            {
                var client = new PagarmeApiSDKClient.Builder().BasicAuthCredentials(authCredentials, "").ServiceRefererName("ServiceRefererName").Build();
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
                byte[] data = Encoding.ASCII.GetBytes(authCredentials);
                var auth = Convert.ToBase64String(data);
                var body = JsonConvert.SerializeObject(payment, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.pagar.me/core/v5/orders"),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "authorization", "Basic " + auth },
                    },
                    Content = new StringContent(body)
//                    Content = new StringContent("{\"closed\":true,\"customer\":{\"name\":\"TonyStark\",\"type\":\"individual\",\"email\":\"avengerstark@ligadajustica.com.br\",\"document\":\"03154435026\",\"address\":{\"line_1\":\"7221,AvenidaDraRuthCardoso,Pinheiro\",\"line_2\":\"Prédio\",\"zip_code\":\"05425070\",\"city\":\"SãoPaulo\",\"state\":\"SP\",\"country\":\"BR\"},\"phones\":{\"home_phone\":{\"country_code\":\"55\",\"area_code\":\"11\",\"number\":\"999999999\"},\"mobile_phone\":{\"country_code\":\"55\",\"area_code\":\"11\",\"number\":\"999999999\"}}},\"items\":[{\"amount\":2990,\"description\":\"ChaveirodoTesseract\",\"quantity\":1,\"code\":123}],\"payments\":[{\"payment_method\":\"checkout\",\"checkout\":{\"expires_in\":120,\"default_payment_method\":\"credit_card\",\"customer_editable\":true,\"billing_address\":{\"line_1\":\"7221,AvenidaDraRuthCardoso,Pinheiro\",\"zip_code\":\"1600234\",\"city\":\"LISBOA\",\"state\":\"UU\",\"country\":\"PT\"},\"billing_address_editable\":true,\"skip_checkout_success_page\":false,\"accepted_payment_methods\":[\"credit_card\",\"boleto\",\"pix\"],\"accepted_brands\":[\"Visa\",\"Mastercard\",\"AmericanExpress\",\"Elo\",\"Hipercard\"],\"accepted_multi_payment_methods\":[[\"credit_card\",\"credit_card\"],[\"credit_card\",\"boleto\"]],\"success_url\":\"https://www.pagar.me\",\"credit_card\":{\"operation_type\":\"auth_and_capture\",\"statement_descriptor\":\"AVENGERS\",\"installments\":[{\"number\":1,\"total\":2990},{\"number\":2,\"total\":2990},{\"number\":3,\"total\":2990},{\"number\":4,\"total\":2990},{\"number\":5,\"total\":2990},{\"number\":6,\"total\":2990},{\"number\":7,\"total\":2990},{\"number\":8,\"total\":2990},{\"number\":9,\"total\":2990},{\"number\":10,\"total\":2990},{\"number\":11,\"total\":2990},{\"number\":12,\"total\":2990}]},\"boleto\":{\"instructions\":\"Sr.Caixa,favornãoaceitarpagamentoapósovencimento\",\"due_at\":\"2023-12-25T00:00:00Z\"},\"pix\":{\"expires_in\":\"3600\",\"additional_information\":[{\"name\":\"information\",\"value\":\"number\"}]}}}]}")
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

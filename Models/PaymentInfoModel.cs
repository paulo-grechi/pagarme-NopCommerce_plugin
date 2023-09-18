using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagarmeApiSDK.Standard.Models;

namespace Nop.Plugin.Payments.PagarMe.Models
{
    public class PaymentInfoModel
    {
        public string UrlPix { get; set; }
        public string UrlCheckout { get; set; }
        public string QRCode { get; set; }
        public string Errors { get; set; }
        public GetOrderResponse PagarMeOrder { get; set; }
        public string OrderId { get; set; }
    }
}

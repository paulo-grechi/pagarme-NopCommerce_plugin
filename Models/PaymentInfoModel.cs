using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.PagarMe.Models
{
    public class PaymentInfoModel
    {
        public string UrlPix { get; set; }
        public string UrlCheckout { get; set; }
        public string QRCode { get; set; }
        public string Errors { get; set; }
        public List<string> PayMethods { get; set; }

        public string SelectedMethod { get; set; }

        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public int ExpireMonth { get; set; }
        public long ExpireYear { get; set; }
        public string CardCode { get; set; }
        public int Installments { get; set; }
    }
}

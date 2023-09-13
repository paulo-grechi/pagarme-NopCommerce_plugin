using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.PagarMe
{
    internal class PagarMeSettings : ISettings
    {
        public string PubKeySand { get; set; }
        public string SecKeySand { get; set; }
        public string PubKeyProd { get; set; }
        public string SecKeyProd { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.PagarMe.Models
{
    public class ConfigPagarMe : ISettings
    {
        public string PubKeyProd { get; set; }
        public string PubKeySand { get; set; }
        public string SecKeyProd { get; set; }
        public string SecKeySand { get; set; }
    }
}

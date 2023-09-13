using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.PagarMe.Models
{
    public class PagarMeItem
    {
        public int Amount { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
    }
}

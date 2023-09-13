using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.PagarMe.Models
{
    public class OrderCreation
    {
        public string CustomerId { get; set; }
        public bool Closed { get; set; }
        public bool AntiFraud { get; set; } = true;
        public List<PagarMeItem> Items { get; set; }
        public PagarMeCustomer Customer { get; set; }

    }
}

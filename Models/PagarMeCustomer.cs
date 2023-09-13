using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.PagarMe.Models
{
    public class PagarMeCustomer
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Doc { get; set; }
        public string DocType { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset BirthDate { get; set; }

    }
}

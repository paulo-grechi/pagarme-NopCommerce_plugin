using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Payments.PagarMe.Models
{
    public class ConfigModel : ISettingsModel
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public bool IsConfigured { get; set; }
        public bool SetCredentialsManually { get; set; } = true;
        [NopResourceDisplayName("Public Key Production")]
        public string PubKeyProd { get; set; }
        [NopResourceDisplayName("Public Key Sandbox")]
        public string PubKeySand { get; set; }
        [NopResourceDisplayName("Secret Key Production")]
        public string SecKeyProd { get; set; }
        [NopResourceDisplayName("Secret Key Sandbox")]
        public string SecKeySand { get; set; }

        public bool DisplayButtonsOnShoppingCart { get; set; }
        public int ActiveStoreScopeConfiguration { get; set; }
        public bool SetCredentialsManually_OverrideForStore { get; set; }
        public bool AppId_OverrideForStore { get; set; }
        public bool Token_OverrideForStore { get; set; }
        public bool SecretKey_OverrideForStore { get; set; }
        public bool ServerKey_OverrideForStore { get; set; }
        public bool UserId_OverrideForStore { get; set; }
    }
}

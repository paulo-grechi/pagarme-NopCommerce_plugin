// <copyright file="CreateCardRequest.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
namespace PagarmeApiSDK.Standard.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using JsonSubTypes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using PagarmeApiSDK.Standard;
    using PagarmeApiSDK.Standard.Utilities;

    /// <summary>
    /// CreateCardRequest.
    /// </summary>
    public class CreateCardRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCardRequest"/> class.
        /// </summary>
        public CreateCardRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCardRequest"/> class.
        /// </summary>
        /// <param name="number">number.</param>
        /// <param name="holderName">holder_name.</param>
        /// <param name="expMonth">exp_month.</param>
        /// <param name="expYear">exp_year.</param>
        /// <param name="cvv">cvv.</param>
        /// <param name="billingAddress">billing_address.</param>
        /// <param name="brand">brand.</param>
        /// <param name="billingAddressId">billing_address_id.</param>
        /// <param name="metadata">metadata.</param>
        /// <param name="type">type.</param>
        /// <param name="options">options.</param>
        /// <param name="holderDocument">holder_document.</param>
        /// <param name="privateLabel">private_label.</param>
        /// <param name="label">label.</param>
        /// <param name="id">id.</param>
        /// <param name="token">token.</param>
        public CreateCardRequest(
            string number = null,
            string holderName = null,
            int? expMonth = null,
            int? expYear = null,
            string cvv = null,
            Models.CreateAddressRequest billingAddress = null,
            string brand = null,
            string billingAddressId = null,
            Dictionary<string, string> metadata = null,
            string type = "credit",
            Models.CreateCardOptionsRequest options = null,
            string holderDocument = null,
            bool? privateLabel = null,
            string label = null,
            string id = null,
            string token = null)
        {
            this.Number = number;
            this.HolderName = holderName;
            this.ExpMonth = expMonth;
            this.ExpYear = expYear;
            this.Cvv = cvv;
            this.BillingAddress = billingAddress;
            this.Brand = brand;
            this.BillingAddressId = billingAddressId;
            this.Metadata = metadata;
            this.Type = type;
            this.Options = options;
            this.HolderDocument = holderDocument;
            this.PrivateLabel = privateLabel;
            this.Label = label;
            this.Id = id;
            this.Token = token;
        }

        /// <summary>
        /// Credit card number
        /// </summary>
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

        /// <summary>
        /// Holder name, as written on the card
        /// </summary>
        [JsonProperty("holder_name", NullValueHandling = NullValueHandling.Ignore)]
        public string HolderName { get; set; }

        /// <summary>
        /// The expiration month
        /// </summary>
        [JsonProperty("exp_month", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpMonth { get; set; }

        /// <summary>
        /// The expiration year, that can be informed with 2 or 4 digits
        /// </summary>
        [JsonProperty("exp_year", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpYear { get; set; }

        /// <summary>
        /// The card's security code
        /// </summary>
        [JsonProperty("cvv", NullValueHandling = NullValueHandling.Ignore)]
        public string Cvv { get; set; }

        /// <summary>
        /// Card's billing address
        /// </summary>
        [JsonProperty("billing_address", NullValueHandling = NullValueHandling.Ignore)]
        public Models.CreateAddressRequest BillingAddress { get; set; }

        /// <summary>
        /// Card brand
        /// </summary>
        [JsonProperty("brand", NullValueHandling = NullValueHandling.Ignore)]
        public string Brand { get; set; }

        /// <summary>
        /// The address id for the billing address
        /// </summary>
        [JsonProperty("billing_address_id", NullValueHandling = NullValueHandling.Ignore)]
        public string BillingAddressId { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Card type
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Options for creating the card
        /// </summary>
        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public Models.CreateCardOptionsRequest Options { get; set; }

        /// <summary>
        /// Document number for the card's holder
        /// </summary>
        [JsonProperty("holder_document", NullValueHandling = NullValueHandling.Ignore)]
        public string HolderDocument { get; set; }

        /// <summary>
        /// Indicates whether it is a private label card
        /// </summary>
        [JsonProperty("private_label", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PrivateLabel { get; set; }

        /// <summary>
        /// Gets or sets Label.
        /// </summary>
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        /// <summary>
        /// Identifier
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// token identifier
        /// </summary>
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"CreateCardRequest : ({string.Join(", ", toStringOutput)})";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj == this)
            {
                return true;
            }
            return obj is CreateCardRequest other &&                ((this.Number == null && other.Number == null) || (this.Number?.Equals(other.Number) == true)) &&
                ((this.HolderName == null && other.HolderName == null) || (this.HolderName?.Equals(other.HolderName) == true)) &&
                ((this.ExpMonth == null && other.ExpMonth == null) || (this.ExpMonth?.Equals(other.ExpMonth) == true)) &&
                ((this.ExpYear == null && other.ExpYear == null) || (this.ExpYear?.Equals(other.ExpYear) == true)) &&
                ((this.Cvv == null && other.Cvv == null) || (this.Cvv?.Equals(other.Cvv) == true)) &&
                ((this.BillingAddress == null && other.BillingAddress == null) || (this.BillingAddress?.Equals(other.BillingAddress) == true)) &&
                ((this.Brand == null && other.Brand == null) || (this.Brand?.Equals(other.Brand) == true)) &&
                ((this.BillingAddressId == null && other.BillingAddressId == null) || (this.BillingAddressId?.Equals(other.BillingAddressId) == true)) &&
                ((this.Metadata == null && other.Metadata == null) || (this.Metadata?.Equals(other.Metadata) == true)) &&
                ((this.Type == null && other.Type == null) || (this.Type?.Equals(other.Type) == true)) &&
                ((this.Options == null && other.Options == null) || (this.Options?.Equals(other.Options) == true)) &&
                ((this.HolderDocument == null && other.HolderDocument == null) || (this.HolderDocument?.Equals(other.HolderDocument) == true)) &&
                ((this.PrivateLabel == null && other.PrivateLabel == null) || (this.PrivateLabel?.Equals(other.PrivateLabel) == true)) &&
                ((this.Label == null && other.Label == null) || (this.Label?.Equals(other.Label) == true)) &&
                ((this.Id == null && other.Id == null) || (this.Id?.Equals(other.Id) == true)) &&
                ((this.Token == null && other.Token == null) || (this.Token?.Equals(other.Token) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Number = {(this.Number == null ? "null" : this.Number == string.Empty ? "" : this.Number)}");
            toStringOutput.Add($"this.HolderName = {(this.HolderName == null ? "null" : this.HolderName == string.Empty ? "" : this.HolderName)}");
            toStringOutput.Add($"this.ExpMonth = {(this.ExpMonth == null ? "null" : this.ExpMonth.ToString())}");
            toStringOutput.Add($"this.ExpYear = {(this.ExpYear == null ? "null" : this.ExpYear.ToString())}");
            toStringOutput.Add($"this.Cvv = {(this.Cvv == null ? "null" : this.Cvv == string.Empty ? "" : this.Cvv)}");
            toStringOutput.Add($"this.BillingAddress = {(this.BillingAddress == null ? "null" : this.BillingAddress.ToString())}");
            toStringOutput.Add($"this.Brand = {(this.Brand == null ? "null" : this.Brand == string.Empty ? "" : this.Brand)}");
            toStringOutput.Add($"this.BillingAddressId = {(this.BillingAddressId == null ? "null" : this.BillingAddressId == string.Empty ? "" : this.BillingAddressId)}");
            toStringOutput.Add($"Metadata = {(this.Metadata == null ? "null" : this.Metadata.ToString())}");
            toStringOutput.Add($"this.Type = {(this.Type == null ? "null" : this.Type == string.Empty ? "" : this.Type)}");
            toStringOutput.Add($"this.Options = {(this.Options == null ? "null" : this.Options.ToString())}");
            toStringOutput.Add($"this.HolderDocument = {(this.HolderDocument == null ? "null" : this.HolderDocument == string.Empty ? "" : this.HolderDocument)}");
            toStringOutput.Add($"this.PrivateLabel = {(this.PrivateLabel == null ? "null" : this.PrivateLabel.ToString())}");
            toStringOutput.Add($"this.Label = {(this.Label == null ? "null" : this.Label == string.Empty ? "" : this.Label)}");
            toStringOutput.Add($"this.Id = {(this.Id == null ? "null" : this.Id == string.Empty ? "" : this.Id)}");
            toStringOutput.Add($"this.Token = {(this.Token == null ? "null" : this.Token == string.Empty ? "" : this.Token)}");
        }
    }
}
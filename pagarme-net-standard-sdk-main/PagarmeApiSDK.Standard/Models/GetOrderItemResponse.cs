// <copyright file="GetOrderItemResponse.cs" company="APIMatic">
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
    /// GetOrderItemResponse.
    /// </summary>
    public class GetOrderItemResponse
    {
        private string id;
        private int? amount;
        private string description;
        private int? quantity;
        private string category;
        private string code;
        private Dictionary<string, bool> shouldSerialize = new Dictionary<string, bool>
        {
            { "id", false },
            { "amount", false },
            { "description", false },
            { "quantity", false },
            { "category", false },
            { "code", false },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrderItemResponse"/> class.
        /// </summary>
        public GetOrderItemResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrderItemResponse"/> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="amount">amount.</param>
        /// <param name="description">description.</param>
        /// <param name="quantity">quantity.</param>
        /// <param name="category">category.</param>
        /// <param name="code">code.</param>
        public GetOrderItemResponse(
            string id = null,
            int? amount = null,
            string description = null,
            int? quantity = null,
            string category = null,
            string code = null)
        {
            if (id != null)
            {
                this.Id = id;
            }

            if (amount != null)
            {
                this.Amount = amount;
            }

            if (description != null)
            {
                this.Description = description;
            }

            if (quantity != null)
            {
                this.Quantity = quantity;
            }

            if (category != null)
            {
                this.Category = category;
            }

            if (code != null)
            {
                this.Code = code;
            }

        }

        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public string Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.shouldSerialize["id"] = true;
                this.id = value;
            }
        }

        /// <summary>
        /// Gets or sets Amount.
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                this.shouldSerialize["amount"] = true;
                this.amount = value;
            }
        }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        [JsonProperty("description")]
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.shouldSerialize["description"] = true;
                this.description = value;
            }
        }

        /// <summary>
        /// Gets or sets Quantity.
        /// </summary>
        [JsonProperty("quantity")]
        public int? Quantity
        {
            get
            {
                return this.quantity;
            }

            set
            {
                this.shouldSerialize["quantity"] = true;
                this.quantity = value;
            }
        }

        /// <summary>
        /// Category
        /// </summary>
        [JsonProperty("category")]
        public string Category
        {
            get
            {
                return this.category;
            }

            set
            {
                this.shouldSerialize["category"] = true;
                this.category = value;
            }
        }

        /// <summary>
        /// Code
        /// </summary>
        [JsonProperty("code")]
        public string Code
        {
            get
            {
                return this.code;
            }

            set
            {
                this.shouldSerialize["code"] = true;
                this.code = value;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"GetOrderItemResponse : ({string.Join(", ", toStringOutput)})";
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetId()
        {
            this.shouldSerialize["id"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetAmount()
        {
            this.shouldSerialize["amount"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetDescription()
        {
            this.shouldSerialize["description"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetQuantity()
        {
            this.shouldSerialize["quantity"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetCategory()
        {
            this.shouldSerialize["category"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetCode()
        {
            this.shouldSerialize["code"] = false;
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeId()
        {
            return this.shouldSerialize["id"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeAmount()
        {
            return this.shouldSerialize["amount"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeDescription()
        {
            return this.shouldSerialize["description"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeQuantity()
        {
            return this.shouldSerialize["quantity"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeCategory()
        {
            return this.shouldSerialize["category"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeCode()
        {
            return this.shouldSerialize["code"];
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
            return obj is GetOrderItemResponse other &&                ((this.Id == null && other.Id == null) || (this.Id?.Equals(other.Id) == true)) &&
                ((this.Amount == null && other.Amount == null) || (this.Amount?.Equals(other.Amount) == true)) &&
                ((this.Description == null && other.Description == null) || (this.Description?.Equals(other.Description) == true)) &&
                ((this.Quantity == null && other.Quantity == null) || (this.Quantity?.Equals(other.Quantity) == true)) &&
                ((this.Category == null && other.Category == null) || (this.Category?.Equals(other.Category) == true)) &&
                ((this.Code == null && other.Code == null) || (this.Code?.Equals(other.Code) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Id = {(this.Id == null ? "null" : this.Id == string.Empty ? "" : this.Id)}");
            toStringOutput.Add($"this.Amount = {(this.Amount == null ? "null" : this.Amount.ToString())}");
            toStringOutput.Add($"this.Description = {(this.Description == null ? "null" : this.Description == string.Empty ? "" : this.Description)}");
            toStringOutput.Add($"this.Quantity = {(this.Quantity == null ? "null" : this.Quantity.ToString())}");
            toStringOutput.Add($"this.Category = {(this.Category == null ? "null" : this.Category == string.Empty ? "" : this.Category)}");
            toStringOutput.Add($"this.Code = {(this.Code == null ? "null" : this.Code == string.Empty ? "" : this.Code)}");
        }
    }
}
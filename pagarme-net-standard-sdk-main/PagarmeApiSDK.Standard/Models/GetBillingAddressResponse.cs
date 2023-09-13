// <copyright file="GetBillingAddressResponse.cs" company="APIMatic">
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
    /// GetBillingAddressResponse.
    /// </summary>
    public class GetBillingAddressResponse
    {
        private string street;
        private string number;
        private string zipCode;
        private string neighborhood;
        private string city;
        private string state;
        private string country;
        private string complement;
        private string line1;
        private string line2;
        private Dictionary<string, bool> shouldSerialize = new Dictionary<string, bool>
        {
            { "street", false },
            { "number", false },
            { "zip_code", false },
            { "neighborhood", false },
            { "city", false },
            { "state", false },
            { "country", false },
            { "complement", false },
            { "line_1", false },
            { "line_2", false },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBillingAddressResponse"/> class.
        /// </summary>
        public GetBillingAddressResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBillingAddressResponse"/> class.
        /// </summary>
        /// <param name="street">street.</param>
        /// <param name="number">number.</param>
        /// <param name="zipCode">zip_code.</param>
        /// <param name="neighborhood">neighborhood.</param>
        /// <param name="city">city.</param>
        /// <param name="state">state.</param>
        /// <param name="country">country.</param>
        /// <param name="complement">complement.</param>
        /// <param name="line1">line_1.</param>
        /// <param name="line2">line_2.</param>
        public GetBillingAddressResponse(
            string street = null,
            string number = null,
            string zipCode = null,
            string neighborhood = null,
            string city = null,
            string state = null,
            string country = null,
            string complement = null,
            string line1 = null,
            string line2 = null)
        {
            if (street != null)
            {
                this.Street = street;
            }

            if (number != null)
            {
                this.Number = number;
            }

            if (zipCode != null)
            {
                this.ZipCode = zipCode;
            }

            if (neighborhood != null)
            {
                this.Neighborhood = neighborhood;
            }

            if (city != null)
            {
                this.City = city;
            }

            if (state != null)
            {
                this.State = state;
            }

            if (country != null)
            {
                this.Country = country;
            }

            if (complement != null)
            {
                this.Complement = complement;
            }

            if (line1 != null)
            {
                this.Line1 = line1;
            }

            if (line2 != null)
            {
                this.Line2 = line2;
            }

        }

        /// <summary>
        /// Gets or sets Street.
        /// </summary>
        [JsonProperty("street")]
        public string Street
        {
            get
            {
                return this.street;
            }

            set
            {
                this.shouldSerialize["street"] = true;
                this.street = value;
            }
        }

        /// <summary>
        /// Gets or sets Number.
        /// </summary>
        [JsonProperty("number")]
        public string Number
        {
            get
            {
                return this.number;
            }

            set
            {
                this.shouldSerialize["number"] = true;
                this.number = value;
            }
        }

        /// <summary>
        /// Gets or sets ZipCode.
        /// </summary>
        [JsonProperty("zip_code")]
        public string ZipCode
        {
            get
            {
                return this.zipCode;
            }

            set
            {
                this.shouldSerialize["zip_code"] = true;
                this.zipCode = value;
            }
        }

        /// <summary>
        /// Gets or sets Neighborhood.
        /// </summary>
        [JsonProperty("neighborhood")]
        public string Neighborhood
        {
            get
            {
                return this.neighborhood;
            }

            set
            {
                this.shouldSerialize["neighborhood"] = true;
                this.neighborhood = value;
            }
        }

        /// <summary>
        /// Gets or sets City.
        /// </summary>
        [JsonProperty("city")]
        public string City
        {
            get
            {
                return this.city;
            }

            set
            {
                this.shouldSerialize["city"] = true;
                this.city = value;
            }
        }

        /// <summary>
        /// Gets or sets State.
        /// </summary>
        [JsonProperty("state")]
        public string State
        {
            get
            {
                return this.state;
            }

            set
            {
                this.shouldSerialize["state"] = true;
                this.state = value;
            }
        }

        /// <summary>
        /// Gets or sets Country.
        /// </summary>
        [JsonProperty("country")]
        public string Country
        {
            get
            {
                return this.country;
            }

            set
            {
                this.shouldSerialize["country"] = true;
                this.country = value;
            }
        }

        /// <summary>
        /// Gets or sets Complement.
        /// </summary>
        [JsonProperty("complement")]
        public string Complement
        {
            get
            {
                return this.complement;
            }

            set
            {
                this.shouldSerialize["complement"] = true;
                this.complement = value;
            }
        }

        /// <summary>
        /// Line 1 for address
        /// </summary>
        [JsonProperty("line_1")]
        public string Line1
        {
            get
            {
                return this.line1;
            }

            set
            {
                this.shouldSerialize["line_1"] = true;
                this.line1 = value;
            }
        }

        /// <summary>
        /// Line 2 for address
        /// </summary>
        [JsonProperty("line_2")]
        public string Line2
        {
            get
            {
                return this.line2;
            }

            set
            {
                this.shouldSerialize["line_2"] = true;
                this.line2 = value;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"GetBillingAddressResponse : ({string.Join(", ", toStringOutput)})";
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetStreet()
        {
            this.shouldSerialize["street"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetNumber()
        {
            this.shouldSerialize["number"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetZipCode()
        {
            this.shouldSerialize["zip_code"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetNeighborhood()
        {
            this.shouldSerialize["neighborhood"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetCity()
        {
            this.shouldSerialize["city"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetState()
        {
            this.shouldSerialize["state"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetCountry()
        {
            this.shouldSerialize["country"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetComplement()
        {
            this.shouldSerialize["complement"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetLine1()
        {
            this.shouldSerialize["line_1"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetLine2()
        {
            this.shouldSerialize["line_2"] = false;
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeStreet()
        {
            return this.shouldSerialize["street"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeNumber()
        {
            return this.shouldSerialize["number"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeZipCode()
        {
            return this.shouldSerialize["zip_code"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeNeighborhood()
        {
            return this.shouldSerialize["neighborhood"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeCity()
        {
            return this.shouldSerialize["city"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeState()
        {
            return this.shouldSerialize["state"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeCountry()
        {
            return this.shouldSerialize["country"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeComplement()
        {
            return this.shouldSerialize["complement"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeLine1()
        {
            return this.shouldSerialize["line_1"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeLine2()
        {
            return this.shouldSerialize["line_2"];
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
            return obj is GetBillingAddressResponse other &&                ((this.Street == null && other.Street == null) || (this.Street?.Equals(other.Street) == true)) &&
                ((this.Number == null && other.Number == null) || (this.Number?.Equals(other.Number) == true)) &&
                ((this.ZipCode == null && other.ZipCode == null) || (this.ZipCode?.Equals(other.ZipCode) == true)) &&
                ((this.Neighborhood == null && other.Neighborhood == null) || (this.Neighborhood?.Equals(other.Neighborhood) == true)) &&
                ((this.City == null && other.City == null) || (this.City?.Equals(other.City) == true)) &&
                ((this.State == null && other.State == null) || (this.State?.Equals(other.State) == true)) &&
                ((this.Country == null && other.Country == null) || (this.Country?.Equals(other.Country) == true)) &&
                ((this.Complement == null && other.Complement == null) || (this.Complement?.Equals(other.Complement) == true)) &&
                ((this.Line1 == null && other.Line1 == null) || (this.Line1?.Equals(other.Line1) == true)) &&
                ((this.Line2 == null && other.Line2 == null) || (this.Line2?.Equals(other.Line2) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Street = {(this.Street == null ? "null" : this.Street == string.Empty ? "" : this.Street)}");
            toStringOutput.Add($"this.Number = {(this.Number == null ? "null" : this.Number == string.Empty ? "" : this.Number)}");
            toStringOutput.Add($"this.ZipCode = {(this.ZipCode == null ? "null" : this.ZipCode == string.Empty ? "" : this.ZipCode)}");
            toStringOutput.Add($"this.Neighborhood = {(this.Neighborhood == null ? "null" : this.Neighborhood == string.Empty ? "" : this.Neighborhood)}");
            toStringOutput.Add($"this.City = {(this.City == null ? "null" : this.City == string.Empty ? "" : this.City)}");
            toStringOutput.Add($"this.State = {(this.State == null ? "null" : this.State == string.Empty ? "" : this.State)}");
            toStringOutput.Add($"this.Country = {(this.Country == null ? "null" : this.Country == string.Empty ? "" : this.Country)}");
            toStringOutput.Add($"this.Complement = {(this.Complement == null ? "null" : this.Complement == string.Empty ? "" : this.Complement)}");
            toStringOutput.Add($"this.Line1 = {(this.Line1 == null ? "null" : this.Line1 == string.Empty ? "" : this.Line1)}");
            toStringOutput.Add($"this.Line2 = {(this.Line2 == null ? "null" : this.Line2 == string.Empty ? "" : this.Line2)}");
        }
    }
}
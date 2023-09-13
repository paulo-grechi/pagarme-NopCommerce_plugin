// <copyright file="GetCreditCardTransactionResponse.cs" company="APIMatic">
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
    /// GetCreditCardTransactionResponse.
    /// </summary>
    public class GetCreditCardTransactionResponse : GetTransactionResponse
    {
        private string statementDescriptor;
        private string acquirerAffiliationCode;
        private string acquirerAuthCode;
        private string operationType;
        private Models.GetCardResponse card;
        private string acquirerMessage;
        private string acquirerReturnCode;
        private int? installments;
        private string threedAuthenticationUrl;
        private string fundingSource;
        private Dictionary<string, bool> shouldSerialize = new Dictionary<string, bool>
        {
            { "statement_descriptor", false },
            { "acquirer_affiliation_code", false },
            { "acquirer_auth_code", false },
            { "operation_type", false },
            { "card", false },
            { "acquirer_message", false },
            { "acquirer_return_code", false },
            { "installments", false },
            { "threed_authentication_url", false },
            { "funding_source", false },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCreditCardTransactionResponse"/> class.
        /// </summary>
        public GetCreditCardTransactionResponse()
        {
            this.TransactionType = "credit_card";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCreditCardTransactionResponse"/> class.
        /// </summary>
        /// <param name="transactionType">transaction_type.</param>
        /// <param name="gatewayId">gateway_id.</param>
        /// <param name="amount">amount.</param>
        /// <param name="status">status.</param>
        /// <param name="success">success.</param>
        /// <param name="createdAt">created_at.</param>
        /// <param name="updatedAt">updated_at.</param>
        /// <param name="attemptCount">attempt_count.</param>
        /// <param name="maxAttempts">max_attempts.</param>
        /// <param name="splits">splits.</param>
        /// <param name="nextAttempt">next_attempt.</param>
        /// <param name="id">id.</param>
        /// <param name="gatewayResponse">gateway_response.</param>
        /// <param name="antifraudResponse">antifraud_response.</param>
        /// <param name="metadata">metadata.</param>
        /// <param name="split">split.</param>
        /// <param name="interest">interest.</param>
        /// <param name="fine">fine.</param>
        /// <param name="maxDaysToPayPastDue">max_days_to_pay_past_due.</param>
        /// <param name="statementDescriptor">statement_descriptor.</param>
        /// <param name="acquirerName">acquirer_name.</param>
        /// <param name="acquirerAffiliationCode">acquirer_affiliation_code.</param>
        /// <param name="acquirerTid">acquirer_tid.</param>
        /// <param name="acquirerNsu">acquirer_nsu.</param>
        /// <param name="acquirerAuthCode">acquirer_auth_code.</param>
        /// <param name="operationType">operation_type.</param>
        /// <param name="card">card.</param>
        /// <param name="acquirerMessage">acquirer_message.</param>
        /// <param name="acquirerReturnCode">acquirer_return_code.</param>
        /// <param name="installments">installments.</param>
        /// <param name="threedAuthenticationUrl">threed_authentication_url.</param>
        /// <param name="fundingSource">funding_source.</param>
        public GetCreditCardTransactionResponse(
            string transactionType = "credit_card",
            string gatewayId = null,
            int? amount = null,
            string status = null,
            bool? success = null,
            DateTime? createdAt = null,
            DateTime? updatedAt = null,
            int? attemptCount = null,
            int? maxAttempts = null,
            List<Models.GetSplitResponse> splits = null,
            DateTime? nextAttempt = null,
            string id = null,
            Models.GetGatewayResponseResponse gatewayResponse = null,
            Models.GetAntifraudResponse antifraudResponse = null,
            Dictionary<string, string> metadata = null,
            List<Models.GetSplitResponse> split = null,
            Models.GetInterestResponse interest = null,
            Models.GetFineResponse fine = null,
            int? maxDaysToPayPastDue = null,
            string statementDescriptor = null,
            string acquirerName = null,
            string acquirerAffiliationCode = null,
            string acquirerTid = null,
            string acquirerNsu = null,
            string acquirerAuthCode = null,
            string operationType = null,
            Models.GetCardResponse card = null,
            string acquirerMessage = null,
            string acquirerReturnCode = null,
            int? installments = null,
            string threedAuthenticationUrl = null,
            string fundingSource = null)
            : base(
                transactionType,
                gatewayId,
                amount,
                status,
                success,
                createdAt,
                updatedAt,
                attemptCount,
                maxAttempts,
                splits,
                nextAttempt,
                id,
                gatewayResponse,
                antifraudResponse,
                metadata,
                split,
                interest,
                fine,
                maxDaysToPayPastDue)
        {
            if (statementDescriptor != null)
            {
                this.StatementDescriptor = statementDescriptor;
            }

            this.AcquirerName = acquirerName;
            if (acquirerAffiliationCode != null)
            {
                this.AcquirerAffiliationCode = acquirerAffiliationCode;
            }

            this.AcquirerTid = acquirerTid;
            this.AcquirerNsu = acquirerNsu;
            if (acquirerAuthCode != null)
            {
                this.AcquirerAuthCode = acquirerAuthCode;
            }

            if (operationType != null)
            {
                this.OperationType = operationType;
            }

            if (card != null)
            {
                this.Card = card;
            }

            if (acquirerMessage != null)
            {
                this.AcquirerMessage = acquirerMessage;
            }

            if (acquirerReturnCode != null)
            {
                this.AcquirerReturnCode = acquirerReturnCode;
            }

            if (installments != null)
            {
                this.Installments = installments;
            }

            if (threedAuthenticationUrl != null)
            {
                this.ThreedAuthenticationUrl = threedAuthenticationUrl;
            }

            if (fundingSource != null)
            {
                this.FundingSource = fundingSource;
            }

        }

        /// <summary>
        /// Text that will appear on the credit card's statement
        /// </summary>
        [JsonProperty("statement_descriptor")]
        public string StatementDescriptor
        {
            get
            {
                return this.statementDescriptor;
            }

            set
            {
                this.shouldSerialize["statement_descriptor"] = true;
                this.statementDescriptor = value;
            }
        }

        /// <summary>
        /// Acquirer name
        /// </summary>
        [JsonProperty("acquirer_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AcquirerName { get; set; }

        /// <summary>
        /// Aquirer affiliation code
        /// </summary>
        [JsonProperty("acquirer_affiliation_code")]
        public string AcquirerAffiliationCode
        {
            get
            {
                return this.acquirerAffiliationCode;
            }

            set
            {
                this.shouldSerialize["acquirer_affiliation_code"] = true;
                this.acquirerAffiliationCode = value;
            }
        }

        /// <summary>
        /// Acquirer TID
        /// </summary>
        [JsonProperty("acquirer_tid", NullValueHandling = NullValueHandling.Ignore)]
        public string AcquirerTid { get; set; }

        /// <summary>
        /// Acquirer NSU
        /// </summary>
        [JsonProperty("acquirer_nsu", NullValueHandling = NullValueHandling.Ignore)]
        public string AcquirerNsu { get; set; }

        /// <summary>
        /// Acquirer authorization code
        /// </summary>
        [JsonProperty("acquirer_auth_code")]
        public string AcquirerAuthCode
        {
            get
            {
                return this.acquirerAuthCode;
            }

            set
            {
                this.shouldSerialize["acquirer_auth_code"] = true;
                this.acquirerAuthCode = value;
            }
        }

        /// <summary>
        /// Operation type
        /// </summary>
        [JsonProperty("operation_type")]
        public string OperationType
        {
            get
            {
                return this.operationType;
            }

            set
            {
                this.shouldSerialize["operation_type"] = true;
                this.operationType = value;
            }
        }

        /// <summary>
        /// Card data
        /// </summary>
        [JsonProperty("card")]
        public Models.GetCardResponse Card
        {
            get
            {
                return this.card;
            }

            set
            {
                this.shouldSerialize["card"] = true;
                this.card = value;
            }
        }

        /// <summary>
        /// Acquirer message
        /// </summary>
        [JsonProperty("acquirer_message")]
        public string AcquirerMessage
        {
            get
            {
                return this.acquirerMessage;
            }

            set
            {
                this.shouldSerialize["acquirer_message"] = true;
                this.acquirerMessage = value;
            }
        }

        /// <summary>
        /// Acquirer Return Code
        /// </summary>
        [JsonProperty("acquirer_return_code")]
        public string AcquirerReturnCode
        {
            get
            {
                return this.acquirerReturnCode;
            }

            set
            {
                this.shouldSerialize["acquirer_return_code"] = true;
                this.acquirerReturnCode = value;
            }
        }

        /// <summary>
        /// Number of installments
        /// </summary>
        [JsonProperty("installments")]
        public int? Installments
        {
            get
            {
                return this.installments;
            }

            set
            {
                this.shouldSerialize["installments"] = true;
                this.installments = value;
            }
        }

        /// <summary>
        /// 3D-S authentication Url
        /// </summary>
        [JsonProperty("threed_authentication_url")]
        public string ThreedAuthenticationUrl
        {
            get
            {
                return this.threedAuthenticationUrl;
            }

            set
            {
                this.shouldSerialize["threed_authentication_url"] = true;
                this.threedAuthenticationUrl = value;
            }
        }

        /// <summary>
        /// Identify when a card is prepaid, credit or debit.
        /// </summary>
        [JsonProperty("funding_source")]
        public string FundingSource
        {
            get
            {
                return this.fundingSource;
            }

            set
            {
                this.shouldSerialize["funding_source"] = true;
                this.fundingSource = value;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"GetCreditCardTransactionResponse : ({string.Join(", ", toStringOutput)})";
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetStatementDescriptor()
        {
            this.shouldSerialize["statement_descriptor"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetAcquirerAffiliationCode()
        {
            this.shouldSerialize["acquirer_affiliation_code"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetAcquirerAuthCode()
        {
            this.shouldSerialize["acquirer_auth_code"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetOperationType()
        {
            this.shouldSerialize["operation_type"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetCard()
        {
            this.shouldSerialize["card"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetAcquirerMessage()
        {
            this.shouldSerialize["acquirer_message"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetAcquirerReturnCode()
        {
            this.shouldSerialize["acquirer_return_code"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetInstallments()
        {
            this.shouldSerialize["installments"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetThreedAuthenticationUrl()
        {
            this.shouldSerialize["threed_authentication_url"] = false;
        }

        /// <summary>
        /// Marks the field to not be serailized.
        /// </summary>
        public void UnsetFundingSource()
        {
            this.shouldSerialize["funding_source"] = false;
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeStatementDescriptor()
        {
            return this.shouldSerialize["statement_descriptor"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeAcquirerAffiliationCode()
        {
            return this.shouldSerialize["acquirer_affiliation_code"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeAcquirerAuthCode()
        {
            return this.shouldSerialize["acquirer_auth_code"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeOperationType()
        {
            return this.shouldSerialize["operation_type"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeCard()
        {
            return this.shouldSerialize["card"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeAcquirerMessage()
        {
            return this.shouldSerialize["acquirer_message"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeAcquirerReturnCode()
        {
            return this.shouldSerialize["acquirer_return_code"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeInstallments()
        {
            return this.shouldSerialize["installments"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeThreedAuthenticationUrl()
        {
            return this.shouldSerialize["threed_authentication_url"];
        }

        /// <summary>
        /// Checks if the field should be serialized or not.
        /// </summary>
        /// <returns>A boolean weather the field should be serialized or not.</returns>
        public bool ShouldSerializeFundingSource()
        {
            return this.shouldSerialize["funding_source"];
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
            return obj is GetCreditCardTransactionResponse other &&                ((this.StatementDescriptor == null && other.StatementDescriptor == null) || (this.StatementDescriptor?.Equals(other.StatementDescriptor) == true)) &&
                ((this.AcquirerName == null && other.AcquirerName == null) || (this.AcquirerName?.Equals(other.AcquirerName) == true)) &&
                ((this.AcquirerAffiliationCode == null && other.AcquirerAffiliationCode == null) || (this.AcquirerAffiliationCode?.Equals(other.AcquirerAffiliationCode) == true)) &&
                ((this.AcquirerTid == null && other.AcquirerTid == null) || (this.AcquirerTid?.Equals(other.AcquirerTid) == true)) &&
                ((this.AcquirerNsu == null && other.AcquirerNsu == null) || (this.AcquirerNsu?.Equals(other.AcquirerNsu) == true)) &&
                ((this.AcquirerAuthCode == null && other.AcquirerAuthCode == null) || (this.AcquirerAuthCode?.Equals(other.AcquirerAuthCode) == true)) &&
                ((this.OperationType == null && other.OperationType == null) || (this.OperationType?.Equals(other.OperationType) == true)) &&
                ((this.Card == null && other.Card == null) || (this.Card?.Equals(other.Card) == true)) &&
                ((this.AcquirerMessage == null && other.AcquirerMessage == null) || (this.AcquirerMessage?.Equals(other.AcquirerMessage) == true)) &&
                ((this.AcquirerReturnCode == null && other.AcquirerReturnCode == null) || (this.AcquirerReturnCode?.Equals(other.AcquirerReturnCode) == true)) &&
                ((this.Installments == null && other.Installments == null) || (this.Installments?.Equals(other.Installments) == true)) &&
                ((this.ThreedAuthenticationUrl == null && other.ThreedAuthenticationUrl == null) || (this.ThreedAuthenticationUrl?.Equals(other.ThreedAuthenticationUrl) == true)) &&
                ((this.FundingSource == null && other.FundingSource == null) || (this.FundingSource?.Equals(other.FundingSource) == true)) &&
                base.Equals(obj);
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected new void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.StatementDescriptor = {(this.StatementDescriptor == null ? "null" : this.StatementDescriptor == string.Empty ? "" : this.StatementDescriptor)}");
            toStringOutput.Add($"this.AcquirerName = {(this.AcquirerName == null ? "null" : this.AcquirerName == string.Empty ? "" : this.AcquirerName)}");
            toStringOutput.Add($"this.AcquirerAffiliationCode = {(this.AcquirerAffiliationCode == null ? "null" : this.AcquirerAffiliationCode == string.Empty ? "" : this.AcquirerAffiliationCode)}");
            toStringOutput.Add($"this.AcquirerTid = {(this.AcquirerTid == null ? "null" : this.AcquirerTid == string.Empty ? "" : this.AcquirerTid)}");
            toStringOutput.Add($"this.AcquirerNsu = {(this.AcquirerNsu == null ? "null" : this.AcquirerNsu == string.Empty ? "" : this.AcquirerNsu)}");
            toStringOutput.Add($"this.AcquirerAuthCode = {(this.AcquirerAuthCode == null ? "null" : this.AcquirerAuthCode == string.Empty ? "" : this.AcquirerAuthCode)}");
            toStringOutput.Add($"this.OperationType = {(this.OperationType == null ? "null" : this.OperationType == string.Empty ? "" : this.OperationType)}");
            toStringOutput.Add($"this.Card = {(this.Card == null ? "null" : this.Card.ToString())}");
            toStringOutput.Add($"this.AcquirerMessage = {(this.AcquirerMessage == null ? "null" : this.AcquirerMessage == string.Empty ? "" : this.AcquirerMessage)}");
            toStringOutput.Add($"this.AcquirerReturnCode = {(this.AcquirerReturnCode == null ? "null" : this.AcquirerReturnCode == string.Empty ? "" : this.AcquirerReturnCode)}");
            toStringOutput.Add($"this.Installments = {(this.Installments == null ? "null" : this.Installments.ToString())}");
            toStringOutput.Add($"this.ThreedAuthenticationUrl = {(this.ThreedAuthenticationUrl == null ? "null" : this.ThreedAuthenticationUrl == string.Empty ? "" : this.ThreedAuthenticationUrl)}");
            toStringOutput.Add($"this.FundingSource = {(this.FundingSource == null ? "null" : this.FundingSource == string.Empty ? "" : this.FundingSource)}");

            base.ToString(toStringOutput);
        }
    }
}
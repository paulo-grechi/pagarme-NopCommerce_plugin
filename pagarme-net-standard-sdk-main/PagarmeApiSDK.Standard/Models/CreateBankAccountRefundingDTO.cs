// <copyright file="CreateBankAccountRefundingDTO.cs" company="APIMatic">
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
    /// CreateBankAccountRefundingDTO.
    /// </summary>
    public class CreateBankAccountRefundingDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBankAccountRefundingDTO"/> class.
        /// </summary>
        public CreateBankAccountRefundingDTO()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBankAccountRefundingDTO"/> class.
        /// </summary>
        /// <param name="holderName">holder_name.</param>
        /// <param name="holderType">holder_type.</param>
        /// <param name="holderDocument">holder_document.</param>
        /// <param name="bank">bank.</param>
        /// <param name="branchNumber">branch_number.</param>
        /// <param name="branchCheckDigit">branch_check_digit.</param>
        /// <param name="accountNumber">account_number.</param>
        /// <param name="accountCheckDigit">account_check_digit.</param>
        /// <param name="type">type.</param>
        public CreateBankAccountRefundingDTO(
            string holderName,
            string holderType,
            string holderDocument,
            string bank,
            string branchNumber,
            string branchCheckDigit,
            string accountNumber,
            string accountCheckDigit,
            string type)
        {
            this.HolderName = holderName;
            this.HolderType = holderType;
            this.HolderDocument = holderDocument;
            this.Bank = bank;
            this.BranchNumber = branchNumber;
            this.BranchCheckDigit = branchCheckDigit;
            this.AccountNumber = accountNumber;
            this.AccountCheckDigit = accountCheckDigit;
            this.Type = type;
        }

        /// <summary>
        /// Nome/razão social do favorecido
        /// </summary>
        [JsonProperty("holder_name")]
        public string HolderName { get; set; }

        /// <summary>
        /// Tipo de titular (pessoa física ou jurídica)
        /// </summary>
        [JsonProperty("holder_type")]
        public string HolderType { get; set; }

        /// <summary>
        /// CPF ou CNPJ do favorecido
        /// </summary>
        [JsonProperty("holder_document")]
        public string HolderDocument { get; set; }

        /// <summary>
        /// Dígitos que identificam cada banco.
        /// </summary>
        [JsonProperty("bank")]
        public string Bank { get; set; }

        /// <summary>
        /// Número da agência bancária
        /// </summary>
        [JsonProperty("branch_number")]
        public string BranchNumber { get; set; }

        /// <summary>
        /// Dígito da agência bancária
        /// </summary>
        [JsonProperty("branch_check_digit")]
        public string BranchCheckDigit { get; set; }

        /// <summary>
        /// Número da conta
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Dígito verificador da conta
        /// </summary>
        [JsonProperty("account_check_digit")]
        public string AccountCheckDigit { get; set; }

        /// <summary>
        /// Tipo de conta
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"CreateBankAccountRefundingDTO : ({string.Join(", ", toStringOutput)})";
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
            return obj is CreateBankAccountRefundingDTO other &&                ((this.HolderName == null && other.HolderName == null) || (this.HolderName?.Equals(other.HolderName) == true)) &&
                ((this.HolderType == null && other.HolderType == null) || (this.HolderType?.Equals(other.HolderType) == true)) &&
                ((this.HolderDocument == null && other.HolderDocument == null) || (this.HolderDocument?.Equals(other.HolderDocument) == true)) &&
                ((this.Bank == null && other.Bank == null) || (this.Bank?.Equals(other.Bank) == true)) &&
                ((this.BranchNumber == null && other.BranchNumber == null) || (this.BranchNumber?.Equals(other.BranchNumber) == true)) &&
                ((this.BranchCheckDigit == null && other.BranchCheckDigit == null) || (this.BranchCheckDigit?.Equals(other.BranchCheckDigit) == true)) &&
                ((this.AccountNumber == null && other.AccountNumber == null) || (this.AccountNumber?.Equals(other.AccountNumber) == true)) &&
                ((this.AccountCheckDigit == null && other.AccountCheckDigit == null) || (this.AccountCheckDigit?.Equals(other.AccountCheckDigit) == true)) &&
                ((this.Type == null && other.Type == null) || (this.Type?.Equals(other.Type) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.HolderName = {(this.HolderName == null ? "null" : this.HolderName == string.Empty ? "" : this.HolderName)}");
            toStringOutput.Add($"this.HolderType = {(this.HolderType == null ? "null" : this.HolderType == string.Empty ? "" : this.HolderType)}");
            toStringOutput.Add($"this.HolderDocument = {(this.HolderDocument == null ? "null" : this.HolderDocument == string.Empty ? "" : this.HolderDocument)}");
            toStringOutput.Add($"this.Bank = {(this.Bank == null ? "null" : this.Bank == string.Empty ? "" : this.Bank)}");
            toStringOutput.Add($"this.BranchNumber = {(this.BranchNumber == null ? "null" : this.BranchNumber == string.Empty ? "" : this.BranchNumber)}");
            toStringOutput.Add($"this.BranchCheckDigit = {(this.BranchCheckDigit == null ? "null" : this.BranchCheckDigit == string.Empty ? "" : this.BranchCheckDigit)}");
            toStringOutput.Add($"this.AccountNumber = {(this.AccountNumber == null ? "null" : this.AccountNumber == string.Empty ? "" : this.AccountNumber)}");
            toStringOutput.Add($"this.AccountCheckDigit = {(this.AccountCheckDigit == null ? "null" : this.AccountCheckDigit == string.Empty ? "" : this.AccountCheckDigit)}");
            toStringOutput.Add($"this.Type = {(this.Type == null ? "null" : this.Type == string.Empty ? "" : this.Type)}");
        }
    }
}
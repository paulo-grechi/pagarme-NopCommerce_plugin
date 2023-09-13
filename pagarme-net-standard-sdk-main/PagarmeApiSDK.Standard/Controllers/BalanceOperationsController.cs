﻿// <copyright file="BalanceOperationsController.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
namespace PagarmeApiSDK.Standard.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using APIMatic.Core;
    using APIMatic.Core.Types;
    using APIMatic.Core.Utilities;
    using APIMatic.Core.Utilities.Date.Xml;
    using Newtonsoft.Json.Converters;
    using PagarmeApiSDK.Standard;
    using PagarmeApiSDK.Standard.Authentication;
    using PagarmeApiSDK.Standard.Http.Client;
    using PagarmeApiSDK.Standard.Utilities;
    using System.Net.Http;

    /// <summary>
    /// BalanceOperationsController.
    /// </summary>
    public class BalanceOperationsController : BaseController, IBalanceOperationsController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceOperationsController"/> class.
        /// </summary>
        public BalanceOperationsController(GlobalConfiguration globalConfiguration) : base(globalConfiguration) { }

        /// <summary>
        /// GetBalanceOperations EndPoint.
        /// </summary>
        /// <param name="status">Optional parameter: Example: .</param>
        /// <param name="createdSince">Optional parameter: Example: .</param>
        /// <param name="createdUntil">Optional parameter: Example: .</param>
        /// <returns>Returns the Models.ListBalanceOperationResponse response from the API call.</returns>
        public Models.ListBalanceOperationResponse GetBalanceOperations(
                string status = null,
                DateTime? createdSince = null,
                DateTime? createdUntil = null)
            => CoreHelper.RunTask(GetBalanceOperationsAsync(status, createdSince, createdUntil));

        /// <summary>
        /// GetBalanceOperations EndPoint.
        /// </summary>
        /// <param name="status">Optional parameter: Example: .</param>
        /// <param name="createdSince">Optional parameter: Example: .</param>
        /// <param name="createdUntil">Optional parameter: Example: .</param>
        /// <param name="cancellationToken"> cancellationToken. </param>
        /// <returns>Returns the Models.ListBalanceOperationResponse response from the API call.</returns>
        public async Task<Models.ListBalanceOperationResponse> GetBalanceOperationsAsync(
                string status = null,
                DateTime? createdSince = null,
                DateTime? createdUntil = null,
                CancellationToken cancellationToken = default)
            => await CreateApiCall<Models.ListBalanceOperationResponse>()
              .RequestBuilder(_requestBuilder => _requestBuilder
                  .Setup(HttpMethod.Get, "/balance/operations")
                  .WithAuth("global")
                  .Parameters(_parameters => _parameters
                      .Query(_query => _query.Setup("status", status))
                      .Query(_query => _query.Setup("created_since", createdSince.HasValue ? createdSince.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK") : null))
                      .Query(_query => _query.Setup("created_until", createdUntil.HasValue ? createdUntil.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK") : null))))
              .ResponseHandler(_responseHandler => _responseHandler
                  .Deserializer(_response => ApiHelper.JsonDeserialize<Models.ListBalanceOperationResponse>(_response)))
              .ExecuteAsync(cancellationToken);

        /// <summary>
        /// GetBalanceOperationById EndPoint.
        /// </summary>
        /// <param name="id">Required parameter: Example: .</param>
        /// <returns>Returns the Models.GetBalanceOperationResponse response from the API call.</returns>
        public Models.GetBalanceOperationResponse GetBalanceOperationById(
                long id)
            => CoreHelper.RunTask(GetBalanceOperationByIdAsync(id));

        /// <summary>
        /// GetBalanceOperationById EndPoint.
        /// </summary>
        /// <param name="id">Required parameter: Example: .</param>
        /// <param name="cancellationToken"> cancellationToken. </param>
        /// <returns>Returns the Models.GetBalanceOperationResponse response from the API call.</returns>
        public async Task<Models.GetBalanceOperationResponse> GetBalanceOperationByIdAsync(
                long id,
                CancellationToken cancellationToken = default)
            => await CreateApiCall<Models.GetBalanceOperationResponse>()
              .RequestBuilder(_requestBuilder => _requestBuilder
                  .Setup(HttpMethod.Get, "/balance/operations/{id}")
                  .WithAuth("global")
                  .Parameters(_parameters => _parameters
                      .Template(_template => _template.Setup("id", id))))
              .ResponseHandler(_responseHandler => _responseHandler
                  .Deserializer(_response => ApiHelper.JsonDeserialize<Models.GetBalanceOperationResponse>(_response)))
              .ExecuteAsync(cancellationToken);
    }
}
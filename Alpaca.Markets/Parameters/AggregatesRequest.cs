﻿using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="PolygonDataClient.ListAggregatesAsync(AggregatesRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class AggregatesRequest
    {
        /// <summary>
        /// Creates new instance of <see cref="AggregatesRequest"/> object.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="period">Aggregation time span (number or bars and base bar size).</param>
        public AggregatesRequest(
            String symbol,
            AggregationPeriod period)
        {
            Symbol = symbol;
            Period = period;
        }

        /// <summary>
        /// Gets asset name for data retrieval.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Gets aggregation time span (number or bars and base bar size).
        /// </summary>
        public AggregationPeriod Period { get; }

        /// <summary>
        /// Gets start time for filtering (inclusive).
        /// </summary>
        public DateTime DateFrom { get; private set; }

        /// <summary>
        /// Gets end time for filtering (inclusive).
        /// </summary>
        public DateTime DateInto { get; private set; }

        /// <summary>
        /// Gets or sets flag indicated that the results should not be adjusted for splits.
        /// </summary>
        public Boolean Unadjusted { get; set; }
        
        /// <summary>
        /// Sets inclusive time interval for request.
        /// </summary>
        /// <param name="dateFrom">Filtering interval start time.</param>
        /// <param name="dateInto">Filtering interval end time.</param>
        /// <returns>Fluent interface method return same <see cref="AggregatesRequest"/> instance.</returns>
        public AggregatesRequest SetInclusiveTimeInterval(
            DateTime dateFrom,
            DateTime dateInto)
        {
            DateFrom = dateFrom;
            DateInto = dateInto;
            return this;
        }

        /// <summary>
        /// Gets all validation exceptions (inconsistent request data errors).
        /// </summary>
        /// <returns>Lazy-evaluated list of validation errors.</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public IEnumerable<RequestValidationException> GetExceptions()
        {
            if (String.IsNullOrEmpty(Symbol))
            {
                yield return new RequestValidationException(
                    "Symbols shouldn't be empty.", nameof(Symbol));
            }

            if (DateFrom == default)
            {
                yield return new RequestValidationException(
                    "Time interval start should be specified.", nameof(DateFrom));
            }

            if (DateInto == default)
            {
                yield return new RequestValidationException(
                    "Time interval end should be specified.", nameof(DateInto));
            }

            if (DateFrom > DateInto)
            {
                yield return new RequestValidationException(
                    "Time interval should be valid.", nameof(DateFrom));
                yield return new RequestValidationException(
                    "Time interval should be valid.", nameof(DateInto));
            }
        }

        internal void Validate()
        {
            var exception = new AggregateException(GetExceptions());
            if (exception.InnerExceptions.Count != 0)
            {
                throw exception;
            }
        }
    }
}
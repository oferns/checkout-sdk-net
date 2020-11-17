using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Checkout
{
    /// <summary>
    /// Base class for HTTP response messages received by the Checkout.com SDK for .NET.
    /// </summary>
    public class CheckoutHttpResponseMessage<TContent> : HttpResponseMessage
    {
        /// <summary>
        /// Creates a new <see cref="CheckoutHttpResponseMessage{TContent}"/> instance with the provided HTTP status code, headers and content.
        /// </summary>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> of the API response.</param>
        /// <param name="headers">The <see cref="HttpResponseHeaders"/> of the API response.</param>
        /// <param name="content">The deserialized <see cref="TContent"/> of the API response.</param>
        public CheckoutHttpResponseMessage(HttpStatusCode statusCode, HttpResponseHeaders headers, TContent content)
            : this(statusCode, content)
        {
            foreach(var header in headers)
            {
                SetHeader(header.Key, header.Value.FirstOrDefault());
            }
            VerifyHeaders();
        }

        /// <summary>
        /// Creates a new <see cref="CheckoutHttpResponseMessage{TContent}"/> instance with the provided HTTP status code and content.
        /// </summary>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> of the API response.</param>
        /// <param name="content">The deserialized <see cref="TContent"/> of the API response.</param>
        public CheckoutHttpResponseMessage(HttpStatusCode statusCode, TContent content)
            : this(statusCode)
        {
            Content = content;
        }

        /// <summary>
        /// Creates a new <see cref="CheckoutHttpResponseMessage{TContent}"/> instance with the provided HTTP status code.
        /// </summary>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> of the API response.</param>
        public CheckoutHttpResponseMessage(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }

        /// <summary>
        /// Is the deserialized content of the API response.
        /// </summary>
        public new TContent Content { get; private set; }

        /// <summary>
        /// Is the unique ID of the request generated by the Checkout.com Gateway.
        /// </summary>
        public string CkoRequestId { get; private set; }

        /// <summary>
        /// Is the version of the Checkout.com Gateway.
        /// </summary>
        public string CkoVersion { get; private set; }

        public void SetHeader(string key, string value)
        {
            Headers.Add(key, value);
            if(key == "Cko-Request-Id") CkoRequestId = value;
            if(key == "Cko-Version") CkoVersion = value;
        }

        private void VerifyHeaders()
        {
            if (!Headers.TryGetValues("Cko-Request-Id", out var _)) throw new KeyNotFoundException("Key \"Cko-Request-Id\" was not present in the HTTP response header.");
            if (!Headers.TryGetValues("Cko-Version", out var _)) throw new KeyNotFoundException("Key \"Cko-Version\" was not present in the HTTP response header.");
        }
    }
}

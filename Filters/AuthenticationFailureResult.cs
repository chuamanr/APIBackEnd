using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EcoOplacementApi.Filters.Security
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        #region Propiedades
             public string reasonPhrase { get; }

            public HttpRequestMessage request { get; }
        #endregion

        /// <summary>
        /// Constructor de Clase
        /// </summary>
        /// <param name="reasonPhrase"></param>
        /// <param name="request"></param>
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            this.reasonPhrase = reasonPhrase;
            this.request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            try {

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    RequestMessage = request,
                    ReasonPhrase = reasonPhrase
                };

                return response;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }
    }
}
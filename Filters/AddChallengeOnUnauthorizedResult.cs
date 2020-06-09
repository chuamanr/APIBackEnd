using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EcoOplacementApi.Filters.Security
{
    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        #region Properties
        public AuthenticationHeaderValue challenge { get; }

        public IHttpActionResult innerResult { get; }
        #endregion

        /// <summary>
        /// Constructor de Clase
        /// </summary>
        /// <param name="challenge"></param>
        /// <param name="innerResult"></param>
        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            this.challenge = challenge;
            this.innerResult = innerResult;
        }
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                HttpResponseMessage response = await this.innerResult.ExecuteAsync(cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Only add one challenge per authentication scheme.
                    if (response.Headers.WwwAuthenticate.All(h => h.Scheme != this.challenge.Scheme))
                    {
                        response.Headers.WwwAuthenticate.Add(this.challenge);
                    }
                }

                return response;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
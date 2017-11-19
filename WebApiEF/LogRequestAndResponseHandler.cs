using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace WebApiEF
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestBody = await request.Content.ReadAsStringAsync();
            Log.Debug(requestBody);

            var result = await base.SendAsync(request, cancellationToken);

            if (result.Content != null)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                Log.Debug(responseBody);
            }

            return result;
        }
    }
}

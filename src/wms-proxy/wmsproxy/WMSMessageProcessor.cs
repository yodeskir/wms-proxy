using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WMSAuthentication.Interfaces;

namespace WMSAuthentication.WMSProxy
{
    public class WMSMessageProcessor : IWMSMessageProcessor
    {
        public async Task PreProcess(HttpContext context, HttpResponseMessage responseMessage)
        {
            context.Response.StatusCode = (int)responseMessage.StatusCode;
            
            //removed because the responseMessage.Content.CopyToAsync(context.Response.Body) replace context.Response.ContentType value;
            //context.Response.ContentType = responseMessage.Content?.Headers.ContentType?.MediaType;

            foreach (var header in responseMessage.Headers)
                context.Response.Headers[header.Key] = header.Value.ToArray();

            context.Response.Headers.Remove("transfer-encoding");

            if (responseMessage.Content != null)
            {
                foreach (var contentHeader in responseMessage.Content.Headers)
                    context.Response.Headers[contentHeader.Key] = contentHeader.Value.ToArray();
                await responseMessage.Content.CopyToAsync(context.Response.Body);
            }
        }
    }
}
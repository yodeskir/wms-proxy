using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WMSAuthentication.Interfaces
{
    public interface IWMSMessageProcessor
    {
        Task PreProcess(HttpContext context, HttpResponseMessage responseMessage);
    }
}
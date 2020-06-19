using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CertMerchant.Common
{
    class CertificateWebUtil
    {
        public static async Task<string> RetrievePFXFromBodyInBase64(HttpRequest req)
        {
            var ms = new MemoryStream();
            await req.Body.CopyToAsync(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}

using CertMerchant.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CertMerchant.Functions
{
    public static class RetrievePEMCACertificates
    {
        [FunctionName("RetrievePEMCACertificates")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Extracting CA certificates from PFX");
            var base64 = await CertificateWebUtil.RetrievePFXFromBodyInBase64(req);

            var pfx = new Chilkat.Pfx();
            pfx.LoadPfxEncoded(base64, "base64", "");

            var pemString = pfx.ToPemEx(false, true, false, false, "", "");
            var caCertsList = pemString.Split("-----END CERTIFICATE-----\r\n")
                                       .Skip(1)
                                       .Where(c => !string.IsNullOrEmpty(c))
                                       .Select(c => c + "-----END CERTIFICATE-----\r\n");
            var caCertStr = string.Join("", caCertsList);
            return new OkObjectResult(caCertStr);
        }
    }
}

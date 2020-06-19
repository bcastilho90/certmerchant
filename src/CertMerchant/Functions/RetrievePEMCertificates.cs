using CertMerchant.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CertMerchant.Functions
{
    public static class RetrievePEMCertificates
    {
        [FunctionName("RetrievePEMCertificates")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Extracting certificate from PFX");
            var base64 = await CertificateWebUtil.RetrievePFXFromBodyInBase64(req);

            var pfx = new Chilkat.Pfx();
            pfx.LoadPfxEncoded(base64, "base64", "");
            var cert = pfx.ToPemEx(false, true, false, false, "", "");

            return new OkObjectResult(cert);
        }
    }
}

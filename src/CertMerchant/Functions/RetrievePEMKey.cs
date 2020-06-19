using CertMerchant.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CertMerchant.Functions
{
    public static class RetrievePEMKey
    {
        [FunctionName("RetrievePEMKey")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Extracting certificate key from PFX");
            var base64 = await CertificateWebUtil.RetrievePFXFromBodyInBase64(req);

            var pfx = new Chilkat.Pfx();
            pfx.LoadPfxEncoded(base64, "base64", "");
            var key = pfx.ToPemEx(false, false, true, true, "", "");

            return new OkObjectResult(key);
        }
    }
}

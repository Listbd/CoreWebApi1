using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi1.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.PowerBI.Api.V1;
using Microsoft.PowerBI.Security;
using Microsoft.Rest;

namespace CoreWebApi1.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        private readonly PowerBIOptions _powerBIOptions;
        public ReportsController(IOptions<PowerBIOptions> powerBIOptionsOptions)
        {
            _powerBIOptions = powerBIOptionsOptions.Value;
        }

        // GET: api/reports
        [HttpGet]
        public IEnumerable<object> Get()
        {
            //This section just loops through to gather reports.
            var reportsList = new List<object>();

            using (var client = this.CreatePowerBIClient())
            {
                var reportsResponse = client.Reports.GetReports(_powerBIOptions.WorkspaceCollection, _powerBIOptions.WorkspaceId);

                for (var i = 0; i<reportsResponse.Value.ToList().Count; i++)
                {
                    reportsList.Add(new 
                    {
                        Id = reportsResponse.Value[i].Id,
                        Name = reportsResponse.Value[i].Name,
                        EmbedUrl = reportsResponse.Value[i].EmbedUrl,
                        WebUrl = reportsResponse.Value[i].WebUrl
                    });
                }
            }
            return reportsList;
        }

        // GET api/reports/{reportId}/{userId}
        [HttpGet("{reportId}/{userId?}")]
        public async Task<ActionResult> Get(string reportId, string userId = null)
        {
            using (var client = this.CreatePowerBIClient())
            {
                const string TESTUSER = "jen@customer.com";

                IEnumerable<string> myRole = new List<string>() { "Customer", "Developer" };

                var reportsResponse = await client.Reports.GetReportsAsync(_powerBIOptions.WorkspaceCollection, _powerBIOptions.WorkspaceId);
                var report = reportsResponse.Value.FirstOrDefault(r => r.Id == reportId);
                var embedToken = PowerBIToken.CreateReportEmbedToken(
                    _powerBIOptions.WorkspaceCollection, _powerBIOptions.WorkspaceId, reportId, userId ?? TESTUSER, myRole);
                var accessToken = embedToken.Generate(_powerBIOptions.AccessKey);

                return Ok(new
                {
                    AccessToken = accessToken,
                    Report = report
                });
            }
        }

        private IPowerBIClient CreatePowerBIClient()
        {
            var credentials = new TokenCredentials(_powerBIOptions.AccessKey, "AppKey");
            var client = new PowerBIClient(credentials)
            {
                BaseUri = new Uri(_powerBIOptions.ApiUrl)
            };

            return client;
        }
    }
}

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi1.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Microsoft.PowerBI.Api.V1;
using Microsoft.PowerBI.Api.V1.Models;
using Microsoft.PowerBI.Security;
using Microsoft.Rest;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreWebApi1.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly PowerBIOptions _powerBIOptionsOptions;
        public ReportController(IOptions<PowerBIOptions> powerBIOptionsOptions)
        {
            _powerBIOptionsOptions = powerBIOptionsOptions.Value;
        }

        // GET: api/report
        [HttpGet]
        public IEnumerable<object> Get()
        {
            //This section just loops through to gather reports.
            var reportsList = new List<object>();

            using (var client = this.CreatePowerBIClient())
            {
                var reportsResponse = client.Reports.GetReports(_powerBIOptionsOptions.WorkspaceCollection, _powerBIOptionsOptions.WorkspaceId);

                for (int i = 0; i<reportsResponse.Value.ToList().Count; i++)
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

        // GET api/report/{reportId}
        [HttpGet("{reportId}")]
        public async Task<ActionResult> Get(string reportId)
        {
            using (var client = this.CreatePowerBIClient())
            {
                //DateTime tokDatetime = DateTime.Now;
                //string myUserID = "jamiem@csgpro.com"; //must pass in this if role is included.
                string myUserID = "jen@customer.com";
                //string myUserID = User.Identity.Name.ToString();
                IEnumerable<string> myRole = new List<string>() { "Customer", "Developer" };

                var reportsResponse = await client.Reports.GetReportsAsync(_powerBIOptionsOptions.WorkspaceCollection, _powerBIOptionsOptions.WorkspaceId);
                var report = reportsResponse.Value.FirstOrDefault(r => r.Id == reportId);
                var embedToken = PowerBIToken.CreateReportEmbedToken(_powerBIOptionsOptions.WorkspaceCollection, _powerBIOptionsOptions.WorkspaceId, reportId, myUserID, myRole);

                var accessToken = embedToken.Generate(_powerBIOptionsOptions.AccessKey);

                return Ok(new
                {
                    AccessToken = accessToken,
                    Report = report
                });
            }
        }

        private IPowerBIClient CreatePowerBIClient()
        {
            var credentials = new TokenCredentials(_powerBIOptionsOptions.AccessKey, "AppKey");
            var client = new PowerBIClient(credentials)
            {
                BaseUri = new Uri(_powerBIOptionsOptions.ApiUrl)
            };

            return client;
        }
    }
}

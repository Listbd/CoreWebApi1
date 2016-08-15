using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi1.Options
{
    public class PowerBIOptions
    {
        public string WorkspaceCollection { get; set; }
        public string WorkspaceId { get; set; }
        public string AccessKey { get; set; }
        public string ApiUrl { get; set; }
    }
}

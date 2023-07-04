using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolizaWebAPI.Application.Common.Security;
public static class Constants
{
    public const string ApiKeyScheme = "ApiKey";

    public static class Permissions
    {
        public const string Read = "r";
        public const string Write = "w";
    }
}

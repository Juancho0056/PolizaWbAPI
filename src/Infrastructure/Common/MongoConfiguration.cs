using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolizaWebAPI.Infrastructure.Common;
public class MongoConfiguration
{
    /// <summary>
    /// Cadena de conexion
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
    /// <summary>
    /// Base de datos 
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;
    public bool CheckCertificateRevocation { get; set; }
    public bool UseTls { get; set; }
    public bool AllowInsecureTls { get; set; }
}
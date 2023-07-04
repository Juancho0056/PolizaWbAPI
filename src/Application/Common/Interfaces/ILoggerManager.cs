using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolizaWebAPI.Application.Common.Interfaces;
public interface ILoggerManager
{
    void LogInfo<T>(T data, string idTransaccion);

    void LogWarning<T>(T data, string idTransaccion);

    void LogError<T>(T data, string idTransaccion);
}

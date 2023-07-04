using System;
using System.Collections.Generic;
using System.Data;
using PolizaWebAPI.Application.Common.Interfaces;
using log4net;
using Newtonsoft.Json;

namespace PolizaWebAPI.Infrastructure.Services;
public class LoggerManager : ILoggerManager
{
    private readonly ILog _logger;

    public LoggerManager(ILog logger)
    {
        _logger = logger;
    }

    private void LogInternal<T>(T data, string idTransaccion, string logLevel)
    {
        string logData = JsonConvert.SerializeObject(data);
        string logInfo = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]|[{logLevel}][{idTransaccion}][{logData}]";
        _logger.Info(logInfo);
    }

    public void LogInfo<T>(T data, string idTransaccion)
    {
        LogInternal(data, idTransaccion, "INFO");
    }

    public void LogWarning<T>(T data, string idTransaccion)
    {
        LogInternal(data, idTransaccion, "WARN");
    }

    public void LogError<T>(T data, string idTransaccion)
    {
        LogInternal(data, idTransaccion, "ERROR");
    }
}
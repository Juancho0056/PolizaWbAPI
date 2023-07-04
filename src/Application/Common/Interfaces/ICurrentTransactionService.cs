using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolizaWebAPI.Application.Common.Interfaces;
public interface ICurrentTransactionService
{
    string? Transaction { get; }
    
}

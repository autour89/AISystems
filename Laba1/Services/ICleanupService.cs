using System.Collections.Generic;
using Laba1.Models;

namespace Laba1.Services;

public interface ICleanupService
{
    Task<IReadOnlyList<Table>> MakeOrders();
    Task<bool> CheckTables();
    Task Cleanup();
    void Reset();
    IReadOnlyList<Table> Tables { get; }
    Action<bool>? OnFinish { get; set; }
}
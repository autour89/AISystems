using System;
using Laba1.Models;
using Laba1.Resources.Languages;
using Microsoft.Extensions.Localization;

namespace Laba1.Services;

public class CleanupService : ICleanupService
{
    private const int tablesCount = 25;
    private readonly IStringLocalizer<Locals> stringLocalizer;
    private Random random;
    List<Table> tables;

    public Action<bool>? OnFinish { get; set; }

    public IReadOnlyList<Table> Tables => tables;

    public CleanupService(IStringLocalizer<Locals> stringLocalizer)
    {
        this.stringLocalizer = stringLocalizer;
        tables = new();
        random = new();
    }

    public async Task<IReadOnlyList<Table>> MakeOrders()
    {
        await Task.Run(() =>
        {
            Reset();

            CreateTableList(tablesCount);
        });

        await CheckTables();

        return Tables;
    }

    public async Task<bool> CheckTables()
    {
        await Task.Run(() =>
        {
            tables.ForEach(table =>
            {
                table.IsClean = Convert.ToBoolean(random.Next(0, 2));
                table.Priority = HeuristicFunction(table.Name ?? string.Empty, table.UsageFrequency, table.Size);
            });
        });

        return tables.Any(table => !table.IsClean);
    }

    public async Task Cleanup()
    {
        await Task.Run(async () =>
        {
            var queue = CalculateCleanupOrder(tables);

            var count = queue.Count;

            for (int i = 0; i < count; i++)
            {
                var table = queue.Dequeue();

                await Task.Delay(500);

                table.IsClean = true;
            }

            OnFinish?.Invoke(true);
        });
    }

    public void Reset()
    {
        tables.Clear();
    }

    private void CreateTableList(int numTables)
    {
        for (int tableNum = 0; tableNum < numTables; tableNum++)
        {
            tables.Add(
                new Table
                {
                    Id = tableNum,
                    Name = $"{stringLocalizer["TableTitle"]} {tableNum + 1}",
                    UsageFrequency = random.Next(1, 101),
                    Size = random.Next(1, 101),
                }
            );
        }
    }

    private PriorityQueue<Table> CalculateCleanupOrder(List<Table> tables)
    {
        // Create a priority queue to order the cleanup of tables
        PriorityQueue<Table> queue = new PriorityQueue<Table>(
            (x, y) =>
            {
                double xPriority = HeuristicFunction(
                    x.Name ?? string.Empty,
                    x.UsageFrequency,
                    x.Size
                );
                double yPriority = HeuristicFunction(
                    y.Name ?? string.Empty,
                    y.UsageFrequency,
                    y.Size
                );
                return yPriority.CompareTo(xPriority); // Sort in descending order of priority
            }
        );

        // Enqueue each table in the priority queue
        foreach (Table table in tables.Where(table => !table.IsClean))
        {
            queue.Enqueue(table);
        }

        return queue;
    }

    private double HeuristicFunction(string tableName, int usage, int size)
    {
        // Define a mapping from table name to priority based on usage and size
        Random random = new(tableName.GetHashCode());
        int usageFactor = random.Next(1, 11); // Assign a random priority based on usage
        int sizeFactor = random.Next(1, 11); // Assign a random priority based on size
        double priority = Math.Sqrt(usage * usageFactor + size * sizeFactor);
        return Math.Round(priority, 4);
    }
}


using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Laba1.Models;

namespace Laba1.ViewModels;

[INotifyPropertyChanged]
public partial class MainViewModel : BaseViewModel
{
    const string tableTitle = "Table";
    const int tablesCount = 25;
    List<Table> tables;
    Random random;


    [ObservableProperty, NotifyPropertyChangedFor(nameof(FullName))]
    string firstName = string.Empty;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(FullName))]
    string lastName = string.Empty;

    public string FullName => $"{FirstName} {LastName}";


    public MainViewModel()
    {
        tables = new();
        random = new();

        CreateTableList(tablesCount);
    }

    [RelayCommand]
    async Task Cleanup()
    {

        await Task.Delay(TimeSpan.FromSeconds(2));


        Debug.WriteLine(FullName);
    }

    [RelayCommand]
    async Task MakeOrders()
    {
        CreateTableList(tablesCount);

        await Task.Delay(TimeSpan.FromSeconds(2));
    }

    void CreateTableList(int numTables)
    {
        for (int tableNum = 0; tableNum < numTables; tableNum++)
        {
            tables.Add(new Table
            {
                Name = $"{tableTitle} {tableNum}",
                UsageFrequency = random.Next(1, 101),
                Size = random.Next(1, 101)
            });
        }
        CalculateCleanupOrder();
    }

    public PriorityQueue<Table> CalculateCleanupOrder(List<Table> tables)
    {
        // Create a priority queue to order the cleanup of tables
        PriorityQueue<Table> queue = new PriorityQueue<Table>((x, y) =>
        {
            double xPriority = HeuristicFunction(x.Name, x.UsageFrequency, x.Size);
            double yPriority = HeuristicFunction(y.Name, y.UsageFrequency, y.Size);
            return yPriority.CompareTo(xPriority); // Sort in descending order of priority
        });

        // Enqueue each table in the priority queue
        foreach (Table table in tables)
        {
            queue.Enqueue(table);
        }

        return queue;
    }

    void CalculateCleanupOrder()
    {
        tables.ForEach(table => table.Priority = HeuristicFunction(table.Name, table.UsageFrequency, table.Size));
        tables.OrderByDescending(x => x.Priority);
    }

    void Clear()
    {

    }

    void Reset()
    {
        tables.Clear();
        CreateTableList(tablesCount);
    }

    public double HeuristicFunction(string tableName, int usage, int size)
    {
        // Define a mapping from table name to priority based on usage and size
        Random random = new(tableName.GetHashCode());
        int usageFactor = random.Next(1, 11); // Assign a random priority based on usage
        int sizeFactor = random.Next(1, 11); // Assign a random priority based on size
        double priority = Math.Sqrt(usage * usageFactor + size * sizeFactor);
        return priority;
    }
}

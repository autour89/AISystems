using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Laba1.Models;

public partial class Table : ObservableObject
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Size { get; set; }
    public int UsageFrequency { get; set; }
    public double Priority { get; set; }

    [ObservableProperty]
    bool isClean = true;
}

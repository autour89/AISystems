using System;
namespace Laba2.Models;

public class SizeModel
{
    public int Value { get; set; }

    public string Name => Value.ToString();
}


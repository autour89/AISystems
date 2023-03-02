using System;
using Laba2.Models;

namespace Laba2.Services;

public interface IGeneticAlgorithm
{
    void Run(int populationSize, int numGenerations, bool isTournament);
    bool IsTournament { get; set; }
    void Reset();
    Action<List<Chromosome>>? OnNextGeneration { get; set; }
}

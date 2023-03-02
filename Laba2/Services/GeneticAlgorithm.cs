using System.Diagnostics;
using Laba2.Models;

namespace Laba2.Services;

class GeneticAlgorithm : IGeneticAlgorithm
{
    const int maxTournamentSize = 10;

    Random random;
    bool initialised;

    List<Chromosome> population;
    List<Chromosome> allPopulation;

    public double MutationRate { get; set; } = 0.9;

    public Action<List<Chromosome>>? OnNextGeneration { get; set; }

    public bool IsTournament { get; set; }

    public GeneticAlgorithm()
    {
        random = new();
        population = new();
        allPopulation = new();
    }

    public void Run(int populationSize, int numGenerations, bool isTournament)
    {
        IsTournament = isTournament;
        int startPoint = 0;

        if (!initialised)
        {
            startPoint = 1;
            Initialise(populationSize);
        }

        for (int generation = startPoint; generation < numGenerations; generation++)
        {
            CreateNextGeneration(generation + 1, populationSize);
        }

        OnNextGeneration?.Invoke(allPopulation);
    }

    public void Reset()
    {
        population.Clear();
        allPopulation.Clear();
        initialised = false;
    }

    void Initialise(int populationSize)
    {
        for (int i = 0; i < populationSize; i++)
        {
            var parent = new Chromosome()
            {
                Index = i,
                Generation = 1
            }.EvaluateFitness();

            population.Add(parent);
        }

        SetBestFitness(population);

        allPopulation.AddRange(population);

        initialised = true;
    }

    List<Chromosome> CreateNextGeneration(int generation, int populationSize)
    {
        List<Chromosome> nextGeneration = new List<Chromosome>();

        for (int i = 0; i < populationSize; i++)
        {
            Chromosome parent1;
            Chromosome parent2;

            if (IsTournament)
            {
                parent1 = TournamentSelection(population);
                parent2 = TournamentSelection(population);
            }
            else
            {
                parent1 = RouletteSelection(population);
                parent2 = RouletteSelection(population);
            }

            Chromosome child = parent1.Crossover(parent2, i, generation).Mutate(MutationRate).EvaluateFitness();

            nextGeneration.Add(child);
        }

        SetBestFitness(nextGeneration);

        population = nextGeneration;

        allPopulation.AddRange(population);

        return nextGeneration;
    }

    Chromosome TournamentSelection(List<Chromosome> population)
    {
        var length = random.Next(2, maxTournamentSize);

        List<Chromosome> tournament = new List<Chromosome>();

        // Select random chromosomes to compete in tournament
        for (int i = 0; i < length; i++)
        {
            tournament.Add(population[random.Next(population.Count)]);
        }

        // Find the best chromosome in the tournament
        Chromosome best = tournament[0];

        for (int i = 1; i < length; i++)
        {
            if (tournament[i].Fitness > best.Fitness)
            {
                best = tournament[i];
            }
        }

        return best;
    }

    Chromosome RouletteSelection(List<Chromosome> population)
    {
        double fitnessSum = 0;
        foreach (Chromosome chromosome in population)
        {
            fitnessSum += chromosome.Fitness;
        }

        double selectionValue = random.NextDouble() * fitnessSum;

        foreach (Chromosome chromosome in population)
        {
            selectionValue -= chromosome.Fitness;

            if (selectionValue <= 0)
            {
                return chromosome;
            }
        }

        return population.First();
    }

    void SetBestFitness(List<Chromosome> generation)
    {
        generation.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));

        var bestF = generation.MaxBy(x => x.Fitness);

        if (bestF is not null)
        {
            bestF.IsWinner = true;
        }
    }

    [Conditional("DEBUG")]
    void Print(List<Chromosome> population)
    {
        Debug.WriteLine($"Population size : {population.Count} {Environment.NewLine}");

        foreach (var chromosome in population)
        {
            Debug.WriteLine($"Generation : {chromosome.Generation}, X: {chromosome.Genes[GeneType.X]}, Y:{chromosome.Genes[GeneType.Y]}, Z: {chromosome.Genes[GeneType.Z]}, Fitness :{chromosome.Fitness}");
        }
    }
}

namespace Laba2.Models;

public enum GeneType { X, Y, Z }

public class Chromosome
{
    Random random;

    public Dictionary<GeneType, int> Genes { get; private set; }

    public int Index { get; set; }

    public int Generation { get; set; }

    public double Fitness { get; private set; }

    public bool IsWinner { get; set; }

    public string? GenesTitle => $"{Genes[GeneType.X]}, {Genes[GeneType.Y]}, {Genes[GeneType.Z]}";

    static Func<Chromosome, double> FitnessFunc => chromosome =>
    {
        double x = chromosome.Genes[GeneType.X];
        double y = chromosome.Genes[GeneType.Y];
        double z = chromosome.Genes[GeneType.Z];

        double result = x + (Math.Pow(y, 2) / x) + (Math.Pow(z, 2) / y) + (2 / z);

        return Math.Round(result, 4);
    };

    public Chromosome(bool autoGenerate = true)
    {
        random = new Random();
        Genes = new Dictionary<GeneType, int>();

        Generate(autoGenerate);
    }

    public Chromosome EvaluateFitness()
    {
        Fitness = FitnessFunc(this);

        return this;
    }

    public Chromosome Mutate(double mutationRate)
    {
        for (int i = 0; i < Genes.Count; i++)
        {
            Genes[(GeneType)i] = random.NextDouble() < mutationRate ? GenerateGene() : Genes[(GeneType)i];
        }

        return this;
    }

    public Chromosome Crossover(Chromosome other, int index, int generation)
    {
        int crossoverPoint = new Random().Next(0, Genes.Count);

        var child = new Chromosome(false)
        {
            Index = index,
            Generation = generation
        };

        for (int i = 0; i < crossoverPoint; i++)
        {
            child.Genes[(GeneType)i] = Genes[(GeneType)i];
        }

        for (int i = crossoverPoint; i < Genes.Count; i++)
        {
            child.Genes[(GeneType)i] = other.Genes[(GeneType)i];
        }

        return child;
    }

    void Generate(bool autoGenerate)
    {
        for (int i = 0; i < Enum.GetNames(typeof(GeneType)).Length; i++)
        {
            Genes[(GeneType)i] = autoGenerate ? GenerateGene() : default;
        }
    }

    int GenerateGene() => random.Next(2, 100);
}

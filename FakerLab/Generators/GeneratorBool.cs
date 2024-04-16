using FakerLab.Generators;

namespace BoolGenerator;

public class GeneratorBool : IGenerator<bool>
{
    private readonly Random _random = new();
    public bool GetValue() => _random.Next(2) == 1;
}
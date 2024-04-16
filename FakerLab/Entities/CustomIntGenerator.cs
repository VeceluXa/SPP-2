using FakerLab.Generators;

namespace FakerLab.Entities;

public class CustomIntGenerator : IGenerator<int>
{
    public int GetValue() => 12345678;
}
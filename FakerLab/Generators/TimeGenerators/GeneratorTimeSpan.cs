using FakerLab.Generators.NumericGenerators.Integers;

namespace FakerLab.Generators.TimeGenerators;

public class GeneratorTimeSpan : IGenerator<TimeSpan>
{
    public TimeSpan GetValue()
    {
        var intGenerator = new GeneratorInt();

        var days = intGenerator.GetValue();
        var hours = intGenerator.GetValue() % 24 + 1;
        var minutes = intGenerator.GetValue() % 60 + 1;
        var seconds = intGenerator.GetValue() % 60 + 1;
        var milliseconds = intGenerator.GetValue() % 1000 + 1;
        var microseconds = intGenerator.GetValue() % 1000 + 1;

        return new TimeSpan(days, hours, minutes, seconds, milliseconds, microseconds);
    }
}
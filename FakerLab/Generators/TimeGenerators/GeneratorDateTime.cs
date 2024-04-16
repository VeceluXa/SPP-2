using FakerLab.Generators.NumericGenerators.Integers;

namespace FakerLab.Generators.TimeGenerators;

public class GeneratorDateTime : IGenerator<DateTime>
{
    public DateTime GetValue()
    {
        var intGenerator = new GeneratorInt();

        var year = intGenerator.GetValue() % 10000;
        var month = intGenerator.GetValue() % 12 + 1;  
        var day = intGenerator.GetValue() % 28 + 1;
        var hour = intGenerator.GetValue() % 24 + 1;
        var minute = intGenerator.GetValue() % 60 + 1;
        var second = intGenerator.GetValue() % 60 + 1;
        var millisecond = intGenerator.GetValue() % 1000 + 1;
        var microsecond = intGenerator.GetValue() % 1000 + 1;

        return new DateTime(year, month, day, hour, minute, second, millisecond, microsecond, DateTimeKind.Utc);
    }
}
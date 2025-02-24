﻿using FakerLab.Generators.NumericGenerators.Integers;

namespace FakerLab.Generators.NumericGenerators.Floats;

public class GeneratorDecimal : IGenerator<decimal>
{
    public decimal GetValue()
    {
        var intGenerator = new GeneratorInt();

        var lo = intGenerator.GetValue();
        var mid = intGenerator.GetValue();
        var hi = intGenerator.GetValue();

        return new decimal(lo, mid, hi, false, 0);
    }
}
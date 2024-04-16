using FakerLab.Generators;
using FakerLab.Generators.StringGenerators;

namespace UriGenerator;

public class GeneratorUri : IGenerator<Uri>
{
    public Uri GetValue()
    {
        var generator = new GeneratorString();

        var builder = new UriBuilder
        {
            Scheme = "http",
            Host = "localhost",
            Path = generator.GetValue()
        };

        return builder.Uri;
    }
}
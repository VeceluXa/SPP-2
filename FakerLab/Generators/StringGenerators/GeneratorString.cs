using System.Text;

namespace FakerLab.Generators.StringGenerators;

public class GeneratorString : IGenerator<string>
{
    private readonly Random _random = new();
    private const int MaxLen = 16;
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    
    public string GetValue()
    {
        var length = _random.Next(MaxLen);
        var stringBuilder = new StringBuilder(length);

        for (var i = 0; i < length; i++)
        {
            var index = _random.Next(Chars.Length);
            var randomChar = Chars[index];
            stringBuilder.Append(randomChar);
        }

        return stringBuilder.ToString();
    }
}
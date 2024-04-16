namespace FakerLab.Entities;

public class OuterTestDTO
{
    public string? String { get; set; }
    public int Int { get; set; }
    public float Float { get; set; }
    public double Double;
    public decimal Decimal;
    public InnerTestDTO? Inner;
    public IEnumerable<int>? Enumerable;
    public List<string>? List;
    public Dictionary<int, int>? Dictionary;
    public DateTime DateTime { get; set; }

    public OuterTestDTO(double dbl, InnerTestDTO testDTO)
    {
        Double = dbl;
        Inner = testDTO;
    }

    public override string ToString() =>
        $"""
         Type: {GetType()}
         string: {String}
         int: {Int}
         float: {Float}
         double: {Double}
         decimal: {Decimal}
         enumerable: {string.Join(", ", Enumerable?.Select(item => $"{item}") ?? [])}
         list: {string.Join(", ", List?.Select(item => $"{item}") ?? [])}
         dictionary: {Dictionary}
         datetime: {DateTime}
         innerDTO: {Inner}
         """;
}
namespace FakerLab.Entities;

public class InnerTestDTO
{
    public int Int { get; set; }

    public override string ToString() =>
        $"""
         Type: {GetType()}:
         int: {Int}
         """;
}
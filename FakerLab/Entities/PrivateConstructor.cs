namespace FakerLab.Entities;

public class PrivateConstructor
{
    public int Id;
    private PrivateConstructor(int id) 
    {
        Id = id;
    }

    public override string ToString() =>
        $"""
         Type: {GetType()}
         int: {Id}
         """;
}
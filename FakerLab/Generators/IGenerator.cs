namespace FakerLab.Generators;

public interface IGenerator<out T>
{
    T GetValue();
}
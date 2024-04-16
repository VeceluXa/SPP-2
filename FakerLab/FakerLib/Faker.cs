using FakerLab.Generators;
using FakerLab.Generators.CollectionGenerators;
using FakerLab.Generators.NumericGenerators.Floats;
using FakerLab.Generators.NumericGenerators.Integers;
using FakerLab.Generators.StringGenerators;
using FakerLab.Generators.TimeGenerators;
using System.Reflection;

namespace FakerLab.FakerLib;

public class Faker(FakerConfig config)
{
    private readonly Dictionary<Type, Type> _generators = new()
    {
        { typeof(int), typeof(GeneratorInt) },
        { typeof(IEnumerable<>), typeof(GeneratorIEnumerable<,>) },
        { typeof(string), typeof(GeneratorString) },
        { typeof(List<>), typeof(GeneratorList<,>) },
        { typeof(decimal), typeof(GeneratorDecimal) },
        { typeof(double), typeof(GeneratorDouble) },
        { typeof(float), typeof(GeneratorFloat) },
        { typeof(byte), typeof(GeneratorByte) },
        { typeof(long), typeof(GeneratorLong) },
        { typeof(short), typeof(GeneratorShort) },
        { typeof(char), typeof(GeneratorChar) },
        { typeof(DateTime), typeof(GeneratorDateTime) },
        { typeof(TimeSpan), typeof(GeneratorTimeSpan) }
    };

    private readonly HashSet<Type> _types = [];

    public Faker() : this(new FakerConfig())
    {
    }

    public void AddGeneratorWithPlugin(string pluginPath)
    {
        var pluginAssembly = Assembly.LoadFrom(pluginPath);
        var pluginTypes = pluginAssembly.GetExportedTypes();

        foreach (var pluginType in pluginTypes)
        {
            if (pluginType.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(IGenerator<>)))
            {
                var targetType = pluginType.GetInterfaces()
                    .Single(x => x.GetGenericTypeDefinition() == typeof(IGenerator<>))
                    .GetGenericArguments()
                    .Single();
                _generators.Add(targetType, pluginType);
            }
        }
    }

    public T? Create<T>()
    {
        var type = typeof(T);

        return (T?)Create(type);
    }

    private object? Create(Type type)
    {
        if (_types.Remove(type))
        {
            return null;
        }

        _types.Add(type);

        var instance = GetInstance(type);
        FillFields(instance);
        FillProperties(instance);

        _types.Remove(type);

        return instance;
    }

    private object? GetInstance(Type type)
    {
        if (type.IsAbstract)
        {
            return null;
        }

        var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        var constructor = constructors.MaxBy(c => c.GetParameters().Length);

        if (constructor is null)
        {
            return Activator.CreateInstance(type)!;
        }

        var parameters = constructor.GetParameters();
        var constructorArgs = new object?[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var parameterType = parameter.ParameterType;

            constructorArgs[i] = config.GetGeneratedValue(type, parameter.Name?.ToLower() ?? string.Empty)
                                 ?? GetGeneratedValue(parameterType);
        }

        try
        {
            return constructor.Invoke(constructorArgs);
        }
        catch (TargetInvocationException)
        {
            return null;
        }
    }

    private void FillProperties(object? instance)
    {
        if (instance is null)
        {
            return;
        }

        var type = instance.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(instance);

            if (HasPublicSetter(property) && (value is null || IsDefaultValue(value)))
            {
                var generated = config.GetGeneratedValue(type, property.Name.ToLower())
                                ?? GetGeneratedValue(property.PropertyType);
                property.SetValue(instance, generated);
            }
        }
    }

    private static bool HasPublicSetter(PropertyInfo property)
    {
        var setter = property.GetSetMethod();

        return setter is not null && setter.IsPublic;
    }

    private void FillFields(object? instance)
    {
        if (instance is null)
        {
            return;
        }

        var type = instance.GetType();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var value = field.GetValue(instance);
            if (value is null || IsDefaultValue(value))
            {
                var generated = config.GetGeneratedValue(type, field.Name.ToLower())
                                ?? GetGeneratedValue(field.FieldType);
                field.SetValue(instance, generated);
            }
        }
    }

    private static bool IsDefaultValue(object value)
    {
        var valueType = value.GetType();
        var localInstance = Activator.CreateInstance(valueType);

        return valueType.IsValueType && value.Equals(localInstance);
    }

    private object? GetGeneratedValue(Type type)
    {
        var baseType = type;

        if (type.IsGenericType)
        {
            type = type.GetGenericTypeDefinition();
        }

        if (_generators.TryGetValue(type, out Type? generatorType))
        {
            if (generatorType.IsGenericType)
            {
                return GetGeneric(baseType, generatorType);
            }
            else
            {
                var generator = Activator.CreateInstance(generatorType);
                var generateMethod = generatorType.GetMethod("GetValue")!;

                return generateMethod.Invoke(generator, null);
            }
        }

        if (type.IsGenericType)
        {
            return null;
        }

        return Create(type);
    }

    private object? GetGeneric(Type entityType, Type generatorType)
    {
        var genericArguments = entityType.GetGenericArguments().ToList();
        var generatorArgumentsPos = genericArguments.Count;
        var generatorArgumentsCount = generatorType.GetGenericArguments().Length;

        for (int i = 0; i < generatorArgumentsCount - generatorArgumentsPos && i < genericArguments.Count; i++)
        {
            genericArguments.Add(_generators[genericArguments[i]]);
        }

        var genericGenerator = generatorType.MakeGenericType([.. genericArguments]);
        var generator = Activator.CreateInstance(genericGenerator);
        var generateMethod = genericGenerator.GetMethod("GetValue")!;

        return generateMethod.Invoke(generator, null);
    }
}
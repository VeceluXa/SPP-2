using FakerLab.Entities;
using FakerLab.FakerLib;

namespace Tests;

public class UnitTests
{
    private Faker _faker = new();
    private readonly FakerConfig _fakerConfig = new();

    [Fact]
    public void Test_CyclicalDependencies()
    {
        var res = _faker.Create<A>();

        Assert.NotNull(res);
        Assert.NotNull(res.B);
        Assert.NotNull(res.B.C);
        Assert.Null(res.B.C.B);
    }

    [Fact]
    public void Test_OneDTO()
    {
        var res = _faker.Create<InnerTestDTO>();

        Assert.NotNull(res);
    }

    [Fact]
    public void Test_NestedDTOs()
    {
        var res = _faker.Create<OuterTestDTO>();

        Assert.NotNull(res);
        Assert.NotNull(res.Inner);
    }

    [Fact]
    public void Test_FakerConfig()
    {
        var generator = new CustomIntGenerator();
        _fakerConfig.Add<InnerTestDTO, int, CustomIntGenerator>(dto => dto.Int);
        _faker = new Faker(_fakerConfig);

        var res = _faker.Create<InnerTestDTO>();

        Assert.NotNull(res);
        Assert.Equal(generator.GetValue(), res.Int);
    }

    [Fact]
    public void Test_Enumerations()
    {
        var res = _faker.Create<OuterTestDTO>();

        Assert.NotNull(res);
        Assert.NotNull(res.Enumerable);
    }

    [Fact]
    public void Test_NonDTOClass()
    {
        var res = _faker.Create<ClassWithNonDTOMember>();

        Assert.Null(res!.Uri);
    }

    [Fact]
    public void Test_SystemClassDateTime()
    {
        var res = _faker.Create<OuterTestDTO>();

        Assert.NotEqual(default, res!.DateTime);
    }

    [Fact]
    public void Test_Plugins()
    {
        _faker.AddGeneratorWithPlugin("plugins/UriGenerator.dll");

        var res = _faker.Create<ClassWithNonDTOMember>();

        Assert.NotNull(res!.Uri);
    }

    [Fact]
    public void Test_PrivateConstructor()
    {
        var res = _faker.Create<PrivateConstructor>();

        Assert.NotNull(res);
    }
}
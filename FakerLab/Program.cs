using FakerLab.Entities;
using FakerLab.FakerLib;

var config = new FakerConfig();
config.Add<B, int, CustomIntGenerator>(b => b.Id);

var faker = new Faker(config);

faker.AddGeneratorWithPlugin("plugins/UriGenerator.dll");
faker.AddGeneratorWithPlugin("plugins/BoolGenerator.dll");

Console.WriteLine(faker.Create<OuterTestDTO>());

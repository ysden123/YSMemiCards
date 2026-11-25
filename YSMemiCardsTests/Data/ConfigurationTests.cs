using System.Text.Json;

namespace YSMemiCardsTests;

[TestClass]
[DoNotParallelize]
public class ConfigurationTests
{
    [TestMethod]
    public void TestConfiguration()
    {
        var config = new YSMemiCardsApp.Data.Configuration
        {
            FirstDays = 1,
            SecondDays = 3,
            ThirdDays = 10
        };

        var json = JsonSerializer.Serialize(config);
        Console.WriteLine(json);

        Assert.IsNotNull(json);

        var deserializedConfig = JsonSerializer.Deserialize<YSMemiCardsApp.Data.Configuration>(json);
        Assert.IsNotNull(deserializedConfig);
        Assert.IsTrue(config.Equals(deserializedConfig));

        Assert.AreEqual(config.FirstDays, deserializedConfig!.FirstDays);
        Assert.AreEqual(config.SecondDays, deserializedConfig!.SecondDays);
        Assert.AreEqual(config.ThirdDays, deserializedConfig!.ThirdDays);
    }

    [TestMethod]
    public void TestCardEquality()
    {
        var config1 = new YSMemiCardsApp.Data.Configuration
        {
            FirstDays = 1,
            SecondDays = 3,
            ThirdDays = 10
        };

        var config2 = new YSMemiCardsApp.Data.Configuration
        {
            FirstDays = 1,
            SecondDays = 3,
            ThirdDays = 10
        };

        Assert.IsTrue(config1.Equals(config2));
        Assert.AreEqual(config1.GetHashCode(), config2.GetHashCode());
    }

    [TestMethod]
    public void TestConfigEquality()
    {
        var config1 = new YSMemiCardsApp.Data.Configuration
        {
            FirstDays = 1,
            SecondDays = 3,
            ThirdDays = 10
        };

        var config2 = new YSMemiCardsApp.Data.Configuration
        {
            FirstDays = 1,
            SecondDays = 3,
            ThirdDays = 10
        };
        Assert.AreEqual(config1, config2);

        config2.FirstDays = 2;
        Assert.AreNotEqual(config1, config2);
    }
}

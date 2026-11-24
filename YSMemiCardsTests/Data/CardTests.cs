using System.Text.Json;

namespace YSMemiCardsTests.Data;

[TestClass]
[DoNotParallelize]
public class CardTests
{
    [TestMethod]
    public void TestCard()
    {
        var card = new YSMemiCardsApp.Data.Card
        {
            Word = "Hello",
            Translations = "Hola, Bonjour",
            CreatedAt = DateTime.UtcNow,
            LastTimeUsed = DateTime.UtcNow
        };
        var json = JsonSerializer.Serialize(card);
        Console.WriteLine(json);

        Assert.IsNotNull(json);

        var deserializedCard = JsonSerializer.Deserialize<YSMemiCardsApp.Data.Card>(json);
        Assert.IsNotNull(deserializedCard);

        Assert.AreEqual(card.Word, deserializedCard!.Word);
        Assert.AreEqual(card.Translations, deserializedCard!.Translations);
        Assert.AreEqual(card.CreatedAt, deserializedCard!.CreatedAt);
        Assert.AreEqual(card.LastTimeUsed, deserializedCard!.LastTimeUsed);
    }

    [TestMethod]
    public void TestCardEquality()
    {
        var card1 = new YSMemiCardsApp.Data.Card
        {
            Word = "Hello",
            Translations = "Hola, Bonjour",
            CreatedAt = DateTime.UtcNow,
            LastTimeUsed = DateTime.UtcNow
        };
        var card2 = new YSMemiCardsApp.Data.Card
        {
            Word = "Hello",
            Translations = "Hola, Bonjour 222",
            CreatedAt = DateTime.UtcNow,
            LastTimeUsed = DateTime.UtcNow,
            Level = YSMemiCardsApp.Data.DifficultyLevel.Easy
        };
        Assert.AreEqual(card1, card2);

        card2.Word = "Not Hello";
        Assert.AreNotEqual(card1, card2);
    }

    [TestMethod]
    public void TestCards()
    {
        var cards = new YSMemiCardsApp.Data.Cards
        {
            CardsList =
            [
                new YSMemiCardsApp.Data.Card
                {
                    Word = "Hello",
                    Translations = "Hola, Bonjour",
                    CreatedAt = DateTime.UtcNow,
                    LastTimeUsed = DateTime.UtcNow
                },
                new YSMemiCardsApp.Data.Card
                {
                    Word = "Goodbye",
                    Translations = "Adiós, Au revoir",
                    CreatedAt = DateTime.UtcNow,
                    LastTimeUsed = DateTime.UtcNow
                }
            ]
        };
        var json = JsonSerializer.Serialize(cards);
        Assert.IsNotNull(json);
        var deserializedCards = JsonSerializer.Deserialize<YSMemiCardsApp.Data.Cards>(json);
        Assert.IsNotNull(deserializedCards);

        Assert.HasCount(cards.CardsList.Count, deserializedCards!.CardsList);

        for (int i = 0; i < cards.CardsList.Count; i++)
        {
            Assert.AreEqual(cards.CardsList[i].Word, deserializedCards.CardsList[i].Word);
            Assert.AreEqual(cards.CardsList[i].Translations, deserializedCards.CardsList[i].Translations);
            Assert.AreEqual(cards.CardsList[i].CreatedAt, deserializedCards.CardsList[i].CreatedAt);
            Assert.AreEqual(cards.CardsList[i].LastTimeUsed, deserializedCards.CardsList[i].LastTimeUsed);
        }
    }
}

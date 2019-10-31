using Capstone;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class ConsumableItemTests
    {
        [TestMethod]
        public void CheckNamePropertyTest()
        {
            Candy candy = new Candy("Candy", 0, 0);
            Assert.AreEqual("Candy", candy.Name, "Should be named from what the constructor provided");
            Drink drink = new Drink("Good Drink", 0, 0);
            Assert.AreEqual("Good Drink", drink.Name, "Should be named from what the constructor provided");
            Gum gum = new Gum("Freesh Mint", 0, 0);
            Assert.AreEqual("Freesh Mint", gum.Name, "Should be named from what the constructor provided");
            Chip chip = new Chip("Crunchie", 0, 0);
            Assert.AreEqual("Crunchie", chip.Name, "Should be named from what the constructor provided");
        }

        [TestMethod]
        public void CheckPricePropertyTest()
        {
            Candy candy = new Candy("Candy", 1.80M, 0);
            Assert.AreEqual(1.80M, candy.Price, "Should be priced from what the constructor provided");
            Drink drink = new Drink("Good Drink", 1.50M, 0);
            Assert.AreEqual(1.50M, drink.Price, "Should be priced from what the constructor provided");
            Gum gum = new Gum("Freesh Mint", .75M, 0);
            Assert.AreEqual(.75M, gum.Price, "Should be priced from what the constructor provided");
            Chip chip = new Chip("Crunchie", .85M, 0);
            Assert.AreEqual(.85M, chip.Price, "Should be priced from what the constructor provided");
        }

        [TestMethod]
        public void CheckCountPropertyTest()
        {
            Candy candy = new Candy("Candy", 1.80M, 5);
            Assert.AreEqual(5, candy.Count, "Should be the count provided by the constructor.");
            Drink drink = new Drink("Good Drink", 1.50M, 0);
            Assert.AreEqual(0, drink.Count, "Should be the count provided by the constructor.");
            Gum gum = new Gum("Freesh Mint", .75M, 2);
            Assert.AreEqual(2, gum.Count, "Should be the count provided by the constructor.");
            Chip chip = new Chip("Crunchie", .85M, 4);
            Assert.AreEqual(4, chip.Count, "Should be the count provided by the constructor.");
        }

        [TestMethod]
        public void CheckIfSoldOutTest()
        {
            Candy candy = new Candy("Candy", 1.80M, 5);
            Assert.IsFalse(candy.SoldOut, "Should not be sold out if items are still available.");

            candy.Count = 2;
            Assert.IsFalse(candy.SoldOut, "Should not be sold out if items are still available.");

            candy.Count = -2;
            Assert.IsTrue(candy.SoldOut, "Should be sold out if items are no longer available.");

            candy.Count = 0;
            Assert.IsTrue(candy.SoldOut, "Should be sold out if items are no longer available.");
        }

        [TestMethod]
        public void CheckDispenseMessageTest()
        {
            Candy candy = new Candy("Candy", 1.80M, 5);
            Assert.AreEqual("Munch Munch, Yum!", candy.DispensedMessage, "Should return correct message for the type.");
            Drink drink = new Drink("Good Drink", 1.50M, 0);
            Assert.AreEqual("Glug Glug, Yum!", drink.DispensedMessage, "Should return correct message for the type.");
            Gum gum = new Gum("Freesh Mint", .75M, 2);
            Assert.AreEqual("Chew Chew, Yum!", gum.DispensedMessage, "Should return correct message for the type.");
            Chip chip = new Chip("Crunchie", .85M, 4);
            Assert.AreEqual("Crunch Crunch, Yum!", chip.DispensedMessage, "Should return correct message for the type.");
        }

        [TestMethod]
        public void ToString_With_Items_Works()
        {
            Candy candy = new Candy("Candy", 1.80M, 5);
            Assert.AreEqual("Candy - $1.80", candy.ToString(), "Should return correct format for item information.");

            Drink drink = new Drink("Drink", 4.80M, 2);
            Assert.AreEqual("Drink - $4.80", drink.ToString(), "Should return correct format for item information.");
        }

        [TestMethod]
        public void ToString_Sold_Out_Works()
        {
            Candy candy = new Candy("Candy", 1.80M, 0);
            Assert.AreEqual("Candy - SOLD OUT.", candy.ToString(), "Should return correct format for item information.");

            Drink drink = new Drink("Drink", 4.80M, -2);
            Assert.AreEqual("Drink - SOLD OUT.", drink.ToString(), "Should return correct format for item information.");

        }
    }
}

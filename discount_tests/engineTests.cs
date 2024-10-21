using acme_discount_engine.Discounts;
using AcmeSharedModels;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace discount_tests
{
    [TestClass]
    public class DiscountEngineTest
    {
        public IClock TestClock = new StubClock();

        [SetUp]
        public void Setup()
        {
        }

        [TestMethod]
        public void BulkDiscountTest()
        {
            var items = new List<Item>
        {
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1))
        };

            var discountEngine = new DiscountEngine
            {
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);
            Assert.AreEqual(49.00, result, 0.00);
        }

        [TestMethod]
        public void TwoForOneDiscountTest()
        {
            var items = new List<Item>
        {
            new Item("Freddo", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Freddo", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Freddo", 5.00, false, new DateTime(2200, 5, 1))
        };

            var discountEngine = new DiscountEngine
            {
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);
            Assert.AreEqual(10.00, result, 0.00);
        }

        [TestMethod]
        public void LoyaltyCardTest()
        {
            var items = new List<Item>
        {
            new Item("Thing", 20.00, false, new DateTime(2200, 5, 1)),
            new Item("Other Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 20.00, false, new DateTime(2200, 5, 1)),
            new Item("Something", 5.00, false, new DateTime(2200, 5, 1))
        };

            var discountEngine = new DiscountEngine
            {
                LoyaltyCard = true
            };

            var result = discountEngine.ApplyDiscounts(items);
            Assert.AreEqual(49.00, result, 0.00);
        }

        [TestMethod]
        public void BestBeforeDiscountTest()
        {
            var items = new List<Item>
        {
            new Item("Thing1", 10.00, false, DateTime.Now.AddDays(10)),
            new Item("Thing2", 10.00, false, DateTime.Now.AddDays(6)),
            new Item("Thing3", 10.00, false, DateTime.Now.AddDays(5)),
            new Item("Thing4", 10.00, false, DateTime.Now.AddDays(-1))
        };

            var discountEngine = new DiscountEngine
            {
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);

            Assert.AreEqual(9.50, items.First(p => p.Name == "Thing1").Price, 0.00);
            Assert.AreEqual(9.50, items.First(p => p.Name == "Thing2").Price, 0.00);
            Assert.AreEqual(9.00, items.First(p => p.Name == "Thing3").Price, 0.00);
            Assert.AreEqual(8.00, items.First(p => p.Name == "Thing4").Price, 0.00);
        }

        [TestMethod]
        public void NoBestBeforeDiscountTest()
        {
            var items = new List<Item>
        {
            new Item("T-Shirt", 10.00, false, DateTime.Now.AddDays(10)),
            new Item("Keyboard", 10.00, false, DateTime.Now.AddDays(6)),
            new Item("Drill", 10.00, false, DateTime.Now.AddDays(5)),
            new Item("Chair", 10.00, false, DateTime.Now.AddDays(-1))
        };

            var discountEngine = new DiscountEngine
            {
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);

            Assert.AreEqual(40.00, result, 0.00);
            Assert.AreEqual(10.00, items.First(p => p.Name == "T-Shirt").Price, 0.00);
            Assert.AreEqual(10.00, items.First(p => p.Name == "Keyboard").Price, 0.00);
            Assert.AreEqual(10.00, items.First(p => p.Name == "Drill").Price, 0.00);
            Assert.AreEqual(10.00, items.First(p => p.Name == "Chair").Price, 0.00);
        }

        [TestMethod]
        public void UseByDiscountFivePercentTest()
        {
            var items = new List<Item>
        {
            new Item("Thing", 10.00, true, DateTime.Now)
        };

            var discountEngine = new DiscountEngine
            {
                Time = DateTime.Today.AddHours(3),
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);

            Assert.AreEqual(9.50, items.First(p => p.Name == "Thing").Price, 0.00);
            Assert.AreEqual(9.50, result, 0.00);
        }

        [TestMethod]
        public void UseByDiscountTenPercentTest()
        {
            var items = new List<Item>
        {
            new Item("Thing", 10.00, true, DateTime.Now)
        };

            var discountEngine = new DiscountEngine
            {
                Time = DateTime.Today.AddHours(13),
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);

            Assert.AreEqual(9.00, items.First(p => p.Name == "Thing").Price, 0.00);
            Assert.AreEqual(9.00, result, 0.00);
        }

        [TestMethod]
        public void UseByDiscountFifteenPercentTest()
        {
            var items = new List<Item>
        {
            new Item("Thing", 10.00, true, DateTime.Now)
        };

            var discountEngine = new DiscountEngine
            {
                Time = DateTime.Today.AddHours(17),
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);

            Assert.AreEqual(8.50, items.First(p => p.Name == "Thing").Price, 0.00);
            Assert.AreEqual(8.50, result, 0.00);
        }

        [TestMethod]
        public void UseByDiscountTwentyFivePercentTest()
        {
            var items = new List<Item>
        {
            new Item("Thing", 10.00, true, DateTime.Now)
        };

            var discountEngine = new DiscountEngine
            {
                Time = DateTime.Today.AddHours(19),
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);

            Assert.AreEqual(7.50, items.First(p => p.Name == "Thing").Price, 0.00);
            Assert.AreEqual(7.50, result, 0.00);
        }

        [TestMethod]
        public void UseByDiscountMeatNotTwentyFivePercentTest()
        {
            var items = new List<Item>
        {
            new Item("Steak (Meat)", 10.00, true, DateTime.Now)
        };

            var discountEngine = new DiscountEngine
            {
                Time = DateTime.Today.AddHours(19),
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);

            Assert.AreEqual(8.50, items.First(p => p.Name.Contains("(Meat)")).Price, 0.00);
            Assert.AreEqual(8.50, result, 0.00);
        }

        [TestMethod]
        public void ComplexBasketWithLoyalty()
        {
            var items = new List<Item>
        {
            new Item("Steak (Meat)", 10.00, true, DateTime.Now),
            new Item("Thing", 10.00, true, DateTime.Now),
            new Item("T-Shirt", 10.00, false, DateTime.Now.AddDays(10)),
            new Item("Thing4", 10.00, false, DateTime.Now.AddDays(-1)),
            new Item("Freddo", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Freddo", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Freddo", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
        };

            var discountEngine = new DiscountEngine
            {
                Time = DateTime.Today.AddHours(19),
                LoyaltyCard = true
            };

            var result = discountEngine.ApplyDiscounts(items);

            Assert.AreEqual(91.09, result, 0.00);
        }

        [TestMethod]
        public void ComplexBasketWithoutLoyalty()
        {
            var items = new List<Item>
        {
            new Item("Steak (Meat)", 10.00, true, DateTime.Now),
            new Item("Thing", 10.00, true, DateTime.Now),
            new Item("T-Shirt", 10.00, false, DateTime.Now.AddDays(10)),
            new Item("Thing4", 10.00, false, DateTime.Now.AddDays(-1)),
            new Item("Freddo", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Freddo", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Freddo", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
            new Item("Thing", 5.00, false, new DateTime(2200, 5, 1)),
        };

            var discountEngine = new DiscountEngine
            {
                Time = DateTime.Today.AddHours(19),
                LoyaltyCard = false
            };

            var result = discountEngine.ApplyDiscounts(items);

            Assert.AreEqual(92.95, result, 0.00);
        }

    }
}
using AcmeSharedModels;

namespace acme_discount_engine.Discounts
{
    public class DiscountEngine
    {
        public bool LoyaltyCard { get; set; }
        public DateTime Time { get; set; } = new SystemClock().GetCurrentTime();
 
        private List<string> NoDiscount = ["T-Shirt", "Keyboard", "Drill", "Chair"];

        public double ApplyDiscounts(List<Item> items)
        {
            LoyaltyDiscount loyaltyScheme = new LoyaltyDiscount(LoyaltyCard);
            GeneralDiscount generalDeals = new GeneralDiscount(NoDiscount, Time);
            BulkDiscount bulkDeals = new BulkDiscount(NoDiscount);
            TwoForOneDiscount twoForOneDeals = new TwoForOneDiscount();

            Checkout checkout = new Checkout(items);
            checkout.SortItemsInBasketForCheckout();

            twoForOneDeals.Process(checkout);
            generalDeals.Process(checkout);
            bulkDeals.Process(checkout);

            checkout.CalculateCheckoutTotal();

            loyaltyScheme.Process(checkout);

            return RoundTotal(checkout.basketTotal);
        }

        private double RoundTotal(double amount) {
            return Math.Round(amount, 2);
        }
    }
}


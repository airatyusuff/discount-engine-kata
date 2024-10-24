using AcmeSharedModels;

namespace acme_discount_engine.Discounts
{
    public class DiscountEngine
    {
        public bool LoyaltyCard { get; set; }
        public DateTime Time { get; set; } = new SystemClock().GetCurrentTime();
 
        private List<string> NoDiscount = new List<string> { "T-Shirt", "Keyboard", "Drill", "Chair" };

        public double ApplyDiscounts(List<Item> items)
        {
            LoyaltyDiscount loyaltyScheme = new LoyaltyDiscount(LoyaltyCard);
            GeneralDiscount generalDiscount = new GeneralDiscount(NoDiscount, Time);
            BulkDiscount bulkDeals = new BulkDiscount(NoDiscount);
            TwoForOneDiscount twoForOneDeals = new TwoForOneDiscount();

            Checkout checkout = new Checkout(items);
            checkout.SortItemsInBasketForCheckout();

            List<Item> basketItems = checkout.BasketItems;

            //code smell here?
            twoForOneDeals.Process(checkout);
            generalDiscount.Process(checkout);
            bulkDeals.Process(checkout);

            checkout.CalculateCheckoutTotal();

            loyaltyScheme.Process(checkout);

            return Math.Round(checkout.basketTotal, 2);
        }
    }
}


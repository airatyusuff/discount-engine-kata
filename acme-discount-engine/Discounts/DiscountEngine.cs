using AcmeSharedModels;

namespace acme_discount_engine.Discounts
{
    public class DiscountEngine
    {
        public bool LoyaltyCard { get; set; }
        public DateTime Time { get; set; }

        private List<string> TwoForOneList = new List<string> { "Freddo" };
        private List<string> NoDiscount = new List<string> { "T-Shirt", "Keyboard", "Drill", "Chair" };

        public DiscountEngine()
        {
            Time = new SystemClock().GetCurrentTime();
        }

        public double ApplyDiscounts(List<Item> items)
        {
            Checkout checkout = new Checkout(items);
            checkout.SortItemsInBasketForCheckout();

            List<Item> basketItems = checkout.BasketItems;

            checkout.ProcessTwoForOneDeals(TwoForOneList);

            foreach (var item in basketItems)
            {
                applyGeneralDiscount(item);
                checkout.UpdateTotal(item.Price);
            }

            checkout.ProcessBulkDiscounts(TwoForOneList);
            
            double finalTotal = checkout.CalculateBasketTotal();

            if (checkout.IsBasketEligibleForLoyaltyDiscount(LoyaltyCard))
            {
                finalTotal -= finalTotal * 0.02;
            }

            return Math.Round(finalTotal, 2);
        }

        private void applyGeneralDiscount(Item item)
        {
            int daysUntilDate = (item.Date - DateTime.Today).Days;
            if (item.IsPastDueDate())
            {
                daysUntilDate = -1;
            }

            if (isPerishableItemEligibleForGeneralDiscount(item, daysUntilDate))
            {
                applyDiscountForPerishable(item);
                return;
            }

            if (isNonPerishableItemEligibleForGeneralDiscount(item))
            {
                applyDiscountForNonPerishable(item, daysUntilDate);

            }
        }

        private bool isPerishableItemEligibleForGeneralDiscount(Item item, int daysUntil)
        {
            return item.IsPerishable && daysUntil == 0;
        }

        private bool isNonPerishableItemEligibleForGeneralDiscount(Item item)
        {
            return !NoDiscount.Contains(item.Name);
        }

        private void applyDiscountForPerishable(Item item)
        {
            if (Time.Hour >= 0 && Time.Hour < 12)
            {
                item.Price -= item.Price * 0.05;
            }
            else if (Time.Hour >= 12 && Time.Hour < 16)
            {
                item.Price -= item.Price * 0.10;
            }
            else if (Time.Hour >= 16 && Time.Hour < 18)
            {
                item.Price -= item.Price * 0.15;
            }
            else if (Time.Hour >= 18)
            {
                item.Price -= item.Price * (!item.Name.Contains("(Meat)") ? 0.25 : 0.15);
            }
        }

        private void applyDiscountForNonPerishable(Item item, int daysUntil)
        {
            if (daysUntil >= 6 && daysUntil <= 10)
            {
                item.Price -= item.Price * 0.05;
            }
            else if (daysUntil >= 0 && daysUntil <= 5)
            {
                item.Price -= item.Price * 0.10;
            }
            else if (daysUntil < 0)
            {
                item.Price -= item.Price * 0.20;
            }
        }

    }
}


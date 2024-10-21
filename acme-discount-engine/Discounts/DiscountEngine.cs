using AcmeSharedModels;

namespace acme_discount_engine.Discounts
{
    public class DiscountEngine
    {
        public bool LoyaltyCard { get; set; }
        public DateTime Time { get; set; }
        public TwoForOneDiscount twoForOneDeals = new TwoForOneDiscount();
        public BulkDiscount bulkDeals;

        private List<string> NoDiscount = new List<string> { "T-Shirt", "Keyboard", "Drill", "Chair" };

        public DiscountEngine()
        {
            Time = new SystemClock().GetCurrentTime();
            bulkDeals = new BulkDiscount(NoDiscount);
        }

        public double ApplyDiscounts(List<Item> items)
        {
            Checkout checkout = new Checkout(items);
            checkout.SortItemsInBasketForCheckout();

            List<Item> basketItems = checkout.BasketItems;

            processDiscount(checkout, twoForOneDeals);

            foreach (var item in basketItems)
            {
                applyGeneralDiscount(item);
                checkout.IncreaseBasketTotalBy(item.Price);
            }

            processDiscount(checkout, bulkDeals);

            double finalTotal = applyLoyaltyDiscount(checkout);

            return Math.Round(finalTotal, 2);
        }

        private void processDiscount(Checkout c, DiscountFactory discountFactory)
        {
            c.ResetCurrentItemCount();
            for (int i = 0; i < c.BasketItems.Count; i++)
            {
                if (c.isItemDifferentFromCurrentItem(i))
                {
                    c.proceedToNextItemInBasket(i);
                    continue;
                }

                c.IncrementCurrentItemCount();
                if (discountFactory.isBasketEligibleForDeal(c))
                {
                    discountFactory.runDiscountOnBasketItem(c, i);
                }

            }
        }

        private double applyLoyaltyDiscount(Checkout c)
        {
            double total = c.BasketItems.Sum(item => item.Price);

            if (IsBasketEligibleForLoyaltyDiscount(c))
            {
                total -= total * 0.02;
            }

            return total;
        }

        private bool isNonPerishableItemEligibleForGeneralDiscount(Item item)
        {
            return !NoDiscount.Contains(item.Name);
        }

        private void applyGeneralDiscount(Item item)
        {
            int daysUntilDate = (item.Date - DateTime.Today).Days;
            if (item.IsPastDueDate())
            {
                daysUntilDate = -1;
            }

            if (item.IsPerishableAndEligibleForGeneralDiscount(daysUntilDate))
            {
                applyDiscountForPerishable(item);
                return;
            }

            if (isNonPerishableItemEligibleForGeneralDiscount(item))
            {
                applyDiscountForNonPerishable(item, daysUntilDate);
            }
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

        private bool IsBasketEligibleForLoyaltyDiscount(Checkout c)
        {
            return LoyaltyCard && c.basketTotal >= 50.00;
        }
    }
}


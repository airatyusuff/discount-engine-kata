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
            checkout.StartCheckoutProcess();

            string currentItem = checkout.currentItem.Name;
            int itemCount = checkout.currentItemCount;
            List<Item> basketItems = checkout.BasketItems;

            checkout.ProcessTwoForOneDeals(TwoForOneList);

            // process general discounts
            foreach (var item in basketItems)
            {
                checkout.UpdateTotal(item.Price);
                int daysUntilDate = (item.Date - DateTime.Today).Days;
                if(item.IsPastDueDate())
                {
                    daysUntilDate = -1;
                }

                if (isPerishableItemEligibleForDiscount(item, daysUntilDate))
                {
                    applyDiscountForPerishable(item);
                    continue;
                }

                if (isNonPerishableItemEligibleForDiscount(item))
                {
                    applyDiscountForNonPerishable(item, daysUntilDate);
                    
                }
            }

            currentItem = string.Empty;
            itemCount = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name != currentItem)
                {
                    currentItem = items[i].Name;
                    itemCount = 1;
                }
                else
                {
                    itemCount++;
                    if (itemCount == 10 && !TwoForOneList.Contains(items[i].Name) && items[i].Price >= 5.00)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            items[i - j].Price -= items[i - j].Price * 0.02;
                        }
                        itemCount = 0;
                    }
                }
            }

            double finalTotal = items.Sum(item => item.Price);

            if (LoyaltyCard && checkout.basketTotal >= 50.00)
            {
                finalTotal -= finalTotal * 0.02;
            }

            return Math.Round(finalTotal, 2);
        }

        private bool isPerishableItemEligibleForDiscount(Item item, int daysUntil)
        {
            return item.IsPerishable && daysUntil == 0;
        }

        private bool isNonPerishableItemEligibleForDiscount(Item item)
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


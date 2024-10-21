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
            Basket basket = new Basket(items);

            basket.SortItemsInBasketForCheckout();
            basket.StartCheckoutProcess();

            string currentItem = basket.currentItemName;
            int itemCount = basket.currentItemCount;
            List<Item> basketItems = basket.Items;

            basket.ProcessTwoForOneDeals(TwoForOneList);

            double itemTotal = 0.00;
            foreach (var item in basketItems)
            {
                itemTotal += item.Price;
                int daysUntilDate = (item.Date - DateTime.Today).Days;
                if(DateTime.Today > item.Date) { daysUntilDate = -1; }

                if (!item.IsPerishable)
                {
                    if (!NoDiscount.Contains(item.Name))
                    {
                        if (daysUntilDate >= 6 && daysUntilDate <= 10)
                        {
                            item.Price -= item.Price * 0.05;
                        }
                        else if (daysUntilDate >= 0 && daysUntilDate <= 5)
                        {
                            item.Price -= item.Price * 0.10;
                        }
                        else if (daysUntilDate < 0)
                        {
                            item.Price -= item.Price * 0.20;
                        }
                    }
                }
                else
                {
                    if (daysUntilDate == 0)
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

            if (LoyaltyCard && itemTotal >= 50.00)
            {
                finalTotal -= finalTotal * 0.02;
            }

            return Math.Round(finalTotal, 2);
        }
    }
}


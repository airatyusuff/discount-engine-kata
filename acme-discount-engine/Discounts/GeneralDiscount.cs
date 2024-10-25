using AcmeSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme_discount_engine.Discounts
{
    public class GeneralDiscount: IDiscount
    {
        private DateTime Time;
        private List<string> NoDiscountItems;

        public GeneralDiscount(List<string> noDiscountItems, DateTime engineTime) {
            Time = engineTime;
            NoDiscountItems = noDiscountItems;
        }

        public void Process(Checkout c)
        {
            foreach (var item in c.BasketItems)
            {
                applyGeneralDiscount(item);
                c.IncreaseBasketTotalBy(item.Price);
            }
        }

        //everything below can clean up better
        private void applyGeneralDiscount(Item item)
        {
            int daysUntilDate = (item.Date - DateTime.Today).Days;
            if (IsPastDueDate(item))
            {
                daysUntilDate = -1;
            }

            if (IsPerishableAndEligibleForGeneralDiscount(item, daysUntilDate))
            {
                applyDiscountForPerishable(item);
                return;
            }

            if (isNonPerishableItemEligibleForGeneralDiscount(item))
            {
                applyDiscountForNonPerishable(item, daysUntilDate);
            }
        }

        private bool isNonPerishableItemEligibleForGeneralDiscount(Item item)
        {
            return !NoDiscountItems.Contains(item.Name);
        }

        public bool IsPastDueDate(Item item)
        {
            return DateTime.Today > item.Date;
        }

        public bool IsPerishableAndEligibleForGeneralDiscount(Item item, int daysUntil)
        {
            return item.IsPerishable && daysUntil == 0;
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

using AcmeSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme_discount_engine.Discounts
{
    public class BulkDiscount : IDiscount
    {
        private List<string> NoDiscountItems;

        public BulkDiscount(List<string> noDiscountItems)
        {
            NoDiscountItems = noDiscountItems;
        }

        public void Process(Checkout c)
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
                if (IsItemEligibleForDeal(c))
                {
                    RunDiscountOnItem(c, i);
                }

            }
        }

        private bool IsItemEligibleForDeal(Checkout c)
        {
            return c.currentItemCount == 10 &&
                c.currentItem.Price >= 5.00 &&
                !NoDiscountItems.Contains(c.currentItem.Name);
        }

        private void RunDiscountOnItem(Checkout c, int itemIndex)
        {
            for (int j = 0; j < 10; j++)
            {
                c.BasketItems[itemIndex - j].Price -= c.BasketItems[itemIndex - j].Price * 0.02;
            }
            c.ResetCurrentItemCount();
        }
    }
}

using AcmeSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme_discount_engine.Discounts
{
    public class TwoForOneDiscount : IBasketDiscount
    {
        private List<string> dealsList = new List<string> { "Freddo" };

        public bool IsItemEligibleForDeal(Checkout basket)
        {
            return basket.currentItemCount == 3 && dealsList.Contains(basket.currentItem.Name);
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

        public void RunDiscountOnItem(Checkout basket, int itemIndex)
        {
            basket.BasketItems[itemIndex].Price = 0.00;
            basket.ResetCurrentItemCount();
        }
    }
}

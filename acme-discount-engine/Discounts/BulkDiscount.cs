using AcmeSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme_discount_engine.Discounts
{
    public class BulkDiscount : DiscountFactory
    {
        private List<string> NoDiscountList;

        public BulkDiscount(List<string> list)
        {
            NoDiscountList = list;
        }
        public override bool isBasketEligibleForDeal(Checkout c)
        {
            return c.currentItemCount == 10 &&
                !NoDiscountList.Contains(c.currentItem.Name) &&
                c.currentItem.Price >= 5.00;
        }

        public override void runDiscountOnBasketItem(Checkout c, int itemIndex)
        {
            for (int j = 0; j < 10; j++)
            {
                c.BasketItems[itemIndex - j].Price -= c.BasketItems[itemIndex - j].Price * 0.02;
            }
            c.ResetCurrentItemCount();
        }
    }
}

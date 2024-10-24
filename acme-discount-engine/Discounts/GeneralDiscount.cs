using AcmeSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme_discount_engine.Discounts
{
    public class GeneralDiscount : IBasketDiscount
    {
        private List<string> IneligibleList = new List<string> { "T-Shirt", "Keyboard", "Drill", "Chair" };

        public bool IsItemEligibleForDeal(Checkout c)
        {
            return !IneligibleList.Contains(c.currentItem.Name);
        }

        public void RunDiscountOnItem(Checkout basket, int itemIndex)
        {
            // apply general discount on either perishable or non perish
        }
    }
}

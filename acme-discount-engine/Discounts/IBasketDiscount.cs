using AcmeSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme_discount_engine.Discounts
{
    public interface IBasketDiscount
    {
        public bool IsItemEligibleForDeal(Checkout c);

        public void RunDiscountOnItem(Checkout c, int itemIndex);
    }
}

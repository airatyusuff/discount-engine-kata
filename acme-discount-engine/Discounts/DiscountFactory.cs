using AcmeSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme_discount_engine.Discounts
{
    public abstract class DiscountFactory
    {

        public abstract bool isBasketEligibleForDeal(Checkout basket);

        public abstract void runDiscountOnBasketItem(Checkout basket, int itemIndex);
    }
}

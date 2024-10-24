using AcmeSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme_discount_engine.Discounts
{
    public class LoyaltyDiscount
    {
        private bool LoyaltyCard;

        public LoyaltyDiscount(bool LoyaltyCard) {
            this.LoyaltyCard = LoyaltyCard;
        }

        public bool IsBasketEligibleForDeal(Checkout c)
        {
            return LoyaltyCard && c.basketTotal >= 50.00;
        }

        public void ApplyLoyaltyDiscount(Checkout c)
        {
            if (IsBasketEligibleForDeal(c))
            {
                double discount = c.basketTotal * 0.02;
                c.DecreaseBasketTotalBy(discount);
            }
        }
    }
}

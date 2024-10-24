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
            Console.WriteLine("total here: " + c.basketTotal + LoyaltyCard);
            return LoyaltyCard && c.basketTotal >= 50.00;
        }

        public void ApplyLoyaltyDiscount(Checkout c)
        {
            Console.WriteLine("gets here");
            double discount = c.basketTotal * 0.02;

            c.DecreaseBasketTotalBy(discount);
            Console.WriteLine("final: " + c.basketTotal);
        }
    }
}

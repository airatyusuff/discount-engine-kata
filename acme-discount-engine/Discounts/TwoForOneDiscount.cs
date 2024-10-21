using AcmeSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme_discount_engine.Discounts
{
    public class TwoForOneDiscount : DiscountFactory
    {
        private List<string> dealsList = new List<string> { "Freddo" };

        public override bool isBasketEligibleForDeal(Checkout basket)
        {
            return basket.currentItemCount == 3 && dealsList.Contains(basket.currentItem.Name);
        }

        public override void runDiscountOnBasketItem(Checkout basket, int itemIndex)
        {
            basket.BasketItems[itemIndex].Price = 0.00;
            basket.ResetCurrentItemCount();
        }
    }
}

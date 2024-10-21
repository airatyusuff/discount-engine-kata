using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AcmeSharedModels.Checkout;

namespace AcmeSharedModels
{
    public class Checkout
    {
        public List<Item> BasketItems {  get; private set; }
        public Item currentItem { get; private set; }
        public int currentItemCount { get; private set; } = 0;
        public double basketTotal { get; private set; } = 0.00;

        public Checkout(List<Item> items) {
            BasketItems = items;
            currentItem = items[0];
        }

        public double CalculateBasketTotal()
        {
            return BasketItems.Sum(item => item.Price);
        }

        public void UpdateTotal(double total)
        {
            basketTotal += total;
        }
        public void SortItemsInBasketForCheckout()
        {
            BasketItems.Sort((firstItem, nextItem) => firstItem.CompareTo(nextItem));
        }

        public void ProcessTwoForOneDeals(List<string> dealsList)
        {
            currentItemCount = 0;
            for (int i = 0; i < BasketItems.Count; i++)
            {
                if (isItemDifferentFromCurrentItem(i))
                {
                    proceedToNextItemInBasket(i);
                    continue;
                }
                
                currentItemCount++;
                if (isCurrentItemEligibleForTwoForOneDeal(dealsList))
                {
                    runTwoForOneDiscountOnItem(i);
                }
            }
        }

        public void ProcessBulkDiscounts(List<string> dealsList)
        {
            currentItemCount = 0;
            for (int i = 0; i < BasketItems.Count; i++)
            {
                if (isItemDifferentFromCurrentItem(i))
                {
                    proceedToNextItemInBasket(i);
                    continue;
                }
                currentItemCount++;
                if (isCurrentItemEligibleForBulkDeal(dealsList))
                {
                    for (int j = 0; j < 10; j++)
                    {
                        BasketItems[i - j].Price -= BasketItems[i - j].Price * 0.02;
                    }
                    currentItemCount = 0;
                }
            }
        }

        public bool IsBasketEligibleForLoyaltyDiscount(bool hasLoyaltyCard)
        {
            return hasLoyaltyCard && basketTotal >= 50.00;
        }

        private bool isItemDifferentFromCurrentItem(int itemIdex)
        {
            return BasketItems[itemIdex].Name != currentItem.Name;
        }

        private bool isCurrentItemEligibleForTwoForOneDeal(List<string> dealList)
        {
            return currentItemCount == 3 && dealList.Contains(currentItem.Name);
        }

        private bool isCurrentItemEligibleForBulkDeal(List<string> dealList)
        {
            return currentItemCount == 10 && !dealList.Contains(currentItem.Name) && currentItem.Price >= 5.00;
        }

        private void proceedToNextItemInBasket(int itemIndex)
        {
            currentItem = BasketItems[itemIndex];
            currentItemCount = 1;
        }
        private void runTwoForOneDiscountOnItem(int itemIndex)
        {
            BasketItems[itemIndex].Price = 0.00;
            currentItemCount = 0;
        }
    }
}

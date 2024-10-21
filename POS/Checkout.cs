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
        }

        public void UpdateTotal(double total)
        {
            basketTotal += total;
        }
        public void SortItemsInBasketForCheckout()
        {
            BasketItems.Sort((firstItem, nextItem) => firstItem.CompareTo(nextItem));
        }

        public void StartCheckoutProcess()
        {
            currentItem = BasketItems[0];
            currentItemCount = 1;
        }

        public void ProcessTwoForOneDeals(List<string> dealsList)
        {
            for (int i = 1; i < BasketItems.Count; i++)
            {
                if (isCurrentItemSingleQuantity(BasketItems[i]))
                {
                    proceedToNextItemInBasket(BasketItems[i]);
                    continue;
                }
                
                currentItemCount++;
                if (isCurrentItemEligibleForTwoForOneDeal(dealsList))
                {
                    BasketItems[i].Price = 0.00;
                    currentItemCount = 0;
                }
            }
        }

        public void ProcessGeneralDiscounts()
        {
            
        }
        
        private bool isCurrentItemSingleQuantity(Item nextItem)
        {
            return nextItem.Name != currentItem.Name;
        }
        private void proceedToNextItemInBasket(Item nextItem)
        {
            currentItem = nextItem;
            currentItemCount = 1;
        }
        private bool isCurrentItemEligibleForTwoForOneDeal(List<string> dealList)
        {
            return currentItemCount == 3 && dealList.Contains(currentItem.Name);
        }
    }

    public struct BasketItem
    {
        public string Name;
        public int count;
    }
}

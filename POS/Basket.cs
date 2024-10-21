using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeSharedModels
{
    public class Basket
    {
        public List<Item> Items {  get; private set; }
        public string currentItemName { get; private set; }
        public int currentItemCount { get; private set; } = 0;

        public Basket(List<Item> items) {
            Items = items;
        }

        public void SortItemsInBasketForCheckout()
        {
            Items.Sort((firstItem, nextItem) => firstItem.CompareTo(nextItem));
        }

        public void StartCheckoutProcess()
        {
            currentItemName = string.Empty;
            currentItemCount = 0;
        }

        public void ProcessTwoForOneDeals(List<string> dealsList)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (isCurrentItemSingleQuantity(Items[i]))
                {
                    processNextItem(Items[i]);
                    continue;
                }
                
                currentItemCount++;
                if (isCurrentItemEligibleForTwoForOneDeal(dealsList))
                {
                    Items[i].Price = 0.00;
                    currentItemCount = 0;
                }
            }
        }

        private bool isCurrentItemSingleQuantity(Item nextItem)
        {
            return nextItem.Name != currentItemName;
        }
        private void processNextItem(Item item)
        {
            currentItemName = item.Name;
            currentItemCount = 1;
        }

        private bool isCurrentItemEligibleForTwoForOneDeal(List<string> dealList)
        {
            return currentItemCount == 3 && dealList.Contains(currentItemName);
        }
    }

    public struct BasketItem
    {
        public string Name;
        public int count;
    }
}

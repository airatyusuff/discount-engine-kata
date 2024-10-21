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

        public void SetCurrentItemCount(int count)
        {
            currentItemCount = count;
        }

        public void SetCurrentItemName(string name)
        {
            currentItemName = name;
        }
    }

    public struct BasketItem
    {
        public string Name;
        public int count;
    }
}

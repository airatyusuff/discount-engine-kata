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

        public double CalculateCheckoutTotal()
        {
            basketTotal = BasketItems.Sum(item => item.Price);
            return basketTotal;
        }

        public void IncrementCurrentItemCount()
        {
            currentItemCount++;
        }
        public void ResetCurrentItemCount()
        {
            currentItemCount = 0;
        }

        public void IncreaseBasketTotalBy(double amount)
        {
            basketTotal += amount;
        }
        public void DecreaseBasketTotalBy(double amount)
        {
            basketTotal -= amount;
        }

        public void SortItemsInBasketForCheckout()
        {
            BasketItems.Sort((firstItem, nextItem) => firstItem.CompareTo(nextItem));
        }

        public bool isItemDifferentFromCurrentItem(int itemIdex)
        {
            return BasketItems[itemIdex].Name != currentItem.Name;
        }

        public void proceedToNextItemInBasket(int itemIndex)
        {
            currentItem = BasketItems[itemIndex];
            currentItemCount = 1;
        }
    }
}

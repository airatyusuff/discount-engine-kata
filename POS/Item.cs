using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeSharedModels
{
    public class Item : IComparable<Item>
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsPerishable { get; set; }
        public DateTime Date { get; set; }

        public Item(string name, double price, bool isPerishable, DateTime date)
        {
            this.Name = name;
            this.Price = price;
            this.IsPerishable = isPerishable;
            this.Date = date;
        }

        public int CompareTo(Item other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}

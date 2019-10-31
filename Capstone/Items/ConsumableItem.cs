using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public abstract class ConsumableItem : IVendingMachineItem
    {
        public string Name { get; }
        public decimal Price { get; }
        public int Count { get; set; }
        public virtual string DispensedMessage { get; }
        public bool SoldOut
        {
            get
            {
                return (Count <= 0) ? true : false;
            }
        }

        public ConsumableItem(string name, decimal price, int itemsAtStart)
        {
            Name = name;
            Price = price;
            Count = itemsAtStart;
        }

        /// <summary>
        /// Displays the name and price of the item.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string displayItem = $"{Name} - {Price:c}";

            if (SoldOut)
            {
                displayItem = $"{Name} - SOLD OUT.";
            }

            return displayItem;
        }
    }
}

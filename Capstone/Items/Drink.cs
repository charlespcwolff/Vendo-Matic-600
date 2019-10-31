using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Drink : ConsumableItem
    {
        public override string DispensedMessage { get; } = "Glug Glug, Yum!";

        public Drink(string name, decimal price, int itemsAtStart) : base(name, price, itemsAtStart)
        {

        }
    }
}

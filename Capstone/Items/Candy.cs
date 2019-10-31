using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Candy : ConsumableItem
    {
        public override string DispensedMessage { get; } = "Munch Munch, Yum!";

        public Candy(string name, decimal price, int itemsAtStart) : base(name, price, itemsAtStart)
        {

        }
    }
}

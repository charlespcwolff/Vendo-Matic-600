using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Gum : ConsumableItem
    {
        public override string DispensedMessage { get; } = "Chew Chew, Yum!";

        public Gum(string name, decimal price, int itemsAtStart) : base(name, price, itemsAtStart)
        {

        }
    }
}

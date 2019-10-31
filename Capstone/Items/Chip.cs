using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Chip : ConsumableItem
    {

        public override string DispensedMessage { get; } = "Crunch Crunch, Yum!";

        public Chip(string name, decimal price, int itemsAtStart) : base(name, price, itemsAtStart)
        {

        }
    }
}

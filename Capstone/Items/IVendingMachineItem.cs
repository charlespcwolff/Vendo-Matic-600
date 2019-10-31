using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public interface IVendingMachineItem
    {
        string Name { get; }
        decimal Price { get; }
        int Count { get; set; }
        string DispensedMessage { get; }
        bool SoldOut { get; }
    }
}

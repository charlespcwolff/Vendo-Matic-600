using System;

namespace Capstone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string inventoryList = @"..\..\..\Files\VendingMachine.txt";
            string logs = @"..\..\..\Files\Logs.txt";
            string salesReport = @"..\..\..\Files\SalesReport.txt";

            VendingMachine vendingMachine = new VendingMachine(inventoryList, logs, salesReport);

            Menu.MainMenu(vendingMachine);
        }
    }
}

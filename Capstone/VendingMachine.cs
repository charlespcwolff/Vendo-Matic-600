using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public class VendingMachine
    {
        private const string CANDY = "Candy";
        private const string CHIP = "Chip";
        private const string DRINK = "Drink";
        private const string GUM = "Gum";

        private const decimal QUARTER_VALUE = 0.25M;
        private const decimal DIME_VALUE = 0.10M;
        private const decimal NICKEL_VALUE = 0.05M;

        private const int ITEMS_AT_START = 5;

        private string LogFilePath { get; }
        private string ReportFilePath { get; }

        private Dictionary<string, IVendingMachineItem> Slots
                { get; set; } = new Dictionary<string, IVendingMachineItem>();

        public decimal Balance { get; private set; } = 0.00M;

        public VendingMachine(string inventoryFilePath, string logFilePath, string reportFilePath)
        {
            ReadInventory(inventoryFilePath);
            LogFilePath = logFilePath;
            ReportFilePath = reportFilePath;
        }

        /// <summary>
        /// Creates a list of strings for each slot and item, so the inferface can print them.
        /// </summary>
        /// <param name="vendingMachine"></param>
        public List<string> SlotsList()
        {
            List<string> slotStrings = new List<string>();

            foreach (var slot in Slots)
            {
                slotStrings.Add($"{slot.Key}) {slot.Value.ToString()}");
            }

            return slotStrings;
        }

        /// <summary>
        /// Adds money to the vending machine.
        /// </summary>
        /// <param name="dollars"></param>
        public void FeedMoney(int dollars)
        {
            if (dollars > 0)
            {
                Balance += dollars;
                MakeLog("FEED MONEY:", dollars); // Only make a log if user inputs money.
            }
        }

        /// <summary>
        /// Returns remaining balance to customer after transaction(s), favoring larger denominations.
        /// </summary>
        /// <returns></returns>
        public string ReturnChange()
        {
            decimal balanceToBeChanged = Balance;

            int quarters = (int)(Balance / QUARTER_VALUE);
            Balance -= quarters * QUARTER_VALUE;
            int dimes = (int)(Balance / DIME_VALUE);
            Balance -= dimes * DIME_VALUE;
            int nickels = (int)(Balance / NICKEL_VALUE);
            Balance -= nickels * NICKEL_VALUE;

            if (quarters != 0 || dimes != 0 || nickels != 0)
            {
                MakeLog("GIVE CHANGE:", balanceToBeChanged);
            }

            string change = "";
            if (quarters > 0)
            {
                change += $"{quarters} Quarter(s)";

                if(dimes > 0 && nickels > 0)
                {
                    change += ", ";
                }
                else if (dimes > 0 || nickels > 0)
                {
                    change += " and ";
                }
            }
            if (dimes > 0)
            {
                change += $"{dimes} Dime(s)";

                if (nickels > 0)
                {
                    change += " and ";
                }
            }
            if (nickels > 0)
            {
                change += $"{nickels} Nickel(s)";
            }

            return change;
        }

        /// <summary>
        /// Tries to dispense item, and returns a message if it succeds or not.
        /// </summary>
        /// <returns></returns>
        public string DispenseItem(string slot)
        {
            var item = Slots[slot];

            string message = $"Sorry, your balance is not sufficent to purchase " +
                          $"{item.Name}. Please select another item. Or input more money.";
            
            if (!item.SoldOut && Balance >= item.Price)
            {
                message = item.DispensedMessage;
                item.Count--;
                Balance -= item.Price;
                MakeLog(item.Name, item.Price); // Only make a log if an item is dispensed.

            }
            else if (item.SoldOut)
            {
                message = $"Sorry, there isn't any more {item.Name} to dispense. Please select another item.";
            }

            return message;
        }

        /// <summary>
        /// Checks input to see whether slot exists in vending machine.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool SlotExists(string slot)
        {
            return Slots.ContainsKey(slot);
        }

        /// <summary>
        /// Makes a sales report for the vending machine owner.
        /// </summary>
        public void MakeSalesReport()
        {
            Dictionary<string, int> saleReportLines = new Dictionary<string, int>();
            decimal totalSales = 0.00M;

            foreach (var slot in Slots)
            {
                int itemsSold = 0;

                if (slot.Value.Count < ITEMS_AT_START)
                {
                    itemsSold = ITEMS_AT_START - slot.Value.Count;
                    totalSales += itemsSold * slot.Value.Price;
                }

                saleReportLines.Add(slot.Value.Name, itemsSold);
            }

            using (StreamWriter sw = new StreamWriter(ReportFilePath))
            {
                foreach (var line in saleReportLines)
                {
                    sw.WriteLine($"{line.Key}|{line.Value}");
                }

                sw.WriteLine();
                sw.Write($"**TOTAL SALES** {totalSales:c}");
            }
        }

        /// <summary>
        /// Makes a log of the the action performed.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="moneyInvolved"></param>
        private void MakeLog(string action, decimal moneyInvolved)
        {
            using (StreamWriter sw = new StreamWriter(LogFilePath, true))
            {
                sw.WriteLine($"{DateTime.UtcNow} {action} {moneyInvolved:c} {Balance:c}");
            }
        }

        /// <summary>
        /// Takes file path to read a file to set up the items
        /// in their slots on our vending machine.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private void ReadInventory(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string itemString = sr.ReadLine();
                    string[] itemProperties = itemString.Split("|");

                    string slot = itemProperties[0];
                    string type = itemProperties[3];

                    string name = itemProperties[1];
                    decimal price = decimal.Parse(itemProperties[2]);

                    IVendingMachineItem item = null;

                    if (type == CANDY)
                    {
                        item = new Candy(name, price, ITEMS_AT_START);
                    }
                    else if (type == CHIP)
                    {
                        item = new Chip(name, price, ITEMS_AT_START);
                    }
                    else if (type == DRINK)
                    {
                        item = new Drink(name, price, ITEMS_AT_START);
                    }
                    else if (type == GUM)
                    {
                        item = new Gum(name, price, ITEMS_AT_START);
                    }

                    Slots.Add(slot, item);
                }
            }
        }
    }
}

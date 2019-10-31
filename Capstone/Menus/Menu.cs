using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Menu
    {
        private const ConsoleKey DISPLAY_KEY = ConsoleKey.D1;
        private const ConsoleKey PURCHASE_KEY = ConsoleKey.D2;
        private const ConsoleKey EXIT_KEY = ConsoleKey.D3;
        private const ConsoleKey REPORT_KEY = ConsoleKey.D4;

        /// <summary>
        /// Runs main menu for our vending machine. Inputs are location of files for program.
        /// </summary>
        /// <param name="inventoryList"></param>
        /// <param name="logs"></param>
        /// <param name="salesReport"></param>
        public static void MainMenu(VendingMachine vendingMachine)
        {
            bool isExit = false;

            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Vendo-Matic 600!");
                Console.WriteLine("by Umbrella Corp.");
                Console.WriteLine();

                Console.WriteLine("Menu options:");
                Console.WriteLine($"{DISPLAY_KEY.ToString().Substring(1)}. Display items.");
                Console.WriteLine($"{PURCHASE_KEY.ToString().Substring(1)}. Purchase items.");
                Console.WriteLine($"{EXIT_KEY.ToString().Substring(1)}. Exit.");

                var menuSelection = Console.ReadKey().Key;

                if (menuSelection == DISPLAY_KEY)
                {
                    DisplayMenu.DisplayItemsMenu(vendingMachine);
                }
                else if (menuSelection == PURCHASE_KEY)
                {
                    PurchaseMenu.PurchaseItemsMenu(vendingMachine);
                }
                else if (menuSelection == EXIT_KEY)
                {
                    isExit = true;
                }
                else if (menuSelection == REPORT_KEY)
                {
                    vendingMachine.MakeSalesReport();

                    DisplayMessage("A new Sales Report has been generated.");
                }
            }
        }

        /// <summary>
        /// Displays the list of slots.
        /// </summary>
        /// <param name="slotsDisplayList"></param>
        public static void DisplaySlots(List<string> slotsDisplayList)
        {
            foreach (var slot in slotsDisplayList)
            {
                Console.WriteLine(slot);
            }
        }
        /// <summary>
        /// Clears the screen, prints a message, and waits for user. 
        /// </summary>
        /// <param name="message"></param>
        public static void DisplayMessage(string message)
        {
            Console.Clear();
            Console.Write(message);
            Console.ReadKey();
        }
    }
}

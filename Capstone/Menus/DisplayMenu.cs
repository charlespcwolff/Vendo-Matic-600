using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class DisplayMenu
    {
        private const ConsoleKey QUIT_KEY = ConsoleKey.Q;

        /// <summary>
        /// Runs menu to display the items of the vending machine.
        /// </summary>
        public static void DisplayItemsMenu(VendingMachine vendingMachine)
        {
            bool isExit = false;

            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("Current Inventory:");

                Menu.DisplaySlots(vendingMachine.SlotsList());

                Console.WriteLine();
                Console.WriteLine($"{QUIT_KEY}) Return to Main Menu.");
                var userInput = Console.ReadKey().Key;

                if (userInput == QUIT_KEY)
                {
                    isExit = true;
                }
            }
        }
    }
}

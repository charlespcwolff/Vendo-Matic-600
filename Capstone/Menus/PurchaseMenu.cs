using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class PurchaseMenu
    {
        private const ConsoleKey FEED_MONEY_KEY = ConsoleKey.A;
        private const ConsoleKey SELECT_PRODUCT_KEY = ConsoleKey.B;
        private const ConsoleKey QUIT_KEY = ConsoleKey.C;

        /// <summary>
        /// This runs the purchase menu.
        /// </summary>
        /// <param name="vendingmachine"></param>
        public static void PurchaseItemsMenu(VendingMachine vendingMachine)
        {
            bool isExit = false;

            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("Welcome, customer");
                Console.WriteLine($"Your current balance is: {vendingMachine.Balance:c}");
                Console.WriteLine();
                Console.WriteLine("Your options are below:");

                Console.WriteLine($"{FEED_MONEY_KEY}) Feed Money.");
                Console.WriteLine($"{SELECT_PRODUCT_KEY}) Select Product.");
                Console.WriteLine($"{QUIT_KEY}) Finish Transaction.");
                var userInput = Console.ReadKey().Key;

                if (userInput == FEED_MONEY_KEY)
                {
                    FeedMoneyMenu(vendingMachine);
                }
                else if (userInput == SELECT_PRODUCT_KEY)
                {
                    if(vendingMachine.Balance != 0)
                    {
                        SelectProductMenu(vendingMachine);
                    }
                    else
                    {
                        Menu.DisplayMessage("You have to insert money before you can purchase items.");
                    }
                }
                else if (userInput == QUIT_KEY)
                {
                    isExit = true;
                    string changeMesssage = vendingMachine.ReturnChange();

                    // If there is no change to dispense, skip displaying message.
                    if(changeMesssage != "")
                    {
                        Menu.DisplayMessage($"Your change is: \n{changeMesssage}.");
                    }
                }
            }
        }

        public static void FeedMoneyMenu(VendingMachine vendingMachine)
        {
            bool isExit = false;

            while (!isExit)
            {
                Console.Clear();
                Console.Write("How many dollars would you like to add? ");
                string dollarsInput = Console.ReadLine();

                if (int.TryParse(dollarsInput, out int dollars))
                {
                    if (dollars > 0)
                    {
                        vendingMachine.FeedMoney(dollars);
                        Console.WriteLine($"{dollars:c} has been added to your balance.");
                        Console.WriteLine($"Your balance is now {vendingMachine.Balance:c}");
                        Console.ReadKey();
                    }
                    else
                    {
                        Menu.DisplayMessage("Please enter a positive whole dollar amount.");
                    }

                    isExit = true;
                }
                // If user doesn't enter anything, allow user to go back to prior menu.
                else if (dollarsInput == "")
                {
                    isExit = true;
                }
                else
                {
                    Menu.DisplayMessage("Please enter money in whole numerals.");
                }
            }
        }

        public static void SelectProductMenu(VendingMachine vendingMachine)
        {
            bool isExit = false;

            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("Your options are: ");

                Menu.DisplaySlots(vendingMachine.SlotsList());

                Console.WriteLine();
                Console.Write("What would you like to choose? ");
                string userSelection = Console.ReadLine().ToUpper();

                if (vendingMachine.SlotExists(userSelection))
                {
                    string dispensedMessage = vendingMachine.DispenseItem(userSelection);
                    isExit = true;

                    Menu.DisplayMessage(dispensedMessage);
                }
                // If user doesn't enter anything, allow user to go back to prior menu.
                else if (userSelection == "")
                {
                    isExit = true;
                }
                else
                {
                    Menu.DisplayMessage("Please enter the selection letter and number of the slot for your desired item.");
                }
            }
        }
    }
}

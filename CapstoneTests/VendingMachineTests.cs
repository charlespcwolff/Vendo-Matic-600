using Capstone;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineTests
    {
        // Adds test log files to maintain seperation between real operations,
        // and serves as testing for MakeLog().
        private string InventoryTestPath { get; } = @"..\..\..\TestFiles\VendingMachineTest.txt";
        private string LogTestPath { get; } = @"..\..\..\TestFiles\LogsTest.txt";
        private string ReportTestPath { get; } = @"..\..\..\TestFiles\SalesReportTest.txt";

        [TestMethod]
        public void FeedMoney_Allows_Adding_Balance()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);
            Assert.AreEqual(0M, vendingMachine.Balance, "The balance should start at 0.");

            vendingMachine.FeedMoney(5);
            Assert.AreEqual(5M, vendingMachine.Balance, "Should add money to the vending machine.");

            vendingMachine.FeedMoney(5);
            Assert.AreEqual(10M, vendingMachine.Balance, "Should not lose prior balance when adding more money.");
        }

        [TestMethod]
        public void FeedMoney_Prevents_Adding_Negative_Balance()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);
            Assert.AreEqual(0M, vendingMachine.Balance, "The balance should start at 0.");

            vendingMachine.FeedMoney(-5);
            Assert.AreEqual(0M, vendingMachine.Balance, "Cannot add a negative amount of money.");

            vendingMachine.FeedMoney(5);
            Assert.AreEqual(5M, vendingMachine.Balance, "Money has been added to the balance");

            vendingMachine.FeedMoney(-5);
            Assert.AreEqual(5M, vendingMachine.Balance, "Cannot subtract money from the balance.");
        }

        [TestMethod]
        public void SlotExists_Exists()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);

            bool result = vendingMachine.SlotExists("D4");
            Assert.IsTrue(result, "Should see an existing slot.");
        }

        [TestMethod]
        public void SlotExists_Does_Not_Exist()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);

            bool result = vendingMachine.SlotExists("D8");
            Assert.IsFalse(result, "Should not see a nonexistent slot.");
        }

        [TestMethod]
        public void DispenseItems_Removes_Item_When_Proper()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);
            vendingMachine.FeedMoney(50);
            string result = vendingMachine.DispenseItem("D4");
            Assert.AreEqual(49.25M, vendingMachine.Balance, "Should deduct the price from the balance.");
            Assert.AreEqual("Chew Chew, Yum!", result, "Should return the correct message for what it did.");
        }


        [TestMethod]
        public void DispenseItems_Does_Not_Remove_When_Sold_Out()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);
            vendingMachine.FeedMoney(50);
            vendingMachine.DispenseItem("D4");
            vendingMachine.DispenseItem("D4");
            vendingMachine.DispenseItem("D4");
            vendingMachine.DispenseItem("D4");
            vendingMachine.DispenseItem("D4");
            string result = vendingMachine.DispenseItem("D4");
            Assert.AreEqual(46.25M, vendingMachine.Balance, "Nothing should be dispensed so, no cost.");
            Assert.AreEqual($"Sorry, there isn't any more Triplemint to dispense. Please select another item.",
                            result, "Should return the correct message for what it did.");
        }

        [TestMethod]
        public void DispenseItems_Does_Not_Remove_When_Balance_Insufficient()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);

            string result = vendingMachine.DispenseItem("D4");
            Assert.AreEqual(0, vendingMachine.Balance, "Not enough money to buy, so should be no change.");
            Assert.AreEqual($"Sorry, your balance is not sufficent to purchase " +
                            $"Triplemint. Please select another item. Or input more money.",
                            result, "Should return the correct message for what it did.");

            vendingMachine.FeedMoney(1);

            result = vendingMachine.DispenseItem("B2");
            Assert.AreEqual(1, vendingMachine.Balance, "Not enough money to buy, so should be no change.");
            Assert.AreEqual($"Sorry, your balance is not sufficent to purchase " +
                            $"Cowtales. Please select another item. Or input more money.",
                            result, "Should return the correct message for what it did.");
        }

        [TestMethod]
        public void ReturnChange_Returns_Correct_Change()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);
            string result = vendingMachine.ReturnChange();
            Assert.AreEqual("", result, "Should return string with correct change.");

            vendingMachine.FeedMoney(10);
            result = vendingMachine.ReturnChange();
            Assert.AreEqual("40 Quarter(s)", result, "Should favor quarters.");

            vendingMachine.FeedMoney(5);
            vendingMachine.DispenseItem("B1");
            result = vendingMachine.ReturnChange();
            Assert.AreEqual("12 Quarter(s) and 2 Dime(s)", result, "Then it should favor dimes.");

            vendingMachine.FeedMoney(5);
            vendingMachine.DispenseItem("D2");
            result = vendingMachine.ReturnChange();
            Assert.AreEqual("16 Quarter(s) and 1 Nickel(s)", result, "But should allow for nickels.");
        }

        [TestMethod]
        public void ReturnChange_Zeros_Out_Balance()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);
            vendingMachine.ReturnChange();
            Assert.AreEqual(0M, vendingMachine.Balance, "Balance should still be zero.");

            vendingMachine.FeedMoney(10);
            vendingMachine.ReturnChange();
            Assert.AreEqual(0M, vendingMachine.Balance, "Balance should be zero after returning change.");
        }

        [TestMethod]
        public void SlotsList_Should_Return_Correct_List_Of_Strings()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);

            List<String> expected = new List<string>
            {
                "A1) Potato Crisps - $3.05", "A2) Stackers - $1.45", "A3) Grain Waves - $2.75", "A4) Cloud Popcorn - $3.65",
                "B1) Moonpie - $1.80", "B2) Cowtales - $1.50", "B3) Wonka Bar - $1.50", "B4) Crunchie - $1.75",
                "C1) Cola - $1.25", "C2) Dr. Salt - $1.50", "C3) Mountain Melter - $1.50", "C4) Heavy - $1.50",
                "D1) U-Chews - $0.85", "D2) Little League Chew - $0.95", "D3) Chiclets - $0.75", "D4) Triplemint - $0.75"
            };
            List<String> result = vendingMachine.SlotsList();

            CollectionAssert.AreEqual(expected, result, "List wasn't returned as expected.");
        }

        [TestMethod]
        public void MakeSalesReport_GeneratesSalesReport()
        {
            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);

            vendingMachine.FeedMoney(50);
            vendingMachine.DispenseItem("B1");
            vendingMachine.DispenseItem("A1");
            vendingMachine.DispenseItem("A2");
            vendingMachine.DispenseItem("A3");
            vendingMachine.DispenseItem("A4");
            vendingMachine.DispenseItem("C3");
            vendingMachine.DispenseItem("D4");

            vendingMachine.MakeSalesReport();

            List<string> expected = new List<string>
                                    { "Potato Crisps|1", "Stackers|1", "Grain Waves|1", "Cloud Popcorn|1",
                                    "Moonpie|1", "Cowtales|0", "Wonka Bar|0", "Crunchie|0", "Cola|0",
                                    "Dr. Salt|0", "Mountain Melter|1", "Heavy|0", "U-Chews|0",
                                    "Little League Chew|0", "Chiclets|0", "Triplemint|1",
                                    "", "**TOTAL SALES** $14.95"};

            List<string> result = new List<string>();

            using (StreamReader sr = new StreamReader(ReportTestPath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    result.Add(line);
                }
            }

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MakeLog_Test()
        {
            File.Delete(LogTestPath);

            VendingMachine vendingMachine = new VendingMachine(InventoryTestPath, LogTestPath, ReportTestPath);

            vendingMachine.FeedMoney(50);
            vendingMachine.FeedMoney(0);
            vendingMachine.DispenseItem("D4");
            vendingMachine.DispenseItem("A2");
            vendingMachine.ReturnChange();
            vendingMachine.ReturnChange();

            List<string> expected = new List<string>
                                    {
                                    "FEED MONEY: $50.00 $50.00", "Triplemint $0.75 $49.25",
                                    "Stackers $1.45 $47.80", "GIVE CHANGE: $47.80 $0.00"
                                    };

            List<string> result = new List<string>();

            using (StreamReader sr = new StreamReader(LogTestPath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] lineSplit = line.Split(new string[] { "AM", "PM" }, StringSplitOptions.None);

                    line = lineSplit[1].Substring(1);
                    result.Add(line);
                }
            }

            CollectionAssert.AreEqual(expected, result);
        }
    }
}

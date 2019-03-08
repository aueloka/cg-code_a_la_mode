using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace code_a_la_mode
{
    internal static class Constants
    {
        public static readonly Dictionary<Dessert, string> DessertStrings = new Dictionary<Dessert, string>
        {
            {Dessert.Blueberries, "BLUEBERRIES"},
            {Dessert.IceCream, "ICE_CREAM"}
        };

        public static readonly Dictionary<Action, string> ActionStrings = new Dictionary<Action, string>
        {
            {Action.Use, "USE"},
            {Action.Move, "MOVE"},
            {Action.Wait, "WAIT"}
        };

        public static readonly Dictionary<char, Item> ItemDictionary = new Dictionary<char, Item>
        {
            {'#', Item.EmptyTable},
            {'.', Item.WalkableCell},
            {'D', Item.Dishwasher},
            {'W', Item.Window},
            {'B', Item.BlueberryCrate},
            {'I', Item.IceCreamCrate}
        };

        public static readonly Dictionary<Item, char> ItemChars = new Dictionary<Item, char>
        {
            {Item.Dishwasher, 'D'},
            {Item.Window, 'W'},
            {Item.BlueberryCrate, 'B'},
            {Item.IceCreamCrate, 'I'}
        };
    }

    internal class Player
    {
        private static void Main(string[] args)
        {
            string[] inputs;
            int numAllCustomers = int.Parse(ReadInputLine());
            for (int i = 0; i < numAllCustomers; i++)
            {
                inputs = ReadInputLine().Split(' ');
                string customerItem = inputs[0]; // the food the customer is waiting for
                int customerAward = int.Parse(inputs[1]); // the number of points awarded for delivering the food
                Console.Error.WriteLine($"Customer {i}: {customerItem} -> {customerAward}");
            }

            IKitchen kitchen = new ArrayKitchen();
            
            for (int i = 0; i < 7; i++)
            {
                string kitchenLine = ReadInputLine();
                Console.Error.WriteLine(kitchenLine);
                kitchen.AddLine(kitchenLine);
            }

            Console.Error.WriteLine($"Dishwasher POS: {kitchen.ItemPosition[Item.Dishwasher]}");
            Console.Error.WriteLine($"Window POS: {kitchen.ItemPosition[Item.Window]}");
            Console.Error.WriteLine($"Blueberry Crate POS: {kitchen.ItemPosition[Item.BlueberryCrate]}");
            Console.Error.WriteLine($"Ice cream crate POS: {kitchen.ItemPosition[Item.IceCreamCrate]}");
            
            // game loop
            while (true)
            {
                int turnsRemaining = int.Parse(ReadInputLine());
                inputs = ReadInputLine().Split(' ');
                int playerX = int.Parse(inputs[0]);
                int playerY = int.Parse(inputs[1]);
                string playerItem = inputs[2];
                inputs = ReadInputLine().Split(' ');
                int partnerX = int.Parse(inputs[0]);
                int partnerY = int.Parse(inputs[1]);
                string partnerItem = inputs[2];
                int numTablesWithItems =
                    int.Parse(ReadInputLine()); // the number of tables in the kitchen that currently hold an item
                for (int i = 0; i < numTablesWithItems; i++)
                {
                    inputs = ReadInputLine().Split(' ');
                    int tableX = int.Parse(inputs[0]);
                    int tableY = int.Parse(inputs[1]);
                    string item = inputs[2];
                }

                inputs = ReadInputLine().Split(' ');
                string ovenContents = inputs[0]; // ignore until wood 1 league
                int ovenTimer = int.Parse(inputs[1]);
                int numCustomers = int.Parse(ReadInputLine()); // the number of customers currently waiting for food
                for (int i = 0; i < numCustomers; i++)
                {
                    inputs = ReadInputLine().Split(' ');
                    string customerItem = inputs[0];
                    int customerAward = int.Parse(inputs[1]);
                }

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");


                // MOVE x y
                // USE x y
                // WAIT
                Console.WriteLine("WAIT");
            }
        }

        private static string ReadInputLine()
        {
            return Console.ReadLine();
        }

        private string GetDessert(Dessert dessert)
        {
            return Constants.DessertStrings[dessert];
        }

        private string DoAction(Action action, string message = "", params object[] args)
        {
            return $"{Constants.ActionStrings[action]}{string.Join(" ", args)}; {message}";
        }
    }

    internal class Customer
    {
    }

    internal struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x = 0, int y = 0)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"{{{this.X}, {this.Y}}}";
        }
    }

    internal class Cell
    {
    }

    internal enum Item
    {
        EmptyTable,
        WalkableCell,
        Dishwasher,
        Window,
        BlueberryCrate,
        IceCreamCrate
    }

    internal class Utility
    {
    }

    internal enum Dessert
    {
        Blueberries,
        IceCream
    }

    internal enum Action
    {
        Move,
        Use,
        Wait
    }

    internal interface IKitchen
    {
        Dictionary<Item, Point> ItemPosition { get; }

        void AddLine(string kitchenLine);

        Item GetItem(Point point);

        IKitchen Clone();
    }

    internal class ArrayKitchen : IKitchen
    {
        private int _addedLines;
        private readonly string[] _kitchen = new string[11];

        public Dictionary<Item, Point> ItemPosition { get; } = new Dictionary<Item, Point>();

        public void AddLine(string kitchenLine)
        {
            UpdateItemPosition(kitchenLine, Item.Dishwasher);
            UpdateItemPosition(kitchenLine, Item.Window);
            UpdateItemPosition(kitchenLine, Item.BlueberryCrate);
            UpdateItemPosition(kitchenLine, Item.IceCreamCrate);

            _kitchen[_addedLines++] = kitchenLine;
        }

        public Item GetItem(Point point)
        {
            char value = _kitchen[point.Y][point.X];
            return Constants.ItemDictionary[value];
        }

        public IKitchen Clone()
        {
            throw new NotImplementedException();
        }

        private void UpdateItemPosition(string kitchenLine, Item item)
        {
            int itemIndex = kitchenLine.IndexOf(Constants.ItemChars[item]);

            if (itemIndex == -1)
            {
                return;
            }
            
            ItemPosition[item] = new Point(itemIndex, _addedLines);
        }
    }
}
using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace code_a_la_mode
{
    internal enum Action
    {
        Move,
        Use,
        Wait
    }

    internal enum Dessert
    {
        Blueberries,
        Blueberry,
        ChoppedDough,
        ChoppedStrawberries,
        Croissant,
        Dough,
        IceCream,
        RawTart,
        Strawberries,
        Strawberry,
        Tart
    }

    internal enum Equipment
    {
        Dishwasher,
        Window,
        BlueberryCrate,
        IceCreamCrate,
        StrawberryCrate,
        ChoppingBoard,
        Oven
    }

    internal enum MapItem
    {
        WalkableCell,
        EmptyTable,
        Dishwasher,
        Window,
        BlueberryCrate,
        IceCreamCrate
    }

    internal static class Constants
    {
        public static readonly IDictionary<Dessert, string> DessertStrings = new Dictionary<Dessert, string>
        {
            {Dessert.Blueberries, "BLUEBERRIES"},
            {Dessert.Blueberry, "BLUEBERRY"},
            {Dessert.ChoppedDough, "CHOPPED_DOUGH"},
            {Dessert.ChoppedStrawberries, "CHOPPED_STRAWBERRIES"},
            {Dessert.Croissant, "CROISSANT"},
            {Dessert.Dough, "DOUGH"},
            {Dessert.IceCream, "ICE_CREAM"},
            {Dessert.RawTart, "RAW_TART"},
            {Dessert.Strawberries, "STRAWBERRIES"},
            {Dessert.Strawberry, "STRAWBERRY"},
            {Dessert.Tart, "TART"}
        };

        public static readonly IDictionary<string, Dessert> DessertDictionary = new Dictionary<string, Dessert>
        {
            {"BLUEBERRIES", Dessert.Blueberries},
            {"BLUEBERRY", Dessert.Blueberry},
            {"CHOPPED_DOUGH", Dessert.ChoppedDough},
            {"CHOPPED_STRAWBERRIES", Dessert.ChoppedStrawberries},
            {"CROISSANT", Dessert.Croissant},
            {"DOUGH", Dessert.Dough},
            {"ICE_CREAM", Dessert.IceCream},
            {"RAW_TART", Dessert.RawTart},
            {"STRAWBERRIES", Dessert.Strawberries},
            {"STRAWBERRY", Dessert.Strawberry},
            {"TART", Dessert.Tart}
        };

        public static readonly IDictionary<Action, string> ActionStrings = new Dictionary<Action, string>
        {
            {Action.Use, "USE"},
            {Action.Move, "MOVE"},
            {Action.Wait, "WAIT"}
        };

        public static readonly IDictionary<char, MapItem> MapItemDictionary = new Dictionary<char, MapItem>
        {
            {'#', MapItem.EmptyTable},
            {'.', MapItem.WalkableCell},
            {'D', MapItem.Dishwasher},
            {'W', MapItem.Window},
            {'B', MapItem.BlueberryCrate},
            {'I', MapItem.IceCreamCrate}
        };

        public static readonly IDictionary<Equipment, char> KitchenEquipmentChars = new Dictionary<Equipment, char>
        {
            {Equipment.Dishwasher, 'D'},
            {Equipment.Window, 'W'},
            {Equipment.BlueberryCrate, 'B'},
            {Equipment.IceCreamCrate, 'I'}
        };
    }

    internal struct Requirements
    {
        public Dessert[] Desserts { get; set; }
        public Equipment Equipment { get; set; }
        private readonly bool _usesEquipment;

        public Requirements(bool usesEquipment = true) : this()
        {
            this._usesEquipment = usesEquipment;
        }

        public bool NeedsDesserts()
        {
            return Desserts != null;
        }

        public bool NeedsEquipment()
        {
            return _usesEquipment;
        }

        public static Requirements DishRequirement()
        {
            return new Requirements
            {
                Equipment = Equipment.Dishwasher
            };
        }

        public static Requirements SingleEquipmentRequirement(Equipment equipment)
        {
            return new Requirements
            {
                Equipment = equipment
            };
        }

        public static Requirements DoubleDessertRequirement(Dessert a, Dessert b)
        {
            return new Requirements
            {
                Desserts = new[] {a, b}
            };
        }

        public static Requirements SingleDessertAndEquipmentRequirement(Dessert dessert, Equipment equipment)
        {
            return new Requirements
            {
                Desserts = new[] {dessert},
                Equipment = equipment
            };
        }
    }

    internal struct GameAction
    {
        public Action Action { get; }

        public Point Position { get; set; }

        public string Message { get; set; }

        public GameAction(Action action) : this()
        {
            Action = action;
        }

        public override string ToString()
        {
            string positionString = Action != Action.Wait ? $" {Position.X} {Position.Y}" : "";
            return $"{Constants.ActionStrings[Action]}{positionString}; {Message}";
        }

        public static GameAction MoveAction(Point point, string message = "")
        {
            return new GameAction(Action.Move)
            {
                Position = point,
                Message = message
            };
        }

        public static GameAction UseAction(Point point, string message = "")
        {
            return new GameAction(Action.Use)
            {
                Position = point,
                Message = message
            };
        }

        public static GameAction WaitAction(string message = "")
        {
            return new GameAction(Action.Wait)
            {
                Message = message
            };
        }
    }

    internal struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{{{X}, {Y}}}";
        }
    }

    internal interface IKitchen
    {
        Dictionary<Equipment, Point> EquipmentPosition { get; }

        void AddLine(string kitchenLine);
    }

    internal class ArrayKitchen : IKitchen
    {
        private int _addedLines;
        private readonly string[] _kitchen = new string[11];

        public Dictionary<Equipment, Point> EquipmentPosition { get; } = new Dictionary<Equipment, Point>();

        public void AddLine(string kitchenLine)
        {
            UpdateEquipmentPosition(kitchenLine, Equipment.Dishwasher);
            UpdateEquipmentPosition(kitchenLine, Equipment.Window);
            UpdateEquipmentPosition(kitchenLine, Equipment.BlueberryCrate);
            UpdateEquipmentPosition(kitchenLine, Equipment.IceCreamCrate);

            _kitchen[_addedLines++] = kitchenLine;
        }

        private void UpdateEquipmentPosition(string kitchenLine, Equipment equipment)
        {
            int itemIndex = kitchenLine.IndexOf(Constants.KitchenEquipmentChars[equipment]);

            if (itemIndex == -1)
            {
                return;
            }

            EquipmentPosition[equipment] = new Point(itemIndex, _addedLines);
        }
    }

    internal class Cell
    {
    }

    internal class Chef
    {
        public static readonly IDictionary<Dessert, Requirements> CookBook = new Dictionary<Dessert, Requirements>
        {
            {Dessert.Blueberries, Requirements.SingleEquipmentRequirement(Equipment.BlueberryCrate)},
            {Dessert.IceCream, Requirements.SingleEquipmentRequirement(Equipment.IceCreamCrate)},
            {Dessert.RawTart, Requirements.DoubleDessertRequirement(Dessert.ChoppedDough, Dessert.Blueberry)},
            {
                Dessert.ChoppedStrawberries,
                Requirements.SingleDessertAndEquipmentRequirement(Dessert.Strawberries, Equipment.ChoppingBoard)
            },
            {
                Dessert.ChoppedDough,
                Requirements.SingleDessertAndEquipmentRequirement(Dessert.Dough, Equipment.ChoppingBoard)
            },
            {Dessert.Tart, Requirements.SingleDessertAndEquipmentRequirement(Dessert.RawTart, Equipment.Oven)},
            {Dessert.Croissant, Requirements.SingleDessertAndEquipmentRequirement(Dessert.Dough, Equipment.Oven)},
        };
        
        private static IDictionary<string, Requirements[]> RecipeLookup = new Dictionary<string, Requirements[]>();

        public static Requirements[] GetRecipe(string item)
        {
            if (RecipeLookup.ContainsKey(item))
            {
                return RecipeLookup[item];
            }

            string[] items = item.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
            Requirements[] requirementsList = new Requirements[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                string foodItem = items[i];

                if (foodItem == "DISH")
                {
                    requirementsList[i] = Requirements.DishRequirement();
                    continue;
                }

                Dessert dessert = Constants.DessertDictionary[foodItem];
                requirementsList[i] = CookBook[dessert];
            }

            RecipeLookup[item] = requirementsList;
            return requirementsList;
        }


        /*
         * Go to Equipment (Dishwasher/Table/Oven/Chopping Board/Food Crate)
         * Go to Equipment with Item
         * Use Equipment
         */
    }

    internal class Customer
    {
    }

    internal class Item
    {
    }

    internal static class Player
    {
        private static void Main(string[] args)
        {
            string[] inputs;
            int numAllCustomers = int.Parse(ReadInputLine());
            Dictionary<string, Requirements[]> recipeLookup = new Dictionary<string, Requirements[]>();

            for (int i = 0; i < numAllCustomers; i++)
            {
                inputs = ReadInputLine().Split(' ');
                string customerItem = inputs[0]; // the food the customer is waiting for
                int customerAward = int.Parse(inputs[1]); // the number of points awarded for delivering the food
                Console.Error.WriteLine($"Customer {i}: {customerItem} -> {customerAward}");

                if (recipeLookup.ContainsKey(customerItem))
                {
                    continue;
                }

                recipeLookup[customerItem] = Chef.GetRecipe(customerItem);
                Console.Error.WriteLine(
                    $"Recipe ({customerItem}) has {recipeLookup[customerItem].Length} requirements.");
            }

            Console.Error.WriteLine();
            IKitchen kitchen = new ArrayKitchen();

            for (int i = 0; i < 7; i++)
            {
                string kitchenLine = ReadInputLine();
                Console.Error.WriteLine(kitchenLine);
                kitchen.AddLine(kitchenLine);
            }

            Console.Error.WriteLine($"Dishwasher POS: {kitchen.EquipmentPosition[Equipment.Dishwasher]}");
            Console.Error.WriteLine($"Window POS: {kitchen.EquipmentPosition[Equipment.Window]}");
            Console.Error.WriteLine($"Blueberry Crate POS: {kitchen.EquipmentPosition[Equipment.BlueberryCrate]}");
            Console.Error.WriteLine($"Ice cream crate POS: {kitchen.EquipmentPosition[Equipment.IceCreamCrate]}");

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

                // MOVE x y
                // USE x y
                // WAIT
                Console.WriteLine(GameAction.WaitAction());
            }
        }

        private static string ReadInputLine()
        {
            return Console.ReadLine();
        }

        private static string GetDessert(Dessert dessert)
        {
            return Constants.DessertStrings[dessert];
        }
    }
}
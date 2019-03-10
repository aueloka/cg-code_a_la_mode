using System;
using System.Collections.Generic;
using System.Linq;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace code_a_la_mode
{
    #region Enums
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
        IceCreamCrate,
        Player,
        Partner
    }
    #endregion

    #region Statics
    internal static class Constants
    {
        public static string Dish = "DISH";

        public static string None = "NONE";

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
            {'I', MapItem.IceCreamCrate},
            {'0', MapItem.Player},
            {'1', MapItem.Partner}
        };

        public static readonly IDictionary<Equipment, char> KitchenEquipmentChars = new Dictionary<Equipment, char>
        {
            {Equipment.Dishwasher, 'D'},
            {Equipment.Window, 'W'},
            {Equipment.BlueberryCrate, 'B'},
            {Equipment.IceCreamCrate, 'I'}
        };
    }

    internal static class Chef
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

        private static IDictionary<string, Composition> ItemComposition = new Dictionary<string, Composition>();

        public static Composition GetComposition(string item)
        {
            if (item == Constants.None)
            {
                return Composition.Invalid;
            }

            if (ItemComposition.ContainsKey(item))
            {
                return ItemComposition[item];
            }

            string[] items = item.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);


            Composition composition = new Composition(item);
            int dessertCount = items.Length;

            if (items.First() == Constants.Dish)
            {
                composition.HasDish = true;
                dessertCount--;
            }

            Dessert[] desserts = new Dessert[dessertCount];

            for (int i = composition.HasDish ? 1 : 0; i < dessertCount + (composition.HasDish ? 1 : 0); i++)
            {
                string foodItem = items[i];

                if (foodItem == Constants.Dish)
                {
                    i--;
                    continue;
                }

                Dessert dessert = Constants.DessertDictionary[foodItem];
                desserts[i - (composition.HasDish ? 1 : 0)] = dessert;
            }

            composition.Desserts = desserts;
            ItemComposition[item] = composition;
            return composition;
        }
    }

    internal static class Validator
    {
        public static bool CanUseWithEquipment(string item, Equipment equipment)
        {
            switch (equipment)
            {
                case Equipment.BlueberryCrate:
                    {
                        return item.Contains(Constants.Dish) || 
                        item.Contains(Constants.None) || 
                        item == Constants.DessertStrings[Dessert.ChoppedDough];
                    }
                case Equipment.ChoppingBoard:
                    {
                        return item == Constants.DessertStrings[Dessert.Strawberries] ||
                            item == Constants.DessertStrings[Dessert.Dough];
                    }
                default:
                    return true;
            }
        }
    }

    internal static class WatchDog
    {
        public static System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();
    }
    #endregion

    #region Structs
    internal struct Composition
    {
        public string Name { get; }

        public bool HasDish { get; set; }

        public Dessert[] Desserts { get; set; }

        public bool IsInvalid { get; private set; }

        public static Composition Invalid = new Composition { IsInvalid = true };

        public Composition(string name) : this()
        {
            Name = name;
            HasDish = false;
            Desserts = Array.Empty<Dessert>();
            IsInvalid = false;
        }

        /**
         * Things in this that are not in that       
         **/
        public Composition GetDiff(Composition other)
        {
            if (other.Desserts.Except(this.Desserts).Any())
            {
                return Composition.Invalid;
            }

            return new Composition("")
            {
                HasDish = this.HasDish && !other.HasDish,
                Desserts = this.Desserts.Except(other.Desserts).ToArray()
            };
        }

        public override string ToString()
        {
            return $"{{Name: {Name}; Has Dish: {HasDish}; Desserts: {Desserts.Length}}}";
        }

        public static Composition DishedComposition(Dessert[] desserts)
        {
            return new Composition("")
            {
                HasDish = true,
                Desserts = desserts
            };
        }

        public static Composition RawComposition(Dessert[] desserts)
        {
            return new Composition("")
            {
                HasDish = false,
                Desserts = desserts
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

    internal struct GameState
    {
        public int TurnsLeft { get; set; }
        public int DishesOnTables { get; set; }
        public Player Player { get; set; }
        public Player Partner { get; set; }
        public string[] ItemsOnTable { get; set; }
        public IDictionary<string, Point> TableItemLocation { get; set; }
        public Order[] WaitingOrders { get; set; }
    }

    internal struct Order
    {
        public string Item { get; set; }
        public int AwardPoints { get; set; }

        public static bool operator >(Order a, Order b)
        {
            return a.AwardPoints > b.AwardPoints;
        }

        public static bool operator <(Order a, Order b)
        {
            return a.AwardPoints < b.AwardPoints;
        }
    }

    internal struct Player
    {
        public Point Position { get; set; }
        public string Item { get; set; }

        public Player(int x, int y, string item) : this()
        {
            Position = new Point(x, y);
            Item = item;
        }
    }

    internal struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsValid => X >= 0 && X < 11 && Y >= 0 && Y < 11;
        public Point[] AdjacentPoints
        {
            get
            {
                return new Point[]
                {
                    new Point(X - 1, Y),
                    new Point(X - 1, Y - 1),
                    new Point(X, Y - 1),
                    new Point(X + 1, Y - 1),
                    new Point(X + 1, Y),
                    new Point(X + 1, Y + 1),
                    new Point(X, Y + 1),
                    new Point(X - 1, Y + 1),

                };
            }
        }

        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public int DistanceTo(Point other)
        {
            return Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));
        }

        public bool IsAdjacentTo(Point other)
        {
            return this.DistanceTo(other) < 2;
        }

        //public static bool operator ==(Point a, Point b)
        //{
        //    return a.X == b.X && a.Y == b.Y;
        //}

        //public static bool operator !=(Point a, Point b)
        //{
        //    return !(a==b);
        //}

        public override string ToString()
        {
            return $"{{{X}, {Y}}}";
        }
    }

    internal struct Requirements
    {
        public Dessert[] Desserts { get; set; }
        public Equipment Equipment { get; set; }
        private readonly bool _usesEquipment;

        public bool NeedsDesserts
        {
            get
            {
                return Desserts != null;
            }
        }

        public bool NeedsEquipment
        {
            get
            {
                return _usesEquipment;
            }
        }

        public Requirements(bool usesEquipment = true) : this()
        {
            this._usesEquipment = usesEquipment;
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
                Desserts = new[] { a, b }
            };
        }

        public static Requirements SingleDessertAndEquipmentRequirement(Dessert dessert, Equipment equipment)
        {
            return new Requirements
            {
                Desserts = new[] { dessert },
                Equipment = equipment
            };
        }
    }
    #endregion

    internal interface IKitchen
    {
        Dictionary<Equipment, Point> EquipmentPosition { get; }

        void AddLine(string kitchenLine);

        MapItem GetItem(Point point);

        Point ClosestEmptyTableToPoint(Point point);

        void AddTemporaryItem(Point point, string item);

        void ClearTemporaryItems();

        void RemoveTemporaryItem(Point point);
    }

    internal class ArrayKitchen : IKitchen
    {
        private int _addedLines;
        private readonly string[] _kitchen = new string[11];
        private IDictionary<Point, string> _temporaryItems = new Dictionary<Point, string>();

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

        public Point ClosestEmptyTableToPoint(Point point)
        {
            //Using BFS cus the map guarantees that a table will be found at depth = 1
            //If this changes, we may switch to A*

            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(point);

            while (queue.Any())
            {
                Point currentPoint = queue.Dequeue();

                if (GetItem(currentPoint) == MapItem.EmptyTable)
                {
                    return currentPoint;
                }

                foreach (var adjacentPoint in point.AdjacentPoints)
                {
                    if (GetItem(adjacentPoint) == MapItem.EmptyTable && !_temporaryItems.ContainsKey(adjacentPoint))
                    {
                        return adjacentPoint;
                    }

                    if (!adjacentPoint.IsValid || GetItem(adjacentPoint) != MapItem.WalkableCell)
                    {
                        continue;
                    }

                    queue.Enqueue(adjacentPoint);
                }
            }

            //No empty tables found
            return new Point(-1, -1);
        }

        public MapItem GetItem(Point point)
        {
            return Constants.MapItemDictionary[_kitchen[point.Y][point.X]];
        }

        public void AddTemporaryItem(Point point, string item)
        {
            _temporaryItems[point] = item;
        }

        public void ClearTemporaryItems()
        {
            _temporaryItems.Clear();
        }

        public void RemoveTemporaryItem(Point point)
        {
            _temporaryItems.Remove(point);
        }
    }


    #region Solvers
    internal static class Randomizer
    {
        private static readonly Random random = new Random(DateTime.Now.Millisecond);

        public static Random Get()
        {
            return random;
        }
    }

    internal interface ISolver
    {
        GameAction Solve(IKitchen kitchen, GameState gameState);
    }

    internal class RandomSolver : ISolver
    {
        public GameAction Solve(IKitchen kitchen, GameState gameState)
        {
            Action action = (Action)Randomizer.Get().Next(3);

            GameAction gameAction = new GameAction(action)
            {
                Position = new Point(Randomizer.Get().Next(11), Randomizer.Get().Next(7)),
                Message = "Random :)"
            };

            return GameAction.UseAction(new Point(0, 6), "Check Use");
        }
    }

    internal class ManualSolver : ISolver
    {
        public GameAction Solve(IKitchen kitchen, GameState gameState)
        {
            //TODO
            //Get top customer
            //Get items
            //Make items
            //Return

            var hand = gameState.Player.Item;
            var handComposition = Chef.GetComposition(hand);

            if (handComposition.IsInvalid)
            {
                var tableItems = gameState.ItemsOnTable;

                //check for deliverable items on tables
                foreach (var item in tableItems)
                {
                    //Check for finished orders
                    if (Array.Exists(gameState.WaitingOrders, order => order.Item == item))
                    {
                        //TODO: Pick closest.
                        return GameAction.UseAction(gameState.TableItemLocation[item], "Found finished item on table.");
                    }
                }

                //Check for almost finished items for a customer
                foreach (var order in gameState.WaitingOrders)
                {
                    var orderComposition = Chef.GetComposition(order.Item);

                    foreach (var item in tableItems)
                    {
                        var itemComposition = Chef.GetComposition(item);
                        var diff = orderComposition.GetDiff(itemComposition);

                        if (diff.IsInvalid || !diff.Desserts.Any())
                        {
                            continue;
                        }

                        //TODO: Pick closest.
                        //return GameAction.UseAction(gameState.TableItemLocation[item].First(), "Found almost finished item on table.");

                        //TODO: Pick easiest/closest desset to make/get
                        var dessertToGet = diff.Desserts.First();
                        var requirements = Chef.CookBook[dessertToGet];

                        while (requirements.NeedsDesserts)
                        {
                            //TODO: Check oven/table for desserts
                            //If they don't exist (or any doesn't exist), get requirements(TODO: for the easiest do make/get)
                            //If they do exist, go get the closest one and take a dish and get the others
                            dessertToGet = requirements.Desserts.First();
                            requirements = Chef.CookBook[dessertToGet];
                        }

                        if (!requirements.NeedsDesserts)
                        {
                            //Use equipment
                            return GameAction.UseAction(kitchen.EquipmentPosition[requirements.Equipment], "Going to use equipment");
                        }
                    }
                }

                //Get top order
                var top = gameState.WaitingOrders.First();
                var composition = Chef.GetComposition(top.Item);

                //Get a dish
                //TODO: Check for closest pickable composition item
                return GameAction.UseAction(kitchen.EquipmentPosition[Equipment.Dishwasher], "Going to get dish.");
            }

            //if holding finished order, deliver it
            if (Array.Exists(gameState.WaitingOrders, order => order.Item == hand))
            {
                return GameAction.UseAction(kitchen.EquipmentPosition[Equipment.Window], "Going to deliver order.");
            }

            //if holding a plain dessert, go get a dish, if not, drop item on table
            if (!handComposition.HasDish)
            {
                //Check for almost finished items for a customer
                foreach (var order in gameState.WaitingOrders)
                {
                    var orderComposition = Chef.GetComposition(order.Item);

                    foreach (var item in gameState.ItemsOnTable)
                    {
                        var itemComposition = Chef.GetComposition(item);
                        var diff = orderComposition.GetDiff(itemComposition);

                        if (diff.IsInvalid || !diff.Desserts.Any())
                        {
                            continue;
                        }

                        //Check if handcomposition can complete it
                        var handDiff = diff.GetDiff(handComposition);
                        if (handDiff.IsInvalid)
                        {
                            continue;
                        }

                        if (handDiff.Desserts.Length < diff.Desserts.Length && itemComposition.HasDish)
                        {
                            return GameAction.UseAction(gameState.TableItemLocation[item], "Going to add dessert to unfinished dish.");
                        }
                    }
                }

                //Get dish
                return GameAction.UseAction(kitchen.EquipmentPosition[Equipment.Dishwasher], "Going to place item in dish.");
            }
            else if (handComposition.Desserts.Any())
            {
                Console.Error.WriteLine("Getting closest table");
                Console.Error.WriteLine(handComposition);
                Console.Error.WriteLine(string.Join(" ", handComposition.Desserts));
                //TODO: Drop close to useful point of continuation.
                return GameAction.UseAction(kitchen.ClosestEmptyTableToPoint(kitchen.EquipmentPosition[Equipment.Window]), "Dropping partially made dish close to window.");
            }
            else
            {
                foreach (var order in gameState.WaitingOrders)
                {
                    var orderComposition = Chef.GetComposition(order.Item);

                    //TODO: Pick closest.
                    //return GameAction.UseAction(gameState.TableItemLocation[item].First(), "Found almost finished item on table.");

                    //TODO: Pick easiest/closest desset to make/get
                    var dessertToGet = orderComposition.Desserts.First();
                    var requirements = Chef.CookBook[dessertToGet];

                    while (requirements.NeedsDesserts)
                    {
                        //TODO: Check oven/table for desserts
                        //If they don't exist (or any doesn't exist), get requirements(TODO: for the easiest do make/get)
                        //If they do exist, go get the closest one and take a dish and get the others
                        dessertToGet = requirements.Desserts.First();
                        requirements = Chef.CookBook[dessertToGet];
                    }

                    //Use equipment
                    return GameAction.UseAction(kitchen.EquipmentPosition[requirements.Equipment], "Going to use equipment");
                }
            }

            return GameAction.WaitAction("Nothing to Do. Waiting...");
        }
    }
    #endregion

    internal static class Solution
    {
        private static void Main_()
        {
            IKitchen k = new ArrayKitchen();
            k.AddTemporaryItem(new Point(1, 1), "TEMP");
            k.RemoveTemporaryItem(new Point(1, 1));
            Console.ReadLine();
        }

        private static void Main(string[] args)
        {
            ISolver solver = new ManualSolver();

            string[] inputs;
            int numAllCustomers = int.Parse(ReadInputLine());

            for (int i = 0; i < numAllCustomers; i++)
            {
                inputs = ReadInputLine().Split(' ');
                string customerItem = inputs[0]; // the food the customer is waiting for
                int customerAward = int.Parse(inputs[1]); // the number of points awarded for delivering the food
                Console.Error.WriteLine($"Customer {i}: {customerItem} -> {customerAward}");
                Chef.GetComposition(customerItem);
                Console.Error.WriteLine($"Dish: {Chef.GetComposition(customerItem)}");
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

            var watch = WatchDog.Stopwatch;

            // game loop
            while (true)
            {
                watch.Reset();
                watch.Start();

                GameState gameState = new GameState
                {
                    TurnsLeft = int.Parse(ReadInputLine())
                };

                kitchen.ClearTemporaryItems();

                inputs = ReadInputLine().Split(' ');
                gameState.Player = new Player(int.Parse(inputs[0]), int.Parse(inputs[1]), inputs[2]);

                inputs = ReadInputLine().Split(' ');
                gameState.Partner = new Player(int.Parse(inputs[0]), int.Parse(inputs[1]), inputs[2]);

                // the number of tables in the kitchen that currently hold an item
                int numTablesWithItems = int.Parse(ReadInputLine());

                gameState.ItemsOnTable = new string[numTablesWithItems];
                gameState.TableItemLocation = new Dictionary<string, Point>(numTablesWithItems);

                for (int i = 0; i < numTablesWithItems; i++)
                {
                    inputs = ReadInputLine().Split(' ');
                    int tableX = int.Parse(inputs[0]);
                    int tableY = int.Parse(inputs[1]);
                    string item = inputs[2];

                    gameState.ItemsOnTable[i] = item;

                    //possibly deleting other existing point for similar item. (Maybe store closest to something)
                    Point tableLocation = new Point(tableX, tableY);
                    gameState.TableItemLocation[item] = tableLocation;
                    kitchen.AddTemporaryItem(tableLocation, item);

                    if (item.Contains(Constants.Dish))
                    {
                        gameState.DishesOnTables++;
                    }
                }

                inputs = ReadInputLine().Split(' ');
                string ovenContents = inputs[0]; // ignore until wood 1 league
                int ovenTimer = int.Parse(inputs[1]);
                int numCustomers = int.Parse(ReadInputLine()); // the number of customers currently waiting for food
                gameState.WaitingOrders = new Order[numCustomers];

                for (int i = 0; i < numCustomers; i++)
                {
                    inputs = ReadInputLine().Split(' ');
                    string customerItem = inputs[0];
                    int customerAward = int.Parse(inputs[1]);
                    gameState.WaitingOrders[i] = new Order
                    {
                        Item = customerItem,
                        AwardPoints = customerAward
                    };

                    Console.Error.WriteLine($"Customer Waiting: {customerItem} -> {customerAward}");
                }

                Array.Sort(gameState.WaitingOrders, delegate (Order a, Order b)
                {
                    return b.AwardPoints - a.AwardPoints;
                });

                watch.Stop();
                var initTime = watch.ElapsedMilliseconds;
                watch.Reset();
                watch.Start();
                Console.WriteLine(solver.Solve(kitchen, gameState));
                watch.Stop();
                Console.Error.WriteLine($"Initialization Time: {initTime} ms; Execution Time: {watch.ElapsedMilliseconds} ms");
            }
        }

        private static string ReadInputLine()
        {
            return Console.ReadLine();
        }
    }
}
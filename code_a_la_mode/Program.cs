using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        Partner,
        StrawberryCrate,
        ChoppingBoard
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

        public static readonly ICollection<Equipment> KitchenEquipments = new Equipment[]
        {
            Equipment.BlueberryCrate,
            Equipment.ChoppingBoard,
            Equipment.Dishwasher,
            Equipment.IceCreamCrate,
            Equipment.Oven,
            Equipment.StrawberryCrate,
            Equipment.Window
        };

        public static readonly ICollection<Dessert> Desserts = new Dessert[]
        {
            Dessert.Blueberries,
            Dessert.Blueberry,
            Dessert.ChoppedDough,
            Dessert.ChoppedStrawberries,
            Dessert.Croissant,
            Dessert.Dough,
            Dessert.IceCream,
            Dessert.RawTart,
            Dessert.Strawberries,
            Dessert.Strawberry,
            Dessert.Tart
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
            {'C', MapItem.ChoppingBoard},
            {'W', MapItem.Window},
            {'B', MapItem.BlueberryCrate},
            {'I', MapItem.IceCreamCrate},
            {'S', MapItem.StrawberryCrate},
            {'0', MapItem.Player},
            {'1', MapItem.Partner}
        };

        public static readonly IDictionary<Equipment, char> KitchenEquipmentChars = new Dictionary<Equipment, char>
        {
            {Equipment.Dishwasher, 'D'},
            {Equipment.Window, 'W'},
            {Equipment.BlueberryCrate, 'B'},
            {Equipment.IceCreamCrate, 'I'},
            {Equipment.ChoppingBoard, 'C'},
            {Equipment.StrawberryCrate, 'S'}
        };
    }

    internal static class Chef
    {
        public static readonly IDictionary<Dessert, Requirements> CookBook = new Dictionary<Dessert, Requirements>
        {
            {Dessert.Blueberries, Requirements.SingleEquipmentRequirement(Equipment.BlueberryCrate, Dessert.Blueberries)},
            {Dessert.IceCream, Requirements.SingleEquipmentRequirement(Equipment.IceCreamCrate, Dessert.IceCream)},
            {Dessert.RawTart, Requirements.DoubleDessertRequirement(Dessert.ChoppedDough, Dessert.Blueberry, Dessert.RawTart)},
            {
                Dessert.ChoppedStrawberries,
                Requirements.SingleDessertAndEquipmentRequirement(Dessert.Strawberries, Equipment.ChoppingBoard, Dessert.ChoppedStrawberries)
            },
            {
                Dessert.ChoppedDough,
                Requirements.SingleDessertAndEquipmentRequirement(Dessert.Dough, Equipment.ChoppingBoard, Dessert.ChoppedDough)
            },
            {Dessert.Tart, Requirements.SingleDessertAndEquipmentRequirement(Dessert.RawTart, Equipment.Oven, Dessert.Tart)},
            {Dessert.Croissant, Requirements.SingleDessertAndEquipmentRequirement(Dessert.Dough, Equipment.Oven, Dessert.Croissant)},
        };

        private static IDictionary<string, Composition> ItemComposition = new Dictionary<string, Composition>();
        private static IDictionary<string, Requirements[]> ItemRequirements = new Dictionary<string, Requirements[]>();


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

        public static Requirements[] GetRequirements(string item)
        {
            if (ItemRequirements.ContainsKey(item))
            {
                return ItemRequirements[item];
            }

            var composition = GetComposition(item);
            Requirements[] requirements = new Requirements[composition.Desserts.Length];
            for (int i = 0; i < composition.Desserts.Length; i++)
            {
                requirements[i] = CookBook[composition.Desserts[i]];
            }

            ItemRequirements[item] = requirements;
            return requirements;
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

        public Dessert FirstDessertByMinDifficulty
        {
            get
            {
                return Desserts.OrderBy(dessert => Chef.CookBook[dessert].Difficulty).First();
            }
        }

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
            //return DiffImpl1(other);
            return DiffImpl2(other);
        }

        private Composition DiffImpl1(Composition other)
        {
            if (other.IsInvalid || other.Desserts.Except(this.Desserts).Any())
            {
                return Composition.Invalid;
            }

            return new Composition("")
            {
                HasDish = this.HasDish && !other.HasDish,
                Desserts = this.Desserts.Except(other.Desserts).ToArray()
            };
        }

        private Composition DiffImpl2(Composition other)
        {
            if (other.IsInvalid)
            {
                return Composition.Invalid;
            }

            List<Dessert> missingDesserts = new List<Dessert>();
            for (int i = 0; i < Desserts.Length; i++)
            {
                if (other.Desserts.Length > i)
                {
                    if (this.Desserts[i] != other.Desserts[i])
                    {
                        return Composition.Invalid;
                    }

                    continue;
                }

                missingDesserts.Add(this.Desserts[i]);
            }

            return new Composition("")
            {
                HasDish = this.HasDish && !other.HasDish,
                Desserts = missingDesserts.ToArray()
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
        public IDictionary<Point, string> OccupiedTables { get; set; }
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
                    new Point(X - 1, Y + 1)
                };
            }
        }

        public Point[] PerpendicularPoints
        {
            get
            {
                return new Point[]
                {
                    new Point(X - 1, Y),
                    new Point(X, Y - 1),
                    new Point(X + 1, Y),
                    new Point(X, Y + 1),
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

        public override string ToString()
        {
            return $"{{{X}, {Y}}}";
        }
    }

    internal class Requirements
    {
        public Dessert[] Desserts { get; set; } = Array.Empty<Dessert>();

        public Equipment Equipment { get; set; }

        public Dessert Result { get; set; }

        public Dessert FirstDessertByMinDifficulty
        {
            get
            {
                return Desserts.OrderBy(element => Chef.CookBook[element].Difficulty).First();
            }
        }

        public bool NeedsDesserts
        {
            get
            {
                return Desserts != null;
            }
        }

        public bool NeedsEquipment { get; }

        public int Difficulty
        {
            get
            {
                int difficulty = NeedsEquipment ? 1 : 0;

                foreach (var dessert in Desserts)
                {
                    difficulty += Chef.CookBook[dessert].Difficulty;
                }

                return difficulty;
            }
        }

        public Requirements(bool usesEquipment = true)
        {
            this.NeedsEquipment = usesEquipment;
        }

        public static Requirements DishRequirement()
        {
            return new Requirements
            {
                Equipment = Equipment.Dishwasher
            };
        }

        public static Requirements SingleEquipmentRequirement(Equipment equipment, Dessert result)
        {
            return new Requirements
            {
                Equipment = equipment,
                Result = result
            };
        }

        public static Requirements DoubleDessertRequirement(Dessert a, Dessert b, Dessert result)
        {
            return new Requirements
            {
                Desserts = new[] { a, b },
                Result = result
            };
        }

        public static Requirements SingleDessertAndEquipmentRequirement(Dessert dessert, Equipment equipment, Dessert result)
        {
            return new Requirements
            {
                Desserts = new[] { dessert },
                Equipment = equipment,
                Result = result
            };
        }
    }
    #endregion

    internal struct TripInfo
    {
        public Point Point { get; set; }
        public int EstimatedDistanceToDestination { get; set; }
        public int DistanceFromOrigin { get; set; }
        public bool Completed { get; set; }
        public int TotalDistance
        {
            get
            {
                return EstimatedDistanceToDestination + DistanceFromOrigin;
            }
        }

        public static bool operator <(TripInfo lhs, TripInfo rhs)
        {
            return lhs.TotalDistance < rhs.TotalDistance;
        }

        public static bool operator >(TripInfo lhs, TripInfo rhs)
        {
            return lhs.TotalDistance > rhs.TotalDistance;
        }
    }

    internal interface IKitchen
    {
        ICollection<Equipment> Equipments { get; }

        Dictionary<Equipment, Point> EquipmentPosition { get; }

        void AddLine(string kitchenLine);

        void UpdatePlayerPosition(Point point);

        void UpdatePartnerPosition(Point point);

        MapItem GetItem(Point point);

        Point ClosestEmptyTableToPoint(Point point);

        void AddTemporaryItem(Point point, string item);

        void ClearTemporaryItems();

        void RemoveTemporaryItem(Point point);

        TripInfo GetTripInfo(Point origin, Point destination);

        int GetTravelDistance(Point start, Point goal);

        int GetTravelDistanceToEquipment(Point position, Equipment equipment);
    }

    internal class ArrayKitchen : IKitchen
    {
        private int _addedLines;
        private readonly string[] _kitchen = new string[11];
        private IDictionary<Point, string> _temporaryItems = new Dictionary<Point, string>();

        public Dictionary<Equipment, Point> EquipmentPosition { get; } = new Dictionary<Equipment, Point>();

        public ICollection<Equipment> Equipments { get; } = new List<Equipment>();

        private Point _playerPosition;
        private Point _partnerPosition;

        public void AddLine(string kitchenLine)
        {
            UpdateEquipmentPosition(kitchenLine, Equipment.Dishwasher);
            UpdateEquipmentPosition(kitchenLine, Equipment.Window);
            UpdateEquipmentPosition(kitchenLine, Equipment.BlueberryCrate);
            UpdateEquipmentPosition(kitchenLine, Equipment.IceCreamCrate);
            UpdateEquipmentPosition(kitchenLine, Equipment.ChoppingBoard);
            UpdateEquipmentPosition(kitchenLine, Equipment.StrawberryCrate);

            UpdatePlayerPositions(kitchenLine);
            _kitchen[_addedLines++] = kitchenLine;
        }

        public void UpdatePlayerPosition(Point point)
        {
            if (GetItem(point) != MapItem.WalkableCell)
            {
                return;
            }

            StringBuilder sb = new StringBuilder(_kitchen[point.Y]);
            sb[point.X] = '0';
            _kitchen[point.Y] = sb.ToString();

            sb = new StringBuilder(_kitchen[_playerPosition.Y]);
            sb[_playerPosition.X] = '.';
            _kitchen[_playerPosition.Y] = sb.ToString();

            _playerPosition = point;
        }

        public void UpdatePartnerPosition(Point point)
        {
            if (GetItem(point) != MapItem.WalkableCell)
            {
                return;
            }

            _partnerPosition = point;
            StringBuilder sb = new StringBuilder(_kitchen[point.Y]);
            sb[point.X] = '1';
            _kitchen[point.Y] = sb.ToString();

            sb = new StringBuilder(_kitchen[_partnerPosition.Y]);
            sb[_partnerPosition.X] = '.';
            _kitchen[_partnerPosition.Y] = sb.ToString();

            _partnerPosition = point;
        }

        private void UpdatePlayerPositions(string kitchenLine)
        {
            int playerIndex = kitchenLine.IndexOf("0", StringComparison.InvariantCulture);
            int partnerIndex = kitchenLine.IndexOf("1", StringComparison.InvariantCulture);

            if (playerIndex != -1)
            {
                _playerPosition = new Point(playerIndex, _addedLines);
            }

            if (partnerIndex != -1)
            {
                _partnerPosition = new Point(playerIndex, _addedLines);
            }
        }

        private void UpdateEquipmentPosition(string kitchenLine, Equipment equipment)
        {
            int itemIndex = kitchenLine.IndexOf(Constants.KitchenEquipmentChars[equipment]);

            if (itemIndex == -1)
            {
                return;
            }

            Equipments.Add(equipment);
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

        public TripInfo GetTripInfo(Point origin, Point destination)
        {
            List<TripInfo> priorityQueue = new List<TripInfo>();
            TripInfo currentNode = new TripInfo
            {
                Point = origin,
                EstimatedDistanceToDestination = origin.DistanceTo(destination)
            };

            IDictionary<Point, bool> visited = new Dictionary<Point, bool>();

            priorityQueue.Add(currentNode);

            while (priorityQueue.Any())
            {
                priorityQueue = priorityQueue.OrderBy((TripInfo node) => node.TotalDistance).ToList();
                currentNode = priorityQueue.First();
                priorityQueue.RemoveAt(0);

                visited[currentNode.Point] = true;

                if (destination.AdjacentPoints.Contains(currentNode.Point))
                {
                    currentNode.Completed = true;
                    return currentNode;
                }

                foreach (var adjacentPoint in currentNode.Point.PerpendicularPoints)
                {
                    if (GetItem(adjacentPoint) != MapItem.WalkableCell && !object.Equals(adjacentPoint, destination))
                    {
                        continue;
                    }

                    if (visited.ContainsKey(adjacentPoint))
                    {
                        continue;
                    }

                    var newNode = new TripInfo
                    {
                        Point = adjacentPoint,
                        EstimatedDistanceToDestination = adjacentPoint.DistanceTo(destination),
                        DistanceFromOrigin = currentNode.DistanceFromOrigin + 1
                    };

                    priorityQueue.Add(newNode);
                }
            }

            return currentNode;
        }

        public int GetTravelDistance(Point start, Point goal)
        {
            TripInfo tripInfo = GetTripInfo(start, goal);
            return tripInfo.Completed ? tripInfo.DistanceFromOrigin : -1;
        }

        public int GetTravelDistanceToEquipment(Point position, Equipment equipment)
        {
            return GetTravelDistance(position, EquipmentPosition[equipment]);
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

    internal class BruteForceSolver : ISolver
    {
        private readonly IGameSimulator gameSimulator;
        private readonly IGameEvaluator gameEvaluator;
        private readonly IActionEvaluator actionEvaluator;

        public BruteForceSolver(IGameSimulator gameSimulator, IGameEvaluator gameEvaluator, IActionEvaluator actionEvaluator)
        {
            this.gameSimulator = gameSimulator;
            this.gameEvaluator = gameEvaluator;
            this.actionEvaluator = actionEvaluator;
        }

        public GameAction Solve(IKitchen kitchen, GameState gameState)
        {
            List<GameAction> gameActions = new List<GameAction>();

            foreach (var item in kitchen.Equipments)
            {
                if (Validator.CanUseWithEquipment(gameState.Player.Item, item))
                {
                    gameActions.Add(GameAction.UseAction(kitchen.EquipmentPosition[item]));
                }
            }

            foreach (var point in gameState.Player.Position.AdjacentPoints)
            {
                if (kitchen.GetItem(point) != MapItem.EmptyTable)
                {
                    continue;
                }

                if (gameState.OccupiedTables.ContainsKey(point))
                {
                    continue;
                }

                gameActions.Add(GameAction.UseAction(point));
                break;
            }

            foreach (var item in gameState.OccupiedTables)
            {
                //TODO: Remove invalid actions
                gameActions.Add(GameAction.UseAction(item.Key));
            }

            return gameActions.OrderByDescending((action) => actionEvaluator.Evaluate(kitchen, gameState, action)).First();
        }
    }

    internal class ManualSolver : ISolver
    {
        public GameAction Solve(IKitchen kitchen, GameState gameState)
        {
            var hand = gameState.Player.Item;
            var handComposition = Chef.GetComposition(hand);

            if (handComposition.IsInvalid)
            {
                var tableItems = gameState.OccupiedTables;

                //check for deliverable items on tables
                foreach (var item in tableItems)
                {
                    //Check for finished orders
                    if (Array.Exists(gameState.WaitingOrders, order => order.Item == item.Value))
                    {
                        return GameAction.UseAction(item.Key, "Found finished item on table.");
                    }
                }

                //Check for almost finished items for a customer
                foreach (var order in gameState.WaitingOrders)
                {
                    var orderComposition = Chef.GetComposition(order.Item);

                    foreach (var item in tableItems)
                    {
                        var itemComposition = Chef.GetComposition(item.Value);
                        var diff = orderComposition.GetDiff(itemComposition);

                        if (diff.IsInvalid || !diff.Desserts.Any())
                        {
                            continue;
                        }

                        //TODO: Pick easiest/closest desset to make/get
                        var dessertToGet = diff.FirstDessertByMinDifficulty;
                        var requirements = Chef.CookBook[dessertToGet];

                        while (requirements.NeedsDesserts)
                        {
                            //TODO: Check oven/table for desserts
                            //If they don't exist (or any doesn't exist), get requirements(TODO: for the easiest do make/get)
                            //If they do exist, go get the closest one and take a dish and get the others
                            dessertToGet = requirements.FirstDessertByMinDifficulty;
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

            //if holding a plain dessert(without dish), check for orders that can be completed with it and check tables with almost complete dishes
            if (!handComposition.HasDish)
            {
                //Check for almost finished items for a customer
                foreach (var order in gameState.WaitingOrders)
                {
                    var orderComposition = Chef.GetComposition(order.Item);

                    foreach (var item in gameState.OccupiedTables)
                    {
                        var itemComposition = Chef.GetComposition(item.Value);
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
                            return GameAction.UseAction(item.Key, "Going to add dessert to unfinished dish.");
                        }
                    }
                }

                //Get dish
                return GameAction.UseAction(kitchen.EquipmentPosition[Equipment.Dishwasher], "Going to place item in dish.");
            }

            //I have desserts in hand (In a dish), I can complete a dish and return
            if (handComposition.Desserts.Any())
            {
                //Check for almost finished items for a customer
                foreach (var order in gameState.WaitingOrders)
                {
                    var orderComposition = Chef.GetComposition(order.Item);
                    var diff = orderComposition.GetDiff(handComposition);

                    if (diff.IsInvalid || !diff.Desserts.Any())
                    {
                        continue;
                    }

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

                    if (!requirements.NeedsDesserts && Validator.CanUseWithEquipment(hand, requirements.Equipment))
                    {
                        //Use equipment
                        return GameAction.UseAction(kitchen.EquipmentPosition[requirements.Equipment], "Going to use equipment");
                    }
                }

                //TODO: Drop close to useful point of continuation.
                return GameAction.UseAction(kitchen.ClosestEmptyTableToPoint(kitchen.EquipmentPosition[Equipment.Window]), "Dropping partially made dish close to window.");
            }

            //Pick order and start execution
            foreach (var order in gameState.WaitingOrders)
            {
                var orderComposition = Chef.GetComposition(order.Item);

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

            return GameAction.WaitAction("Nothing to Do. Waiting...");
        }
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

    #endregion

    internal interface IGameEvaluator
    {
        /**
         * - Customer award point for each waiting customer
         * + Diff to customer items for each table item and player item
         * - Travel distance (divided by 4) to table item multipled by degree of order completeness
         */
        double Evaluate(IKitchen kitchen, GameState gameState);
    }

    internal class DefaultGameEvaluator : IGameEvaluator
    {
        private const double orderAwardCoef = 0.5;
        private const double otherCompletenessCoef = 0;//0.2;
        private const double playerItemCompletenessCoef = 0;//0.9;
        private const double travelDistanceCoef = 0.00;
        private const double ordersCountCoef = -10;
        private const double completeRequirementsCoef = 0.6;
        private const double requirementIngredientCoef = 0.4;
        private const double playerCompleteRequirementsCoef = 0.8;
        private const double playerRequirementIngredientCoef = 0.7;

        public double Evaluate(IKitchen kitchen, GameState gameState)
        {
            double score = 0;
            double maxAward = gameState.WaitingOrders.First().AwardPoints;

            foreach (var order in gameState.WaitingOrders)
            {
                score -= order.AwardPoints / maxAward * orderAwardCoef;
                var orderComposition = Chef.GetComposition(order.Item);
                var orderRequirements = Chef.GetRequirements(order.Item);


                foreach (var occupiedTable in gameState.OccupiedTables)
                {
                    var tableItem = occupiedTable.Value;
                    var tableItemComposition = Chef.GetComposition(tableItem);
                    double completenessScore = GetCompletenessScore(ref orderComposition, tableItem);
                    score += completenessScore * otherCompletenessCoef;

                    //TODO: combine distance in turns to completeness score
                    int distanceInTurns = kitchen.GetTravelDistance(gameState.Player.Position, occupiedTable.Key) / 4;
                    score -= distanceInTurns * travelDistanceCoef;

                    foreach (var requirement in orderRequirements)
                    {
                        foreach (var dessert in tableItemComposition.Desserts)
                        {
                            if (dessert == requirement.Result)
                            {
                                score += 1 * completeRequirementsCoef;
                            }

                            if (requirement.Desserts.Contains(dessert))
                            {
                                score += 1 * requirementIngredientCoef;
                            }
                        }

                    }
                }

                var playerItemComposition = Chef.GetComposition(gameState.Player.Item);
                double playerItemScore = GetCompletenessScore(ref orderComposition, gameState.Player.Item);
                score += playerItemScore * playerItemCompletenessCoef;

                if (gameState.Player.Item != Constants.None)
                {
                    foreach (var requirement in orderRequirements)
                    {
                        foreach (var dessert in playerItemComposition.Desserts)
                        {
                            if (dessert == requirement.Result)
                            {
                                score += 1 * playerCompleteRequirementsCoef;
                            }

                            if (requirement.Desserts.Contains(dessert))
                            {
                                score += 1 * playerRequirementIngredientCoef;
                            }
                        }
                    }
                }
            }

            score /= gameState.WaitingOrders.Length;

            score += gameState.WaitingOrders.Length * ordersCountCoef;
            return score;
        }

        private static double GetCompletenessScore(ref Composition referencedComposition, string item)
        {
            var itemComposition = Chef.GetComposition(item);
            var diff = referencedComposition.GetDiff(itemComposition);
            double score = itemComposition.HasDish && referencedComposition.HasDish ? 1 : 0;

            if (!diff.IsInvalid)
            {
                score = referencedComposition.Desserts.Length - diff.Desserts.Length;
            }

            return score;
        }
    }

    internal interface IActionEvaluator
    {
        /**
         * - Travel distance (divided by 4) to action point
         * + Evaluation of resulting game state        
         */
        double Evaluate(IKitchen kitchen, GameState gameState, GameAction gameAction);
    }

    internal class DefaultActionEvaluator : IActionEvaluator
    {
        private readonly IGameEvaluator gameEvaluator;
        private readonly IGameSimulator gameSimulator;
        private const double travelDistanceCoef = 0.05;
        private const double noProgressCoef = 5;

        public DefaultActionEvaluator(IGameEvaluator gameEvaluator, IGameSimulator gameSimulator)
        {
            this.gameEvaluator = gameEvaluator;
            this.gameSimulator = gameSimulator;
        }

        public double Evaluate(IKitchen kitchen, GameState gameState, GameAction gameAction)
        {
            double score = 0;
            double distanceInTurns = kitchen.GetTravelDistance(gameState.Player.Position, gameAction.Position) / 4.0;
            score -= distanceInTurns * travelDistanceCoef;
            var newState = gameSimulator.Simulate(kitchen, gameState, gameAction);
            score += gameEvaluator.Evaluate(kitchen, newState);
            bool isSameState = object.Equals(newState, gameState);
            score -= (isSameState ? 1 : 0) * noProgressCoef;
            return score;
        }
    }

    internal interface IGameSimulator
    {
        GameState Simulate(IKitchen kitchen, GameState currentState, GameAction action);
    }

    internal class DefaultGameSimulator : IGameSimulator
    {
        public GameState Simulate(IKitchen kitchen, GameState currentState, GameAction action)
        {
            if (action.Action == Action.Wait)
            {
                return currentState;
            }

            TripInfo tripInfo = kitchen.GetTripInfo(currentState.Player.Position, action.Position);
            Player player = currentState.Player;
            int dishesOnTable = currentState.DishesOnTables;
            IDictionary<Point, string> occupiedTables = currentState.OccupiedTables;
            List<Order> waitingOrders = new List<Order>(currentState.WaitingOrders);

            player.Position = tripInfo.Point;

            //Do use only if movement reached the specified location
            if (action.Action == Action.Use && action.Position.AdjacentPoints.Contains(tripInfo.Point))
            {
                MapItem usedItem = kitchen.GetItem(action.Position);
                string carriedItem = player.Item;

                switch (usedItem)
                {
                    case MapItem.BlueberryCrate:

                        if (!UseBlueberryCrate(ref carriedItem))
                        {
                            return currentState;
                        }

                        player.Item = carriedItem;
                        break;
                    case MapItem.IceCreamCrate:
                        if (!UseDessertCrate(ref carriedItem, Dessert.IceCream))
                        {
                            return currentState;
                        }

                        player.Item = carriedItem;
                        break;
                    case MapItem.StrawberryCrate:
                        if (!UseStrawberryCrate(ref carriedItem))
                        {
                            return currentState;
                        }

                        player.Item = carriedItem;
                        break;
                    case MapItem.ChoppingBoard:
                        if (!UseChoppingBoard(ref carriedItem))
                        {
                            return currentState;
                        }

                        player.Item = carriedItem;
                        break;
                    case MapItem.Dishwasher:
                        int totalDishesOutside = currentState.DishesOnTables;
                        totalDishesOutside += currentState.Partner.Item.Contains(Constants.Dish) ? 1 : 0;
                        totalDishesOutside += currentState.Player.Item.Contains(Constants.Dish) ? 1 : 0;

                        if (!UseDishwasher(ref carriedItem, totalDishesOutside < 3))
                        {
                            return currentState;
                        }

                        player.Item = carriedItem;
                        break;
                    case MapItem.Window:
                        if (player.Item == Constants.None)
                        {
                            return currentState;
                        }

                        if (Array.Exists(currentState.WaitingOrders, order => order.Item == player.Item))
                        {
                            waitingOrders.Remove(waitingOrders.First((order) => order.Item == player.Item));
                            player.Item = Constants.None;
                            break;
                        }

                        return currentState;
                    case MapItem.EmptyTable:
                        //table is occupied
                        if (occupiedTables.ContainsKey(action.Position))
                        {
                            //player's hand is empty so he pickes the item
                            if (player.Item == Constants.None)
                            {
                                player.Item = occupiedTables[action.Position];
                                occupiedTables.Remove(action.Position);
                                dishesOnTable -= 1;
                                break;
                            }

                            //player has a dish
                            if (player.Item.StartsWith(Constants.Dish, StringComparison.InvariantCulture))
                            {
                                //table has a dish so invalid
                                if (occupiedTables[action.Position].StartsWith(Constants.Dish, StringComparison.InvariantCulture))
                                {
                                    return currentState;
                                }

                                //table doesn't have a dish so table item is placed in player's dish
                                player.Item = Constants.Dish + "-" + occupiedTables[action.Position];
                                occupiedTables.Remove(action.Position);
                                dishesOnTable -= 1;
                                break;
                            }

                            //player has blueberries or chopped dough and table has chopped dough or blueberries
                            // equals raw tart
                            if ((player.Item == Constants.DessertStrings[Dessert.ChoppedDough] &&
                                occupiedTables[action.Position] == Constants.DessertStrings[Dessert.Blueberries]) ||
                                (player.Item == Constants.DessertStrings[Dessert.Blueberries] &&
                                occupiedTables[action.Position] == Constants.DessertStrings[Dessert.ChoppedDough]))
                            {
                                player.Item = Constants.DessertStrings[Dessert.RawTart];
                                occupiedTables.Remove(action.Position);
                                dishesOnTable -= 1;
                                break;
                            }

                            //any thing else is invalid
                            return currentState;
                        }

                        //table is not occupied, player drops item on table
                        if (player.Item != Constants.None)
                        {
                            occupiedTables[action.Position] = player.Item;
                            player.Item = Constants.None;
                            break;
                        }

                        // else invalid
                        return currentState;
                    default:
                        //I'm not sure if we still do the move operation (verify)
                        return currentState;
                }
                //TODO: Simulate all use cases
            }

            return new GameState
            {
                TurnsLeft = currentState.TurnsLeft - 1,
                DishesOnTables = dishesOnTable,
                Player = player,
                Partner = currentState.Partner,
                OccupiedTables = occupiedTables,
                WaitingOrders = waitingOrders.ToArray()
            };
        }

        private bool UseChoppingBoard(ref string carriedItem)
        {
            if (carriedItem != Constants.DessertStrings[Dessert.Strawberries] || carriedItem != Constants.DessertStrings[Dessert.Dough])
            {
                return false;
            }

            carriedItem = "CHOPPED_" + carriedItem;
            return true;
        }

        private bool UseBlueberryCrate(ref string carriedItem)
        {
            bool result = UseDessertCrate(ref carriedItem, Dessert.Blueberries);

            if (result)
            {
                return result;
            }

            if (carriedItem == Constants.DessertStrings[Dessert.ChoppedDough])
            {
                carriedItem = Constants.DessertStrings[Dessert.RawTart];
                return true;
            }

            return false;
        }

        private bool UseStrawberryCrate(ref string carriedItem)
        {
            if (carriedItem != Constants.None)
            {
                return false;
            }

            carriedItem = Constants.DessertStrings[Dessert.Strawberries];
            return true;
        }

        private bool UseDessertCrate(ref string carriedItem, Dessert dessert)
        {
            if (carriedItem == Constants.None)
            {
                carriedItem = Constants.DessertStrings[dessert];
                return true;
            }

            if (carriedItem.StartsWith(Constants.Dish, StringComparison.InvariantCulture))
            {
                if (carriedItem.Contains(Constants.DessertStrings[dessert]))
                {
                    return false;
                }

                carriedItem += "-" + Constants.DessertStrings[dessert];
                return true;
            }

            return false;
        }

        private bool UseDishwasher(ref string carriedItem, bool areDishesAvailable)
        {
            if(carriedItem == Constants.DessertStrings[Dessert.Strawberries])
            {
                return false;
            }

            if (carriedItem.StartsWith(Constants.Dish, StringComparison.InvariantCulture))
            {
                carriedItem = Constants.Dish;
                return true;
            }

            if (!areDishesAvailable)
            {
                return false;
            }

            if (carriedItem == Constants.None)
            {
                carriedItem = Constants.Dish;
                return true;
            }

            carriedItem = Constants.Dish + "-" + carriedItem;
            return true;
        }
    }

    internal static class Solution
    {
        private static void Main_()
        {
            int[] a = { 1, 2, 3 };
            int[] b = { 2, 1, 3 };
            var c = a.Except(b).ToArray();
            Console.ReadLine();
        }

        private static void Main(string[] args)
        {
            IKitchen kitchen = new ArrayKitchen();
            IGameSimulator simulator = new DefaultGameSimulator();
            IGameEvaluator gameEvaluator = new DefaultGameEvaluator();
            IActionEvaluator actionEvaluator = new DefaultActionEvaluator(gameEvaluator, simulator);
            ISolver solver = new BruteForceSolver(simulator, gameEvaluator, actionEvaluator);//new ManualSolver();

            string[] inputs;
            int numAllCustomers = int.Parse(ReadInputLine());

            for (int i = 0; i < numAllCustomers; i++)
            {
                inputs = ReadInputLine().Split(' ');
                // the food the customer is waiting for
                string customerItem = inputs[0];
                // the number of points awarded for delivering the food
                int customerAward = int.Parse(inputs[1]);
                Chef.GetComposition(customerItem);
                Chef.GetRequirements(customerItem);
            }

            for (int i = 0; i < 7; i++)
            {
                string kitchenLine = ReadInputLine();
                Console.Error.WriteLine(kitchenLine);
                kitchen.AddLine(kitchenLine);
            }

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

                kitchen.UpdatePlayerPosition(gameState.Player.Position);
                kitchen.UpdatePartnerPosition(gameState.Partner.Position);

                // the number of tables in the kitchen that currently hold an item
                int numTablesWithItems = int.Parse(ReadInputLine());

                gameState.OccupiedTables = new Dictionary<Point, string>(numTablesWithItems);

                for (int i = 0; i < numTablesWithItems; i++)
                {
                    inputs = ReadInputLine().Split(' ');
                    int tableX = int.Parse(inputs[0]);
                    int tableY = int.Parse(inputs[1]);
                    string item = inputs[2];

                    Point tableLocation = new Point(tableX, tableY);
                    gameState.OccupiedTables[tableLocation] = item;
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
                }

                Array.Sort(gameState.WaitingOrders, delegate (Order a, Order b)
                {
                    return b.AwardPoints - a.AwardPoints;
                });

                watch.Stop();
                Console.Error.WriteLine($"Initialization Time: {watch.ElapsedMilliseconds} ms");
                watch.Reset();
                watch.Start();
                Console.WriteLine(solver.Solve(kitchen, gameState));
                watch.Stop();
                Console.Error.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            }
        }

        private static string ReadInputLine()
        {
            return Console.ReadLine();
        }
    }
}
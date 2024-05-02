using game;

Food banana= new("Banana", "A ripe banana.", 20);
Food poop = new("Poop", "This is poop.",  -10);
Food bread = new("Bread", "A stale but still nutrient rich piece of Sourdough.",  15);
Food chicken = new("Chicken", "A mysteriously warm, cooked chicken breast. Where did this come from.",  25);
Food potion = new("Healing Potion", "A magical potion that will restore half of your health.",  50);

Weapon sword = new("Sword", "A razor sharp sword.", 25);
Weapon mace = new("Mace", "A very blunt yet sharp mace.", 20);
Weapon knife = new("Knife", "A small but threatening blade.", 15);

Item jewel = new("Jewel", "A shiny and seemingly very value Jewel Stone");

Monster ogre = new("Oggle", 80, 10);
Monster cyclops = new("Clyde", 80, 10);
Monster zombie = new("Zed", 60, 10);
Monster minotaur = new("Missy", 100, 20);

Room start = new("Starting Room", [knife, bread, banana], null, null, null, null, null);
Room south = new("South Room", [poop, bread], zombie, null, null, null, null);
Room southEast = new("South East Room", [mace, banana], null, null, null, null, null);
Room southWest = new("South West Room", [knife, chicken], null, null, null, null, null);
Room east = new("East Room", [poop], ogre, null, null, null, null);
Room west = new("West Room", [poop], cyclops, null, null, null, null);
Room north = new("North Room", [chicken, sword], null, null, null, null, null);
Room northEast = new("North East Room", [knife, poop], null, null, null, null, null);
Room northWest = new("North West Room", [knife, banana], null, null, null, null, null);
Room center = new("Center Room", [jewel, potion], minotaur, null, null, null, null);


start.North = south;
south.East = southEast;
south.West = southWest;
southWest.East = south;
southWest.North = west;
southEast.West = south;
southEast.North = east;
west.North = northWest;
west.South = southWest;
east.South = southEast;
east.North = northEast;
northWest.East = north;
northWest.South = west;
northEast.South = east;
northEast.West = north;
north.East = northEast;
north.West = northWest;
north.South = center;
center.North = north;

Player player = new(start);
string currentStatement = "";

Console.WriteLine(@"
Welcome to the underground dungeons of Marikoz. A very important gem has been stolen by an infamous group of monsters,
and we need your help to get it back! The monsters a messy creatures and leave food and weapons lying around,
feel free to use whatever you can find. Defeat the monsters and reclaim the jewel!");

while (true)
{
    switch (currentStatement.ToLower())
    {
        case "":
            currentStatement = Player.ShowOptions();
            break;

        case "move":
            if (player.CurrentRoom.Monster == null)
            {
                Console.WriteLine("\nWhich direction?");
                player.CurrentRoom.ShowDoors();
                string? direction = Console.ReadLine();
                currentStatement = player.Move(direction ?? "");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nMoved into {player.CurrentRoom.Name}.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nThere is a monster, {player?.CurrentRoom?.Monster?.Name}, blocking the exits!");
                Console.ResetColor();
                currentStatement = "";
            }
            break;

        case "status":
            Console.ForegroundColor= ConsoleColor.DarkGray;
            Console.WriteLine($"\nCurrent Room: {player.CurrentRoom.Name}\nHealth: {player.Health}");
            if (player.CurrentRoom.Monster != null) Console.WriteLine($"Moster: {player.CurrentRoom.Monster.Name} - {player.CurrentRoom.Monster.Health} Health");
            Console.ResetColor();
            currentStatement = "";
            break;

        case "search":
            Console.WriteLine();
            if (player.CurrentRoom.ShowItems())
            {
                currentStatement = "searching";
            }
            else
            {
                currentStatement = "";
            }
            break;

        case "searching":
            Console.WriteLine("\nSelect an item by name to pick it up or type 'b' to go back.");
            string? response = Console.ReadLine();
            if (response == "b")
            {
                currentStatement = "";
            }
            else
            {
                Item? item = player.CurrentRoom.Items.Find(item => item.Name.ToLower() == response?.ToLower());
                if (item != null) 
                {
                    player.PickUpItem(item);
                    player.CurrentRoom.TakeItem(item);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{item.Name} added to Inventory!\n");
                    Console.ResetColor();
                    currentStatement = "search";
                }
                else
                {
                    Console.WriteLine("Item not in room.");
                }
            }
            break;

        case "fight":
            if (player.CurrentRoom.Monster != null)
            {
                player.Attack(player.CurrentRoom.Monster);
                currentStatement = "";
            }
            else
            {
                Console.WriteLine("There are no monsters to fight in this room.");
                currentStatement = "";
            }
            break;

         case "items":
            if (player.ShowItems())
            {
                currentStatement = "inventory";
            }
            else 
            {
                currentStatement = "";
            }
            break;  
        
        case "inventory":
            Console.WriteLine("\nSelect an item to DROP or type 'b' to go back.");
            string? itemResponse = Console.ReadLine();
            if (itemResponse == "b")
            {
                currentStatement = "";
            }
            else
            {
                Item? itemToDrop = player.Items.Find(item => item.Name.ToLower() == itemResponse?.ToLower());
                if (itemToDrop != null)
                {
                    player.DropItem(itemToDrop);
                    currentStatement = "items";
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n{itemResponse} is not i  your inventory.");
                    Console.ResetColor();
                }
            }
            break;
        
        case "eat":
            if (player.ShowEdibles())
            {
                Console.WriteLine("\nSelect an item to eat or type 'b' to go back.");
                string? eatResponse = Console.ReadLine();
                if (eatResponse == "b")
                {
                    currentStatement = "";
                }
                else 
                {
                    Item? itemToEat = player.Items.Find(item => item.Name.ToLower() == eatResponse?.ToLower());
                    if (itemToEat != null && itemToEat is Food foodItem)
                    {
                        player.Eat(foodItem);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n{eatResponse} is not i  your inventory.");
                        Console.ResetColor();
                    }
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("There is nothing edible in you inventory.");
                Console.ResetColor();
                currentStatement = "";
            }
            break;

        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nInvalid input.");
            Console.ResetColor();
            currentStatement = "";
            break;
    }

    if (player?.Health == 0)
    {
        Console.WriteLine("You have been defeated. GAME OVER.");
        break;
    }

    if (player?.CurrentRoom?.Monster?.Health <= 0) player.CurrentRoom.Monster = null;

    if (player.Items.Any(item => item.Name == "Jewel"))
    {
        south.South = start;
    }

    if (player.Items.Any(item => item.Name == "Jewel") && player.CurrentRoom.Name == "Starting Room")
    {
        Console.WriteLine("YOU FOUND THE SACRED JEWEL! YOU WIN!");
        break;
    }
}

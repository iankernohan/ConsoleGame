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

Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(
@"Welcome to the underground dungeons of Marikoz. A very important gem has been stolen by an infamous group of monsters,
and we need your help to get it back! The monsters a messy creatures and leave food and weapons lying around,
feel free to use whatever you can find. Defeat the monsters and reclaim the jewel!");
Console.ResetColor();
Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");

while (true)
{
    switch (currentStatement.ToLower())
    {
        case "":
            currentStatement = Player.ShowOptions();
            break;

        case "move":
        case "m":
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
        case "st":
            player.ShowStatus();
            currentStatement = "";
            break;

        case "search":
        case "s":
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
            Console.WriteLine("\nSelect an item by name to pick it up. Type 'a' to take all or type 'b' to go back.");
            string? response = Console.ReadLine();
            if (response == "b")
            {
                currentStatement = "";
            }
            else if (response?.ToLower() == "a" || response?.ToLower() == "all")
            {
                player.PickUpAllItems();
                currentStatement = "";
            }
            else
            {
                bool didPickUp = player.PickUpItem(response ?? "");
                if (didPickUp)
                {
                    currentStatement = "search";
                }
            }
            break;

        case "fight":
        case "f":
            if (player.CurrentRoom.Monster != null)
            {
                player.Attack(player.CurrentRoom.Monster);
                currentStatement = "";
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nThere are no monsters to fight in this room.");
                Console.ResetColor();
                currentStatement = "";
            }
            break;

         case "items":
         case "i":
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
            Console.WriteLine("\nType an items name to DROP item or 'b' to go back.");
            string? itemResponse = Console.ReadLine();
            if (itemResponse == "b")
            {
                currentStatement = "";
            }
            else
            {
                bool didDrop = player.DropItem(itemResponse ?? "");
                if (didDrop)
                {
                    currentStatement = "items";
                }
            }
            break;
        
        case "eat":
        case "e":
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
                    player.Eat(eatResponse ?? "");
                }
            }
            else
            {
                currentStatement = "";
            }
            break;
        case "map":
            Player.ShowMap();
            currentStatement = "";
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

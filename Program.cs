using game;

Room startRoom = new("Starting Room", [], null, null, null, null, null);
Room leftRoom = new("East Room", [], null, null, null, null, null);
Room rightRoom = new("West Room", [], null, null, null, null, null);
Room topRoom = new("North Room", [], null, null, null, null, null);

Food banana= new("Banana", "A ripe banana.", 20);
Food poop = new("Poop", "This is poop.",  -10);
Food bread = new("Bread", "A stale but still nutrient rich piece of Sourdough.",  15);
Food chicken = new("Chicken", "A mysteriously warm, cooked chicken breast. Where did this come from.",  25);
Food potion = new("Healing Potion", "A magical potion that will restore half of your health.",  15);
Weapon sword = new("Sword", "A razor sharp sword.", 20);
Weapon mace = new("Mace", "A very blunt yet sharp mace.", 20);
Weapon knife = new("Knife", "A small but threatening blade.", 15);
Item jewel = new("Jewel", "A shiny and seemingly very value Jewel Stone");
Monster ogre = new("Oggle", 80, 10);
Monster cyclops = new("Clyde", 70, 10);
Monster zombie = new("zed", 60, 10);

startRoom.East = leftRoom;
startRoom.West = rightRoom;
startRoom.Items = [banana, knife];
leftRoom.West = startRoom;
leftRoom.North = topRoom;
leftRoom.Items = [ mace, knife ];
leftRoom.Monster = ogre;
rightRoom.East = startRoom;
rightRoom.North = topRoom;
rightRoom.Items = [ poop, bread, chicken];
rightRoom.Monster = zombie;
topRoom.East = leftRoom;
topRoom.West = rightRoom;
topRoom.Items = [ sword, potion, jewel];

Player player = new(startRoom);
string currentStatement = "";

while (!player.Items.Any(item => item.Name == "Jewel"))
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
            }
            else
            {
                Console.WriteLine($"\nThere is a monster, {player?.CurrentRoom?.Monster?.Name}, blocking the exits!");
                currentStatement = "";
            }
            break;
        case "status":
            Console.WriteLine($"\nCurrent Room: {player.CurrentRoom.Name}\nHealth: {player.Health}");
            if (player.CurrentRoom.Monster != null) Console.WriteLine($"Moster: {player.CurrentRoom.Monster.Name} - {player.CurrentRoom.Monster.Health} Health");
            currentStatement = "";
            break;
        case "search":
            Console.WriteLine();
            foreach (Item itemObj in player.CurrentRoom.Items)
            {
                Console.WriteLine($"{itemObj.Name}  -  {itemObj.Description}");
            }
            currentStatement = "searching";
            break;
        case "searching":
            Console.WriteLine("\nSelect an item by name to pick it up or type 'back'.");
            string? response = Console.ReadLine();
            if (response == "back")
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
                    Console.WriteLine($"\n{item.Name} added to Inventory!\n");
                    currentStatement = "";
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
        default:
            Console.WriteLine("Invalid input.");
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
        Console.WriteLine("YOU FOUND THE SACRED JEWEL! YOU WIN!");
    }
}

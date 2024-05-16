namespace game
{
    public class Player(Room startRoom)
    {
        public int Health { get; set; } = 100;
        public bool HasWeapon { get; set; } = false;
        public List<Item> Items { get; set; } = [];
        public Room CurrentRoom {get; set;} = startRoom;
        private int Damage {get; set;} = 10;


        public static string ShowOptions()
        {
            Console.Write("\nWhat shall you do? (Search[s], Move[m], Fight[f], Eat[e], Status[st], Items[i], Map[map]): ");
            string? playerChoice = Console.ReadLine();
            return playerChoice ?? "";
    
        }

        public void ShowStatus()
        {
            Console.ForegroundColor= ConsoleColor.DarkGray;
            Console.WriteLine($"\nCurrent Room: {CurrentRoom.Name}\nHealth: {Health}\nDamage: +{Damage}");
            if (CurrentRoom.Monster != null) Console.WriteLine($"Moster: {CurrentRoom.Monster.Name} | {CurrentRoom.Monster.Health} Health | {CurrentRoom.Monster.Damage} Damage");
            Console.ResetColor();
        }
        public bool PickUpItem(string itemName)
        {
            Item? item;
            if (int.TryParse(itemName, out int result))
            {
                if (result < 0 || result > CurrentRoom.Items.Count - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nIndex out of range.");
                    Console.ResetColor();
                    return false;
                }
                item = CurrentRoom.Items[result];
            }
            else
            {
                item = CurrentRoom.Items.Find(item => item.Name.ToLower() == itemName?.ToLower());
            }
            if (item != null) 
            {
                if (item is Weapon weapon1)
                {
                    if (HasWeapon)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nYou already have a weapon, type 'y' if you would like to replace it.");
                        Console.ResetColor();
                        string? replace = Console.ReadLine();
                        if (replace == "y")
                        {
                            Item? weaponItem = Items.Find(item => item is Weapon);
                            DropItem(weaponItem);
                            HasWeapon = true;
                            Damage = weapon1.Damage;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        HasWeapon = true;
                        Damage = weapon1.Damage;
                    }
                }
                Items.Add(item);
                CurrentRoom.TakeItem(item);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{item.Name} added to Inventory!");
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nItem not in room.");
                Console.ResetColor();
                return false;
            }

        }

        public void PickUpAllItems()
        {
            if (CurrentRoom.Items.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                List<Item> ItemsCopy = new(CurrentRoom.Items);
                foreach (Item item in ItemsCopy)
                {
                    PickUpItem(item.Name);
                    CurrentRoom.Items.Remove(item);
                }
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\nRoom is currently empty");
            }
        }

        public bool DropItem(string itemName)
        {
            Item? itemToDrop;
            if (int.TryParse(itemName, out int result))
            {
                if (result < 0 || result > Items.Count - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nIndex out of range.");
                    Console.ResetColor();
                    return false;
                }
                itemToDrop = Items[result];
            }
            else
            {
                itemToDrop = Items.Find(item => item.Name.ToLower() == itemName?.ToLower());
            }
            if (itemToDrop != null)
            {
                Items.Remove(itemToDrop);
                CurrentRoom.AddItem(itemToDrop);
                if (itemToDrop is Weapon) {
                    HasWeapon = false;
                    Damage = 10;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{itemToDrop.Name} removed from inventory.");
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{itemName} is not i  your inventory.");
                Console.ResetColor();
                return false;
            }
        }

        public bool DropItem(Item item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
                CurrentRoom.AddItem(item);
                if (item is Weapon) HasWeapon = false;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{item.Name} removed from inventory.");
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{item} is not i  your inventory.");
                Console.ResetColor();
                return false;
            }
        }

        public bool ShowItems()
        {
            Console.WriteLine();
            if (Items.Count > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"({i}) {Items[i].Name}  -  {Items[i].Description}");
                    Console.ResetColor();
                }
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nInventory is currently empty.");
                Console.ResetColor();
                return false;
            }
        }

        public bool ShowEdibles()
        {
            int count = 0;
            Console.WriteLine();
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] is Food foodItem)
                {
                    count++;
                    Console.WriteLine($"({i}) {foodItem.Name}  -  {foodItem.Description}  -  {foodItem.Eat()}");
                }
            }
            if (count <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nThere is nothing edible in you inventory.");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        public bool Eat(string itemName)
        {
                Item? itemToEat;
                if (int.TryParse(itemName, out int result))
                {
                if (result < 0 || result > Items.Count - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nIndex out of range.");
                        Console.ResetColor();
                        return false;
                    }
                    itemToEat = Items[result];
                }
                else
                {
                    itemToEat = Items.Find(item => item.Name.ToLower() == itemName?.ToLower());
                }
                if (itemToEat != null && itemToEat is Food foodItem)
                {
                    Health += foodItem.Eat();
                    Items.Remove(itemToEat);
                    if (foodItem.Eat() > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{foodItem.Eat()} added to Health.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n{foodItem.Eat()} removed from Health.");
                        Console.ResetColor();
                    }
                    if (Health > 100) Health = 100;
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n{itemName} is not i your an edible item in your inventory.");
                    Console.ResetColor();
                    return false;
                }
        }

        public bool Move()
        {
            Console.WriteLine("\nWhich direction? ('b' to go back)");
            CurrentRoom.ShowDoors();
            string? direction = Console.ReadLine();
            switch (direction?.ToLower())
            {
                case "north":
                case "n":
                    if (CurrentRoom.North != null) CurrentRoom = CurrentRoom.North;
                    break;
                case "east":
                case "e":
                    if (CurrentRoom.East != null) CurrentRoom = CurrentRoom.East;
                    break;
                case "south":
                case "s":
                    if (CurrentRoom.South != null) CurrentRoom = CurrentRoom.South;
                    break;
                case "west":
                case "w":
                    if (CurrentRoom.West != null) CurrentRoom = CurrentRoom.West;
                    break;
                case "b":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nStayed in {CurrentRoom.Name}.");
                    Console.ResetColor();
                    return true;
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nInvalid direction");
                    Console.ResetColor();
                    return false;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nMoved into {CurrentRoom.Name}.");
            Console.ResetColor();
            return true;
        }

        public void Attack(Monster monster)
        {
            monster.Health -= Damage;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{Damage} damage done to {CurrentRoom?.Monster?.Name}!");
            Console.ResetColor();
            if (monster.Health <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{monster.Name} defeated!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n{monster.Name} attacked back! {monster.Damage} damage taken!");
                Console.ResetColor();
                Health -= monster.Damage;
            }
        }

        public void ShowMap()
        {
            if (Items.Any(item => item.Name == "Jewel"))
            {
                            Console.WriteLine(
@"
   ----------  ---------- ----------
   |  North | |         | |  North |
   |        |=|  North  |=|        |
   |  West  | |         | |  East  |
   ----------  ---------- ----------
       ||          ||         ||
   ----------  ---------- ----------
   |        | |         | |        |
   |  West  | |  Center | |  East  |
   |        | |         | |        |
   ----------  ---------- ----------
       ||          ||         ||
   ----------  ---------- ----------
   |  South | |         | |  South |
   |        |=|  South  |=|        |
   |  West  | |         | |  East  |
   ----------  ---------- ----------
                   ||
               ----------      
              |         |
              |  Start  |
              |         |
               ----------"
);
            }
            else
            {
            Console.WriteLine(
@"
   ----------  ---------- ----------
   |  North | |         | |  North |
   |        |=|  North  |=|        |
   |  West  | |         | |  East  |
   ----------  ---------- ----------
       ||          ||         ||
   ----------  ---------- ----------
   |        | |         | |        |
   |  West  | |  Center | |  East  |
   |        | |         | |        |
   ----------  ---------- ----------
       ||                     ||
   ----------  ---------- ----------
   |  South | |         | |  South |
   |        |=|  South  |=|        |
   |  West  | |         | |  East  |
   ----------  ---------- ----------
                   ||
               ----------      
              |         |
              |  Start  |
              |         |
               ----------"
);
            }

        }
    }
}
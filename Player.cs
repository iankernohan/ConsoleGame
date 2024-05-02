using System.Drawing;

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
            Console.Write("\nWhat shall you do? (Search, Move, Fight, Eat, Status, Items): ");
            string? playerChoice = Console.ReadLine();
            return playerChoice ?? "";
    
        }
        public void PickUpItem(Item item)
        {
            Items.Add(item);
            if (item is Weapon weapon1)
            {
                HasWeapon = true;
                Damage = weapon1.Damage;
            }
        }

        public void DropItem(Item item)
        {
            Items.Remove(item);
            if (item is Weapon) HasWeapon = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{item.Name} removed from inventory.");
            Console.ResetColor();
        }

        public bool ShowItems()
        {
            Console.WriteLine();
            if (Items.Count > 0)
            {
                foreach (Item item in Items)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{item.Name}  -  {item.Description}");
                    Console.ResetColor();
                }
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Inventory is currently empty.");
                Console.ResetColor();
                return false;
            }
        }

        public bool ShowEdibles()
        {
            int count = 0;
            Console.WriteLine();
            foreach (Item item in Items)
            {
                if (item is Food foodItem)
                {
                    count++;
                    Console.WriteLine($"{foodItem.Name}  -  {foodItem.Description}  -  {foodItem.HealthPoints}");
                }
            }
            return count > 0;
        }

        public void Eat(Food item)
        {
                Items.Remove(item);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{item.HealthPoints} added to Health.");
                Console.ResetColor();
                Health += item.Eat();
                if (Health > 100) Health = 100;
        }

        public string Move(string direction)
        {
            switch (direction.ToLower())
            {
                case "north":
                    if (CurrentRoom.North != null) CurrentRoom = CurrentRoom.North;
                    break;
                case "east":
                    if (CurrentRoom.East != null) CurrentRoom = CurrentRoom.East;
                    break;
                case "south":
                    if (CurrentRoom.South != null) CurrentRoom = CurrentRoom.South;
                    break;
                case "west":
                    if (CurrentRoom.West != null) CurrentRoom = CurrentRoom.West;
                    break;
                default:
                    Console.WriteLine("Invalid direction");
                    return "move";
            }
            return "";
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
    }
}
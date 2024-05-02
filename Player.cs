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
            Console.WriteLine("\nChoose what to do: \nSearch, Move, Fight, Eat, Status");
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
        }

        public void ShowItems()
        {
            Console.WriteLine("Items:\n---------------");
            foreach (Item item in Items)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine("---------------\n");
        }

        public void EatItem(Food item)
        {
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
            if (monster.Health <= 0)
            {
                Console.WriteLine($"\n{monster.Name} defeated!");
            }
            else
            {
                Console.WriteLine($"\n{monster.Name} attacked back! {monster.Damage} damage taken!");
                Health -= monster.Damage;
            }
        }
    }
}
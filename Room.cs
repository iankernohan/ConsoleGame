using System.Text;

namespace game
{
    public class Room(string name, List<Item> items, Monster? monster, Room? north, Room? east, Room? south, Room? west)
    {
        public string Name { get; set; } = name;
        public List<Item> Items {get; set;} = items;
        public Room? North {get; set;} = north;
        public Room? East {get; set;} = east;
        public Room? South {get; set;} = south;
        public Room? West {get; set;} = west;
        public Monster? Monster {get; set;} = monster;

        public void ShowDoors()
        {
            StringBuilder directions = new();
            if (North != null) directions.Append("North ");
            if (East != null) directions.Append("East ");
            if (South != null) directions.Append("South ");
            if (West != null) directions.Append("West ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(directions.ToString());
            Console.WriteLine();
            Console.ResetColor();
        }

        public bool ShowItems()
        {
            if (Items.Count > 0)
            {
                Console.ForegroundColor= ConsoleColor.DarkGray;
                foreach (Item itemObj in Items)
                {
                    Console.WriteLine($"{itemObj.Name}  -  {itemObj.Description}");
                }
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("This room is currently empty");
                Console.ResetColor();
                return false;
            }
        }

        public void TakeItem(Item item)
        {
            Items.Remove(item);
        }
    }
}
namespace game
{
    public class Item(string name, string description)
    {
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;

        public void ShowDetails()
        {
            Console.WriteLine($"Name: {Name}\nDesctiption: {Description}\n");
        }
    }

}
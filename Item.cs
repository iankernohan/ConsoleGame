namespace game
{
    public class Item(string name, string description, string type, bool isEdible)
    {
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public string Type { get; set; } = type;
        public bool IsEdible { get; set; } = isEdible;
    }
}
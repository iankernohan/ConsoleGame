namespace game
{
    public class Food(string name, string description, string type, bool edible, int healthPoints) : Item(name, description, type, edible)
    {
        public int HealthPoints { get; set; } = healthPoints;
    }
}
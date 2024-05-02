namespace game
{
    public class Food(string name, string description, int healthPoints) : Item(name, description)
    {
        public int HealthPoints { get; set; } = healthPoints;

        public int Eat()
        {
            return HealthPoints;
        }
    }
}
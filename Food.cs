namespace game
{
    public class Food(string name, string description, int healthPoints) : Item(name, description)
    {
        private int HealthPoints { get; set; } = healthPoints;

        public int Eat()
        {
            return HealthPoints;
        }
    }
}
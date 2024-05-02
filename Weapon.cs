namespace game
{
    public class Weapon(string name, string description, int damage) : Item(name, description)
    {
        public int Damage { get; set; } = damage;
    }
}
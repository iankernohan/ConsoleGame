namespace game
{
    public class Monster(string name, int health, int damage)
    {
        public string Name {get; set;} = name;
        public int Health {get; set;} = health;
        public int Damage {get; set;} = damage;
    }
}
namespace game
{
    public class Player()
    {
        public int Health { get; set; } = 100;
        public bool HasWeapon { get; set; } = false;
        public List<Item> Items { get; set; } = [];


        public void PickUpItem(Item item)
        {
            Items.Add(item);
            if (item.Type == "weapon")
            {
                HasWeapon = true;
            }
        }

        public void DropItem(Item item)
        {
            Items.Remove(item);
        }
    }
}
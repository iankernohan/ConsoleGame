using game;

Player player = new();

Food food= new("Banana", "Fresh banana", "Food", true, 50);
Item item = new("Poop", "This is poop", "Generic", false);

player.PickUpItem(item);
player.PickUpItem(food);

foreach (Item i in player.Items)
{
    Console.WriteLine(i.Name);
}

Console.WriteLine("------------------------------");

player.DropItem(item);

foreach (Item i in player.Items)
{
    Console.WriteLine(i.Name);
}

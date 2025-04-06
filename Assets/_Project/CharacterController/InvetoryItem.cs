using System.Collections.Generic;

public class Inventory
{
    private List<ItemDefinition> container = new();
    private int selectedIndex = 0;

    public void Acquire(ItemDefinition item)
    {
        container.Add(item);
    }

    public void Remove(ItemDefinition item)
    {
        container.Remove(item);
    }
    public ItemDefinition GetSelectedItem() => container[selectedIndex];

    public void Increment()
    {
        selectedIndex = (selectedIndex + 1) % container.Count;
    }
    public void Decrement()
    {
        selectedIndex = (selectedIndex - 1 + container.Count) % container.Count;
    }
}
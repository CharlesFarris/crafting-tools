namespace CraftingTools.Domain;

public class ItemManager
{
    public Item GetItemById(Guid id)
    {
        return this._items.TryGetValue(id, out var item) 
            ? item 
            : Item.None;
    }
    
    private Dictionary<Guid, Item> _items = new();
}
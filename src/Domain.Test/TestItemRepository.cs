using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

internal sealed class TestItemRepository : IItemRepository
{
    static TestItemRepository()
    {
        
    }
    public Maybe<Item> GetItemById(Guid id)
    {
        return TestItemRepository.Items.TryGetValue(id, out var item)
            ? item.ToMaybe()
            : Maybe<Item>.None;
    }

    public ImmutableList<Item> GetItems()
    {
        return TestItemRepository.Items.Values.ToImmutableList();
    }

    private static readonly ImmutableDictionary<Guid, Item> Items = new[]
    {
        Item.FromParameters(id: "5E1BDF20-E35D-4238-B453-F486F299054B", name: "Flour").Unwrap(),
        Item.FromParameters(id: "24244AA3-409B-4C1F-842B-95C174518525", name: "Sugar").Unwrap(),
        Item.FromParameters(id: "A552F0B7-8A74-45F8-81B5-0C1404D79F04", name: "Salt").Unwrap(),
        Item.FromParameters(id: "A913792D-1428-478E-9BC2-B34120ADC192", name: "Egg").Unwrap(),
        Item.FromParameters(id: "73541F4E-8E46-438D-85B8-28D5A1094B51", name: "Water").Unwrap(),
        Item.FromParameters(id: "E3A93EAF-E4EA-488B-A177-3B1EA35F027C", name: "Pepper").Unwrap(),
        Item.FromParameters(id: "5932A384-D670-412B-A86D-D059CEA26834", name: "Garlic").Unwrap(),
        Item.FromParameters(id: "4EF846B3-A7BA-4250-9E0C-075A8EAF0E23", name: "Thyme").Unwrap(),
        Item.FromParameters(id: "0F670E6C-A554-4F3E-A578-3B1E5A6186A6", name: "Onion").Unwrap(),
        Item.FromParameters(id: "0F670E6C-A554-4F3E-A578-3B1E5A6186A6", name: "Potato").Unwrap(),
        Item.FromParameters(id: "94082299-5761-44F2-9714-064F07026199", name: "Milk").Unwrap(),
        Item.FromParameters(id: "D701D917-0133-4F82-A4F8-46752A0D0FE6", name: "Apple").Unwrap(),
        Item.FromParameters(id: "FC440E16-A393-48F1-AC02-430F89673E8A", name: "Lemon").Unwrap(),
        Item.FromParameters(id: "2551B773-BF7E-46F1-AA9D-5054ADBBDFC7", name: "Tomato").Unwrap(),
        Item.FromParameters(id: "D67191AE-7EAA-4E2D-8CFE-517BDEC3C533", name: "Cinnamon").Unwrap()
    }.ToImmutableDictionary(i => i.Id, i => i);
}

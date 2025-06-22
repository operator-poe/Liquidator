using System.Collections.Generic;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;

namespace Liquidator;

public static class Inventory
{
  private static Liquidator Instance = Liquidator.Instance;
  private static GameController GameController
  {
    get
    {
      return Instance.GameController;
    }
  }
  public static ExileCore.PoEMemory.MemoryObjects.Inventory Panel
  {
    get
    {
      return Liquidator.Instance.GameController.Game.IngameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory];
    }
  }
  public static bool IsVisible
  {
    get
    {
      return Panel.IsVisible;
    }
  }

  public static Dictionary<string, int> GetAvailableItems()
  {
    var snapshot = Stash.Inventory.CreateSnapshot();
    var items = new Dictionary<string, int>();
    foreach (var item in snapshot.GetAllItems())
    {
      var baseItem = item.Item.GetComponent<Base>();
      var stack = item.Item.GetComponent<Stack>();
      if (items.ContainsKey(baseItem.Name))
      {
        items[baseItem.Name] += stack.Size;
      }
      else
      {
        items[baseItem.Name] = stack.Size;
      }
    }
    return items;
  }
}
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
}
using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Elements.Village;
using ExileCore.PoEMemory.Models;

namespace Liquidator;

public static class Exchange
{
  private static Liquidator Instance = Liquidator.Instance;

  private static GameController GameController
  {
    get
    {
      return Instance.GameController;
    }
  }

  public static CurrencyExchangePanel Window
  {
    get
    {
      return Instance.GameController.IngameState.IngameUi.CurrencyExchangePanel;
    }
  }

  public static bool IsVisible
  {
    get
    {
      return Window.IsVisible;
    }
  }

  public static BaseItemType LeftSideCurrency
  {
    get
    {
      return Window.WantedItemType;
    }
  }

  public static BaseItemType RightSideCurrency
  {
    get
    {
      return Window.OfferedItemType;
    }
  }

  public static Element LeftSidePickButton
  {
    get
    {
      return Window.GetChildAtIndex(7);
    }
  }

  public static Element RightSidePickButton
  {
    get
    {
      return Window.GetChildAtIndex(10);
    }
  }

  public static CurrencyExchangeCurrencyPickerElement CurrencyPicker
  {
    get
    {
      return Window.CurrencyPicker;
    }
  }

}
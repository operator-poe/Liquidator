using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Elements.Village;

namespace Liquidator;

public static class CurrencyPicker
{
  private static Liquidator Instance = Liquidator.Instance;

  private static GameController GameController
  {
    get
    {
      return Instance.GameController;
    }
  }

  public static CurrencyExchangeCurrencyPickerElement Panel
  {
    get
    {
      return Exchange.CurrencyPicker;
    }
  }

  public static bool IsVisible
  {
    get
    {
      return Panel.IsVisible;
    }
  }

  public static Element SearchBar
  {
    get
    {
      return Panel.GetChildFromIndices(4, 0);
    }
  }

  public static Element ClearSearchButton
  {
    get
    {
      return Panel.GetChildFromIndices(4, 1);
    }
  }

  public static Element CurrencyList
  {
    get
    {
      return Panel.GetChildFromIndices(6, 1);
    }
  }

  public static Element GetCurrencyElementFromList(string currency)
  {
    return CurrencyList.FindChildRecursive(x => x.Text == currency);
  }
}
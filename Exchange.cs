using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Elements.Village;
using ExileCore.PoEMemory.Models;

namespace Liquidator;

public enum ExchangeValueFieldState
{
  Open,
  Fixed,
  NotEnoughStock,
}

public static class ElementExtensions
{
  public static int GetExchangeValue(this Element element)
  {
    return element.Text != null ? int.Parse(element.Text) : 0;
  }

  public static ExchangeValueFieldState GetExchangeState(this Element element)
  {
    return DetermineValueFieldStateByColor(element.TextColor);
  }

  private static ExchangeValueFieldState DetermineValueFieldStateByColor(SharpDX.ColorBGRA color)
  {
    // A:128 R:255 G:234 B:197 - Open
    // A:255 R:255 G:234 B:197 - Fixed
    // A:255 R:210 G:0 B:0 - Not enough stock
    if (color.A == 255 && color.R == 255 && color.G == 234 && color.B == 197)
    {
      return ExchangeValueFieldState.Fixed;
    }
    if (color.A == 255 && color.R == 210 && color.G == 0 && color.B == 0)
    {
      return ExchangeValueFieldState.NotEnoughStock;
    }
    return ExchangeValueFieldState.Open;
  }
}

public class ExchangeValueField
{
  private readonly Element _element;

  public ExchangeValueField(Element element)
  {
    _element = element;
  }

  public string Text => _element.Text;
  public SharpDX.ColorBGRA TextColor => _element.TextColor;

  public int Value => _element.GetExchangeValue();

  public ExchangeValueFieldState State => _element.GetExchangeState();
}

public static class Exchange
{
  private static Liquidator Instance = Liquidator.Instance;
  private static ExchangeValueField _cachedRightSideValueField;
  private static ExchangeValueField _cachedLeftSideValueField;
  private static Element _lastRightSideElement;
  private static Element _lastLeftSideElement;

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

  public static ExchangeValueField RightSideValueField
  {
    get
    {
      var currentElement = Window.GetChildAtIndex(8);
      if (_cachedRightSideValueField == null || _lastRightSideElement != currentElement)
      {
        _cachedRightSideValueField = new ExchangeValueField(currentElement);
        _lastRightSideElement = currentElement;
      }
      return _cachedRightSideValueField;
    }
  }

  public static ExchangeValueField LeftSideValueField
  {
    get
    {
      var currentElement = Window.GetChildAtIndex(5);
      if (_cachedLeftSideValueField == null || _lastLeftSideElement != currentElement)
      {
        _cachedLeftSideValueField = new ExchangeValueField(currentElement);
        _lastLeftSideElement = currentElement;
      }
      return _cachedLeftSideValueField;
    }
  }
}
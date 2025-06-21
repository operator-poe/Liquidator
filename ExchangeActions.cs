using ExileCore.Shared;

namespace Liquidator;

public static class ExchangeActions
{
  public static async SyncTask<bool> SelectLeftSideCurrency(string currency)
  {
    var currencyElement = Exchange.Window.WantedItemType;
    Log.Debug($"Currency element: {currencyElement?.BaseName}");
    if (currencyElement != null && currencyElement.BaseName == currency)
    {
      Log.Debug($"Currency element is already selected: {currencyElement?.BaseName}");
      return true;
    }
    Log.Debug("Clicking left side pick button");
    await InputAsync.ClickElement(Exchange.LeftSidePickButton.GetClientRect());
    Log.Debug("Waiting for currency picker to be visible");
    await InputAsync.Wait(() => CurrencyPicker.IsVisible, 100, "Currency picker not visible");
    Log.Debug("Currency picker is visible");

    var element = CurrencyPicker.GetCurrencyElementFromList(currency);
    if (element == null)
    {
      await CurrencyPickerActions.Search(currency);
      element = CurrencyPicker.GetCurrencyElementFromList(currency);
      if (element == null)
      {
        Log.Error($"Currency element not found: {currency}");
        return false;
      }
    }


    await InputAsync.ClickElement(element.GetClientRect());
    await InputAsync.Wait(() => Exchange.LeftSideCurrency.BaseName == currency, 100, "Currency not selected");
    return true;
  }
}
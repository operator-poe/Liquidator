using ExileCore.Shared;

namespace Liquidator;

public static class CurrencyPickerActions
{
  public static async SyncTask<bool> Search(string currency)
  {
    await InputAsync.ClickElement(CurrencyPicker.ClearSearchButton.GetClientRect());
    await InputAsync.ClickElement(CurrencyPicker.SearchBar.GetClientRect());
    await Stash.Utils.TypeText(currency);
    return true;
  }
}
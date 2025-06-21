using System.Windows.Forms;
using ExileCore.PoEMemory.Elements;
using ExileCore.Shared;

namespace Liquidator;

public static class MoveAround
{
  public static async SyncTask<bool> FindAndClickFaustus()
  {
    Log.Debug("Finding and clicking Faustus");
    var itemsOnGround = Liquidator.Instance.GameController.IngameState.IngameUi.ItemsOnGroundLabels;

    if (Exchange.IsVisible)
    {
      if (CurrencyPicker.IsVisible)
      {
        await InputAsync.KeyPress(Keys.Escape);
        await InputAsync.Wait();
        await InputAsync.Wait();
        await InputAsync.Wait();
      }
      return true;
    }
    await EnsureEverythingIsClosed();

    foreach (LabelOnGround labelOnGround in itemsOnGround)
    {
      if (!labelOnGround.ItemOnGround.Path.Contains("/VillageFaustusHideout"))
      {
        continue;
      }
      if (!labelOnGround.IsVisible)
      {
        Log.Error("Faustus not visible.\nMake sure that he is positioned within short reach.");
        return false;
      }
      await InputAsync.KeyDown(Keys.ControlKey);
      await InputAsync.Wait();
      // await InputAsync.ClickElement(labelOnGround.Label.GetClientRect().Center);
      await InputAsync.ClickElement(labelOnGround.Label.GetClientRect());
      await InputAsync.KeyUp(Keys.ControlKey);
      await InputAsync.Wait(() => Exchange.IsVisible, 1000);
      if (!Exchange.IsVisible)
      {
        Log.Error("Could not reach Faustus in time.\nMake sure that he is positioned within short reach.");
        return false;
      }
    }
    Log.Debug("Found and clicked Faustus");
    return true;
  }

  public static async SyncTask<bool> EnsureStash()
  {
    if (Liquidator.Instance.GameController.IngameState.IngameUi.StashElement.IsVisible && Liquidator.Instance.GameController.IngameState.IngameUi.InventoryPanel.IsVisible)
    {
      return true;
    }
    await EnsureEverythingIsClosed();

    var itemsOnGround = Liquidator.Instance.GameController.IngameState.IngameUi.ItemsOnGroundLabels;
    var stash = Liquidator.Instance.GameController.IngameState.IngameUi.StashElement;

    if (stash is { IsVisible: true })
    {
      return true;
    }

    foreach (LabelOnGround labelOnGround in itemsOnGround)
    {
      if (!(labelOnGround?.ItemOnGround?.Path?.Contains("/Stash") ?? true))
      {
        continue;
      }
      if (!labelOnGround.IsVisible)
      {
        Log.Error("Stash not visible");
        return false;
      }
      await InputAsync.ClickElement(labelOnGround.Label.GetClientRect());
      await InputAsync.Wait(() => stash is { IsVisible: true }, 2000, "Stash not reached in time");
      if (stash is { IsVisible: false })
      {
        Log.Error("Stash not visible");
        return false;
      }
    }
    return true;
  }

  public static async SyncTask<bool> EnsureInventoryIsOpen()
  {
    if (Inventory.IsVisible)
    {
      return true;
    }
    await InputAsync.KeyDown(Keys.I);
    await InputAsync.KeyUp(Keys.I);
    await InputAsync.Wait(() => Inventory.IsVisible, 1000, "Inventory not opened");
    return true;
  }

  public static async SyncTask<bool> EnsureEverythingIsClosed()
  {
    if (Exchange.IsVisible)
    {
      await InputAsync.KeyDown(Keys.Escape);
      await InputAsync.KeyUp(Keys.Escape);
      await InputAsync.Wait(() => Exchange.IsVisible, 1000, "Atlas not closed");
    }
    if (Inventory.IsVisible)
    {
      await InputAsync.KeyDown(Keys.Escape);
      await InputAsync.KeyUp(Keys.Escape);
      await InputAsync.Wait(() => !Inventory.IsVisible, 1000, "Inventory not closed");
    }
    if (Stash.Stash.IsVisible)
    {
      await InputAsync.KeyDown(Keys.Escape);
      await InputAsync.KeyUp(Keys.Escape);
      await InputAsync.Wait(() => !Stash.Stash.IsVisible, 1000, "Stash not closed");
    }
    return true;
  }
}
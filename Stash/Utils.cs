using Liquidator;
using ExileCore.Shared;
using ExileCore;
using ExileCore.PoEMemory.Elements;
using System.Runtime.InteropServices;
using System;

namespace Liquidator.Stash;

public static class Utils
{
  [DllImport("user32.dll", SetLastError = true)]
  public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  public static extern bool SetForegroundWindow(IntPtr hWnd);
  public static async SyncTask<bool> ForceFocusAsync()
  {
    if (!Liquidator.Instance.GameController.Window.IsForeground())
    {
      IntPtr handle = FindWindow(null, "Path of Exile");
      if (handle != IntPtr.Zero)
      {
        SetForegroundWindow(handle);
      }
    }
    return await InputAsync.Wait(() => Liquidator.Instance.GameController.Window.IsForeground(), 1000, "Window could not be focused");
  }
  public static GameController GameController
  {
    get
    {
      return Liquidator.Instance.GameController;
    }
  }
  public static async SyncTask<bool> EnsureEverythingIsClosed()
  {
    if (GameController.IngameState.IngameUi.InventoryPanel != null && GameController.IngameState.IngameUi.InventoryPanel.IsVisible)
    {
      await InputAsync.KeyDown(Liquidator.Instance.Settings.HotKeySettings.InventoryHotKey.Value);
      await InputAsync.KeyUp(Liquidator.Instance.Settings.HotKeySettings.InventoryHotKey.Value);
      await InputAsync.Wait(() => !GameController.IngameState.IngameUi.InventoryPanel.IsVisible, 1000, "Inventory not closed");
    }
    if (GameController.IngameState.IngameUi.StashElement != null && GameController.IngameState.IngameUi.StashElement.IsVisible)
    {
      await InputAsync.KeyDown(System.Windows.Forms.Keys.Escape);
      await InputAsync.KeyUp(System.Windows.Forms.Keys.Escape);
      await InputAsync.Wait(() => !GameController.IngameState.IngameUi.StashElement.IsVisible, 1000, "Inventory not closed");
    }
    return true;
  }
  public static async SyncTask<bool> EnsureStash()
  {
    if (GameController.IngameState.IngameUi.StashElement.IsVisible && GameController.IngameState.IngameUi.InventoryPanel.IsVisible)
    {
      return true;
    }
    await EnsureEverythingIsClosed();

    var itemsOnGround = GameController.IngameState.IngameUi.ItemsOnGroundLabels;
    var stash = GameController.IngameState.IngameUi.StashElement;

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

  public static async SyncTask<bool> StashItemTypeToTab(string tabName, string type)
  {
    var items = Inventory.GetByType(type);
    if (items.Length > 0)
    {
      if (Stash.HasStash(tabName))
      {
        await Stash.SelectTab(tabName);
        await InputAsync.Wait(() => Stash.ActiveTab.Name == tabName, 1000, "Tab not selected");
        await InputAsync.HoldCtrl();
        foreach (var item in items)
        {
          await InputAsync.ClickElement(item.GetClientRect());
          await InputAsync.Wait(() => Stash.ActiveTab.Name == tabName, 1000, "Tab not selected");
        }
        await InputAsync.ReleaseCtrl();
      }
      return true;
    }
    return false;
  }
}
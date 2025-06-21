using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using System.Collections.Generic;
using System.Windows.Forms;
using ExileCore.Shared.Attributes;

namespace Liquidator;

public class LiquidatorSettings : ISettings
{
    //Mandatory setting to allow enabling/disabling your plugin
    public ToggleNode Enable { get; set; } = new ToggleNode(false);

    //Put all your settings here if you can.
    //There's a bunch of ready-made setting nodes,
    //nested menu support and even custom callbacks are supported.
    //If you want to override DrawSettings instead, you better have a very good reason.

    public HotKeySettings HotKeySettings { get; set; } = new HotKeySettings();


    public ListNode LogLevel { get; set; } = new ListNode
    {
        Values = ["None", "Debug", "Error"],
        Value = "Error"
    };
}

[Submenu(CollapsedByDefault = true)]
public class HotKeySettings
{
    public HotkeyNode StartHotKey { get; set; } = new HotkeyNode(Keys.F1);
    public HotkeyNode StopHotKey { get; set; } = new HotkeyNode(Keys.Delete);
    public HotkeyNode InventoryHotKey { get; set; } = new HotkeyNode(Keys.I);
    public HotkeyNode TestHotKey { get; set; } = new HotkeyNode(Keys.F3);
}
using ExileCore.Shared.Enums;
using Liquidator;

namespace Liquidator.Stash;

public static class StashTabVisible
{
    public static int ItemCount
    {
        get
        {
            return (int)Ex.StashElement.VisibleStash.ItemCount;
        }
    }

    public static InventoryType Type
    {
        get
        {
            return Ex.StashElement.VisibleStash.InvType;
        }
    }

    public static int MaxItemCount
    {
        get
        {
            return (int)(Ex.StashElement.VisibleStash.TotalBoxesInInventoryRow * Ex.StashElement.VisibleStash.TotalBoxesInInventoryRow);
        }
    }

}

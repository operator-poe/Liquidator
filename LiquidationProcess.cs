using System.Collections.Generic;

namespace Liquidator;

public enum LiquidationProcessState
{
  Idle,
  Selling,
  WaitingForExchange,
  WaitingForExchangeToOpen,
}

public class LiquidationProcess
{
  public Dictionary<string, int> ItemsToSell { get; set; }
  public LiquidationProcessState State { get; set; }

  public LiquidationProcess()
  {
    ItemsToSell = Inventory.GetAvailableItems();
    State = LiquidationProcessState.Idle;
  }
}
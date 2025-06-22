using ExileCore;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using SharpDX;
using System.Windows.Forms;
using Vector2 = System.Numerics.Vector2;

namespace Liquidator;

public class Liquidator : BaseSettingsPlugin<LiquidatorSettings>
{
    internal static Liquidator Instance;
    public Scheduler Scheduler { get; private set; }

    public override bool Initialise()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Scheduler = new Scheduler();

        Input.RegisterKey(Settings.HotKeySettings.StartHotKey);
        Settings.HotKeySettings.StartHotKey.OnValueChanged += () => { Input.RegisterKey(Settings.HotKeySettings.StartHotKey); };

        Input.RegisterKey(Settings.HotKeySettings.StopHotKey);
        Settings.HotKeySettings.StopHotKey.OnValueChanged += () => { Input.RegisterKey(Settings.HotKeySettings.StopHotKey); };

        Input.RegisterKey(Settings.HotKeySettings.InventoryHotKey);
        Settings.HotKeySettings.InventoryHotKey.OnValueChanged += () => { Input.RegisterKey(Settings.HotKeySettings.InventoryHotKey); };

        return true;
    }

    public override void AreaChange(AreaInstance area)
    {
        //Perform once-per-zone processing here
        //For example, Radar builds the zone map texture here
    }

    public override Job Tick()
    {
        if (Settings.HotKeySettings.StartHotKey.PressedOnce())
        {
            Log.Debug("Test Hotkey pressed");
            if (Core.ParallelRunner.FindByName("_TEST_") == null)
            {
                Scheduler.AddTask(TestCoroutine(), "_TEST_");
            }
        }
        return null;
    }

    private LiquidationProcess _liquidationProcess;

    private async SyncTask<bool> TestCoroutine()
    {
        if (_liquidationProcess == null)
        {
            _liquidationProcess = new LiquidationProcess();
        }

        await MoveAround.FindAndClickFaustus();
        await InputAsync.Wait();
        await MoveAround.EnsureInventoryIsOpen();
        await InputAsync.Wait();
        await ExchangeActions.SelectLeftSideCurrency("Chaos Orb");
        return true;
    }

    public override void Render()
    {
        Scheduler.Run();
        if (_liquidationProcess != null)
        {
            var y = 100;
            foreach (var item in _liquidationProcess.ItemsToSell)
            {
                Graphics.DrawText(item.Key, new Vector2(100, y), Color.White);
                Graphics.DrawText(item.Value.ToString(), new Vector2(100, y + 20), Color.White);
                y += 40;
            }
        }
    }

    public override void EntityAdded(Entity entity)
    {
    }

    public void StopAllRoutines(bool skipSchedulerStop = false)
    {
        Log.Debug("Stopping all routines");
        if (!skipSchedulerStop)
        {
            Scheduler.Stop();
        }
        Scheduler.Clear();
        InputAsync.LOCK_CONTROLLER = false;
        InputAsync.IControllerEnd();
        Input.KeyUp(Keys.ControlKey);
        Input.KeyUp(Keys.ShiftKey);
    }
}
using System;
using Dalamud.Logging;
using Dalamud.Plugin;
using Dalamud.Game.Command;


namespace ARealmRecorded;

public class ARealmRecorded : IDalamudPlugin
{
    public string Name => "A Realm Recorded";

    private const string commandName = "/showplayback";
    public static ARealmRecorded Plugin { get; private set; }
    public static Configuration Config { get; private set; }

    private CommandManager CommandManager { get; init; }

    public ARealmRecorded(DalamudPluginInterface pluginInterface, CommandManager commandManager)
    {
        Plugin = this;
        DalamudApi.Initialize(this, pluginInterface);

        Config = (Configuration)DalamudApi.PluginInterface.GetPluginConfig() ?? new();
        Config.Initialize();

        try
        {
            Game.Initialize();

            //DalamudApi.Framework.Update += Update;
            DalamudApi.PluginInterface.UiBuilder.Draw += Draw;
            //DalamudApi.PluginInterface.UiBuilder.OpenConfigUi += ToggleConfig;
            this.CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });
        }
        catch (Exception e)
        {
            PluginLog.Error($"Failed loading ARealmRecorded\n{e}");
        }
    }

    //public void ToggleConfig() => PluginUI.isVisible ^= true;

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just display our main ui
        PluginUI.Draw();
    }

    public static void PrintEcho(string message) => DalamudApi.ChatGui.Print($"[A Realm Recorded] {message}");
    public static void PrintError(string message) => DalamudApi.ChatGui.PrintError($"[A Realm Recorded] {message}");

    //private void Update(Framework framework) { }

    private void Draw() => PluginUI.Draw();

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        Config.Save();

        //DalamudApi.Framework.Update -= Update;
        DalamudApi.PluginInterface.UiBuilder.Draw -= Draw;
        //DalamudApi.PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfig;
        DalamudApi.Dispose();

        Game.Dispose();
        Memory.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
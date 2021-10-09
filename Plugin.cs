using System;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using FFXIVAtkArrayDataBrowserPlugin.Attributes;

namespace FFXIVAtkArrayDataBrowserPlugin
{
    public class Plugin : IDalamudPlugin 
    {
        [PluginService] public static DalamudPluginInterface PluginInterface { get; set; } = null!;
        [PluginService] public static CommandManager Commands { get; set; } = null!;

        private readonly PluginCommandManager<Plugin> commandManager;
        private readonly PluginUI ui;

        public string Name => "AtkArrayData Browser";

        public Plugin()
        {
            this.ui = new PluginUI(this);
            this.commandManager = new PluginCommandManager<Plugin>(this);
            this.ui.IsVisible = true;
            PluginInterface.UiBuilder.Draw += this.ui.Draw;
        }
        
        [Command("/atkarraydata")]
        [HelpMessage("Show AtkArrayData browser.")]
        public void ShowUI(string command, string args)
        {
            this.ui.IsVisible = true;
        }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            this.commandManager.Dispose();
            PluginInterface.UiBuilder.Draw -= this.ui.Draw;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

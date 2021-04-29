using System;
using Dalamud.Plugin;
using FFXIVAtkArrayDataBrowserPlugin.Attributes;

namespace FFXIVAtkArrayDataBrowserPlugin
{
    public class Plugin : IDalamudPlugin
    {
        public DalamudPluginInterface pluginInterface;
        private PluginCommandManager<Plugin> commandManager;
        private PluginUI ui;

        public string Name => "AtkArrayData Browser";

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;

            this.ui = new PluginUI(this);
            this.pluginInterface.UiBuilder.OnBuildUi += this.ui.Draw;

            this.ui.IsVisible = true;

            this.commandManager = new PluginCommandManager<Plugin>(this, this.pluginInterface);
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

            this.pluginInterface.UiBuilder.OnBuildUi -= this.ui.Draw;

            this.pluginInterface.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

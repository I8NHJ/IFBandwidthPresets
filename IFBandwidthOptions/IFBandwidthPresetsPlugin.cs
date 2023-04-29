using SDRSharp.Common;
using System.Windows.Forms;

namespace SDRSharp.Plugin.Diagnostics
{
    public class IFBandwidthPresetsPlugin : ISharpPlugin, ICanLazyLoadGui, ISupportStatus, IExtendedNameProvider
    {
        private ISharpControl _control;

        private BandwidthPanel _configGui;

        public bool IsActive => _configGui != null && _configGui.Visible;

        public string DisplayName => "IF Bandwidth Presets";

        public string Category => "IF";

        public string MenuItemName => DisplayName;

        public UserControl Gui
        {
            get
            {
                LoadGui();
                return _configGui;
            }
        }

        public void Initialize(ISharpControl control)
        {
            _control = control;
        }

        public void LoadGui()
        {
            if (_configGui == null)
            {
                _configGui = new BandwidthPanel(_control);
            }
        }

        public void Close()
        {
        }
    }
}

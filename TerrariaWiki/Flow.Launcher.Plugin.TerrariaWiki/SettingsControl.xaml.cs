using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.TerrariaWiki
{
    public partial class SettingsControl 
    {
        public Settings _settings;

        public SettingsControl(Settings settings)
        {
            InitializeComponent();
            _settings = settings;
        }
    }
}

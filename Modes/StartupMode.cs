using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Modes
{
    internal class StartupMode : Mode
    {
        public StartupMode() { }

        public override Mode CheckForModeChange()
        {
            return new MainMenuMode();
        }
    }
}

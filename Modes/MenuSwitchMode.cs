using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Modes
{
    internal class MenuSwitchMode : Mode
    {
        public enum MenuToSwitchTo
        {
            Settings,
            SinglePlayer
        }

        private MenuToSwitchTo _switchTo;
        public MenuSwitchMode(MenuToSwitchTo nextMenu)
        {
            _switchTo = nextMenu;
        }

        public MenuToSwitchTo GetNextMenu()
        {
            return _switchTo;
        }

        public override Mode CheckForModeChange()
        {
            throw new NotImplementedException();
        }
    }
}

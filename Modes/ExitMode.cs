using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Modes
{
    internal class ExitMode : Mode
    {
        public ExitMode() { }

        public override Mode CheckForModeChange()
        {
            return null;
        }
    }
}

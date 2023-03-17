using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal abstract class UIElement : TTFObject
    {

        public UIElement() { }

        public override int IsCollisionPoint(Rectangle other)
        {
            return 0;
        }
    }
}

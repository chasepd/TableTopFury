using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Menus.Settings.Graphics
{
    internal class Resolution
    {
        private float width;
        private float height;
        public Resolution(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public float GetWidth()
        {
            return width;
        }

        public float GetHeight()
        {
            return height;
        }
    }
}

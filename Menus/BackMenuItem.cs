using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Modes;

namespace TableTopFury.Menus
{
    internal class BackMenuItem : MainMenuItem
    {
        public BackMenuItem(int menuPosition) : base()
        {
            _menuPosition = menuPosition;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("UniversalMenu-Back");
        }

        public override Mode CheckForNextScreen()
        {
            if (_navigate)
            {
                return new BackMode();
            }
            return null;
        }
    }
}

using Microsoft.Xna.Framework;
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
    internal class SinglePlayerMenuItem : MainMenuItem
    {
        public SinglePlayerMenuItem() : base()
        {
            _menuPosition = 6;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("MainMenu-SinglePlayer");
        }

        public override Mode CheckForNextScreen()
        {
            if (_navigate)
            {
                return new MenuSwitchMode(MenuSwitchMode.MenuToSwitchTo.SinglePlayer);
            }
            return null;
        }

    }
}

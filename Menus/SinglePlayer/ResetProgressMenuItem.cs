using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Modes;

namespace TableTopFury.Menus.SinglePlayer
{
    internal class ResetProgressMenuItem : MainMenuItem
    {
        public ResetProgressMenuItem(): base()
        {
            _menuPosition = 5;
        }

        public override void LoadContent()
        {
            texture = GameState.Content.Load<Texture2D>("SPMenu-ResetProgress");
        }

        public override Mode CheckForNextScreen()
        {
            if (_navigate)
            {
                return null;
            }
            return null;
        }
    }
}

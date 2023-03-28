using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Modes;

namespace TableTopFury.Menus.Versus
{
    internal class TwoPlayersMenuItem : MainMenuItem
    {
        public TwoPlayersMenuItem() : base()
        {
            _menuPosition = 4;
        }

        public override void LoadContent()
        {
            texture = GameState.Content.Load<Texture2D>("VersusMenu-2Players");
        }

        public override Mode CheckForNextScreen()
        {
            if (_navigate)
            {
                return new VersusMode(false);
            }
            return null;
        }
    }
}

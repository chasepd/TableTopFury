using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Modes;

namespace TableTopFury.Menus.Settings
{
    internal class GraphicsSettingsMenuItem : MainMenuItem
    {
        public GraphicsSettingsMenuItem() : base()
        {
            _menuPosition = 4;
        }

        public override void LoadContent()
        {
            texture = GameState.Content.Load<Texture2D>("SettingsMenu-Graphics");
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

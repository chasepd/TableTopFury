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
    internal class VersusMenuItem : MainMenuItem
    {
        public VersusMenuItem() : base()
        {
            _menuPosition = 7;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("MainMenu-Versus");
        }

        public override Mode CheckForNextScreen()
        {
            if (_navigate)
            {
                return new VersusMode();
            }
            return null;
        }
    }
}

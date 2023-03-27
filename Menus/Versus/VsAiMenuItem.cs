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
    internal class VsAiMenuItem : MainMenuItem
    {
        public VsAiMenuItem() : base()
        {
            _menuPosition = 5;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("VersusMenu-VSBot");
        }

        public override Mode CheckForNextScreen()
        {
            if (_navigate)
            {
                return new VersusMode(true);
            }
            return null;
        }
    }
}

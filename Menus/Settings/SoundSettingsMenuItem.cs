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
    internal class SoundSettingsMenuItem : MainMenuItem
    {
        public SoundSettingsMenuItem() : base()
        {
            _menuPosition = 6;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("SettingsMenu-Sound");
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

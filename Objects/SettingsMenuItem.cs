﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Modes;

namespace TableTopFury.Objects
{
    internal class SettingsMenuItem : MainMenuItem
    {
        public SettingsMenuItem() : base()
        {
            _menuPosition = 8;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("MainMenu-Settings");
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

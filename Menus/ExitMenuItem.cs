using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Modes;
using TableTopFury.Objects;

namespace TableTopFury.Menus
{
    internal class ExitMenuItem : MainMenuItem
    {
        public ExitMenuItem() : base()
        {
            _menuPosition = 9;
        }

        public override void LoadContent()
        {
            texture = GameState.Content.Load<Texture2D>("MainMenu-Exit");
        }

        public override void Update(List<TTFObject> objects)
        {
            base.Update(objects);
            sourceRectangle = new Rectangle(0 + (texture.Width / framesPerRow + 1) * (animationFrame - 1), 0, (texture.Width / framesPerRow) - 1, texture.Height / frameRows);
            _timeInExistence += GameState.GameTime.ElapsedGameTime.TotalSeconds;
        }

        public override Mode CheckForNextScreen()
        {
            if (_navigate)
            {
                return new ExitMode();
            }
            return null;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Menus
{
    internal class PopUpMenuItem
    {
        protected string text;
        protected Vector2 position;
        protected float scaleModifier;
        public PopUpMenuItem(string text, int menuPosition)
        {
            this.text = text;
            scaleModifier = GameState.Graphics.PreferredBackBufferHeight / 1440f;
            position = new Vector2(GameState.Graphics.PreferredBackBufferWidth / 4 + (20 * scaleModifier),
                GameState.Graphics.PreferredBackBufferHeight / 4 +((40 * scaleModifier) * menuPosition));
        }

        public virtual void Draw()
        {
            GameState.CurrentSpriteBatch.DrawString(
                GameState.Content.Load<SpriteFont>("Arial"),
                text,
                position,
                Color.Black,
                0f,
                Vector2.Zero,
                scaleModifier,
                SpriteEffects.None,
                0f
                );
        }
    }
}

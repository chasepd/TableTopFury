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
        protected bool selected;
        protected int menuPosition;
        public PopUpMenuItem(string text, int menuPosition)
        {
            this.text = text;
            scaleModifier = GameState.Graphics.PreferredBackBufferHeight / 1440f;
            position = new Vector2(GameState.Graphics.PreferredBackBufferWidth / 4 + (20 * scaleModifier),
                GameState.Graphics.PreferredBackBufferHeight / 4 +((40 * scaleModifier) * menuPosition));
            this.menuPosition = menuPosition;
        }

        public virtual void Update() 
        {
            scaleModifier = GameState.Graphics.PreferredBackBufferHeight / 1440f;
            position = new Vector2(GameState.Graphics.PreferredBackBufferWidth / 4 + (20 * scaleModifier),
                GameState.Graphics.PreferredBackBufferHeight / 4 + ((40 * scaleModifier) * menuPosition));
        }

        public virtual void Draw()
        {
            var color = Color.Black;
            if (selected)
            {
                color = Color.White;
            }
            GameState.CurrentSpriteBatch.DrawString(
                GameState.Content.Load<SpriteFont>("Arial"),
                text,
                position,
                color,
                0f,
                Vector2.Zero,
                scaleModifier,
                SpriteEffects.None,
                0f
                );
        }

        public void Select()
        {
            selected = true;
        }

        public void Unselect()
        {
            selected = false;
        }
    }
}

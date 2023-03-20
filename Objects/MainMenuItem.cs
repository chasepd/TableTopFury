using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal abstract class MainMenuItem : MenuItem
    {
        protected int _menuPosition;
        
        public override void Initialize(GraphicsDeviceManager graphics)
        {
            position = new Vector2(graphics.PreferredBackBufferWidth / 2 - ((texture.Width / framesPerRow) / 2), graphics.PreferredBackBufferHeight / 6 * _menuPosition - ((texture.Height / frameRows) / 2));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(
                 texture,
                 position,
                 sourceRectangle,
                 Color.White,
                 0,
                 new Vector2(texture.Width / (framesPerRow * 2), texture.Height / (frameRows * 2)),
                 1f,
                 SpriteEffects.None,
                 0f
                );
        }
    }
}

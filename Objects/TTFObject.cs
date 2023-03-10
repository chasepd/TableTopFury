using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal abstract class TTFObject
    {
        public Vector2 position;
        public Texture2D texture;
        public int animationFrame;
        public Rectangle sourceRectangle;

        public int framesPerRow;
        public int frameRows;

        public abstract void Initialize(GraphicsDeviceManager graphics);

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                 texture,
                 position,
                 sourceRectangle,
                 Color.White,
                 0f,
                 new Vector2(texture.Width / (framesPerRow * 2), texture.Height / (frameRows * 2)),
                 Vector2.One,
                 SpriteEffects.None,
                 0f
             );
        }
    }
}

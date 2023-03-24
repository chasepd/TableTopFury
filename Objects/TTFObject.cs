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
        public float rotation;
        public int speedX;
        public int speedY;
        public float scaleModifier;
        public int framesPerRow;
        public int frameRows;

        public abstract void Initialize(GraphicsDeviceManager graphics);

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects);        

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics);

        public abstract int IsCollisionPoint(Rectangle other);

        public int GetWidth()
        {
            return (int)(texture.Width / framesPerRow * scaleModifier);
        }
        public int GetHeight()
        {
            return (int)(texture.Height / frameRows * scaleModifier);
        }
    }
}

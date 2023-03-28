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

        public abstract void Initialize();

        public abstract void LoadContent();

        public abstract void Update(List<TTFObject> objects);        

        public abstract void Draw();

        public abstract int GetCollisionIntensity(Rectangle other);

        public float GetWidth()
        {
            return texture.Width / framesPerRow * scaleModifier;
        }
        public float GetHeight()
        {
            return texture.Height / frameRows * scaleModifier;
        }
    }
}

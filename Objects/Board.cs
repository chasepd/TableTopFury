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
    internal class Board : TTFObject
    {
        public Board() : base() 
        {
            frameRows = 1;
            framesPerRow = 2;
            animationFrame = 1;
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            scaleModifier = (float)(graphics.PreferredBackBufferHeight / 1440.0 * 0.9);
            position = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);            
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("BoardBackground");
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects)
        {
            sourceRectangle = new Rectangle((texture.Width / framesPerRow) * (animationFrame - 1), 0, texture.Width / framesPerRow, texture.Height / frameRows);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(
                 texture,
                 position,
                 sourceRectangle,
                 Color.White,
                 0f,
                 new Vector2(texture.Width / (framesPerRow * 2), texture.Height / (frameRows * 2)),
                 scaleModifier,
                 SpriteEffects.None,
                 0.9f
                );
        }

        public override int IsCollisionPoint(Rectangle other)
        {
            if (other.Location.Y + other.Height > position.Y + (texture.Height * scaleModifier / 2) || other.Location.Y < position.Y - (texture.Height * scaleModifier / 2))
            {
                return -50;
            }
            else
            {
                return 0;
            }
        }
    }
}

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

        public override void Initialize()
        {
            scaleModifier = (float)(GameState.Graphics.PreferredBackBufferHeight / 1440.0 * 0.9);
            position = new Vector2(GameState.Graphics.PreferredBackBufferWidth / 2, GameState.Graphics.PreferredBackBufferHeight / 2);            
        }

        public override void LoadContent()
        {
            texture = GameState.Content.Load<Texture2D>("BoardBackground");
        }

        public override void Update(List<TTFObject> objects)
        {
            sourceRectangle = new Rectangle((texture.Width / framesPerRow) * (animationFrame - 1), 0, texture.Width / framesPerRow, texture.Height / frameRows);
        }

        public override void Draw()
        {
            GameState.CurrentSpriteBatch.Draw(
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

        public override int GetCollisionIntensity(Rectangle other)
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal class LifeIndicator : UIElement
    {
        int playerNum;
        int lifeNum;
        const int sideBuffer = 90;
        const int lifeSpacing = 10;
        const int lifeY = 20;
        public LifeIndicator(int playerNum, int lifeNum) : base() 
        {
            this.playerNum = playerNum;
            this.lifeNum = lifeNum;
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            int lifeX;
            if (playerNum == 1)
            {
                lifeX = sideBuffer + ((texture.Width + lifeSpacing) * (lifeNum - 1)) ;
            }
            else
            {
                lifeX = graphics.PreferredBackBufferWidth - (sideBuffer + ((texture.Width + lifeSpacing) * (lifeNum - 1)));
            }
            position = new Vector2 (lifeX, lifeY);
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Life");
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects) {}

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(
                 texture,
                 position,
                 null,
                 Color.White,
                 0,
                 new Vector2(texture.Width / 2, texture.Height / 2),
                 1f,
                 SpriteEffects.None,
                 0f
                );
        }

    }
}

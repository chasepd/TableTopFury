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
        int sideBuffer = 90;
        int lifeSpacing = 20;
        int lifeY = 20;
        public LifeIndicator(int playerNum, int lifeNum) : base() 
        {
            this.playerNum = playerNum;
            this.lifeNum = lifeNum;
        }

        public override void Initialize()
        {
            int lifeX;

            scaleModifier = GameState.Graphics.PreferredBackBufferHeight / 480f;

            if (GameState.Graphics.PreferredBackBufferWidth * 9 > GameState.Graphics.PreferredBackBufferHeight * 16)
            {
                sideBuffer += (GameState.Graphics.PreferredBackBufferWidth - (16 * GameState.Graphics.PreferredBackBufferHeight) / 9) / 2;
            }

            sideBuffer = (int)(sideBuffer * scaleModifier);

            if (playerNum == 1)
            {
                lifeX = sideBuffer + ((texture.Width + lifeSpacing) * (lifeNum - 1)) ;
            }
            else
            {
                lifeX = GameState.Graphics.PreferredBackBufferWidth - (sideBuffer + ((texture.Width + lifeSpacing) * (lifeNum - 1)));
            }
            position = new Vector2 (lifeX, lifeY);



            lifeSpacing = (int)(lifeSpacing * scaleModifier);

            lifeY += (int)(scaleModifier * texture.Height / 2) - 16;
        }

        public override void LoadContent()
        {
            texture = GameState.Content.Load<Texture2D>("Life");
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
                 scaleModifier,
                 SpriteEffects.None,
                 0f
                );
        }

    }
}

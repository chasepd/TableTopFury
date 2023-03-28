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
        int sideBuffer;
        int lifeSpacing;
        int lifeY;
        int animationStep;
        Texture2D explosionTexture;
        double animationTimeTracker;
        bool isExploding;
        public LifeIndicator(int playerNum, int lifeNum) : base() 
        {
            this.playerNum = playerNum;
            this.lifeNum = lifeNum;
            sideBuffer = 460;
            lifeSpacing = 20;
            lifeY = 20;
            frameRows = 1;
            framesPerRow = 4;
            animationTimeTracker = 0.0;
            animationStep = 1;
            isExploding = false;
        }

        public override void Initialize()
        {
            int lifeX;

            scaleModifier = GameState.Graphics.PreferredBackBufferHeight / 1920f;

            if (GameState.Graphics.PreferredBackBufferWidth * 9 > GameState.Graphics.PreferredBackBufferHeight * 16)
            {
                sideBuffer += (GameState.Graphics.PreferredBackBufferWidth - (16 * GameState.Graphics.PreferredBackBufferHeight) / 9) / 2;
            }

            sideBuffer = (int)(sideBuffer * scaleModifier);

            if (playerNum == 1)
            {
                lifeX = (int)(sideBuffer + (((texture.Width * scaleModifier / framesPerRow) + lifeSpacing) * (lifeNum - 1)));
            }
            else
            {
                lifeX = (int)(GameState.Graphics.PreferredBackBufferWidth - (sideBuffer + (((texture.Width * scaleModifier / framesPerRow) + lifeSpacing) * (lifeNum - 1))));
            }
            position = new Vector2 (lifeX, lifeY);



            lifeSpacing = (int)(lifeSpacing * scaleModifier);

            lifeY += (int)(scaleModifier * texture.Height / frameRows / 2) - 16;
        }

        public override void LoadContent()
        {
            texture = GameState.Content.Load<Texture2D>("Life");
            explosionTexture = GameState.Content.Load<Texture2D>("LifeExplode");
        }

        public override void Update(List<TTFObject> objects) 
        {
            animationTimeTracker += GameState.GameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimeTracker > 0.2)
            {
                if (animationFrame + animationStep > framesPerRow || animationFrame + animationStep < 1)
                {
                    animationStep *= -1;
                }
                animationFrame += animationStep;
                animationTimeTracker = 0.0;
            }

            sourceRectangle = new Rectangle((texture.Width / framesPerRow) * (animationFrame - 1), 0, texture.Width / framesPerRow, texture.Height / frameRows);
        }

        public override void Draw()
        {
            if (!isExploding || animationFrame <= framesPerRow)
            {
                GameState.CurrentSpriteBatch.Draw(
                     texture,
                     position,
                     sourceRectangle,
                     Color.White,
                     0,
                     new Vector2(texture.Width / framesPerRow / 2, texture.Height / frameRows / 2),
                     scaleModifier,
                     SpriteEffects.None,
                     0f
                    );
            }
        }

        public void Explode()
        {
            animationFrame = 1;
            isExploding = true;            
            position = new Vector2(position.X - (1 * scaleModifier), position.Y - (20 * scaleModifier));
            framesPerRow = 21;
            texture = explosionTexture;
        }

        public bool IsDoneExploding()
        {
            return isExploding && animationFrame >= framesPerRow;
        }

    }
}

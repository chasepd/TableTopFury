using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TableTopFury
{
    public class TableTopFuryGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D ballTexture;
        Vector2 ballPosition;
        int frame;
        double timeTracker;
        Rectangle ballSourceRectangle;

        public TableTopFuryGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
               _graphics.PreferredBackBufferHeight / 2);

            frame = 1;
            timeTracker = 0.0;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("Ball");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            timeTracker += gameTime.ElapsedGameTime.TotalSeconds;
            
            if (timeTracker >= 0.1)
            {
                frame = frame + 1;
                timeTracker = 0.0;
            }
            if (frame < 1 || frame > 13)
            {
                frame = 1;
            }
            ballSourceRectangle = new Rectangle((ballTexture.Width / 13) * (frame - 1), 0, ballTexture.Width / 13, ballTexture.Height);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);
            

            _spriteBatch.Begin();
            _spriteBatch.Draw(
                ballTexture,
                ballPosition,
                ballSourceRectangle,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 26, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
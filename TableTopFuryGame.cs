using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using TableTopFury.Modes;

namespace TableTopFury
{
    public class TableTopFuryGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Mode _currentMode;

        public TableTopFuryGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            GameState.Graphics = _graphics;
            var displaymodes = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            //_graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _currentMode = new StartupMode();            
        }

        protected override void Initialize()
        {
            base.Initialize();
            GameState.Content = Content;
        }

        protected override void LoadContent()
        {            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
                        
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _currentMode.Update(gameTime, _graphics);

            Mode _nextMode = _currentMode.CheckForModeChange();

            if(_nextMode != null)
            {
                if(_nextMode is ExitMode)
                {
                    Exit();
                }
                _currentMode = _nextMode;                
                _currentMode.LoadContent(Content);
                _currentMode.Initialize(_graphics);
                GameState.CurrentMode = _nextMode;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            

            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            _currentMode.Draw(gameTime, _spriteBatch, _graphics);
            
            _spriteBatch.End();            

            base.Draw(gameTime);
        }
    }
}
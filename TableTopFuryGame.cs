﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using TableTopFury.Objects;

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
            var displaymodes = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            //_graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _currentMode = new MainMenuMode();

        }

        protected override void Initialize()
        {
            base.Initialize();

            _currentMode.Initialize(_graphics);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentMode.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
                        
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _currentMode.Update(gameTime, _graphics);

            Mode _nextMode = _currentMode.CheckForModeChange();

            if(_nextMode != null)
            {
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
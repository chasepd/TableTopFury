using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using TableTopFury.Menus;
using TableTopFury.Modes;

namespace TableTopFury
{
    public class TableTopFuryGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Mode _currentMode;
        //private List<PopUpMenu> popUpMenus;

        public TableTopFuryGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            GameState.Graphics = _graphics;
            var displaymode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            _graphics.PreferredBackBufferHeight = displaymode.Height;
            _graphics.PreferredBackBufferWidth = displaymode.Width;
            GameState.CurrentResolution = new Menus.Settings.Graphics.Resolution(displaymode.Width, displaymode.Height);
            //_graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _currentMode = new StartupMode();            
            //popUpMenus = new List<PopUpMenu>();
        }

        protected override void Initialize()
        {
            base.Initialize();
            GameState.Content = Content;
        }

        protected override void LoadContent()
        {            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameState.MenuArrow = Content.Load<Texture2D>("MenuChoiceArrow");
        }

        protected override void Update(GameTime gameTime)
        {
            bool resolutionChanged = false;
            if (_graphics.PreferredBackBufferHeight != (int)GameState.CurrentResolution.GetHeight())
            {
                _graphics.PreferredBackBufferHeight = (int)GameState.CurrentResolution.GetHeight();
                resolutionChanged = true;
            }
            if (_graphics.PreferredBackBufferWidth != (int)GameState.CurrentResolution.GetWidth())
            {
                _graphics.PreferredBackBufferWidth = (int)GameState.CurrentResolution.GetWidth();
                resolutionChanged = true;
            }

            if (resolutionChanged)
            {
                _graphics.ApplyChanges();
            }
            GameState.GameTime = gameTime;
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
            //    if (popUpMenus.Count > 0)
            //    {
            //        popUpMenus.Clear();
            //    }
            //    else
            //    {
            //        PopUpMenu popUp = new PopUpMenu();
            //        popUp.Initialize();
            //        popUpMenus.Add(popUp);
            //    }
            //}
            //    //Exit();

            _currentMode.Update();

            Mode _nextMode = _currentMode.CheckForModeChange();

            if(_nextMode != null)
            {
                if(_nextMode is ExitMode)
                {
                    Exit();
                }
                _currentMode = _nextMode;                
                _currentMode.LoadContent();
                _currentMode.Initialize();
                GameState.CurrentMode = _nextMode;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            

            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            GameState.CurrentSpriteBatch = _spriteBatch;

            _currentMode.Draw();
            //foreach (PopUpMenu popUpMenu in popUpMenus)
            //{
            //    popUpMenu.Draw();
            //}
            _spriteBatch.End();            

            base.Draw(gameTime);
        }
    }
}
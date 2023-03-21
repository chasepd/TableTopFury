using Microsoft.Xna.Framework;
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
        //private LifeBar _player1LifeBar;
        //private LifeBar _player2LifeBar;
        //private Dictionary<int, LifeBar> _lifeBars;
        //List<TTFObject> _collisionObjects;
        //List<TTFObject> _uiObjects;
        //Dictionary<Ball, bool> _ballExplosionTracker;
        //bool gameEnded = false;
        //private int _losingPlayer;
        //private Texture2D _playerOneWinsTexture;
        //private Texture2D _playerTwoWinsTexture;
        //private int _messageFrame;
        //private double _messsageAnimationTimer;
        //private const int maxMessageFrames = 2;
        //private Song _battleTheme;

        public TableTopFuryGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            var displaymodes = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
            _graphics.PreferredBackBufferHeight = 1440;
            _graphics.PreferredBackBufferWidth = 3440;
            //_graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _currentMode = new MainMenuMode();
            //_ballExplosionTracker = new Dictionary<Ball, bool>();
            //_lifeBars = new Dictionary<int, LifeBar>();
            //_player1LifeBar = new LifeBar(1);
            //_player2LifeBar = new LifeBar(2);
            //_lifeBars[1] = _player1LifeBar;
            //_lifeBars[2] = _player2LifeBar;
            //Ball ball = new RegularBall();
            //_ballExplosionTracker[ball] = false;
            //_uiObjects = new List<TTFObject>() { _player1LifeBar, _player2LifeBar };
            //_collisionObjects = new List<TTFObject>() { new PlayerPaddle(1), new PlayerPaddle(2), ball};
            //gameEnded = false;
            //_losingPlayer = 0;
            //_messageFrame = 1;
            //_messsageAnimationTimer = 0.0;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _currentMode.Initialize(_graphics);

            //foreach (TTFObject obj in _collisionObjects)
            //{
            //    obj.Initialize(_graphics);
            //}
            //foreach (TTFObject obj in _uiObjects)
            //{
            //    obj.Initialize(_graphics);
            //}

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //_playerOneWinsTexture = Content.Load<Texture2D>("Player1Win");
            //_playerTwoWinsTexture = Content.Load<Texture2D>("Player2Win");
            //_battleTheme = Content.Load<Song>("BattleMusic");

            //MediaPlayer.Play(_battleTheme);
            //MediaPlayer.IsRepeating = true;

            //foreach (TTFObject obj in _collisionObjects)
            //{
            //    obj.LoadContent(Content);
            //}
            //foreach (TTFObject obj in _uiObjects)
            //{
            //    obj.LoadContent(Content);
            //}
            // TODO: use this.Content to load your game content here
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
            }

            //foreach (TTFObject obj in _uiObjects)
            //{
            //    obj.Update(gameTime, _graphics, _collisionObjects);
            //}
            //if (!gameEnded)
            //{
            //    foreach (TTFObject obj in _collisionObjects)
            //    {
            //        obj.Update(gameTime, _graphics, _collisionObjects);
            //        if (obj is Ball)
            //        {
            //            Ball ball = (Ball)obj;
            //            if (ball.isExploding && !_ballExplosionTracker[ball])
            //            {
            //                _ballExplosionTracker[ball] = true;
            //                if (_lifeBars[ball.GetPlayerDamaged()].TakeDamage(ball.DamageValue()))
            //                {
            //                    gameEnded = true;
            //                    _losingPlayer = ball.GetPlayerDamaged();
            //                    break;
            //                }
            //            }
            //            else if (!ball.isExploding && _ballExplosionTracker[ball])
            //            {
            //                _ballExplosionTracker[ball] = false;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    _messsageAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //    if (_messsageAnimationTimer > 1)
            //    {
            //        _messsageAnimationTimer = 0;
            //    }
            //    if (_messageFrame > maxMessageFrames)
            //    {
            //        _messageFrame = 0;
            //    }
            //}
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            

            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            _currentMode.Draw(gameTime, _spriteBatch, _graphics);
            //foreach (TTFObject obj in _uiObjects)
            //{
            //    obj.Draw(gameTime, _spriteBatch, _graphics);
            //}
            //if (!gameEnded)
            //{
            //    foreach (TTFObject obj in _collisionObjects)
            //    {
            //        obj.Draw(gameTime, _spriteBatch, _graphics);
            //    }
            //}
            //else
            //{
            //    Texture2D winMessageTexture;
            //    if(_losingPlayer == 1)
            //    {
            //        winMessageTexture = _playerTwoWinsTexture;
            //    }
            //    else
            //    {
            //        winMessageTexture= _playerOneWinsTexture;
            //    }
            //    Rectangle sourceRectangle = new Rectangle(winMessageTexture.Width * (_messageFrame - 1), 0, winMessageTexture.Width / 2, winMessageTexture.Height);
            //    _spriteBatch.Draw(
            //     winMessageTexture,
            //     new Vector2(_graphics.PreferredBackBufferWidth / 2 + winMessageTexture.Width / (maxMessageFrames * 2) , _graphics.PreferredBackBufferHeight / 2 + winMessageTexture.Height / (maxMessageFrames * 2)),
            //     sourceRectangle,
            //     Color.White,
            //     0,
            //     new Vector2(winMessageTexture.Width / 2, winMessageTexture.Height / 2),
            //     1f,
            //     SpriteEffects.None,
            //     0f
            //    );
            //}
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal class VersusMode : GameplayMode
    {
        private LifeBar _player1LifeBar;
        private LifeBar _player2LifeBar;
        private Dictionary<int, LifeBar> _lifeBars;
        List<TTFObject> _collisionObjects;
        List<TTFObject> _uiObjects;
        Dictionary<Ball, bool> _ballExplosionTracker;
        bool gameEnded = false;
        private int _losingPlayer;
        private Texture2D _playerOneWinsTexture;
        private Texture2D _playerTwoWinsTexture;
        private int _messageFrame;
        private double _messsageAnimationTimer;
        private const int maxMessageFrames = 2;
        private Song _battleTheme;

        public VersusMode() 
        {
            _ballExplosionTracker = new Dictionary<Ball, bool>();
            _lifeBars = new Dictionary<int, LifeBar>();
            _player1LifeBar = new LifeBar(1);
            _player2LifeBar = new LifeBar(2);
            _lifeBars[1] = _player1LifeBar;
            _lifeBars[2] = _player2LifeBar;
            Ball ball = new RegularBall();
            _ballExplosionTracker[ball] = false;
            _uiObjects = new List<TTFObject>() { _player1LifeBar, _player2LifeBar };
            _collisionObjects = new List<TTFObject>() { new PlayerPaddle(1), new PlayerPaddle(2), ball };
            gameEnded = false;
            _losingPlayer = 0;
            _messageFrame = 1;
            _messsageAnimationTimer = 0.0;
        }
        public override void Initialize(GraphicsDeviceManager graphics)
        {
            base.Initialize(graphics);
            
            foreach (TTFObject obj in _collisionObjects)
            {
                obj.Initialize(graphics);
            }
            foreach (TTFObject obj in _uiObjects)
            {
                obj.Initialize(graphics);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            _playerOneWinsTexture = content.Load<Texture2D>("Player1Win");
            _playerTwoWinsTexture = content.Load<Texture2D>("Player2Win");
            _battleTheme = content.Load<Song>("BattleMusic");

            MediaPlayer.Play(_battleTheme);
            MediaPlayer.IsRepeating = true;

            foreach (TTFObject obj in _collisionObjects)
            {
                obj.LoadContent(content);
            }
            foreach (TTFObject obj in _uiObjects)
            {
                obj.LoadContent(content);
            }
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            base.Update(gameTime, graphics);

            foreach (TTFObject obj in _uiObjects)
            {
                obj.Update(gameTime, graphics, _collisionObjects);
            }
            if (!gameEnded)
            {
                foreach (TTFObject obj in _collisionObjects)
                {
                    obj.Update(gameTime, graphics, _collisionObjects);
                    if (obj is Ball)
                    {
                        Ball ball = (Ball)obj;
                        if (ball.isExploding && !_ballExplosionTracker[ball])
                        {
                            _ballExplosionTracker[ball] = true;
                            if (_lifeBars[ball.GetPlayerDamaged()].TakeDamage(ball.DamageValue()))
                            {
                                gameEnded = true;
                                _losingPlayer = ball.GetPlayerDamaged();
                                break;
                            }
                        }
                        else if (!ball.isExploding && _ballExplosionTracker[ball])
                        {
                            _ballExplosionTracker[ball] = false;
                        }
                    }
                }
            }
            else
            {
                _messsageAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (_messsageAnimationTimer > 1)
                {
                    _messsageAnimationTimer = 0;
                }
                if (_messageFrame > maxMessageFrames)
                {
                    _messageFrame = 0;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(gameTime, spriteBatch, graphics);

            foreach (TTFObject obj in _uiObjects)
            {
                obj.Draw(gameTime, spriteBatch, graphics);
            }
            if (!gameEnded)
            {
                foreach (TTFObject obj in _collisionObjects)
                {
                    obj.Draw(gameTime, spriteBatch, graphics);
                }
            }
            else
            {
                Texture2D winMessageTexture;
                if (_losingPlayer == 1)
                {
                    winMessageTexture = _playerTwoWinsTexture;
                }
                else
                {
                    winMessageTexture = _playerOneWinsTexture;
                }
                Rectangle sourceRectangle = new Rectangle(winMessageTexture.Width * (_messageFrame - 1), 0, winMessageTexture.Width / 2, winMessageTexture.Height);
                spriteBatch.Draw(
                 winMessageTexture,
                 new Vector2(graphics.PreferredBackBufferWidth / 2 + winMessageTexture.Width / (maxMessageFrames * 2), graphics.PreferredBackBufferHeight / 2 + winMessageTexture.Height / (maxMessageFrames * 2)),
                 sourceRectangle,
                 Color.White,
                 0,
                 new Vector2(winMessageTexture.Width / 2, winMessageTexture.Height / 2),
                 1f,
                 SpriteEffects.None,
                 0f
                );
            }
        }
    }
}

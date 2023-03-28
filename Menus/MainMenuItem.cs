using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Menus
{
    internal abstract class MainMenuItem : MenuItem
    {
        protected int _menuPosition;
        protected float _scale;
        protected const int _maxMenuPositions = 10;

        public override void Initialize()
        {
            position = new Vector2(GameState.Graphics.PreferredBackBufferWidth / 2, GameState.Graphics.PreferredBackBufferHeight / _maxMenuPositions * _menuPosition);
            _scale = 0.75f * (float)(GameState.Graphics.PreferredBackBufferHeight / 1440.0);
        }

        public override void Draw()
        {
            GameState.CurrentSpriteBatch.Draw(
                 texture,
                 position,
                 sourceRectangle,
                 Color.White,
                 0,
                 new Vector2(texture.Width / (framesPerRow * 2), texture.Height / (frameRows * 2)),
                 _scale,
                 SpriteEffects.None,
                 0f
                );
        }
    }
}

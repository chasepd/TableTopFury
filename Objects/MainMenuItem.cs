﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal abstract class MainMenuItem : MenuItem
    {
        protected int _menuPosition;
        protected const float _scale = 0.25f;
        protected const int _maxMenuPositions = 10;
        
        public override void Initialize(GraphicsDeviceManager graphics)
        {
            position = new Vector2(graphics.PreferredBackBufferWidth / 2 , graphics.PreferredBackBufferHeight / _maxMenuPositions * _menuPosition);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(
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
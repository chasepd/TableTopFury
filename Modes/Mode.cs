﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Objects;

namespace TableTopFury.Modes
{
    internal abstract class Mode
    {

        private List<TTFObject> _onScreenObjects;
        public Mode()
        {
            _onScreenObjects = new List<TTFObject>();
        }

        protected virtual void AddOnscreenObject(TTFObject obj)
        {
            _onScreenObjects.Add(obj);
        }

        protected virtual void ClearOnscreenObjects()
        {
            _onScreenObjects?.Clear();
        }

        public virtual void Initialize()
        {
            foreach (TTFObject obj in _onScreenObjects)
            {
                obj.Initialize();
            }
        }

        public virtual void LoadContent()
        {
            foreach (TTFObject obj in _onScreenObjects)
            {
                obj.LoadContent();
            }
        }

        public virtual void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            foreach (TTFObject obj in _onScreenObjects)
            {
                obj.Update(gameTime, graphics, _onScreenObjects);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            foreach (TTFObject obj in _onScreenObjects)
            {
                obj.Draw(gameTime, spriteBatch, graphics);
            }
        }

        public abstract Mode CheckForModeChange();
    }
}

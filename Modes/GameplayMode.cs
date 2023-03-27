using Microsoft.Xna.Framework;
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
    internal abstract class GameplayMode : Mode
    {
        protected List<TTFObject> _collisionObjects;
        protected List<UIElement> _uiObjects;

        protected GameplayMode()
        {
            _collisionObjects = new List<TTFObject>();
            _uiObjects = new List<UIElement>();
        }

        protected override void AddOnscreenObject(TTFObject obj)
        {
            if (obj.GetType() == typeof(UIElement))
            {
                _uiObjects.Add((UIElement)obj);
            }
            else
            {
                _collisionObjects.Add(obj);
            }
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
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
            foreach (TTFObject obj in _collisionObjects)
            {
                obj.Update(gameTime, graphics, _collisionObjects);
            }
            foreach (TTFObject obj in _uiObjects)
            {
                obj.Update(gameTime, graphics, new List<TTFObject>());
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            foreach (TTFObject obj in _collisionObjects)
            {
                obj.Draw(gameTime, spriteBatch, graphics);
            }
            foreach (TTFObject obj in _uiObjects)
            {
                obj.Draw(gameTime, spriteBatch, graphics);
            }
        }
    }
}

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

        public override void Initialize()
        {
            foreach (TTFObject obj in _collisionObjects)
            {
                obj.Initialize();
            }

            foreach (TTFObject obj in _uiObjects)
            {
                obj.Initialize();
            }
        }

        public override void LoadContent()
        {
            foreach (TTFObject obj in _collisionObjects)
            {
                obj.LoadContent();
            }
            foreach (TTFObject obj in _uiObjects)
            {
                obj.LoadContent();
            }
        }

        public override void Update()
        {
            foreach (TTFObject obj in _collisionObjects)
            {
                obj.Update(_collisionObjects);
            }
            foreach (TTFObject obj in _uiObjects)
            {
                obj.Update(new List<TTFObject>());
            }
        }

        public override void Draw()
        {
            foreach (TTFObject obj in _collisionObjects)
            {
                obj.Draw();
            }
            foreach (TTFObject obj in _uiObjects)
            {
                obj.Draw();
            }
        }
    }
}

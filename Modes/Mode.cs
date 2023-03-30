using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Menus;
using TableTopFury.Objects;

namespace TableTopFury.Modes
{
    internal abstract class Mode
    {

        private List<TTFObject> _onScreenObjects;
        protected PopUpMenu currentPopUpMenu;
        public Mode()
        {
            _onScreenObjects = new List<TTFObject>();
            currentPopUpMenu = null;
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

        public virtual void Update()
        {
            if(currentPopUpMenu != null)
            {
                currentPopUpMenu.Update();
            }
            foreach (TTFObject obj in _onScreenObjects)
            {
                obj.Update(_onScreenObjects);
            }
        }

        public virtual void Draw()
        {
            foreach (TTFObject obj in _onScreenObjects)
            {
                obj.Draw();
            }
            if(currentPopUpMenu != null)
            {
                currentPopUpMenu.Draw();
            }
        }

        public abstract Mode CheckForModeChange();
    }
}

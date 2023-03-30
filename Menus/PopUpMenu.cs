using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Menus
{
    internal abstract class PopUpMenu
    {

        protected Rectangle drawBoundaries;
        private List<PopUpMenuItem> _menuItems;
        private int _selectedIndex;
        private const double _selectionDelay = 0.2;
        private double _selectionTimeTracker;

        public PopUpMenu() 
        {
            _menuItems = new List<PopUpMenuItem>();            
            _selectedIndex = 0;
            _selectionTimeTracker = 0;
        }

        public virtual void Initialize()
        {
            drawBoundaries = new Rectangle(
                GameState.Graphics.PreferredBackBufferWidth / 4,
                GameState.Graphics.PreferredBackBufferHeight / 4,
                GameState.Graphics.PreferredBackBufferWidth / 2,
                GameState.Graphics.PreferredBackBufferHeight / 2);
            _menuItems = GetMenuItems();
        }

        protected abstract List<PopUpMenuItem> GetMenuItems();

        protected virtual void ClearMenuItems()
        {
            _menuItems.Clear();
        }

        public abstract void LoadContent();

        public virtual void Update()
        {
            drawBoundaries = new Rectangle(
                GameState.Graphics.PreferredBackBufferWidth / 4,
                GameState.Graphics.PreferredBackBufferHeight / 4,
                GameState.Graphics.PreferredBackBufferWidth / 2,
                GameState.Graphics.PreferredBackBufferHeight / 2);
            _selectionTimeTracker += GameState.GameTime.ElapsedGameTime.TotalSeconds;
            var kstate = Keyboard.GetState();
            if ((kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W)) && _selectionTimeTracker > _selectionDelay && _selectedIndex > 0)
            {
                _selectedIndex--;
                _selectionTimeTracker = 0;
            }
            else if ((kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)) && _selectionTimeTracker > _selectionDelay && _selectedIndex < _menuItems.Count - 1)
            {
                _selectedIndex++;
                _selectionTimeTracker = 0;
            }

            foreach (var item in _menuItems)
            {
                item.Unselect();
            }
            _menuItems[_selectedIndex].Select();

            foreach(var item in _menuItems)
            {
                item.Update();
            }
        }

        public virtual void Draw() 
        {            
            Texture2D _texture;

            _texture = new Texture2D(GameState.Graphics.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.Orange });

            GameState.CurrentSpriteBatch.Draw(_texture, drawBoundaries, Color.White);

            foreach (PopUpMenuItem menuItem in _menuItems)
            {
                menuItem.Draw();
            }
        }
    }
}

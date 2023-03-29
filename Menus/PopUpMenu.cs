using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public PopUpMenu() 
        {
            _menuItems = new List<PopUpMenuItem>();
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

        public abstract void Update();

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

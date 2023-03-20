using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal class MainMenuMode : Mode
    {
        protected int _selectedItem;
        protected double _selectionTimeTracker;
        protected List<MainMenuItem> _menuItems;
        public MainMenuMode() : base()
        {
            _selectedItem = 0;
            _selectionTimeTracker = 0.0;
            _menuItems = new List<MainMenuItem>();
            _menuItems.Add(new SinglePlayerMenuItem());
            _menuItems.Add(new VersusMenuItem());

            foreach (MainMenuItem item in _menuItems)
            {
                AddOnscreenObject(item);
            }
            _menuItems[_selectedItem].Select();
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            base.Update(gameTime, graphics);
            var kstate = Keyboard.GetState();



            if (kstate.IsKeyDown(Keys.Up) && _selectedItem > 0)
            {
                _selectionTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
                if (_selectionTimeTracker > 0.5)
                {
                    _selectedItem -= 1;
                    _selectionTimeTracker = 0.0;
                }
            }
            else if (kstate.IsKeyDown(Keys.Down) && _selectedItem < _menuItems.Count - 1)
            {
                _selectionTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
                if (_selectionTimeTracker > 0.5)
                {
                    _selectedItem += 1;
                    _selectionTimeTracker = 0.0;
                }
            }
        }
    }
}

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
        protected const double _selectionTimeDelay = 0.05;
        protected Mode _nextMode;
        public MainMenuMode() : base()
        {
            _selectedItem = 0;
            _selectionTimeTracker = 0.0;
            _menuItems = new List<MainMenuItem>() { new SinglePlayerMenuItem(), new VersusMenuItem() };
            _nextMode = null;

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

            Mode nextScreenCheck = _menuItems[_selectedItem].CheckForNextScreen();
            if (nextScreenCheck != null)
            {
                _nextMode = nextScreenCheck;
            }
            else
            {

                if (kstate.IsKeyDown(Keys.Up) && _selectedItem > 0)
                {
                    _selectionTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
                    if (_selectionTimeTracker > _selectionTimeDelay)
                    {
                        _menuItems[_selectedItem].Unselect();
                        _selectedItem -= 1;
                        _selectionTimeTracker = 0.0;

                    }
                }
                else if (kstate.IsKeyDown(Keys.Down) && _selectedItem < _menuItems.Count - 1)
                {
                    _selectionTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
                    if (_selectionTimeTracker > _selectionTimeDelay)
                    {
                        _menuItems[_selectedItem].Unselect();
                        _selectedItem += 1;
                        _selectionTimeTracker = 0.0;
                    }
                }
                _menuItems[_selectedItem].Select();
            }
        }

        public override Mode CheckForModeChange()
        {
            return _nextMode;
        }
    }
}

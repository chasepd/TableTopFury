using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Menus;
using TableTopFury.Menus.Settings;
using TableTopFury.Menus.SinglePlayer;
using TableTopFury.Menus.Versus;
using TableTopFury.Objects;

namespace TableTopFury.Modes
{
    internal class MainMenuMode : Mode
    {
        protected int _selectedItem;
        protected double _selectionTimeTracker;
        protected List<MainMenuItem> _menuItems;
        protected const double _selectionTimeDelay = 0.4;
        protected Mode _nextMode;
        protected Stack<MenuSwitchMode.MenuToSwitchTo> menuHistory;
        protected MenuSwitchMode.MenuToSwitchTo currentMenu;
        protected Song _menuTheme;
        public MainMenuMode() : base()
        {        
            currentMenu = MenuSwitchMode.MenuToSwitchTo.MainMenu;
            menuHistory = new Stack<MenuSwitchMode.MenuToSwitchTo>();
            InitializeMenu();            
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _menuTheme = GameState.Content.Load<Song>("MenuLoop");

            MediaPlayer.Play(_menuTheme);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            base.Update(gameTime, graphics);
            var kstate = Keyboard.GetState();
            _selectionTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;

            Mode nextScreenCheck = _menuItems[_selectedItem].CheckForNextScreen();
            if (nextScreenCheck != null && nextScreenCheck is not MenuSwitchMode && nextScreenCheck is not BackMode)
            {
                if (_selectionTimeTracker > _selectionTimeDelay)
                {
                    _selectionTimeTracker = 0.0;
                    _nextMode = nextScreenCheck;
                }
            }
            else if(nextScreenCheck is MenuSwitchMode)
            {
                if (_selectionTimeTracker > _selectionTimeDelay)
                {
                    _selectionTimeTracker = 0.0;
                    menuHistory.Push(currentMenu);
                    currentMenu = ((MenuSwitchMode)nextScreenCheck).GetNextMenu();
                    InitializeMenu();
                    foreach (MainMenuItem item in _menuItems)
                    {                        
                        item.Update(gameTime, graphics, new List<TTFObject>());
                    }
                }
            }
            else if (nextScreenCheck is BackMode)
            {
                currentMenu = menuHistory.Pop();
                InitializeMenu();
            }
            else
            {

                if (kstate.IsKeyDown(Keys.Up) && _selectedItem > 0)
                {
                    
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
                else
                {
                    _selectionTimeTracker = 1000;
                }
                _menuItems[_selectedItem].Select();
            }
        }

        public override Mode CheckForModeChange()
        {
            return _nextMode;
        }

        private void InitializeMenu()
        {
            _selectedItem = 0;
            _selectionTimeTracker = 0.0;
            _nextMode = null;
            switch (currentMenu)
            {
                case MenuSwitchMode.MenuToSwitchTo.MainMenu:
                    _menuItems = new List<MainMenuItem>() { new SinglePlayerMenuItem(), new VersusMenuItem(), new SettingsMenuItem(), new ExitMenuItem() };
                    break;
                case MenuSwitchMode.MenuToSwitchTo.Settings:
                    _menuItems = new List<MainMenuItem>() { new GraphicsSettingsMenuItem(), new ControlsSettingsMenuItem(), new SoundSettingsMenuItem(), new BackMenuItem(7) };
                    break;
                case MenuSwitchMode.MenuToSwitchTo.SinglePlayer:
                    _menuItems = new List<MainMenuItem>() { new CampaignMenuItem(), new ResetProgressMenuItem(), new BackMenuItem(6) };
                    break;
                case MenuSwitchMode.MenuToSwitchTo.Versus:
                    _menuItems = new List<MainMenuItem>() { new TwoPlayersMenuItem(), new VsAiMenuItem(), new BackMenuItem(6) };
                    break;
            }

            ClearOnscreenObjects();
            foreach (MainMenuItem item in _menuItems)
            {
                item.Initialize();
                item.LoadContent();
                AddOnscreenObject(item);
            }
            _menuItems[_selectedItem].Select();
        }
    }
}

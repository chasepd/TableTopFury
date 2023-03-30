using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Menus.Settings.Graphics
{
    internal class GraphicsPopUpMenu : PopUpMenu
    {
        List<Resolution> resolutions;
        protected int selectedResolutionIndex;
        protected ArrowChoicePopUpMenuItem _resolutionChooser;
        public GraphicsPopUpMenu() : base() 
        {
            
        }

        public override void Initialize()
        {
            
            resolutions = new List<Resolution>();
            var resolutionsAvailable = GameState.Graphics.GraphicsDevice.Adapter.SupportedDisplayModes;
            foreach ( var resolution in resolutionsAvailable ) 
            { 
                resolutions.Add(new Resolution(resolution.Width, resolution.Height));
            }
            int counter = 0;
            foreach (var resolution in resolutions)
            {
                if (resolution.GetWidth() == GameState.CurrentResolution.GetWidth() && resolution.GetHeight() == GameState.CurrentResolution.GetHeight())
                {
                    break;
                }
                counter++;
            }
            selectedResolutionIndex = counter;
            base.Initialize();
        }

        public override void LoadContent() { }

        protected override List<PopUpMenuItem> GetMenuItems()
        {
            List<PopUpMenuItem> items = new List<PopUpMenuItem>();
            
            List<string> resolutionTexts = new List<string>();

            foreach (var resolution in resolutions )
            {
                resolutionTexts.Add(resolution.GetWidth().ToString() + "x" + resolution.GetHeight().ToString());
            }
            _resolutionChooser = new ArrowChoicePopUpMenuItem("Resolution", 1, resolutionTexts, selectedResolutionIndex);
            items.Add(_resolutionChooser);
            return items;
        }

        public override void Update()
        {
            base.Update();
            var selectedResolution = _resolutionChooser.GetSelectedChoice();
            int selectedIndex = 0;
            foreach (var resolution in resolutions)
            {
                if(resolution.GetWidth().ToString() + "x" + resolution.GetHeight().ToString() == selectedResolution)
                {
                    selectedIndex = resolutions.IndexOf(resolution);
                    break;
                }
            }
            GameState.CurrentResolution = resolutions[selectedIndex];
        }
    }
}

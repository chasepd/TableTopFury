using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Menus.Settings.Graphics
{
    internal class GraphicsPopUpMenu : PopUpMenu
    {
        public GraphicsPopUpMenu() : base() { }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent() { }

        protected override List<PopUpMenuItem> GetMenuItems()
        {
            List<PopUpMenuItem> items = new List<PopUpMenuItem>();
            items.Add(new PopUpMenuItem("TEST THIS STUFF", 1));
            return items;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}

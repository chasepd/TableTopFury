using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal abstract class MenuItem : TTFObject
    {
        protected int value;
        protected bool selected;
        protected bool _navigate;

        public MenuItem() 
        {
            value = 0;
            selected = false;
            animationFrame = 1;
            frameRows = 1;
            framesPerRow = 2;
        }
        public virtual int GetCurrentValue()
        {
            return value;
        }

        public virtual int SetCurrentValue(int value)
        {
            this.value = value;

            return this.value;
        }

        public virtual void Select()
        {
            selected = true;
        }

        public virtual void Unselect()
        {
            selected = false;
        }
        
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects)
        {
            if (selected)
            {
                animationFrame = 2;
                var kstate = Keyboard.GetState();
                if (kstate.IsKeyDown(Keys.Enter)) 
                { 
                    _navigate = true;
                }
            }
            else
            {
                animationFrame = 1;
            }

            sourceRectangle = new Rectangle(0 + ((texture.Width / framesPerRow  + 1) * (animationFrame - 1)), 0, texture.Width / framesPerRow, texture.Height / frameRows);
        }

        public abstract Mode CheckForNextScreen();


        public override int GetCollisionIntensity(Rectangle other)
        {
            return 0;
        }

    }
}

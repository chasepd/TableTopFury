using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Modes;
using TableTopFury.Objects;

namespace TableTopFury.Menus
{
    internal abstract class MenuItem : TTFObject
    {
        protected int value;
        protected bool selected;
        protected bool _navigate;
        protected double _timeInExistence;
        protected bool canBeSelected;

        public MenuItem()
        {
            value = 0;
            selected = false;
            canBeSelected = false;
            animationFrame = 1;
            frameRows = 1;
            framesPerRow = 2;
            _timeInExistence = 0.0;
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

        public virtual bool IsSelected()
        {
            return selected;
        }

        public override void Update(List<TTFObject> objects)
        {
            if (selected)
            {
                animationFrame = 2;
                if (_timeInExistence > 0.09)
                {
                    var kstate = Keyboard.GetState();
                    if (kstate.IsKeyDown(Keys.Enter) && canBeSelected)
                    {
                        _navigate = true;
                    }
                    else if (!kstate.IsKeyDown(Keys.Enter))
                    {
                        canBeSelected = true;
                    }
                }
            }
            else
            {
                animationFrame = 1;
            }

            sourceRectangle = new Rectangle(0 + (texture.Width / framesPerRow + 1) * (animationFrame - 1), 0, texture.Width / framesPerRow, texture.Height / frameRows);
            _timeInExistence += GameState.GameTime.ElapsedGameTime.TotalSeconds;
        }

        public abstract Mode CheckForNextScreen();


        public override int GetCollisionIntensity(Rectangle other)
        {
            return 0;
        }

    }
}

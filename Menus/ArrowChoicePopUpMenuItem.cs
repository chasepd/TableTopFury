using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Menus
{
    internal class ArrowChoicePopUpMenuItem : PopUpMenuItem
    {
        protected List<string> choices;
        protected int selectedChoice;
        protected int arrow1AnimationFrame;
        protected int arrow2AnimationFrame;
        protected int selectedArrow;
        private double _arrowChoiceTimeTracker;
        private const double _arrowChoiceTimeDelay = 0.2;
        public ArrowChoicePopUpMenuItem(string text, int menuPosition, List<string> choices) : base(text, menuPosition)
        {
            this.choices = choices;
            selectedChoice = 0;
            arrow1AnimationFrame = 1;
            arrow2AnimationFrame = 1;
            selectedArrow = 0;
            _arrowChoiceTimeTracker = 0.0;
        }

        public ArrowChoicePopUpMenuItem(string text, int menuPosition, List<string> choices, int selectedChoice) : base(text, menuPosition)
        {
            this.choices = choices;
            this.selectedChoice = selectedChoice;
            arrow1AnimationFrame = 1;
            arrow2AnimationFrame = 1;
            selectedArrow = 0;
            _arrowChoiceTimeTracker = 0.0;
        }

        public override void Update()
        {
            base.Update();
            _arrowChoiceTimeTracker += GameState.GameTime.ElapsedGameTime.TotalSeconds;
            if (selected) 
            {
                var kstate = Keyboard.GetState();
                if (kstate.IsKeyDown(Keys.Right) && selectedChoice < choices.Count - 1 && _arrowChoiceTimeTracker > _arrowChoiceTimeDelay)
                {
                    selectedArrow = 2;
                    selectedChoice++;
                    _arrowChoiceTimeTracker = 0;
                }
                else if(kstate.IsKeyDown(Keys.Left) && selectedChoice > 0 && _arrowChoiceTimeTracker > _arrowChoiceTimeDelay)
                {
                    selectedArrow = 1;
                    selectedChoice--;
                    _arrowChoiceTimeTracker = 0;
                }
                else
                {
                    selectedArrow = 0;
                }
            }
        }

        public override void Draw()
        {
            if (selected)
            {
                if (selectedArrow == 1)
                {
                    arrow1AnimationFrame++;
                }
                else
                {
                    arrow1AnimationFrame = 1;
                }

                if(selectedArrow == 2)
                {
                    arrow2AnimationFrame++;
                }
                else
                {
                    arrow2AnimationFrame = 1;
                }
            }            
            base.Draw();
            Rectangle arrow1SourceRectangle = new Rectangle(32 * (arrow1AnimationFrame - 1), 0, 32, 32);
            Rectangle arrow2SourceRectangle = new Rectangle(32 * (arrow2AnimationFrame - 1), 0, 32, 32);
            GameState.CurrentSpriteBatch.Draw(
                GameState.MenuArrow,
                new Vector2(position.X + (400 * scaleModifier), position.Y),
                arrow1SourceRectangle,
                Color.White,
                0,
                Vector2.One,
                scaleModifier,
                SpriteEffects.FlipHorizontally,
                0.0f
                );

            GameState.CurrentSpriteBatch.DrawString(
                GameState.Content.Load<SpriteFont>("Arial"),
                choices[selectedChoice],
                new Vector2(position.X + (450 * scaleModifier), position.Y),
                Color.Black,
                0f,
                Vector2.Zero,
                scaleModifier,
                SpriteEffects.None,
                0f
                );

            GameState.CurrentSpriteBatch.Draw(
                GameState.MenuArrow,
                new Vector2(position.X + (600 * scaleModifier), position.Y),
                arrow2SourceRectangle,
                Color.White,
                0,
                Vector2.One,
                scaleModifier,
                SpriteEffects.None,
                0.0f
                );

        }

        public string GetSelectedChoice()
        {
            return choices[selectedChoice];
        }
    }
}

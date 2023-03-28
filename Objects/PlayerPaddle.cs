using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal class PlayerPaddle : Paddle
    {
        
        public PlayerPaddle(int playerNumber) : base(playerNumber) { }
        

        public override void LoadContent()
        {
            base.LoadContent();
            texture = GameState.Content.Load<Texture2D>("BasicPaddle");
        }        

        public override void Update(List<TTFObject> objects)
        {            
            var kstate = Keyboard.GetState();
            if (playerNumber == 1)
            {
                if (kstate.IsKeyDown(Keys.W))
                {
                    UpwardMovement(kstate.IsKeyDown(Keys.LeftShift));                      
                }
                else if (kstate.IsKeyDown(Keys.S))
                {
                    DownwardMovement(kstate.IsKeyDown(Keys.LeftShift));
                }
                else
                {
                    SlowToStop();
                }
            }
            else
            {
                if (kstate.IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                {
                    UpwardMovement(kstate.IsKeyDown(Keys.RightShift) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed);
                }
                else if (kstate.IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                {
                    DownwardMovement(kstate.IsKeyDown(Keys.RightShift) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed);
                }
                else
                {
                    SlowToStop();
                }
            }

            base.Update(objects);
        }
        
    }
}

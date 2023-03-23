using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal class AIPaddle : Paddle
    {
        protected int difficultyLevel;
        protected bool movingUpward;
        protected bool movingDownward;
        protected bool boosting;
        protected const double movementUpdateDelay = 0;
        protected double movementUpdateTimeTracker;
        public AIPaddle(int playerNumber, int difficultyLevel) : base(playerNumber)
        {
            this.difficultyLevel = difficultyLevel;
            movingDownward = false;
            movingUpward = false;
            boosting = false;
            movementUpdateTimeTracker = 0.0;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("BasicPaddle");
        }

        private Ball GetClosestBall()
        {
            var balls = GameState.Balls;
            Ball closestBall = null;
            foreach (Ball ball in balls)
            {
                if (closestBall == null)
                {
                    closestBall = ball;
                }
                else
                {
                    if (playerNumber == 1)
                    {
                        if (ball.position.X < closestBall.position.X)
                        {
                            closestBall = ball;
                        }
                    }
                    else
                    {
                        if (ball.position.X > closestBall.position.X)
                        {
                            closestBall = ball;
                        }
                    }
                }
            }
            return closestBall;
        }

        private int DetermineBallFinalHeight()
        {

            Ball closestBall = GetClosestBall();

            int currentX = (int)closestBall.position.X;
            int currentY = (int)closestBall.position.Y;
            int ySpeed = closestBall.speedY;
            int xSpeed = closestBall.speedX;


            while (currentX > GameState.Paddles[0].position.X && currentX < GameState.Paddles[1].position.X)
            {
                if(GameState.Board.IsCollisionPoint(
                    new Rectangle(
                        currentX, 
                        currentY, 
                        (int)(closestBall.texture.Width / closestBall.framesPerRow * closestBall.scaleModifier),
                        (int)(closestBall.texture.Height / closestBall.frameRows * closestBall.scaleModifier)
                        )) != 0)
                {
                    ySpeed *= -1;
                }
                currentX += closestBall.speedX;
                currentY += closestBall.speedY;
            }           
            
            return currentY;
        }

        protected bool AtCollisionWithClosestBall()
        {
            //adjust size to make sure we're not cutting it too close by hitting the ball right at the edge of the paddle
            float size_modifier = 30 * scaleModifier;
            int slowdownForcedMovement = 0;
            int currentSpeed = Math.Abs(speedY);
            while (currentSpeed > 0)
            {
                if (currentSpeed < speedStep)
                {
                    slowdownForcedMovement += currentSpeed;
                    currentSpeed = 0;
                }
                else
                {
                    slowdownForcedMovement += speedStep;
                    currentSpeed -= speedStep;
                }
            }
            Rectangle thisSprite = new Rectangle((int)position.X - paddleWidthModifier, (int)position.Y - paddleHeightModifier + (int)size_modifier, paddleWidthModifier * 2, paddleHeightModifier * 2 - (2 * (int) size_modifier));
            Rectangle slowDownLocation = new Rectangle(0, 0, 1, 1);

            if (speedY > 0)
            {
                slowDownLocation = new Rectangle((int)position.X - paddleWidthModifier, (int)position.Y - paddleHeightModifier + slowdownForcedMovement + (int)size_modifier, paddleWidthModifier * 2, paddleHeightModifier * 2 -  (2 * (int)size_modifier));
            }
            else if (speedY < 0)
            {
                slowDownLocation = new Rectangle((int)position.X - paddleWidthModifier, (int)position.Y - paddleHeightModifier - slowdownForcedMovement + (int)size_modifier, paddleWidthModifier * 2, paddleHeightModifier * 2 - (2 * (int)size_modifier));
            }
            Ball closestBall = GetClosestBall();

            Rectangle finalBallLocation = new Rectangle(
                            (int)position.X,
                            DetermineBallFinalHeight(),
                            (int)(closestBall.texture.Width / closestBall.framesPerRow * closestBall.scaleModifier),
                            (int)(closestBall.texture.Height / closestBall.frameRows * closestBall.scaleModifier)
                            );

            return thisSprite.Intersects(finalBallLocation) || slowDownLocation.Intersects(finalBallLocation);                
        }

        protected bool BoosterCheck()
        {
            return false;
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects)
        {
            

            if (((playerNumber == 1 && GetClosestBall().speedX < 0) || (playerNumber == 2 && GetClosestBall().speedX > 0))) // && movementUpdateTimeTracker > movementUpdateDelay)
            {
                boosting = BoosterCheck();
                if (position.Y < GetClosestBall().position.Y && !AtCollisionWithClosestBall()) // && position.Y + (paddleWidthModifier * 2) + slowdownForcedMovement < DetermineBallFinalHeight())
                {
                    movingDownward = true;
                    movingUpward = false;                    

                }
                else if (position.Y > GetClosestBall().position.Y && !AtCollisionWithClosestBall()) //&& position.Y - (paddleWidthModifier * 2) - slowdownForcedMovement > DetermineBallFinalHeight())
                {
                    movingUpward = true;
                    movingDownward = false;
                }
                else
                {
                    movingDownward = false;
                    movingUpward = false;                    
                }

                if (movingDownward)
                {
                    Debug.WriteLine("Moving Down. Ball Pos:" + GetClosestBall().position + " My pos: " + position + " Ending pos for ball: " + DetermineBallFinalHeight() + " Collision check:" + AtCollisionWithClosestBall());
                    DownwardMovement(boosting);
                }
                else if (movingUpward)
                {
                    Debug.WriteLine("Moving Up. Ball Pos:" + GetClosestBall().position + " My pos: " + position + " Ending pos for ball: " + DetermineBallFinalHeight() + " Collision check:" + AtCollisionWithClosestBall());
                    UpwardMovement(boosting);
                }
                else
                {
                    Debug.WriteLine("Slowing to stop");
                    SlowToStop();
                }
            }
            else
            {
                SlowToStop();
            }

            base.Update(gameTime, graphics, objects);
        }

    }
}

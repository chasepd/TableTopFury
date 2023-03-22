using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        protected const double movementUpdateDelay = 0.6;
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
            Ball closestBall = GetClosestBall();
            return IsCollisionPoint(new Rectangle(
                            (int)closestBall.position.X,
                            (int)closestBall.position.Y,
                            (int)(closestBall.texture.Width / closestBall.framesPerRow * closestBall.scaleModifier),
                            (int)(closestBall.texture.Height / closestBall.frameRows * closestBall.scaleModifier)
                            )) != 0;
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects)
        {
            base.Update(gameTime, graphics, objects);
            int currentSpeed = Math.Abs(speedY);
            int slowdownForcedMovement = 0;
            movementUpdateTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
            while (currentSpeed > 0)
            {
               if(currentSpeed < speedStep)
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

            if (((playerNumber == 1 && GetClosestBall().speedX < 0) || (playerNumber == 2 && GetClosestBall().speedX > 0)) && movementUpdateTimeTracker > movementUpdateDelay)
            {
                movementUpdateTimeTracker = 0.0;
                if (position.Y + slowdownForcedMovement < DetermineBallFinalHeight() && !AtCollisionWithClosestBall()) // && position.Y + (paddleWidthModifier * 2) + slowdownForcedMovement < DetermineBallFinalHeight())
                {
                    //if (Math.Abs(position.Y + slowdownForcedMovement - DetermineBallFinalHeight()) < graphics.PreferredBackBufferHeight / 2 && Math.Abs(position.Y + (paddleWidthModifier * 2) + slowdownForcedMovement - DetermineBallFinalHeight()) < graphics.PreferredBackBufferHeight)
                    //{
                    //    DownwardMovement(true);
                    //}
                    //else
                    //{
                    //    DownwardMovement(false);
                    //}
                    movingDownward = true;
                    movingUpward = false;
                }
                else if (position.Y - slowdownForcedMovement > DetermineBallFinalHeight() && !AtCollisionWithClosestBall()) //&& position.Y - (paddleWidthModifier * 2) - slowdownForcedMovement > DetermineBallFinalHeight())
                {
                    //if (Math.Abs(position.Y - DetermineBallFinalHeight() - slowdownForcedMovement) > graphics.PreferredBackBufferHeight / 2 && Math.Abs(position.Y - (paddleWidthModifier * 2) - slowdownForcedMovement + DetermineBallFinalHeight()) > graphics.PreferredBackBufferHeight / 2)
                    //{
                    //    UpwardMovement(true);
                    //}
                    //else
                    //{
                    //    UpwardMovement(false);
                    //}
                    movingUpward = true;
                    movingDownward = false;
                }
                else
                {
                    movingDownward = false;
                    movingUpward = false;
                }

                if (Math.Abs(position.Y - DetermineBallFinalHeight()) > graphics.PreferredBackBufferHeight / 2)
                {
                    boosting = true;
                }
                else
                {
                    boosting = false;
                }

                if (movingDownward)
                {
                    DownwardMovement(boosting);
                }
                else if (movingUpward)
                {
                    UpwardMovement(boosting);
                }
                else
                {
                    SlowToStop();
                }
            }
        }

    }
}

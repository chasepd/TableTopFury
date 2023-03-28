using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private int finalBallY;
        private int lastRecordedBallYSpeed;
        private int lastRecordedBallXSpeed;

        public AIPaddle(int playerNumber, int difficultyLevel) : base(playerNumber)
        {
            this.difficultyLevel = difficultyLevel;
            movingDownward = false;
            movingUpward = false;
            boosting = false;
            movementUpdateTimeTracker = 0.0;
            finalBallY = -101010;
            lastRecordedBallXSpeed = -101010;
            lastRecordedBallYSpeed = -101010;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            texture = GameState.Content.Load<Texture2D>("BasicPaddle");
        }

        private Ball GetClosestBall()
        {
            var balls = GameState.Balls;
            if (balls.Count == 1)
            {
                return balls[0];
            }
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
            //if (closestBall.speedY == lastRecordedBallYSpeed && closestBall.speedX == lastRecordedBallXSpeed)
            //{
            //    return finalBallY;
            //}
            //else
            //{
            //    lastRecordedBallYSpeed = closestBall.speedY;
            //    lastRecordedBallXSpeed = closestBall.speedX;
            //}

            Paddle otherPaddle = GameState.Paddles[0];

            if(playerNumber == 1)
            {
                otherPaddle = GameState.Paddles[1];
            }

            int currentX = (int)closestBall.position.X;
            int currentY = (int)closestBall.position.Y;
            int ySpeed = closestBall.speedY;
            int xSpeed = closestBall.speedX;

            Rectangle collisionArea = new Rectangle(
                        currentX - (int)(closestBall.texture.Width / 2 / closestBall.framesPerRow * closestBall.scaleModifier),
                        currentY - (int)(closestBall.texture.Height / 2 / closestBall.frameRows * closestBall.scaleModifier),
                        (int)(closestBall.texture.Width / closestBall.framesPerRow * closestBall.scaleModifier),
                        (int)(closestBall.texture.Height / closestBall.frameRows * closestBall.scaleModifier)
                        );


            Rectangle ourSide;

            if (playerNumber == 1) {
                ourSide = new Rectangle(0, 0,
                    (int)(GameState.Board.position.X - (GameState.Board.GetWidth() / 2)),
                    (int)(GameState.Board.GetHeight() + GameState.Board.position.Y * 2));
            }
            else
            {
                ourSide = new Rectangle((int)(position.X - (GetWidth()/2)), 0,
                    (int)GetWidth(),
                    (int)(GameState.Board.GetHeight() + GameState.Board.position.Y * 2));
            }

            while (!ourSide.Intersects(collisionArea))
            {
                if(GameState.Board.GetCollisionIntensity(collisionArea) != 0)
                {
                    ySpeed *= -1;
                }

                if(Math.Abs(currentX - otherPaddle.position.X) <= otherPaddle.GetWidth() / 2)
                {
                    return currentY;
                }
                currentX += closestBall.speedX;
                currentY += ySpeed;

                collisionArea = new Rectangle(
                        currentX - (int)(closestBall.texture.Width / 2 / closestBall.framesPerRow * closestBall.scaleModifier),
                        currentY - (int)(closestBall.texture.Height / 2 / closestBall.frameRows * closestBall.scaleModifier),
                        (int)(closestBall.texture.Width / closestBall.framesPerRow * closestBall.scaleModifier),
                        (int)(closestBall.texture.Height / closestBall.frameRows * closestBall.scaleModifier)
                        );
            }
            finalBallY = currentY;
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
            var finalLocation = DetermineBallFinalHeight();
            if (finalLocation < position.Y - (GameState.Board.GetHeight() / 3) ||
                finalLocation > position.Y + GetHeight() + (GameState.Board.GetHeight() / 3))
            {
                return true;
            }
            return false;
        }

        public override void Update(List<TTFObject> objects)
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
            else
            {
                SlowToStop();
            }

            base.Update(objects);
        }

    }
}

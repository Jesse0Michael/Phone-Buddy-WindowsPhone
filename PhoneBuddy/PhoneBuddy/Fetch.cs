using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PhoneBuddy
{
    class Fetch
    {
        private Dog dog;
        private mouseHelp mouse;
        private AppDJ appDJ;

        Texture2D ball;
        Texture2D shadow;

        public int ballPosX;
        public int ballPosY;
        public int bounceCount;

        public float Magnitude;
        public float ballScale;
        public float ballRot;
        public float ballPosZ;
        public float initialVel;
        public float ballStartWidth;
        public float bounceY;
        public float bounceSpeed;
        public float xFactor;
        public float yFactor;
        public float sFactor;
        public float shadowScale;
        public float oldPoint;
        public float oldOldPoint;
        public float nowPoint;

        public Rectangle ballRec;
        public Rectangle touchRec;

        public Vector2 vanishingPoint;
        public Vector2 farthestBack;
        public Vector2 released;
        public Vector2 ballOrigin;
        public Vector2 ballPos;
        public Vector2 linePos;
        public Vector2 dogLine;
        public Vector2 shadowLine;

        public TimeSpan reactionTime;
        public TimeSpan idleTime;
        public TimeSpan backTime;
        public TimeSpan releaseTime;

        public int screenWidth;
        public int screenHeight;
        

        public enum ballState
        {
            ballIdle,
            ballHolding,
            ballReleased,
            ballWaiting,
            ballReturning
        };

        public ballState state;

        
        public Fetch(Dog dog, mouseHelp mouse, AppDJ appDJ)
        {
            this.dog = dog;
            this.mouse = mouse;
            this.appDJ = appDJ;
            screenHeight = 480;
            screenWidth = 800;

            restart();
            
        }

        public void restart()
        {
            ballPosX = (int)((float)screenWidth * .86);
            ballPosY = (int)((float)screenHeight * .80);
            ballPosZ = 0.4f;
            ballScale = 1.0f;
            ballRot = 0.0f;
            initialVel = 0.0f;
            bounceCount = 0;
            Magnitude = 250.0f;
            bounceY = 0.0f;
            bounceSpeed = 0.0f;

            shadowScale = 1.0f;
            xFactor = 1.0f;
            yFactor = 2.0f;
            sFactor = 3.0f;

            oldPoint = 0.0f;
            oldOldPoint = 0.0f;
            nowPoint = 0.0f;

            state = ballState.ballIdle;

            vanishingPoint = new Vector2((int)((float)screenWidth * .55), (int)((float)screenHeight * .23));
            farthestBack = new Vector2(0, 0);
            released = new Vector2(0, 0);
            ballOrigin = new Vector2(ballPosX, ballPosY);
            ballPos = new Vector2(ballPosX, ballPosY);
            linePos = new Vector2(ballPosX, ballPosY);
            dogLine = new Vector2(dog.dogPos.X, dog.dogPos.Y);
            shadowLine = new Vector2(ballPosX, (int)((float)screenHeight * .88));
            reactionTime = new TimeSpan(0, 0, 0, 0, 250);
            idleTime = new TimeSpan(0, 0, 30);
            backTime = new TimeSpan(0, 0, 0, 0, 0);
            releaseTime = new TimeSpan(0, 0, 0, 0, 0);
            

            
        }

        public void LoadContent(ContentManager Content)
        {
            ball = Content.Load<Texture2D>("Textures/actFetch");
            shadow = Content.Load<Texture2D>("Textures/ballShadow");

            ballRec = new Rectangle(0, 0, ball.Width, ball.Height);
            touchRec = new Rectangle(ballPosX, ballPosY, ball.Width, ball.Height);
            ballStartWidth = ball.Width;
            
        }


        public void Update(GameTime gameTime)
        {
            touchRec = new Rectangle((int)ballPos.X - ball.Width/2, (int)ballPos.Y-ball.Height/2, ball.Width, ball.Height);
            

            switch (state)
            {
                case ballState.ballIdle:
                    idleing(gameTime);

                    break;

                case ballState.ballHolding:
                    holding(gameTime);

                    break;

                case ballState.ballReleased:
                    releasing(gameTime);

                    break;


                case ballState.ballReturning:
                    returning(gameTime);

                    break;


            }

        }

        public void idleing(GameTime gameTime)
        {

            if (mouse.mState.LeftButton == ButtonState.Pressed && touchRec.Contains((int)mouse.position.X, (int)mouse.position.Y))
            {
                state = ballState.ballHolding;

            }

        }

        public void holding(GameTime gameTime)
        {
            
            if (mouse.mState.LeftButton == ButtonState.Pressed)
            {
                ballPos.X = (int)mouse.position.X;
                ballPos.Y = (int)mouse.position.Y;


                if (mouse.position.Y >= farthestBack.Y)
                {
                    farthestBack.X = mouse.position.X;
                    farthestBack.Y = mouse.position.Y;
                    backTime = gameTime.TotalGameTime;
                }

            }
            else if (mouse.mState.LeftButton == ButtonState.Released)
            {
                released.X = mouse.position.X;
                released.Y = mouse.position.Y;
                releaseTime = gameTime.TotalGameTime;
                initialVel = (float)((Math.Sqrt(Math.Pow((released.X - farthestBack.X), 2) + Math.Pow((released.Y - farthestBack.Y), 2)))/(releaseTime.TotalMilliseconds - backTime.TotalMilliseconds));
                
                linePos = new Vector2(ballPos.X, ballPos.Y);
                
                if (initialVel <= 0.45f)
                {


                    restart();
                }
                else if(initialVel <= 1.0f)
                {
                    initialVel = 1.0f;
                    state = ballState.ballReleased;
                }
                else if (initialVel >= 1.5f)
                {
                    initialVel = 1.5f;
                
                    state = ballState.ballReleased;
                }

            }
        }

        public void releasing(GameTime gameTime)
        {
            if (reactionTime >= TimeSpan.Zero)
            {
                reactionTime -= gameTime.ElapsedGameTime;
            }
            else
            {
                waiting();

            }


            if (bounceCount < 4)
            {
                ballRot -= (float)Math.PI / (8 * initialVel);

                bounceSpeed += initialVel / 50.0f;
                bounceY = (float)Math.Abs(Math.Sin(bounceSpeed)) * ((Magnitude * initialVel));

                ballScale = 1 / ((bounceSpeed) + 1);

                oldOldPoint = oldPoint;
                oldPoint = nowPoint;
                nowPoint = ((float)Math.Abs(Math.Sin(bounceSpeed)));

                if (oldPoint < oldOldPoint && oldPoint < nowPoint)
                {
                    dog.vibrate();
                    appDJ.playThud(1.0f - (float)(bounceCount/10));
                    Magnitude /= 2.0f;
                    bounceCount++;
                }

                if (linePos.Y != vanishingPoint.Y)
                {
                    if (linePos.Y >= vanishingPoint.Y - (4 / (bounceSpeed + 1)) && linePos.Y <= vanishingPoint.Y + (4 / (bounceSpeed + 1)))
                    {
                        linePos.Y = vanishingPoint.Y;
                    }
                    else if (linePos.Y >= vanishingPoint.Y)
                    {
                        linePos.Y -= 2 / (bounceSpeed + 1); 
                    }
                    else if (linePos.Y <= vanishingPoint.Y)
                    {
                        linePos.Y += 10 / (bounceSpeed + 1); 
                    }

                }

                linePos.X -= 2.5f / (bounceSpeed + 1); 

            }

            ballPos.X = linePos.X;
            ballPos.Y = linePos.Y - bounceY;


            shadowScale = ballScale / ((float)Math.Abs(Math.Sin(bounceSpeed)) + 1);

            if (shadowLine.X >= linePos.X - (2 / bounceSpeed + 1) && shadowLine.X <= linePos.X + (2 / bounceSpeed + 1))
            {
                shadowLine.X = linePos.X;
            }
            else if (shadowLine.X >= linePos.X)
            {
                shadowLine.X -= (2 / bounceSpeed + 1);
            }
            else if (shadowLine.X <= linePos.X)
            {
                shadowLine.X += (2 / bounceSpeed + 1);
            }

            if (shadowLine.Y >= linePos.Y+ (50.0f * ballScale) - (2 / bounceSpeed + 1) && shadowLine.Y <= linePos.Y+ (50.0f * ballScale) + (2 / bounceSpeed + 1))
            {
                shadowLine.Y = linePos.Y + (50.0f * ballScale); 
            }
            else if (shadowLine.Y >= linePos.Y+ (50.0f * ballScale))
            {
                shadowLine.Y -= (2 / bounceSpeed + 1);
            }
            else if (shadowLine.Y <= linePos.Y+ (50.0f * ballScale))
            {
                shadowLine.Y += (2 / bounceSpeed + 1);
            }
            


            

        }

        public void waiting()
        {
            ballPosZ = 0.6f;


            if (dog.dogPos.Y != linePos.Y || dog.dogPos.X != ballPos.X)
            {
                dog.myAnimate = Dog.animate.dogRunAway;
                appDJ.runningOn = true;
                if (dog.dogPos.Y >= linePos.Y - (dog.returnSpeedY / yFactor) && dog.dogPos.Y <= linePos.Y + (dog.returnSpeedY / yFactor))
                {
                    dog.dogPos.Y = linePos.Y;
                }
                else if (dog.dogPos.Y >= linePos.Y)
                {
                    dog.dogPos.Y -= (dog.returnSpeedY / yFactor);

                }
                else if (dog.dogPos.Y <= linePos.Y)
                {
                    dog.dogPos.Y += (dog.returnSpeedY / yFactor);

                }

                if (dog.dogPos.X >= ballPos.X - (dog.returnSpeedX / xFactor) && dog.dogPos.X <= ballPos.X + (dog.returnSpeedX / xFactor))
                {
                    dog.dogPos.X = ballPos.X;

                }
                else if (dog.dogPos.X >= ballPos.X)
                {
                    dog.dogPos.X -= (dog.returnSpeedX / xFactor);
                }
                else if (dog.dogPos.X <= ballPos.X)
                {
                    dog.dogPos.X += (dog.returnSpeedX / xFactor);
                }

                if (dog.dogScale >= ballScale - (dog.returnSpeedS / sFactor) && dog.dogScale <= ballScale + (dog.returnSpeedS / sFactor) && ballScale >= 0.1f)
                {
                    dog.dogScale = ballScale;
                }
                else if (dog.dogScale >= ballScale && ballScale >= 0.1f)
                {
                    dog.dogScale -= (dog.returnSpeedS / sFactor);
                }
                else if (dog.dogScale <= ballScale && ballScale >= 0.1f)
                {
                    dog.dogScale += (dog.returnSpeedS / sFactor);
                }
                

            }
            else
            {
                shadowLine.Y = -100;
                state = ballState.ballReturning;
                appDJ.runningOn = false;
                
            }



        }

        public void returning(GameTime gameTime)
        {
            ballPosZ = 0.4f;
            if (dog.dogPos.Y != dog.origin.Y || dog.dogPos.X != dog.origin.X || dog.dogScale < 1.0f)
            {
                dog.myAnimate = Dog.animate.dogRunTowards;

                if (dog.dogPos.Y >= dog.origin.Y - (dog.returnSpeedY / yFactor) && dog.dogPos.Y <= dog.origin.Y + (dog.returnSpeedY / yFactor))
                {
                    dog.dogPos.Y = dog.origin.Y;
                    ballPos.Y = dog.origin.Y;
                }
                else if (dog.dogPos.Y <= dog.origin.Y)
                {
                    dog.dogPos.Y += (dog.returnSpeedY / yFactor);
                    ballPos.Y += (dog.returnSpeedY / yFactor);

                }
                else if (dog.dogPos.Y >= dog.origin.Y)
                {
                    dog.dogPos.Y -= (dog.returnSpeedY / yFactor);
                    ballPos.Y -= (dog.returnSpeedY / yFactor);

                }


                if (dog.dogPos.X >= dog.origin.X - (dog.returnSpeedX / xFactor) && dog.dogPos.X <= dog.origin.X + (dog.returnSpeedX / xFactor))
                {
                    dog.dogPos.X = dog.origin.X;
                    ballPos.X = dog.origin.X;

                }
                else if (dog.dogPos.X >= dog.origin.X)
                {
                    dog.dogPos.X -= (dog.returnSpeedX / xFactor);
                    ballPos.X -= (dog.returnSpeedX / xFactor);
                }
                else if (dog.dogPos.X <= dog.origin.X)
                {
                    dog.dogPos.X += (dog.returnSpeedX / xFactor);
                    ballPos.X += (dog.returnSpeedX / xFactor);
                }

                if (dog.dogScale >= 1.0f - (dog.returnSpeedS / sFactor) && dog.dogScale <= 1.0f + (dog.returnSpeedS / sFactor))
                {
                    dog.dogScale = 1.0f;

                }
                else if (dog.dogScale >= 1.0f)
                {
                    dog.dogScale -= (dog.returnSpeedS / sFactor);
                    ballScale -= (dog.returnSpeedS / sFactor);

                }
                else if (dog.dogScale <= 1.0f)
                {
                    dog.dogScale += (dog.returnSpeedS / sFactor);
                    ballScale += (dog.returnSpeedS / (sFactor * 2));
                }

            }
            else
            {
                dog.statEntertainment += 0.4f;
                dog.myAnimate = Dog.animate.dogSitting;
                restart();
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ball, ballPos, ballRec, Color.White, ballRot, new Vector2(ball.Width/2, ball.Height/2), ballScale, SpriteEffects.None, ballPosZ);
            spriteBatch.Draw(shadow, shadowLine, ballRec, Color.White, 0.0f, new Vector2(ball.Width / 2, (ball.Height / 2)), shadowScale, SpriteEffects.None, ballPosZ + .05f);
        }
    }
}
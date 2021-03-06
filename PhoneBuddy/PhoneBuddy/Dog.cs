﻿using System;
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
using Microsoft.Devices;

namespace PhoneBuddy 
{
    class Dog
    {
        public Fetch fetch;
        public Water water;
        public Food food;
        public Tug tug;
        

        private mouseHelp mouse;
        private AppDJ appDJ;

        public float statHunger;
        public float statHygiene;
        public float statThirst;
        public float statEntertainment;
        public float statHappiness;

        public Texture2D dogContainer;
        public Texture2D tugContainer;

        public Rectangle dogRec;

        public int dogX;
        public int dogY;
        public int returnSpeedX;
        public int returnSpeedY;

        public float dogScale;
        public float dogZ;
        public float dogRot;
        public float returnSpeedS;

        public Boolean returnHome;

        public Vector2 origin;
        public Vector2 dogPos;

        public int screenWidth;
        public int screenHeight;


        public enum activity
        {
            dogFetch,       //0
            dogTug,         //1
            dogFood,        //2
            dogWater,       //3
            dogPoo,         //4
            dogPet,         //5
            dogIdle         //6

        };
        public enum animate
        {
            dogSitting,       //0
            dogRunAway,       //1
            dogRunTowards,    //2
            dogRunRight,      //3
            dogRunLeft,       //4
            dogEatRight,      //5
            dogEatLeft,       //6
            dogTug,           //7
            dogTugRightUp,    //8
            dogTugLeftUp,     //9
            dogTugRightDown,  //10
            dogTugLeftDown    //11
        };

        public activity myActivity;
        public animate myAnimate;
        public int myFPS;
        public int FPS;
        public int aniX;
        public int aniY;
        public bool tugBool;

        public Dog(mouseHelp mouse, AppDJ appDJ)
        {
            this.appDJ = appDJ;
            this.mouse = mouse;
            screenHeight = 480;
            screenWidth = 800;

            fetch = new Fetch(this, this.mouse, this.appDJ);
            water = new Water(this, this.mouse, this.appDJ);
            food = new Food(this, this.mouse, this.appDJ);
            tug = new Tug(this, this.mouse, this.appDJ);

            statThirst = 1.0f;
            statHygiene = 1.0f;
            statHunger = 1.0f;
            statEntertainment = 1.0f;
            statHappiness = 1.0f;
            myActivity = activity.dogIdle;
            myAnimate = animate.dogSitting;
            aniX = 0;
            aniY = 0;
            myFPS = 0;
            FPS = 12;
            returnHome = false;
            tugBool = false;

            dogX = (int)((float)screenWidth * .51);
            dogY = (int)((float)screenHeight * .54);
            dogZ = 0.5f;
            dogScale = 1.0f;
            dogRec = new Rectangle(0, 0, 200, 200);
            dogRot = 0.0f;
            origin = new Vector2(dogX, dogY);
            dogPos = new Vector2(dogX, dogY);
            returnSpeedX = 2;
            returnSpeedY = 1;
            returnSpeedS = .005f;
            

        }

        public void LoadContent(ContentManager Content)
        {

            dogContainer = Content.Load<Texture2D>("Textures/dogSheet");
            tugContainer = Content.Load<Texture2D>("Textures/tugSheet");
            fetch.LoadContent(Content);
            water.LoadContent(Content);
            food.LoadContent(Content);
            tug.LoadContent(Content);
        }
            

        public void Update(GameTime gameTime)
        {
            // This part of the dog update method controls the frames per second for the animation
            myFPS++;
            dogRec = new Rectangle(aniX, aniY, 200, 200);
            if (myFPS >= FPS)
            {
                myFPS = 0;
                if (aniX < 600)
                {
                    aniX += 200;
                }
                else
                {
                    aniX = 0;
                }
            }
            
            
            // This part of the dog update method controls the dogs stats (hunger, entertainment, hygiene, thirst, and happyness.
            statHappiness = (statEntertainment + statHunger + statHygiene + statThirst) / 4.0f;

            if (statThirst > 0.0f)
            {
                statThirst -= .0001f;
            }
            if (statHunger > 0.0f)
            {
                statHunger -= .0001f;
            }
            if (statEntertainment > 0.0f)
            {
                statEntertainment -= .0001f;
            }
            if (statHygiene > 0.0f)
            {
                statHygiene -= .00005f;
            }


            // This part of the update mentod controls what activity is currently running
            if (returnHome == false)
            {
                switch (myActivity)
                {
                    case Dog.activity.dogFetch:
                        fetch.Update(gameTime);
                        break;

                    case Dog.activity.dogTug:
                        tug.Update(gameTime);
                        break;

                    case Dog.activity.dogFood:
                        food.Update(gameTime);
                        break;

                    case Dog.activity.dogWater:
                        water.Update(gameTime);
                        break;

                    case Dog.activity.dogIdle:

                        break;


                }
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (tugBool == true)
            {
                returnSpeedS = .1f;
            }
            else
            {
                returnSpeedS = .005f;
            }
            // This will have the dog return to the start position if called.
            if (returnHome == true)
            {
                appDJ.drinkOn = false;
                appDJ.foodOn = false;

                if (dogPos.X != origin.X || dogPos.Y != origin.Y || dogScale != 1.0)
                {
                    appDJ.runningOn = true;
                    if (dogPos.X <= origin.X + returnSpeedX && dogPos.X >= origin.X - returnSpeedX)
                    {
                        dogPos.X = origin.X;
                        myAnimate = Dog.animate.dogSitting;
                    }
                    else if (dogPos.X >= origin.X)
                    {
                        dogPos.X -= returnSpeedX;
                        myAnimate = Dog.animate.dogRunLeft;
                    }
                    else if (dogPos.X <= origin.X)
                    {
                        dogPos.X += returnSpeedX;
                        myAnimate = Dog.animate.dogRunRight;
                    }

                    if (dogPos.Y <= origin.Y + returnSpeedY && dogPos.Y >= origin.Y - returnSpeedY)
                    {
                        dogPos.Y = origin.Y;

                    }
                    else if (dogPos.Y >= origin.Y)
                    {
                        dogPos.Y -= returnSpeedY;
                        myAnimate = Dog.animate.dogRunAway;
                        
                    }
                    else if (dogPos.Y <= origin.Y)
                    {
                        dogPos.Y += returnSpeedY;
                        myAnimate = Dog.animate.dogRunTowards;
                        
                    }

                    

                    if (dogScale <= 1.0f + returnSpeedS && dogScale >= 1.0f - returnSpeedS)
                    {
                        dogScale = 1.0f;
                        
                    }
                    else if (dogScale >= 1.0f)
                    {
                        dogScale -= returnSpeedS;
                        
                    }
                    else if (dogScale <= 1.0f)
                    {
                        dogScale += returnSpeedS;
                        
                    }

                }
                else
                {
                    appDJ.runningOn = false;
                    returnHome = false;
                    tugBool = false;
                }
            }
            else
            {
                // This will draw the proper objects depending on what activity is running
                switch (myActivity)
                {
                    case Dog.activity.dogFetch:
                        fetch.Draw(spriteBatch);
                        break;

                    case Dog.activity.dogTug:
                        tug.Draw(spriteBatch);
                        break;

                    case Dog.activity.dogFood:
                        food.Draw(spriteBatch);
                        break;

                    case Dog.activity.dogWater:
                        water.Draw(spriteBatch);
                        break;

                    case Dog.activity.dogIdle:
                        myAnimate = Dog.animate.dogSitting;
                        break;

                }
            }

            // This draws the dog depending on what animation should be shown on the sprite sheet currently
            switch (myAnimate)
            {
                case Dog.animate.dogSitting:
                    aniY = 0;
                    spriteBatch.Draw(dogContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogRunAway:
                    aniY = 200;
                    spriteBatch.Draw(dogContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogRunTowards:
                    aniY = 400;
                    spriteBatch.Draw(dogContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogRunRight:
                    aniY = 600;
                    spriteBatch.Draw(dogContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogRunLeft:
                    aniY = 600;
                    spriteBatch.Draw(dogContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.FlipHorizontally, dogZ);
                    break;

                case Dog.animate.dogEatRight:
                    aniY = 800;
                    spriteBatch.Draw(dogContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogEatLeft:
                    aniY = 1000;
                    spriteBatch.Draw(dogContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogTug:
                    aniY = 0;
                    spriteBatch.Draw(tugContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogTugRightUp:
                    aniY = 200;
                    spriteBatch.Draw(tugContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogTugLeftUp:
                    aniY = 400;
                    spriteBatch.Draw(tugContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogTugRightDown:
                    aniY = 600;
                    spriteBatch.Draw(tugContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;

                case Dog.animate.dogTugLeftDown:
                    aniY = 800;
                    spriteBatch.Draw(tugContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.None, dogZ);
                    break;
            }
        }

        public void vibrate()
        {
            if (appDJ.volOn)
            {
                VibrateController myVibrate = VibrateController.Default;
                myVibrate.Start(new TimeSpan(0, 0, 0, 0, 100));
            }
        }
    }
}

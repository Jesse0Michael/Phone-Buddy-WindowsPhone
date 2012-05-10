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
    class Dog
    {
        public Fetch fetch;
        public Water water;
        public Food food;
        public Tug tug;
        

        private mouseHelp mouse;

        public float statHunger;
        public float statHygiene;
        public float statThirst;
        public float statEntertainment;
        public float statHappiness;

        public Texture2D dogContainer;

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
            dogEatLeft        //6

        };

        public activity myActivity;
        public animate myAnimate;
        public int myFPS;
        public int FPS;
        public int aniX;
        public int aniY;

        public Dog(mouseHelp mouse)
        {
            this.mouse = mouse;
            screenHeight = 480;
            screenWidth = 800;

            fetch = new Fetch(this, this.mouse);
            water = new Water(this, this.mouse);
            food = new Food(this, this.mouse);
            tug = new Tug(this, this.mouse);

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

            dogX = (int)((float)screenWidth * .51);
            dogY = (int)((float)screenHeight * .54);
            dogZ = 0.5f;
            dogScale = 1.0f;
            dogRec = new Rectangle(0, 0, 200, 200);
            dogRot = 0.0f;
            origin = new Vector2(dogX, dogY);
            dogPos = new Vector2(dogX, dogY);
            returnSpeedX = 1;
            returnSpeedY = 1;
            returnSpeedS = .005f;
            

        }

        public void LoadContent(ContentManager Content)
        {

            dogContainer = Content.Load<Texture2D>("Textures/dogSheet");
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
            // This will have the dog return to the start position if called.
            if (returnHome == true)
            {
                if (dogPos.X != origin.X || dogPos.Y != origin.Y || dogScale != 1.0)
                {
                    //Console.WriteLine(dogPos.X + "   " + origin.X);
                    if (dogPos.X <= origin.X + returnSpeedX && dogPos.X >= origin.X - returnSpeedX)
                    {
                        dogPos.X = origin.X;
                        //sitting ani
                        myAnimate = Dog.animate.dogSitting;
                    }
                    else if (dogPos.X >= origin.X)
                    {
                        dogPos.X -= returnSpeedX;
                        //running left ani
                        myAnimate = Dog.animate.dogRunLeft;
                    }
                    else if (dogPos.X <= origin.X)
                    {
                        dogPos.X += returnSpeedX;
                        //running right ani
                        myAnimate = Dog.animate.dogRunRight;
                    }

                    if (dogPos.Y <= origin.Y + returnSpeedY && dogPos.Y >= origin.Y - returnSpeedY)
                    {
                        dogPos.Y = origin.Y;
                        //sitting ani
                    }
                    else if (dogPos.Y >= origin.Y)
                    {
                        dogPos.Y -= returnSpeedY;
                        
                        //running left ani
                    }
                    else if (dogPos.Y <= origin.Y)
                    {
                        dogPos.Y += returnSpeedY;
                        myAnimate = Dog.animate.dogRunTowards;
                        //running right ani
                    }

                    if (dogScale <= 1.0f + returnSpeedS && dogScale >= 1.0f - returnSpeedS)
                    {
                        dogScale = 1.0f;
                        //sitting ani
                    }
                    else if (dogScale >= 1.0f)
                    {
                        dogScale -= returnSpeedS;
                        //running left ani
                    }
                    else if (dogScale <= 1.0f)
                    {
                        dogScale += returnSpeedS;
                        //running right ani
                    }

                }
                else
                {
                    returnHome = false;
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
                    aniY = 800;
                    spriteBatch.Draw(dogContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(100, 100), dogScale, SpriteEffects.FlipHorizontally, dogZ);
                    break;
            }
        }
    }
}

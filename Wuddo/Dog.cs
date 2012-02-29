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

namespace Buddy 
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
        public Texture2D dogSitting;

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

        public activity myActivity;

        public Dog(mouseHelp mouse)
        {
            this.mouse = mouse;

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
            returnHome = false;

            dogX = 525;
            dogY = 425;
            dogZ = 0.5f;
            dogScale = 1.0f;
            dogRec = new Rectangle(0, 0, 256, 256);
            dogRot = 0.0f;
            origin = new Vector2(dogX, dogY);
            dogPos = new Vector2(dogX, dogY);
            returnSpeedX = 1;
            returnSpeedY = 1;
            returnSpeedS = .005f;
            

        }

        public void LoadContent(ContentManager Content)
        {

            dogContainer = Content.Load<Texture2D>("Textures/dogSitting");
            dogSitting = Content.Load<Texture2D>("Textures/dogSitting");
            fetch.LoadContent(Content);
            water.LoadContent(Content);
            food.LoadContent(Content);
            tug.LoadContent(Content);
        }
            

        public void Update(GameTime gameTime)
        {
            

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

            if (returnHome == true)
            {
                if (dogPos.X != origin.X || dogPos.Y != origin.Y || dogScale != 1.0)
                {
                    //Console.WriteLine(dogPos.X + "   " + origin.X);
                    if (dogPos.X <= origin.X + returnSpeedX && dogPos.X >= origin.X - returnSpeedX)
                    {
                        dogPos.X = origin.X;
                        //sitting ani
                    }
                    else if (dogPos.X >= origin.X)
                    {
                        dogPos.X -= returnSpeedX;
                        //running left ani
                    }
                    else if (dogPos.X <= origin.X)
                    {
                        dogPos.X += returnSpeedX;
                        //running right ani
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

                        break;

                }
            }


            spriteBatch.Draw(dogContainer, dogPos, dogRec, Color.White, dogRot, new Vector2(dogContainer.Width / 2, dogContainer.Height / 2), dogScale, SpriteEffects.None, dogZ);


        }
    }
}

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
    class Food
    {
        private Dog dog;
        private mouseHelp mouse;

        Texture2D food;


        public float foodScale;
        public float foodRot;
        public float foodPosZ;
        public float xFactor;
        public float yFactor;

        public Rectangle foodRec;

        public Vector2 foodPos;
        public Vector2 eatingPos;

        public Boolean atFood;

        public TimeSpan timeEating;

        public int screenWidth;
        public int screenHeight;

        public Food(Dog dog, mouseHelp mouse)
        {
            this.dog = dog;
            this.mouse = mouse;
            screenHeight = 480;
            screenWidth = 800;

            restart();

        }

        public void restart()
        {
            foodPos = new Vector2((int)((float)screenWidth * .81), (int)((float)screenHeight * .77));
            eatingPos = new Vector2((int)((float)screenWidth * .77), (int)((float)screenHeight * .70));
            foodPosZ = 0.4f;
            foodScale = 1.0f;
            foodRot = 0.0f;

            xFactor = 1.0f;
            yFactor = 1.0f;

            atFood = false;
            timeEating = new TimeSpan(0, 0, 0, 5, 500);


        }

        public void LoadContent(ContentManager Content)
        {
            food = Content.Load<Texture2D>("Textures/actfood");

            foodRec = new Rectangle(0, 0, food.Width, food.Height);
        }


        public void Update(GameTime gameTime)
        {
            if (atFood == false)
            {
                //Console.WriteLine(dog.statHunger);
                if (dog.statHunger <= 0.9f)
                {

                    if (dog.dogPos.Y != eatingPos.Y || dog.dogPos.X != eatingPos.X)
                    {
                        dog.myAnimate = Dog.animate.dogRunRight;
                        if (dog.dogPos.Y >= eatingPos.Y - (dog.returnSpeedY / yFactor) && dog.dogPos.Y <= eatingPos.Y + (dog.returnSpeedY / yFactor))
                        {
                            dog.dogPos.Y = eatingPos.Y;
                        }
                        else if (dog.dogPos.Y >= eatingPos.Y)
                        {
                            dog.dogPos.Y -= (dog.returnSpeedY / yFactor);

                        }
                        else if (dog.dogPos.Y <= eatingPos.Y)
                        {
                            dog.dogPos.Y += (dog.returnSpeedY / yFactor);

                        }

                        if (dog.dogPos.X >= eatingPos.X - (dog.returnSpeedX / xFactor) && dog.dogPos.X <= eatingPos.X + (dog.returnSpeedX / xFactor))
                        {
                            dog.dogPos.X = eatingPos.X;

                        }
                        else if (dog.dogPos.X >= eatingPos.X)
                        {
                            dog.dogPos.X -= (dog.returnSpeedX / xFactor);
                        }
                        else if (dog.dogPos.X <= eatingPos.X)
                        {
                            dog.dogPos.X += (dog.returnSpeedX / xFactor);
                            
                        }




                    }
                    else
                    {
                        dog.myAnimate = Dog.animate.dogEatRight;
                        atFood = true;
                    }


                }
                else
                {
                    ate();
                }
            }
            else
            {
                if (timeEating >= TimeSpan.Zero)
                {
                    timeEating -= gameTime.ElapsedGameTime;
                }
                else
                {
                    dog.statHunger += 0.3f;
                    restart();
                    
                    

                }


            }

        }

        public void ate()
        {
            if (dog.dogPos.Y != dog.origin.Y || dog.dogPos.X != dog.origin.X || dog.dogScale < 1.0f)
            {
                dog.myAnimate = Dog.animate.dogRunLeft;

                if (dog.dogPos.Y >= dog.origin.Y - (dog.returnSpeedY / yFactor) && dog.dogPos.Y <= dog.origin.Y + (dog.returnSpeedY / yFactor))
                {
                    dog.dogPos.Y = dog.origin.Y;
                }
                else if (dog.dogPos.Y <= dog.origin.Y)
                {
                    dog.dogPos.Y += (dog.returnSpeedY / yFactor);

                }
                else if (dog.dogPos.Y >= dog.origin.Y)
                {
                    dog.dogPos.Y -= (dog.returnSpeedY / yFactor);

                }


                if (dog.dogPos.X >= dog.origin.X - (dog.returnSpeedX / xFactor) && dog.dogPos.X <= dog.origin.X + (dog.returnSpeedX / xFactor))
                {
                    dog.dogPos.X = dog.origin.X;
                    

                }
                else if (dog.dogPos.X >= dog.origin.X)
                {
                    dog.dogPos.X -= (dog.returnSpeedX / xFactor);
                    
                }
                else if (dog.dogPos.X <= dog.origin.X)
                {
                    dog.dogPos.X += (dog.returnSpeedX / xFactor);
                }



            }
            else
            {
                dog.myAnimate = Dog.animate.dogSitting;
                restart();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(food, foodPos, foodRec, Color.White, foodRot, new Vector2(0, 0), foodScale, SpriteEffects.None, foodPosZ);

        }
    }
}

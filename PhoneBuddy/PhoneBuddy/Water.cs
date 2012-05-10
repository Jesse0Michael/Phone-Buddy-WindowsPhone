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
    class Water
    {
        private Dog dog;
        private mouseHelp mouse;

        Texture2D water;


        public float waterScale;
        public float waterRot;
        public float waterPosZ;
        public float xFactor;
        public float yFactor;

        public Rectangle waterRec;

        public Boolean atWater;

        public Vector2 drinkingPos;
        public Vector2 waterPos;

        public TimeSpan timeDrinking;

        public int screenWidth;
        public int screenHeight;


        public Water(Dog dog, mouseHelp mouse)
        {
            this.dog = dog;
            this.mouse = mouse;
            screenHeight = 480;
            screenWidth = 800;


            restart();

        }

        public void restart()
        {

            waterPos = new Vector2((int)((float)screenWidth * .14), (int)((float)screenHeight * .77));
            drinkingPos = new Vector2((int)((float)screenWidth * .29), (int)((float)screenHeight * .70));
            
            waterPosZ = 0.4f;
            waterScale = 1.0f;
            waterRot = 0.0f;

            xFactor = 1.0f;
            yFactor = 1.0f;

            atWater = false;
            timeDrinking = new TimeSpan(0, 0, 0, 3, 500);
        }

        public void LoadContent(ContentManager Content)
        {
            water = Content.Load<Texture2D>("Textures/actWater");

            waterRec = new Rectangle(0, 0, water.Width, water.Height);
        }


        public void Update(GameTime gameTime)
        {
            if (atWater == false)
            {
                if (dog.statThirst <= 0.9f)
                {

                    if (dog.dogPos.Y != drinkingPos.Y || dog.dogPos.X != drinkingPos.X)
                    {
                        dog.myAnimate = Dog.animate.dogRunLeft;

                        if (dog.dogPos.Y >= drinkingPos.Y - (dog.returnSpeedY / yFactor) && dog.dogPos.Y <= drinkingPos.Y + (dog.returnSpeedY / yFactor))
                        {
                            dog.dogPos.Y = drinkingPos.Y;
                            
                        }
                        else if (dog.dogPos.Y >= drinkingPos.Y)
                        {
                            dog.dogPos.Y -= (dog.returnSpeedY / yFactor);

                        }
                        else if (dog.dogPos.Y <= drinkingPos.Y)
                        {
                            dog.dogPos.Y += (dog.returnSpeedY / yFactor);

                        }

                        if (dog.dogPos.X >= drinkingPos.X - (dog.returnSpeedX / xFactor) && dog.dogPos.X <= drinkingPos.X + (dog.returnSpeedX / xFactor))
                        {
                            dog.dogPos.X = drinkingPos.X;
                            

                        }
                        else if (dog.dogPos.X >= drinkingPos.X)
                        {
                            dog.dogPos.X -= (dog.returnSpeedX / xFactor);
                            
                        }
                        else if (dog.dogPos.X <= drinkingPos.X)
                        {
                            dog.dogPos.X += (dog.returnSpeedX / xFactor);
                            
                        }




                    }
                    else
                    {
                        dog.myAnimate = Dog.animate.dogEatLeft;
                        atWater = true;
                    }

                }
                else
                {

                    drank();

                }

            }
            else
            {
                if (timeDrinking >= TimeSpan.Zero)
                {
                    timeDrinking -= gameTime.ElapsedGameTime;
                }
                else
                {
                    dog.statThirst += 0.3f;
                    restart();

                }

            }

        }

        public void drank()
        {
            if (dog.dogPos.Y != dog.origin.Y || dog.dogPos.X != dog.origin.X || dog.dogScale < 1.0f)
            {
                dog.myAnimate = Dog.animate.dogRunRight;
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
            spriteBatch.Draw(water, waterPos, waterRec, Color.White, waterRot, new Vector2(0, 0), waterScale, SpriteEffects.None, waterPosZ);

        }
    }
}

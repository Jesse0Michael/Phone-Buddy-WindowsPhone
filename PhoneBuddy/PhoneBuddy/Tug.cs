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
    class Tug
    {
        private Dog dog;
        private mouseHelp mouse;
        private AppDJ appDJ;

        private Texture2D ropetex;
        private Rectangle ropeRec;
        private Vector2 ropePos;
        private float ropeZ;

        private Texture2D ropetex2;
        private Rectangle ropeRec2;
        private Vector2 ropePos2;
        private float rope2Z;

        private Rectangle startRec;

        private Vector2 playPlace;
        private float playScale;
        private float speedScale;
        private float speedY;

        private Vector2 twitchPos;
        private TimeSpan twitchTime;
        

        private Rectangle targetRec;
        private Texture2D testr;

        private Vector2 oldMousePoint;

        private bool playPos;
        private bool inPlay;

        private float playVal;
        private float gameSpeed;


        public int screenWidth;
        public int screenHeight;

        private Color tarCol;
        private SpriteFont font;

        private Texture2D tugBar;
        private Rectangle tugRec;
        private Vector2 barPos;

        private Texture2D barFace;
        private Rectangle faceRec;

        public Vector2 oldMouse;

        private static Random rand;

        public Tug(Dog dog, mouseHelp mouse, AppDJ appDJ)
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
            dog.tugBool = true;
            dog.myAnimate = Dog.animate.dogSitting;
            rand = new Random();;

            playPos = false;
            inPlay = false;

            oldMouse = mouse.position;

            targetRec = new Rectangle((int)((float)screenWidth * .47), (int)((float)screenHeight * .57), 
                                    (int)((float)screenWidth * .06), (int)((float)screenHeight * .06));

            playScale = 3.0f;
            playPlace = new Vector2((int)((float)screenWidth * .51), (int)((float)screenHeight * .62));

            ropePos = new Vector2((int)((float)screenWidth * .5), (int)((float)screenHeight * .6));
            ropePos2 = new Vector2((int)((float)screenWidth * .5), (int)((float)screenHeight * .6));
            rope2Z = .4f;
            ropeZ = .41f;

            playVal = 0.5f;
            speedY = 1.0f;
            speedScale = .01f;

            gameSpeed = .001f;

            barPos = new Vector2((int)((float)screenWidth * .9), (int)((float)screenHeight * .15));
            tugRec = new Rectangle(0, 0, 10, 350);

            
            twitchTime = new TimeSpan(0, 0, 0, 0, 500);

        }

        public void LoadContent(ContentManager Content)
        {
            testr = Content.Load<Texture2D>("Textures/testr");
            ropetex = Content.Load<Texture2D>("Textures/rope60");
            ropetex2 = Content.Load<Texture2D>("Textures/rope");

            ropeRec = new Rectangle(0, 0,
                                    (int)((float)screenWidth * .45), (int)((float)screenHeight * .65));
            ropeRec2 = new Rectangle(0, 0,
                                    (int)((float)screenWidth * .45), (int)((float)screenHeight * .65));

            startRec = new Rectangle((int)ropePos.X - ( ropeRec.Width / 2), (int)ropePos.Y - (ropeRec.Height / 2),
                                    (int)((float)screenWidth * .45), (int)((float)screenHeight * .65));

            font = Content.Load<SpriteFont>("SpriteFont");

            tugBar = Content.Load<Texture2D>("Textures/tugBar");
            barFace = Content.Load<Texture2D>("Textures/barPhone");

            faceRec = new Rectangle(0, 0, barFace.Width, barFace.Height);
        }


        public void Update(GameTime gameTime)
        {
            

            if (playPos == false)
            {
                

                if (dog.dogPos.Y != playPlace.Y || dog.dogScale != playScale)
                {
                    dog.myAnimate = Dog.animate.dogRunTowards;
                    appDJ.runningOn = true;

                    if (dog.dogPos.Y <= playPlace.Y + speedY && dog.dogPos.Y >= playPlace.Y - speedY)
                    {
                        dog.dogPos.Y = playPlace.Y;
                    }
                    else if (dog.dogPos.Y >= playPlace.Y)
                    {
                        dog.dogPos.Y -= speedY;
                    }
                    else if (dog.dogPos.Y <= playPlace.Y)
                    {
                        dog.dogPos.Y += speedY;
                    }

                    if (dog.dogScale <= playScale + speedScale && dog.dogScale >= playScale - speedScale)
                    {
                        dog.dogScale = playScale;
                    }
                    else if (dog.dogScale >= playScale)
                    {
                        dog.dogScale -= speedScale;
                    }
                    else if (dog.dogScale <= playScale)
                    {
                        dog.dogScale += speedScale;
                    }

                }
                else
                {
                    appDJ.runningOn = false;
                    playPos = true;
                }



            }
            else
            {
               

                if (inPlay == true)
                {

                    if (mouse.position.X <= ((float)screenWidth * .33))
                    {
                        if (mouse.position.Y <= ((float)screenHeight * .45))
                        {

                            dog.myAnimate = Dog.animate.dogTugLeftUp;
                            if (oldMouse.Y > ((float)screenHeight * .45))
                            {
                                dog.vibrate();
                                appDJ.playGrowl();
                            }
                        }
                        else
                        {
                            dog.myAnimate = Dog.animate.dogTugLeftDown;

                            if (oldMouse.Y <= ((float)screenHeight * .45))
                            {
                                dog.vibrate();
                                appDJ.playGrowl();
                            }
                        }

                        if (oldMouse.X > ((float)screenWidth * .33))
                        {
                            dog.vibrate();
                            appDJ.playGrowl();
                        }

                    }
                    else if (mouse.position.X >= ((float)screenWidth * .66))
                    {
                        if (mouse.position.Y <= ((float)screenHeight * .45))
                        {

                            dog.myAnimate = Dog.animate.dogTugRightUp;

                            if (oldMouse.Y > ((float)screenHeight * .45))
                            {
                                dog.vibrate();
                                appDJ.playGrowl();
                            }
                        }
                        else
                        {
                            dog.myAnimate = Dog.animate.dogTugRightDown;

                            if (oldMouse.Y <= ((float)screenHeight * .45))
                            {
                                dog.vibrate();
                                appDJ.playGrowl();
                            }
                        }

                        if (oldMouse.X < ((float)screenWidth * .66))
                        {
                            dog.vibrate();
                            appDJ.playGrowl();
                        }

                    }
                    else
                    {
                        dog.myAnimate = Dog.animate.dogTug;
                        if (oldMouse.X > ((float)screenWidth * .66))
                        {
                            dog.vibrate();
                            appDJ.playGrowl();
                        }

                        if (oldMouse.X < ((float)screenWidth * .33))
                        {
                            dog.vibrate();
                            appDJ.playGrowl();
                        }

                    }

                    if (mouse.mState.LeftButton == ButtonState.Released)
                    {
                        restart();
                    }


                    ropePos2 = mouse.position;

                }
                else
                {
                    dog.myAnimate = Dog.animate.dogSitting;
                    if (mouse.mState.LeftButton == ButtonState.Pressed && startRec.Contains((int)mouse.position.X, (int)mouse.position.Y))
                    {

                        inPlay = true;
                    }


                }



            }


           oldMouse = mouse.position;
        }

        

        public void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(testr, targetRec, tarCol);
            //SpriteFont sf =new SpriteFont();

            //spriteBatch.DrawString(font, playVal.ToString(), new Vector2(700, 20), Color.White);

            //spriteBatch.Draw(tugBar, barPos, tugRec, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, .5f);

            //facePos = new Vector2((int)((float)screenWidth * .91), (int)((float)screenHeight * .13) + (350 * (1.0f - playVal)));
            //spriteBatch.Draw(barFace, facePos, faceRec, Color.White, 0.0f, new Vector2(barFace.Width / 2, barFace.Height / 2), 1.0f, SpriteEffects.None, .45f);


            if (playPos == true && inPlay == false)
            {
                spriteBatch.Draw(ropetex, ropePos, ropeRec, Color.White, 0.0f, new Vector2(ropeRec.Width / 2, ropeRec.Height / 2), 1.0f, SpriteEffects.None, ropeZ);

            }
            
            if(inPlay == true)
            {
                spriteBatch.Draw(ropetex2, ropePos2, ropeRec2, Color.White, 0.0f, new Vector2(ropeRec2.Width / 2, ropeRec2.Height / 2), 1.0f, SpriteEffects.None, rope2Z);

            }
        }
    }
}
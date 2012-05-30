using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


namespace PhoneBuddy
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        private mouseHelp mouse;
        private actionSlider actionSlider;
        private Dog dog;
        Texture2D field;
        Texture2D pooBag;
        Texture2D splash;
        bool splashBool;
        TimeSpan splashTime;

        private int pooCounter;
        private List<Poo> pooList;
        private Boolean holdingPoo;

        private List<Cloud> cloudList;
        private Boolean direction;
        private Random rand;
        private int initialCloud;
        private TimeSpan newCloud;

        public int screenHeight;
        public int screenWidth;

       

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            screenHeight = 480;
            screenWidth = 800;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            mouse = new mouseHelp();

            actionSlider = new actionSlider(mouse, screenWidth, screenHeight);

            dog = new Dog(mouse);
            
            
            
            // Frame rate is 30 fps by default for Windows Phone.
            //TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            //InactiveSleepTime = TimeSpan.FromSeconds(1);
        }


        protected override void Initialize()
        {

            Content.RootDirectory = "Content";

            pooList = new List<Poo>();
            pooCounter = 0;
            holdingPoo = false;

            splashBool = true;
            splashTime = new TimeSpan(0, 0, 5);

            cloudList = new List<Cloud>();
            rand = new Random();
            newCloud = new TimeSpan(0, 0, rand.Next(5, 11));

            if (rand.Next(0, 2) == 0)
            {
                direction = false;
            }
            else
            {
                direction = true;
            }


            initialCloud = rand.Next(5, 7);
            for (int i = 0; i < initialCloud; i++)
            {
                cloudList.Add(new Cloud(rand.Next(1, 6), Content, mouse, dog, new Vector2(rand.Next(0, screenWidth), rand.Next(0 - (int)((float)screenHeight * .15), (int)((float)screenHeight * .08))), direction));

            }

            

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            field = Content.Load<Texture2D>("Textures/field");
            pooBag = Content.Load<Texture2D>("Textures/pooBag");
            splash = Content.Load<Texture2D>("Textures/phone buddy");

            actionSlider.LoadContent(Content);

            dog.LoadContent(Content);

        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || (GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
)
            {
                this.Exit();
            }
            if (splashBool == true)
            {
                splashTime -= gameTime.ElapsedGameTime;
                if (splashTime.Seconds <= 0)
                {
                    splashBool = false;
                }
            }
            else
            {


                mouse.Update(gameTime);
                actionSlider.Update(gameTime);
                dog.Update(gameTime);
                pooControl(gameTime);

                if (newCloud >= TimeSpan.Zero)
                {
                    newCloud -= gameTime.ElapsedGameTime;
                }
                else
                {
                    if (direction == true)
                    {
                        cloudList.Add(new Cloud(rand.Next(1, 6), Content, mouse, dog, new Vector2(-150, rand.Next(0 - (int)((float)screenHeight * .15), (int)((float)screenHeight * .08))), direction));

                    }
                    else
                    {
                        cloudList.Add(new Cloud(rand.Next(1, 6), Content, mouse, dog, new Vector2(1050, rand.Next(0 - (int)((float)screenHeight * .15), (int)((float)screenHeight * .08))), direction));

                    }
                    newCloud = new TimeSpan(0, 0, rand.Next(25, 60));
                }

                for (int i = 0; i < cloudList.Count; i++)
                {
                    cloudList[i].Update(gameTime);

                    if (cloudList[i].cloudPos.X <= -150.0f || cloudList[i].cloudPos.X >= screenWidth)
                    {
                        cloudList.Remove(cloudList[i]);

                    }
                }

                if (dog.myActivity == Dog.activity.dogPoo)
                {
                    for (int i = 0; i < pooList.Count; i++)
                    {
                        if (pooList[i].grabbedPoo == true)
                        {
                            holdingPoo = true;
                            pooList[i].Update(gameTime);
                        }
                    }

                    if (holdingPoo == false)
                    {
                        foreach (Poo poo in pooList)
                        {
                            poo.Update(gameTime);
                        }
                    }
                    else if (holdingPoo == true)
                    {

                        if (mouse.mouseUpped == true)
                        {
                            holdingPoo = false;
                        }
                    }

                }

                if (mouse.mouseDown == true)
                {
                    if (actionSlider.recFetch.Contains((int)mouse.position.X, (int)mouse.position.Y))
                    {
                        actionSlider.slideLeft = true;
                        dog.returnHome = true;
                        dog.myActivity = Dog.activity.dogFetch;
                        dog.fetch.restart();

                    }
                    if (actionSlider.recTug.Contains((int)mouse.position.X, (int)mouse.position.Y))
                    {
                        actionSlider.slideLeft = true;
                        dog.returnHome = true;
                        dog.myActivity = Dog.activity.dogTug;
                        dog.tug.restart();

                    }
                    if (actionSlider.recFood.Contains((int)mouse.position.X, (int)mouse.position.Y))
                    {
                        actionSlider.slideLeft = true;
                        dog.returnHome = true;
                        dog.myActivity = Dog.activity.dogFood;
                        dog.food.restart();
                    }
                    if (actionSlider.recWater.Contains((int)mouse.position.X, (int)mouse.position.Y))
                    {
                        actionSlider.slideLeft = true;
                        dog.returnHome = true;
                        dog.myActivity = Dog.activity.dogWater;
                        dog.water.restart();
                    }
                    if (actionSlider.recPoo.Contains((int)mouse.position.X, (int)mouse.position.Y))
                    {
                        actionSlider.slideLeft = true;
                        dog.returnHome = true;
                        dog.myActivity = Dog.activity.dogPoo;
                    }

                }
            }

            base.Update(gameTime);
        }


        public void pooControl(GameTime gameTime)
        {
            if (dog.statHygiene >= 0.8f)
            {
                pooCounter = 0;
            }
            else if (dog.statHygiene >= 0.6f && dog.statHygiene < 0.8f)
            {
                pooCounter = 1;
            }
            else if (dog.statHygiene >= 0.4f && dog.statHygiene < 0.6f)
            {
                pooCounter = 2;
            }
            else if (dog.statHygiene >= 0.2f && dog.statHygiene < 0.4f)
            {
                pooCounter = 3;
            }
            else if (dog.statHygiene < 0.2f)
            {
                pooCounter = 4;
            }


            if (pooCounter > pooList.Count)
            {
                Random newRand = new Random();
                Poo poo = new Poo(newRand.Next(1, 3), Content, mouse, dog);
                pooList.Add(poo);

            }


            for (int i = 0; i < pooList.Count; i++)
            {
                if (pooList[i].pooPos.X < 0)
                {
                    pooList.Remove(pooList[i]);

                }
            }


        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            if (splashBool == true)
            {
                spriteBatch.Draw(splash, new Rectangle(0, 0, screenWidth, screenHeight), new Rectangle(0, 0, screenWidth, screenHeight), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1.0f);
            
            }
            else
            {

                actionSlider.Draw(spriteBatch);
                dog.Draw(spriteBatch);

                foreach (Poo poo in pooList)
                {
                    poo.Draw(spriteBatch);

                }
                foreach (Cloud cloud in cloudList)
                {
                    cloud.Draw(spriteBatch);

                }


                if (dog.myActivity == Dog.activity.dogPoo)
                {
                    spriteBatch.Draw(pooBag, new Vector2((int)((float)screenWidth * .80), (int)((float)screenHeight * .75)), new Rectangle(0, 0, pooBag.Width, pooBag.Height), Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.9f);

                }

                spriteBatch.Draw(field, new Rectangle(0, 0, screenWidth, screenHeight), new Rectangle(0, 0, field.Width, field.Height), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1.0f);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


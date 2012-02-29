using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Buddy
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

        private int pooCounter;
        private List<Poo> pooList;
        private Boolean holdingPoo;

        private List<Cloud> cloudList;
        private Boolean direction;
        private Random rand;
        private int initialCloud;
        private TimeSpan newCloud;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }


        protected override void Initialize()
        {

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Buddy";

            mouse = new mouseHelp();
            IsMouseVisible = true;

            actionSlider = new actionSlider(mouse);

            dog = new Dog(mouse);

            pooList = new List<Poo>();
            pooCounter = 0;
            holdingPoo = false;

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
                cloudList.Add(new Cloud(rand.Next(1, 6), Content, mouse, dog, new Vector2(rand.Next(0, 1000), rand.Next(-30, 85)), direction));

            }

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            field = Content.Load<Texture2D>("Textures/field");
            pooBag = Content.Load<Texture2D>("Textures/pooBag");

            actionSlider.LoadContent(Content);

            dog.LoadContent(Content);

        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

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
                    cloudList.Add(new Cloud(rand.Next(1, 6), Content, mouse, dog, new Vector2(-150, rand.Next(-30, 85)), direction));
                
                }
                else
                {
                    cloudList.Add(new Cloud(rand.Next(1, 6), Content, mouse, dog, new Vector2(1050, rand.Next(-30, 85)), direction));
                
                }
                newCloud = new TimeSpan(0, 0, rand.Next(25, 60));
            }

            for (int i = 0; i < cloudList.Count; i++)
            {
                cloudList[i].Update(gameTime);

                if (cloudList[i].cloudPos.X <= -200.0f || cloudList[i].cloudPos.X >= 1100.0f)
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
                spriteBatch.Draw(pooBag, new Vector2(825, 425), new Rectangle(0, 0, pooBag.Width, pooBag.Height), Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.9f);

            }

            spriteBatch.Draw(field, new Rectangle(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height), new Rectangle(0, 0, field.Width, field.Height), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1.0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

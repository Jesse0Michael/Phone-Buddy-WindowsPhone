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
    class actionSlider
    {

        public Texture2D slider;
        public Texture2D sliderPull;
        public Texture2D actFetch;
        public Texture2D actTug;
        public Texture2D actFood;
        public Texture2D actWater;
        public Texture2D actPoo;
        
        public Rectangle recSlider;
        public Rectangle recSliderPull;
        public Rectangle recFetch;
        public Rectangle recTug;
        public Rectangle recFood;
        public Rectangle recWater;
        public Rectangle recPoo;
        
        private int sliderXloc;
        private int sliderGap;
        private int mouseGrabbed;
        private int mouseLet;

        public Boolean pullerClicked;
        public Boolean slideRight;
        public Boolean slideLeft;

        private mouseHelp mouse;



        public actionSlider(mouseHelp mouse)
        {
            this.sliderXloc = -25;
            this.mouse = mouse;
            this.pullerClicked = false;
            this.slideRight = false;
            this.slideLeft = false;

            

        }

        public void LoadContent(ContentManager Content)
        {

            slider = Content.Load<Texture2D>("Textures/slider");
            sliderPull = Content.Load<Texture2D>("Textures/sliderPull");
            actFetch = Content.Load<Texture2D>("Textures/actFetch");
            actTug = Content.Load<Texture2D>("Textures/actTug");
            actFood = Content.Load<Texture2D>("Textures/actFood");
            actWater = Content.Load<Texture2D>("Textures/actWater");
            actPoo = Content.Load<Texture2D>("Textures/actPoo");
            

        }

        public void Update(GameTime gameTime)
        {

            if (mouse.mState.LeftButton == ButtonState.Pressed && recSliderPull.Contains((int)mouse.position.X, (int)mouse.position.Y) && pullerClicked == false)
            {
                pullerClicked = true;
                sliderGap = (int)mouse.position.X - (slider.Width + sliderXloc);
                mouseGrabbed = (int)mouse.position.X;
                slideLeft = false;
                slideRight = false;

            }
            else if(mouse.mouseUpped == true && pullerClicked == true)
            {
                pullerClicked = false;
                mouseLet = (int)mouse.position.X;

                if (mouseGrabbed > mouseLet)
                {
                    if ((mouseGrabbed - mouseLet) > 50)
                    {
                        slideLeft = true;
                    }
                    else
                    {
                        slideRight = true;
                    }
                }
                else if (mouseGrabbed < mouseLet)
                {
                    if ((mouseLet - mouseGrabbed) < 50)
                    {
                        slideLeft = true;
                    }
                    else
                    {
                        slideRight = true;
                    }
                }

            }

            if (mouse.position.X <= slider.Width -25  + sliderGap && mouse.position.X >= sliderGap && pullerClicked == true)
            {
                sliderXloc = (int)mouse.position.X - slider.Width - sliderGap;
            }
            else if (slideRight == true && sliderXloc < -25)
            {
                sliderXloc+= 5;
                slideLeft = false;
            }
            else if (slideLeft == true && sliderXloc > - slider.Width)
            {
                sliderXloc-= 5;
                slideRight = false;
            }
            else if (sliderXloc >= -25 && sliderXloc <= -30)
            {
                slideRight = false;
                sliderXloc = -25;
            }
            else if (sliderXloc >= -slider.Width && sliderXloc <= -slider.Width + 5)
            {
                slideLeft = false;
                sliderXloc = -slider.Width;
            }

            

            this.recSlider = new Rectangle(sliderXloc, 0, slider.Width, slider.Height);
            this.recSliderPull = new Rectangle(sliderXloc + 225, 225, sliderPull.Width, sliderPull.Height);
            this.recFetch = new Rectangle(sliderXloc + 70, 20, actFetch.Width, actFetch.Height);
            this.recTug = new Rectangle(sliderXloc + 70, 155, actTug.Width, actTug.Height);
            this.recFood = new Rectangle(sliderXloc + 70, 270, actFood.Width, actFood.Height);
            this.recWater = new Rectangle(sliderXloc + 70, 375, actWater.Width, actWater.Height);
            this.recPoo = new Rectangle(sliderXloc + 90, 465, actPoo.Width, actPoo.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {



            
            spriteBatch.Draw(slider, recSlider, new Rectangle(0, 0, slider.Width, slider.Height), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, .2f);
            spriteBatch.Draw(sliderPull, recSliderPull, new Rectangle(0, 0, sliderPull.Width, sliderPull.Height), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, .19f);
            spriteBatch.Draw(actFetch, recFetch, new Rectangle(0, 0, actFetch.Width, actFetch.Height), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, .18f);
            spriteBatch.Draw(actTug, recTug, new Rectangle(0, 0, actTug.Width, actTug.Height), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, .17f);
            spriteBatch.Draw(actFood, recFood, new Rectangle(0, 0, actFood.Width, actFood.Height), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, .16f);
            spriteBatch.Draw(actWater, recWater, new Rectangle(0, 0, actWater.Width, actWater.Height), Color.White, 0.0f, new Vector2(0,0), SpriteEffects.None, .15f);
            spriteBatch.Draw(actPoo, recPoo, new Rectangle(0, 0, actPoo.Width, actPoo.Height), Color.White, 0.0f, new Vector2(0,0), SpriteEffects.None, .13f);
            



        }
    }
}
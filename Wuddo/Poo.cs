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
    class Poo
    {
        private mouseHelp mouse;
        private Dog dog;

        Texture2D pooImage;

        public Vector2 pooPos;
        public Vector2 pooStart;

        public int pooNumber;

        public float pooZ;
        public float pooScale;


        public Rectangle pooRec;
        public Rectangle touchRec;
        public Rectangle bagRec;

        public Random randX;
        public Random randY;

        public Boolean grabbedPoo;
        

        public Poo(int pooNumber, ContentManager Content, mouseHelp mouse, Dog dog)
        {
            this.pooNumber = pooNumber;
            this.mouse = mouse;
            this.dog = dog;

            

            grabbedPoo = false;
            
            randX = new Random();
            randY = new Random();

            pooPos = new Vector2(randX.Next(50, 800), randY.Next(250, 500));
            pooStart = pooPos;

            pooScale = pooPos.Y / 500.0f;
            pooZ = 0.6f - pooScale / 100.0f;

            pooImage = Content.Load<Texture2D>("Textures/actPoo" + pooNumber);

            pooRec = new Rectangle(0, 0, pooImage.Width, pooImage.Height);
            bagRec = new Rectangle(761, 361, 889, 489);
        }


        public void Update(GameTime gameTime)
        {
            touchRec = new Rectangle((int)pooPos.X - pooImage.Width / 2, (int)pooPos.Y - pooImage.Height / 2, pooImage.Width, pooImage.Height);
            

            if (mouse.mState.LeftButton == ButtonState.Pressed && touchRec.Contains((int)mouse.position.X, (int)mouse.position.Y))
            {
                grabbedPoo = true;

            }
            else if (mouse.mouseUpped == true)
            {

                grabbedPoo = false;

                if (bagRec.Contains((int)mouse.position.X, (int)mouse.position.Y) && bagRec.Contains((int)pooPos.X, (int)pooPos.Y))
                {
                    dog.statHygiene += 0.3f;
                    pooPos.X = -250;


                }
                else
                {
                    pooPos = pooStart;


                }

            }

            if (grabbedPoo == true)
            {
                pooPos.X = mouse.position.X;
                pooPos.Y = mouse.position.Y;
            }
            

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pooImage, pooPos, pooRec, Color.White, 0.0f, new Vector2(pooImage.Width / 2, pooImage.Height / 2), pooScale, SpriteEffects.None, pooZ);

        }
    }
}

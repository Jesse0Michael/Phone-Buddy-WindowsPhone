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
    class Cloud
    {
        private mouseHelp mouse;
        private Dog dog;

        Texture2D cloudImage;

        public int cloudNumber;

        public float cloudZ;
        public float cloudScale;
        public float move;

        public Vector2 cloudPos;

        public Rectangle cloudRec;

        public Random rand;


        public Cloud(int cloudNumber, ContentManager Content, mouseHelp mouse, Dog dog, Vector2 pos, Boolean direction)
        {

            this.cloudNumber = cloudNumber;
            this.mouse = mouse;
            this.dog = dog;
            this.cloudPos = pos;

            rand = new Random();

            cloudImage = Content.Load<Texture2D>("Textures/cloud" + cloudNumber);

            cloudRec = new Rectangle(0, 0, cloudImage.Width, cloudImage.Height);

            
            cloudScale = 0.8f +((float)rand.NextDouble() - 0.5f) / 10f;

            if (direction == true)
            {
                move = ((float)rand.NextDouble() / 10f);
            }
            else
            {
                move = (((float)rand.NextDouble() - 1.0f) / 10f);
            }

            cloudZ = 0.7f - Math.Abs(move) / 100f;
        }

        public void Update(GameTime gameTime)
        {
            cloudPos.X += move;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(cloudImage, cloudPos, cloudRec, Color.White, 0.0f, Vector2.Zero, cloudScale, SpriteEffects.None, cloudZ);

        }
    }
}

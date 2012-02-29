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
    class Tug
    {
        private Dog dog;
        private mouseHelp mouse;

        
        


        public Tug(Dog dog, mouseHelp mouse)
        {
            this.dog = dog;
            this.mouse = mouse;

            restart();

        }

        public void restart()
        {
            



        }

        public void LoadContent(ContentManager Content)
        {
            

        }


        public void Update(GameTime gameTime)
        {
            

        }

        

        public void Draw(SpriteBatch spriteBatch)
        {
            
        
        }
    }
}
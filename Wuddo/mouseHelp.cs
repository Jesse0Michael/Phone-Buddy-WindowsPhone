﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Buddy
{
    class mouseHelp 
    {
        public MouseState mState;
        public Boolean mouseDown;
        public Boolean mouseUpped;
        public MouseState oldMouse;
        public Vector2 position;

        public mouseHelp()
        {
            mState = Mouse.GetState();

            this.position = new Vector2(mState.X, mState.Y);
            



        }

        public void Update(GameTime gameTime)
        {


            this.mState = Mouse.GetState();
            this.mouseDown = ((mState.LeftButton == ButtonState.Pressed) && (oldMouse.LeftButton == ButtonState.Released));
            this.mouseUpped = ((mState.LeftButton == ButtonState.Released) && (oldMouse.LeftButton == ButtonState.Pressed));
            this.position.X = this.mState.X;
            this.position.Y = this.mState.Y;


            oldMouse = mState;



            
        }


    }
}

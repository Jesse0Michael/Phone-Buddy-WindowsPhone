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


namespace PhoneBuddy
{
    
    public class AppDJ 
    {
        public bool volOn;

        public bool runningOn;
        public bool foodOn;
        public bool drinkOn;

        public float runVol;

        private Random rand;
        private TimeSpan barkSpan;

        private SoundEffect bark1;
        private SoundEffect bark2;
        private SoundEffect bark3;
        private SoundEffect bark4;
        private SoundEffect bark5;
        private SoundEffect bird1;
        private SoundEffect bird2;
        private SoundEffect bird3;
        private SoundEffect bird4;
        private SoundEffect bird5;
        private SoundEffect drink;
        private SoundEffect food;
        private SoundEffect growl1;
        private SoundEffect growl2;
        private SoundEffect growl3;
        private SoundEffect growl4;
        private SoundEffect thud;
        private SoundEffect bag;

        private SoundEffectInstance ibark1;
        private SoundEffectInstance ibark2;
        private SoundEffectInstance ibark3;
        private SoundEffectInstance ibark4;
        private SoundEffectInstance ibark5;
        private SoundEffectInstance ibird1;
        private SoundEffectInstance ibird2;
        private SoundEffectInstance ibird3;
        private SoundEffectInstance ibird4;
        private SoundEffectInstance ibird5;
        private SoundEffectInstance idrink;
        private SoundEffectInstance ifood;
        private SoundEffectInstance igrowl1;
        private SoundEffectInstance igrowl2;
        private SoundEffectInstance igrowl3;
        private SoundEffectInstance igrowl4;
        private SoundEffectInstance ithud;
        private SoundEffectInstance ibag;

        public AppDJ()
        {
            volOn = true;
            runningOn = false;
            drinkOn = false;
            foodOn = false;
            runVol = 1.0f;

            rand = new Random();
            barkSpan = new TimeSpan(0, 0, 0, 1, 0);
            
        }

        public void LoadContent(ContentManager Content)
        {
            bark1 = Content.Load<SoundEffect>("Sounds/bark1");
            bark2 = Content.Load<SoundEffect>("Sounds/bark2");
            bark3 = Content.Load<SoundEffect>("Sounds/bark3");
            bark4 = Content.Load<SoundEffect>("Sounds/bark4");
            bark5 = Content.Load<SoundEffect>("Sounds/bark5");
            bird1 = Content.Load<SoundEffect>("Sounds/bird1");
            bird2 = Content.Load<SoundEffect>("Sounds/bird2");
            bird3 = Content.Load<SoundEffect>("Sounds/bird3");
            bird4 = Content.Load<SoundEffect>("Sounds/bird4");
            bird5 = Content.Load<SoundEffect>("Sounds/bird5");
            drink = Content.Load<SoundEffect>("Sounds/drink");
            food = Content.Load<SoundEffect>("Sounds/food");
            growl1 = Content.Load<SoundEffect>("Sounds/growl1");
            growl2 = Content.Load<SoundEffect>("Sounds/growl2");
            growl3 = Content.Load<SoundEffect>("Sounds/growl3");
            growl4 = Content.Load<SoundEffect>("Sounds/growl4");
            thud = Content.Load<SoundEffect>("Sounds/thud");
            bag = Content.Load<SoundEffect>("Sounds/bag");



            ibark1 = bark1.CreateInstance();
            ibark2 = bark2.CreateInstance();
            ibark3 = bark3.CreateInstance();
            ibark4 = bark4.CreateInstance();
            ibark5 = bark5.CreateInstance();
            ibird1 = bird1.CreateInstance();
            ibird2 = bird2.CreateInstance();
            ibird3 = bird3.CreateInstance();
            ibird4 = bird4.CreateInstance();
            ibird5 = bird5.CreateInstance();
            idrink = drink.CreateInstance();
            ifood = food.CreateInstance();
            igrowl1 = growl1.CreateInstance();
            igrowl2 = growl2.CreateInstance();
            igrowl3 = growl3.CreateInstance();
            igrowl4 = growl4.CreateInstance();
            ithud = thud.CreateInstance();
            ibag = bag.CreateInstance();

            ifood.IsLooped = true;
            idrink.IsLooped = true;

        }

        
        public void Update(GameTime gameTime)
        {
            if (volOn)
            {
                if (runningOn)
                {
                    if (barkSpan <= TimeSpan.Zero)
                    {
                        int bark = rand.Next(1, 6);
                        switch (bark)
                        {
                            case 1:
                                ibark1.Play();
                                break;

                            case 2:
                                ibark2.Play();
                                break;

                            case 3:
                                ibark3.Play();
                                break;

                            case 4:
                                ibark4.Play();
                                break;

                            case 5:
                                ibark5.Play();
                                break;

                        }

                        barkSpan = new TimeSpan(0, 0, 0, rand.Next(2, 4), rand.Next(100, 700));
                    }
                    else
                    {

                        barkSpan -= gameTime.ElapsedGameTime;
                    }
                    

                }

                if (foodOn)
                {
                    
                    ifood.Play();

                }
                else
                {
                    ifood.Stop();
                }

                if (drinkOn)
                {
                    
                    idrink.Play();
                }
                else
                {
                    idrink.Stop();
                }
            }

        }

        public void playThud(float dist)
        {
            if (volOn)
            {
                ithud.Volume = dist;
                ithud.Play();

            }
        }

        public void playBird()
        {
            if (volOn)
            {

                int bird = rand.Next(1, 6);
                switch (bird)
                {
                    case 1:
                        ibird1.Volume = 0.4f;
                        ibird1.Play();
                        break;

                    case 2:
                        ibird2.Volume = 0.4f;
                        ibird2.Play();
                        break;

                    case 3:
                        ibird3.Volume = 0.4f;
                        ibird3.Play();
                        break;

                    case 4:
                        ibird4.Volume = 0.4f;
                        ibird4.Play();
                        break;

                    case 5:
                        ibird5.Volume = 0.4f;
                        ibird5.Play();
                        break;

                }
            }
        }

        public void playGrowl()
        {
            if (volOn)
            {
                if (growlPlaying() == false)
                {
                    int growl = rand.Next(1, 7);

                    switch (growl)
                    {
                        case 1:
                            igrowl1.Play();
                            break;

                        case 2:
                            igrowl2.Play();
                            break;

                        case 3:
                            igrowl3.Play();
                            break;

                        case 4:
                            igrowl4.Play();
                            break;

                        case 5:
                            break;

                        case 6:
                            break;
                    }
                }
            }
        }

        public bool growlPlaying()
        {

            if (igrowl1.State.Equals(SoundState.Playing))
            {
                return true;
            }
            else if (igrowl2.State.Equals(SoundState.Playing))
            {
                return true;
            }
            else if (igrowl3.State.Equals(SoundState.Playing))
            {
                return true;
            }
            else if (igrowl4.State.Equals(SoundState.Playing))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void playBag()
        {
            if (volOn)
            {
                ibag.Volume = 0.77f;
                ibag.Play();



            }
        }

        public void stopEverything()
        {

            ibark1.Stop();
            ibark2.Stop();
            ibark3.Stop();
            ibark4.Stop();
            ibark5.Stop();
            ibird1.Stop();
            ibird2.Stop();
            ibird3.Stop();
            ibird4.Stop();
            ibird5.Stop();
            idrink.Stop();
            ifood.Stop();
            igrowl1.Stop();
            igrowl2.Stop();
            igrowl3.Stop();
            igrowl4.Stop();
            ithud.Stop();
            ibag.Stop();

        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace XNA3
{
    public class ActionScene : GameScene
    {
        //basics
        protected Texture2D actonTexture;
        protected SpriteBatch spriteBatch = null;
        protected double secs = -1.0;

        //game elements
        private Survivor survivor;
        private Zombie zombie;
        private BossZombie bossZombie;
        private Zombie[] zombies;
        private BulletComponent[] bullets;
        private HealthBar health;
        private HealthBar bossHealth;
        private HealthPack healthPack;
        private AmmoPack ammoPack;
        private Texture2D healthPackTexture;
        private Texture2D ammoPackTexture;
        //private Player2 player2, player2IT;
        //private Warp warp;

        private MouseComponent mc;
        private BulletComponent myBullet;
        private Texture2D bulletTexture;
        private Texture2D zombieTexture;
        private Texture2D bossZombieTexture;
        //private Texture2D backgroundTexture;
        private SpriteFont gameFont;

        protected ImageComponent background;

        //float currentTime;
        private const int SPAWN = 6000;
        private const int BOSSSPAWN = 12000;
        private const int HEALTHTIME = 30000;
        private const int AMMOTIME = 20000;

        private int lastTickCount;
        private int bossTickCount;
        private int player1score = 0;
        private int player2score = 0;
        private int countdown = 20;
        private int counter = 0;
        //bool WarpBool = false;
        //bool Play1IT = true;

        private AudioComponent audioComponent;
        Cue myLoopingSound = null;
        private Game game;

        public int gameTimer = -1;
        int LastSecond = 0;
        
        int time = 0;
        int ammoTime = 0;
        
        bool packHit = false;
        bool ammopackHit = false;
        Random RandomClass = new Random();
        bool roundStart = true;
        bool ammoroundStart = true;
        bool audioNeedHealth = false;
        private int bulletCount = 50;

        private int NUMZOMBIES = 8;
        bool bossAlive = false;

        bool gameStart = true;
        //Cue Player1Loop, Player2Loop;

        //gui stuff

        public ActionScene(Game game, Texture2D backgroundTexture, Texture2D survivorTexture,Texture2D pointerTexture,Texture2D bulletTexture,Texture2D healthTexture,Texture2D zombieTexture,Texture2D bossZombieTexture,Texture2D healthPackText,Texture2D ammoPackText, SpriteFont font)
            : base(game)
        {
            this.game = game;
            this.bulletTexture = bulletTexture;
            this.zombieTexture = zombieTexture;
            this.bossZombieTexture = bossZombieTexture;
            this.healthPackTexture = healthPackText;
            this.ammoPackTexture = ammoPackText;
            gameFont = font;
            //get audio component
            audioComponent = (AudioComponent)Game.Services.GetService(typeof(AudioComponent));
            
            //Create the background for this scene
            background = new ImageComponent(game, backgroundTexture, ImageComponent.DrawMode.Stretch);
            Components.Add(background);

            health = new HealthBar(game, ref healthTexture,30,Color.Red);
            Components.Add(health);

            bossHealth = new HealthBar(game, ref healthTexture,708,Color.Blue);
            Components.Add(bossHealth);

            mc = new MouseComponent(game, ref pointerTexture);
            Components.Add(mc);

            survivor = new Survivor(game, ref survivorTexture, ref mc);
            Components.Add(survivor);

            healthPack = new HealthPack(game, ref healthPackTexture);
            Components.Add(healthPack);

            ammoPack = new AmmoPack(game, ref ammoPackTexture);
            Components.Add(ammoPack);

            zombies = new Zombie[100];
            bullets = new BulletComponent[100];

            bossZombie = new BossZombie(game, ref bossZombieTexture, ref survivor);
            Components.Add(bossZombie);

            gameTimer = -1;

            //set up audio variables
            //myLoopingSound = audioComponent.sb.GetCue("DeadAirTime");
            //Player2Loop = audioComponent.sb.GetCue("startmusic");*/
        }

        public override void Show()
        {
            time = 0;
            gameTimer = -1;
            LastSecond = 0;
            audioComponent.PlayCue("AnswerReady05");
            myLoopingSound = audioComponent.sb.GetCue("DeadAirTime");
            myLoopingSound.Play();
            bulletCount = 50;
            bossHealth.Hide();
            bossZombie.goAway();
            for (int i = 0; i < zombies.Length; i++)
            {

                Components.Remove(zombies[i]);
            }

            
            survivor.PutinStartPosition();
            health.Health = 100;
            bossHealth.Health = 100;
            NUMZOMBIES = 8;
            for (int i = 0; i < NUMZOMBIES; i++)
            {
                zombie = new Zombie(game, ref zombieTexture, ref survivor);
                zombies[i] = zombie;
                Components.Add(zombie);
                zombies[i].PutonRandomWall();
            }
            lastTickCount = System.Environment.TickCount;
            bossTickCount = System.Environment.TickCount;

            ammoTime = System.Environment.TickCount;
            time = System.Environment.TickCount;
            packHit = true;
            ammopackHit = true;
            ammoPack.PutinStartPosition(2000, 2000);
            healthPack.PutinStartPosition(2000, 2000);

            gameTimer = -1;
            base.Show();
        }

        public override void  Hide()
        {
            gameTimer = -1;
 	         base.Hide();
        }

        public override void Update(GameTime gameTime)
        {
            DoGameLogic(gameTime);
 	        base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        public int getGreenScore()
        {
            return player1score;
        }
        public int getBlueScore()
        {
            return player2score;
        }
        public int getTimeLeft()
        {
            return countdown;
        }

        public int getHealth()
        {
            return health.getHealth();
        }

        private void packTimer(GameTime gameTime)
        {
            //Reposition health pack onto the playing board
            if (((packHit == true) && ((System.Environment.TickCount - time) > HEALTHTIME)) || ((packHit == false) && (roundStart == true) && ((System.Environment.TickCount - time) > HEALTHTIME)))
            {
                roundStart = false;
                healthPack.position.X = RandomClass.Next(0, 994);
                //Makes sure warp does not spawn onto sprites X position
                while (true)
                {
                    if ((healthPack.position.X >= survivor.getPosition().X) && (healthPack.position.X <= survivor.getPosition().X + 32))
                        healthPack.position.X = RandomClass.Next(0, 994);
                    else
                        break;
                }
                healthPack.position.Y = RandomClass.Next(0, 748);
                //Makes sure warp does not spawn onto sprites Y position
                while (true)
                {
                    if ((healthPack.position.Y >= survivor.getPosition().Y) && (healthPack.position.Y <= survivor.getPosition().Y + 32))
                        healthPack.position.Y = RandomClass.Next(0, 748);
                    else
                        break; ;
                }
                audioComponent.PlayCue("SpotFirstAid03");
                packHit = false;
            }
        }
        private void ammoTimer(GameTime gameTime)
        {
            //Reposition ammo pack onto the playing board
            if (((ammopackHit == true) && ((System.Environment.TickCount - ammoTime) > AMMOTIME)) || ((ammopackHit == false) && (ammoroundStart == true) && ((System.Environment.TickCount - ammoTime) > AMMOTIME)))
            {
                ammoroundStart = false;
                ammoPack.position.X = RandomClass.Next(0, 994);
                //Makes sure warp does not spawn onto sprites X position
                while (true)
                {
                    if ((ammoPack.position.X >= survivor.getPosition().X) && (ammoPack.position.X <= survivor.getPosition().X + 32))
                        ammoPack.position.X = RandomClass.Next(0, 994);
                    else
                        break;
                }
                ammoPack.position.Y = RandomClass.Next(0, 748);
                //Makes sure warp does not spawn onto sprites Y position
                while (true)
                {
                    if ((ammoPack.position.Y >= survivor.getPosition().Y) && (ammoPack.position.Y <= survivor.getPosition().Y + 32))
                        ammoPack.position.Y = RandomClass.Next(0, 748);
                    else
                        break; ;
                }
                audioComponent.PlayCue("SpotAmmo01");
                ammopackHit = false;
            }
        }

        public int getBulletsLeft()
        {
            return bulletCount;
        }

        public void updateTimer(GameTime gameTime)
        {
            if ((int)gameTime.TotalRealTime.TotalSeconds > LastSecond)
            {
                LastSecond += 1;
                gameTimer += 1;
            }
        }

        public int getTimer()
        {
            return gameTimer;
        }
        public void setTimer()
        {
            gameTimer = -1;
        }

        private void DoGameLogic(GameTime gameTime)
        {
            packTimer(gameTime);
            ammoTimer(gameTime);
            updateTimer(gameTime);

            if (bossHealth.getHealth() == 0)
            {
                bossAlive = false;
            }

            if ((getHealth() <= 40) && (audioNeedHealth == false))
            {
                audioComponent.PlayCue("GoingToDie12");
                audioNeedHealth = true;
            }

            if (mc.LeftButton == ButtonState.Pressed)
            {
                if (((secs + .3) < (double)gameTime.TotalRealTime.TotalSeconds)&&(bulletCount > 0))
                {
                    bulletCount--;
                    secs = (double)gameTime.TotalRealTime.TotalSeconds;
                    mc.PointerColor = Color.Green;
                    audioComponent.PlayCue("pistol_fire");
                    myBullet = new BulletComponent(game, ref bulletTexture, survivor.getPosition(), mc.getPosition());
                    bullets[counter] = myBullet;
                    counter++;
                    if (counter >= 99)
                        counter = 0;
                    Components.Add(myBullet);
                    //myBullet.Dispose();
                }
            }

            
            //check to see if a bullet in the bullet array has collided with a zombie in the zombie array
            for (int i = 0; i < bullets.Length ; i++)
            {
                for (int k = 0; k < NUMZOMBIES; k++)
                {
                    Rectangle zombieRec = zombies[k].GetBounds();
                    Rectangle bzrec = bossZombie.GetBounds();
                    //bullet and zombie have collided
                    if (bullets[i] != null)
                    {
                        if (((BulletComponent)bullets[i]).CheckCollision(zombieRec))
                        {
                            zombies[k].PutonRandomWall();
                            bullets[i].goAway();
                        }
                        //also take this moment to check if the bullet hits a boss zombie
                        if (((BulletComponent)bullets[i]).CheckCollision(bzrec))
                        {
                            bullets[i].goAway();
                            bossHealth.Hit(20);
                            if(bossHealth.getHealth() <= 0)
                            {
                                bossHealth.Hide();
                                bossZombie.goAway();
                                //bossTickCount = System.Environment.TickCount;
                            }
                        }
                    }
                }
            }

            //check to see if a zombie in the zombie array has collided with the survivor and check for boss
            Rectangle survivorRec = survivor.GetBounds();
            for (int i = 0; i < NUMZOMBIES; i++)
            {
                if (zombies[i].CheckCollision(survivorRec))
                {
                    if ((secs + .2) < (double)gameTime.TotalRealTime.TotalSeconds)
                    {
                        secs = (double)gameTime.TotalRealTime.TotalSeconds;
                        if (getHealth() == 5)
                        {
                            myLoopingSound.Stop(AudioStopOptions.Immediate);
                            audioComponent.PlayCue("DeathScream10");
                        }
                        health.Hit(5);
                    }
                }
            }
            if (bossZombie.CheckCollision(survivorRec))
            {
                if ((secs + .2) < (double)gameTime.TotalRealTime.TotalSeconds)
                {
                    secs = (double)gameTime.TotalRealTime.TotalSeconds;
                    if (getHealth() <= 10)
                    {
                        myLoopingSound.Stop(AudioStopOptions.Immediate);
                        audioComponent.PlayCue("DeathScream10");
                    }
                    health.Hit(10);
                }
            }

            
            //Check to see if the survivor collides with a health pack
            Rectangle healthPackRectangle = healthPack.GetBounds();
            if (healthPack.CheckCollision(survivorRec))
            {
                audioNeedHealth = false;
                packHit = true;
                time = System.Environment.TickCount;
                if (health.Health > 50)
                    health.Health = 100;
                else
                    health.Health += 50;
                healthPack.PutinStartPosition(2000, 2000);
            }

            //Check to see if the survivor collides with a ammo pack
            Rectangle ammoPackRectangle = ammoPack.GetBounds();
            if (ammoPack.CheckCollision(survivorRec))
            {
                ammopackHit = true;
                ammoTime = System.Environment.TickCount;
                if (bulletCount > 25)
                    bulletCount = 50;
                else
                    bulletCount += 25;
                ammoPack.PutinStartPosition(2000, 2000);
            }

            //add another zombie every 10 seconds
            if ((System.Environment.TickCount - lastTickCount) > SPAWN)
            {
                
                zombie = new Zombie(game, ref zombieTexture, ref survivor);
                zombies[NUMZOMBIES] = zombie;
                Components.Add(zombie);
                zombies[NUMZOMBIES].PutonRandomWall();
                NUMZOMBIES++;
                lastTickCount = System.Environment.TickCount;
                
            }

            //spawn a boss zombie every 25 seconds
            if (((System.Environment.TickCount - bossTickCount) > BOSSSPAWN) && (bossAlive == false))
            {
                bossAlive = true;
                audioComponent.PlayCue("WarnTank03");
                bossHealth.Health = 100;
                bossZombie.PutonRandomWall();
                bossZombie.Show();
                bossHealth.Show();
                bossTickCount = System.Environment.TickCount;

            }
        }

    }
}

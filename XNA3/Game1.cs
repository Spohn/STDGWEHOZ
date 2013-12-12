using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace XNA3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        protected HelpScene helpScene;
        protected StartScene startScene;
        protected ActionScene actionScene;
        protected GameScene activeScene;

        //textures
        protected Texture2D helpTexture;
        protected Texture2D backgroundTexture;
        private Texture2D PointerTexture;
        private Texture2D BulletTexture;
        private Texture2D survivorTexture;
        private Texture2D healthTexture;
        private Texture2D zombieTexture;
        private Texture2D bossZombieTexture;
        private Texture2D healthPackTexture;
        private Texture2D ammoPackTexture;

        //audio stuff
        private AudioComponent audioComponent;

        //StartScene Stufff
        protected Texture2D startBackgroundTexture;
        protected Texture2D gameBackgroundTexture;
        private SpriteFont smallFont, largeFont, gameFont;

        //handle input
        protected KeyboardState oldKeyboardState;

        bool start = true;

        //public int gameTimer = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = false;
            audioComponent = new AudioComponent(this);
            Components.Add(audioComponent);
            Services.AddService(typeof(AudioComponent), audioComponent);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            //set textures and fonts
            helpTexture = Content.Load<Texture2D>("help");
            smallFont = Content.Load<SpriteFont>("menuSmall");
            largeFont = Content.Load<SpriteFont>("menuLarge");
            gameFont = Content.Load<SpriteFont>("font");
            healthTexture = Content.Load<Texture2D>("HealthBar");
            survivorTexture = Content.Load<Texture2D>("Survivor");
            PointerTexture = Content.Load<Texture2D>("reticle");
            BulletTexture = Content.Load<Texture2D>("bullet");
            zombieTexture = Content.Load<Texture2D>("Zombie");
            bossZombieTexture = Content.Load<Texture2D>("BossZombie");
            healthPackTexture = Content.Load<Texture2D>("HealthPack");
            ammoPackTexture = Content.Load<Texture2D>("AmmoPack");


            startBackgroundTexture = Content.Load<Texture2D>("Menu");
            gameBackgroundTexture = Content.Load<Texture2D>("road_background");

            //Create the Help Scene
            helpScene = new HelpScene(this, helpTexture);
            Components.Add(helpScene);

            //Create the Start Scene
            startScene = new StartScene(this, smallFont, largeFont, startBackgroundTexture);
            Components.Add(startScene);

            //create the actionScene
            actionScene = new ActionScene(this, gameBackgroundTexture,survivorTexture,PointerTexture,BulletTexture,healthTexture,zombieTexture,bossZombieTexture,healthPackTexture,ammoPackTexture, smallFont);
            Components.Add(actionScene);

            //start game in start scene
            startScene.Show();
            activeScene = startScene;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (activeScene == startScene)
            {
                HandleStartSceneInput();
            }
            else if(activeScene == actionScene)
            {
                if (actionScene.getHealth() <= 0)
                {
                    //gameTimer = 0;
                    ShowScene(startScene);
                }
            }
            else if (activeScene == helpScene)
            {
                if (CheckEnterA())
                {
                    ShowScene(startScene);
                }
            }

            base.Update(gameTime);
        }

        private bool CheckEnterA()
        {
            KeyboardState ks = Keyboard.GetState();

            bool result = (oldKeyboardState.IsKeyDown(Keys.Enter) && (ks.IsKeyUp(Keys.Enter)));

            oldKeyboardState = ks;

            return result;
        }
    
        private void HandleStartSceneInput()
        {
            if (CheckEnterA())
            {
                switch (startScene.menu.SelectedIndex)
                {
                    case 0:
                        actionScene.setTimer();
                        ShowScene(actionScene);
                        break;
                    case 1:
                        ShowScene(helpScene);
                        break;
                    case 2:
                        Exit();
                        break;
                }
            }
        }

        protected void ShowScene(GameScene scene)
        {
            //gameTimer = 0;
            activeScene.Hide();
            activeScene = scene;
            scene.Show();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //start rendering
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
           
            //draw game components
            base.Draw(gameTime);
            if (activeScene == actionScene)
            {
                spriteBatch.DrawString(gameFont, "Ammo:", new Vector2(35, 15), Color.Black);
                spriteBatch.DrawString(gameFont, "Time:", new Vector2(900, 15), Color.Black);
                if(actionScene.getBulletsLeft()>10)
                    spriteBatch.DrawString(gameFont, (actionScene.getBulletsLeft()).ToString(), new Vector2(60, 35), Color.Black);
                else
                    spriteBatch.DrawString(gameFont, (actionScene.getBulletsLeft()).ToString(), new Vector2(60, 35), Color.Red);
                if (start == true)
                {
                    actionScene.setTimer();
                    start = false;
                }
                spriteBatch.DrawString(gameFont, (actionScene.getTimer()).ToString(), new Vector2(930, 35), Color.Yellow);
            }
            //spriteBatch.DrawString(gameFont, "Green Score: " + (actionScene.getGreenScore()).ToString(), new Vector2(3, 0), Color.Black);
            //spriteBatch.DrawString(gameFont, "Blue Score: " + (actionScene.getBlueScore()).ToString(), new Vector2(3, 18), Color.Black);
            //spriteBatch.DrawString(gameFont, "Time to Switch: " + (actionScene.getTimeLeft()).ToString(), new Vector2(840, 0), Color.Black);
            //end rendering
            spriteBatch.End();            
        }
    }
}

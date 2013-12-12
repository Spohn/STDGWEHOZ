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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HealthBar : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D healthTexture;
        protected int currentHealth = 100;
        protected Boolean hide = false;
        protected Color color;
        protected int height;

        public HealthBar(Game game, ref Texture2D theTexture, int height, Color color)
            : base(game)
        {
            healthTexture = theTexture;
            this.height = height;
            this.color = color;
        }

        public void Hit(int damage)
        {
            currentHealth -= damage;
        }
        public int getHealth()
        {
            return currentHealth;
        }
        public int Health
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            currentHealth = (int)MathHelper.Clamp(currentHealth, 0, 100);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            //graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch sBatch =
                (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            if (!hide)
            {
                //Draw the negative space for the health bar
                sBatch.Draw(healthTexture, new Rectangle(1024 / 2 - healthTexture.Width / 2, height, healthTexture.Width, 44), new Rectangle(0, 45, healthTexture.Width, 44), Color.Gray);

                //Draw the current health level based on the current Health
                sBatch.Draw(healthTexture, new Rectangle(1024 / 2 - healthTexture.Width / 2, height, (int)(healthTexture.Width * ((double)currentHealth / 100)), 44), new Rectangle(0, 45, healthTexture.Width, 44), color);

                //Draw the box around the health bar
                sBatch.Draw(healthTexture, new Rectangle(1024 / 2 - healthTexture.Width / 2, height, healthTexture.Width, 44), new Rectangle(0, 0, healthTexture.Width, 44), Color.White);
            }
            base.Draw(gameTime);
        }
        public void Hide()
        {
            hide = true;
        }
        public void Show()
        {
            hide = false;
        }
    }
}
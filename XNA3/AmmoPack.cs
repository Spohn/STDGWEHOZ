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
    public class AmmoPack : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D ammoTex;
        protected const int packWidth = 30;
        protected const int packHeight = 20;
        protected Rectangle spriteRectangle;
        public Vector2 position;
        protected Random random;
        protected int seconds;
        //protected int randX;
        //protected int randY;

        public AmmoPack(Game game, ref Texture2D theTexture)
            : base(game)
        {
            ammoTex = theTexture;
            position = new Vector2();

            spriteRectangle = new Rectangle(0, 0, packWidth, packHeight);
            PutinStartPosition(2000, 2000);
        }

        public void PutinStartPosition(int posX, int posY)
        {
            position.X = posX;
            position.Y = posY;
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

            base.Update(gameTime);
        }
        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, packWidth, packHeight);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch =
                (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            sBatch.Draw(ammoTex, position, spriteRectangle, Color.White);
            base.Draw(gameTime);
        }
        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, packWidth, packHeight);

            return spriterect.Intersects(rect);
        }
    }
}
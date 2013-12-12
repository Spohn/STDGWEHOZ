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
    /// 
    public class BulletComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D bulletTex;
        protected Vector2 mousePos, position;
        protected Rectangle bulletRectangle;
        protected Vector2 velocity;
        protected bool stop = false;
        public BulletComponent(Game game, ref Texture2D theTexture, Vector2 Sposition, Vector2 Mposition)
            : base(game)
        {
            bulletTex = theTexture;
            mousePos = Mposition;
            position = Sposition;
            bulletRectangle = new Rectangle(0, 0, 5, 5);
            double angle = Math.Atan2(((double)(mousePos.Y - position.Y)),(double)(mousePos.X - position.X));
            velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            velocity.X = velocity.X * 7;
            velocity.Y = velocity.Y * 7;
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

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch =
                (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            sBatch.Draw(bulletTex, position, bulletRectangle, Color.White);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if(!stop)
                position += velocity;            
            base.Update(gameTime);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, 5, 5);
        }

        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, 5, 5);

            return spriterect.Intersects(rect);
        }
        public void goAway()
        {
            position.X = 5000;
            position.Y = 5000;
            stop = true;
        }
    }
}
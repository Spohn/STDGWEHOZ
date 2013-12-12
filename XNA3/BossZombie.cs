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
    public class BossZombie : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D texture;
        protected Rectangle spriteRectangle;
        protected Vector2 position;
        protected Vector2 velocity;
        float rotation;

        protected const int PLAYERWIDTH = 100;
        protected const int PLAYERHEIGHT = 100;
        protected const int speed = 4;

        protected Rectangle screenBounds;

        protected bool show = true;

        Random random;

        Survivor survivor;

        //MouseComponent myMouse;

        public BossZombie(Game game, ref Texture2D theTexture, ref Survivor s)
            : base(game)
        {

            // TODO: Construct any child components here
            texture = theTexture;
            position = new Vector2();
            random = new Random(this.GetHashCode());
            survivor = s;

            spriteRectangle = new Rectangle(0, 0, PLAYERWIDTH, PLAYERHEIGHT);

            screenBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            //myMouse = mouse;
        }

        public void PutinStartPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }
        public void PutonRandomWall()
        {
            int wall = random.Next(4);

            if (wall == 0)
                PutRandomNorthWall();
            else if (wall == 1)
                PutRandomSouthWall();
            else if (wall == 2)
                PutRandomEastWall();
            else if (wall == 3)
                PutRandomWestWall();
        }
        public void PutinRandomPosition()
        {
            int x = random.Next(Game.Window.ClientBounds.Width - PLAYERWIDTH);
            int y = random.Next(Game.Window.ClientBounds.Height - PLAYERHEIGHT);

            position.X = x;
            position.Y = y;
        }

        public void PutRandomNorthWall()
        {
            int x = random.Next(Game.Window.ClientBounds.Width - PLAYERWIDTH);
            int y = 0;

            position.X = x;
            position.Y = y;
        }
        public void PutRandomSouthWall()
        {
            int x = random.Next(Game.Window.ClientBounds.Width - PLAYERWIDTH);
            int y = Game.Window.ClientBounds.Height - PLAYERHEIGHT;

            position.X = x;
            position.Y = y;
        }
        public void PutRandomEastWall()
        {
            int x = Game.Window.ClientBounds.Width - PLAYERWIDTH;
            int y = random.Next(Game.Window.ClientBounds.Height - PLAYERHEIGHT);

            position.X = x;
            position.Y = y;
        }
        public void PutRandomWestWall()
        {
            int x = 0;
            int y = random.Next(Game.Window.ClientBounds.Height - PLAYERHEIGHT);

            position.X = x;
            position.Y = y;
        }

        public void setPos(BossZombie s)
        {
            position.X = s.position.X;
            position.Y = s.position.Y;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            random = new Random(this.GetHashCode());
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if(show)
            {
                float XDistance = position.X - survivor.getPosition().X;
                float YDistance = position.Y - survivor.getPosition().Y;

                rotation = (float)Math.Atan2(YDistance, XDistance);

                double angle = Math.Atan2(((double)(survivor.getPosition().Y - position.Y)), (double)(survivor.getPosition().X - position.X));
                velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                velocity.X = velocity.X * speed;
                velocity.Y = velocity.Y * speed;

                position += velocity;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //get current sprite batch
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            //Draw the survivor
            if (show)
            {
                //Draw the sprite with required rotation and origin (centre of rotation) set to the centre of the sprite
                sBatch.Draw(texture, position, spriteRectangle, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
            }
            base.Draw(gameTime);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, PLAYERWIDTH, PLAYERHEIGHT);
        }

        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, PLAYERWIDTH, PLAYERHEIGHT);

            return spriterect.Intersects(rect);
        }

        public void Show()
        {
            show = true;
        }
        public void Hide()
        {
            show = false;
        }
        public void goAway()
        {
            position.X = 5000;
            position.Y = 5000;
            Hide();
        }

        public Vector2 getPosition()
        {
            return position;
        }
    }
}
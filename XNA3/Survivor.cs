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
    public class Survivor : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D texture;
        protected Rectangle spriteRectangle;
        protected Vector2 position;
        float rotation;

        protected const int PLAYERWIDTH = 32;
        protected const int PLAYERHEIGHT = 32;
        protected const int speed = 5;

        protected Rectangle screenBounds;

        protected bool show = true;

        Random random;

        MouseComponent myMouse;

        public Survivor(Game game, ref Texture2D theTexture, ref MouseComponent mouse)
            : base(game)
        {
            
            // TODO: Construct any child components here
            texture = theTexture;
            position = new Vector2();

            spriteRectangle = new Rectangle(0, 0, PLAYERWIDTH, PLAYERHEIGHT);

            screenBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            myMouse = mouse;
        }

        public void PutinStartPosition()
        {
            position.X = 500;
            position.Y = 400;
        }

        public void PutinRandomPosition()
        {
            random = new Random();
            int x = random.Next(Game.Window.ClientBounds.Width - PLAYERWIDTH);
            int y = random.Next(Game.Window.ClientBounds.Height - PLAYERHEIGHT);

            position.X = x;
            position.Y = y;
        }

        public void setPos(Survivor s)
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

            // Add the player component
            

            if (myMouse.LeftButton == ButtonState.Pressed)
            {
                myMouse.PointerColor = Color.Green;
                //myBullet = new BulletComponent(this, ref BulletTexture);
                //Components.Add(myBullet);
                //myBullet.Dispose();
            }

            else if (myMouse.RightButton == ButtonState.Pressed)
                myMouse.PointerColor = Color.Red;
            else
                myMouse.PointerColor = Color.White;

            //Store the mouse state
            MouseState mouse = Mouse.GetState(); ;
            KeyboardState keyboardState = Keyboard.GetState();

            //Calculate the distance from the square to the mouse's X and Y position
            float XDistance = position.X - mouse.X;
            float YDistance = position.Y - mouse.Y;

            //Calculate the required rotation by doing a two-variable arc-tan
            rotation = (float)Math.Atan2(YDistance, XDistance);

            if (keyboardState.IsKeyDown(Keys.W)) //up
            {
                if (!(position.Y  - (PLAYERHEIGHT/2) <= 0))
                    position.Y -= speed;
            }
            if (keyboardState.IsKeyDown(Keys.S)) //down
            {
                if (!(position.Y + (PLAYERHEIGHT/2) >= screenBounds.Height))
                    position.Y += speed;
            }
            if (keyboardState.IsKeyDown(Keys.A)) //left
            {
                if (!(position.X  - (PLAYERWIDTH/2) <= 0))
                    position.X -= speed;
            }
            if (keyboardState.IsKeyDown(Keys.D)) //right
            {
                if (!(position.X + (PLAYERWIDTH/2) >= screenBounds.Width))
                    position.X += speed;
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

        public Vector2 getPosition()
        {
            return position;
        }
    }
}
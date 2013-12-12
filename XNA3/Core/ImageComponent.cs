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
    public class ImageComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum DrawMode
        {
            Center = 1,
            Stretch,
        };

        protected readonly Texture2D texture;
        protected readonly DrawMode drawMode;
        protected SpriteBatch spriteBatch = null;
        protected Rectangle imageRect;

        public ImageComponent(Game game, Texture2D texture, DrawMode drawMode)
            : base(game)
        {
            this.texture = texture;
            this.drawMode = drawMode;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            switch (drawMode)
            {
                case DrawMode.Center:
                    imageRect = new Rectangle((Game.Window.ClientBounds.Width - texture.Width) / 2, (Game.Window.ClientBounds.Height - texture.Height) / 2, texture.Width, texture.Height);
                    break;
                case DrawMode.Stretch:
                    imageRect = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
                    break;

            }
            // TODO: Construct any child components here
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

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(texture, imageRect, Color.White);
            base.Draw(gameTime);
        }
    }
}
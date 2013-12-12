using System;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace XNA3
{
    public class StartScene : GameScene
    {
        public TextMenuComponent menu;
        protected readonly Texture2D elements;

        protected SpriteBatch spriteBatch = null;


        public StartScene(Game game, SpriteFont smallFont, SpriteFont largeFont, Texture2D background)
            : base(game)
        {
            //this.elements = elements;
            Components.Add(new ImageComponent(game, background, ImageComponent.DrawMode.Center));

            //Create the menu
            string[] items = { "Play STDSGWEHOZ","Help","Quit" };
            menu = new TextMenuComponent(game, smallFont, largeFont);
            menu.SetMenuItems(items);
            Components.Add(menu);

            //get current spritebatch
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

        }

        public override void Update(GameTime gameTime)
        {
            
            menu.Visible = true;
            menu.Enabled = true;
            

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Show()
        {
            //put menu cnetered on screen
            menu.Position = new Vector2((Game.Window.ClientBounds.Width - menu.Width)/2, 330);

            //menu.Visible = false;
            //menu.Enabled = false;

            base.Show();
        }
    }
}

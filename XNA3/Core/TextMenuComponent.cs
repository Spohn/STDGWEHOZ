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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TextMenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected SpriteBatch spriteBatch = null;
        protected readonly SpriteFont regularFont, selectedFont;
        protected Color regularColor = Color.White, selectedColor = Color.Yellow;
        protected Vector2 position = new Vector2();
        protected int selectedIndex = 0;
        private readonly StringCollection menuItems;
        protected int width, height;
        protected KeyboardState oks;

        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);
            CalculateBounds();
        }

        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }

        public Color RegularColor
        {
            get { return regularColor; }
            set { regularColor = value; }
        }

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        protected void CalculateBounds()
        {
            width = 0;
            height = 0;
            foreach(string item in menuItems)
            {
                Vector2 size = selectedFont.MeasureString(item);
                if (size.X > Width)
                {
                    width = (int)size.X;
                }
                height += selectedFont.LineSpacing;
            }
        }

        public TextMenuComponent(Game game,SpriteFont normalFont,SpriteFont selectedFont)
            : base(game)
        {
            // TODO: Construct any child components here
            regularFont = normalFont;
            this.selectedFont = selectedFont;
            menuItems = new StringCollection();

            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            oks = Keyboard.GetState();
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
            KeyboardState ks = Keyboard.GetState();
            bool down, up;
            down = (oks.IsKeyDown(Keys.Down) && (ks.IsKeyUp(Keys.Down)));
            up = (oks.IsKeyDown(Keys.Up) && (ks.IsKeyUp(Keys.Up)));

            if (down)
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Count)
                {
                    selectedIndex = 0;
                }
            }
            if (up)
            {
                selectedIndex--;
                if(selectedIndex == -1)
                {
                    selectedIndex = menuItems.Count -1;
                }
            }
            oks = ks;


            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            float y = position.Y;
            for (int i = 0; i < menuItems.Count; i++)
            {
                SpriteFont font;
                Color theColor;
                if (i == selectedIndex)
                {
                    font = selectedFont;
                    theColor = selectedColor;
                }
                else
                {
                    font = regularFont;
                    theColor = regularColor;
                }

                spriteBatch.DrawString(font, menuItems[i], new Vector2(position.X + 1, y + 1), Color.White);
                spriteBatch.DrawString(font, menuItems[i], new Vector2(position.X, y), Color.Yellow);
                y += font.LineSpacing;
            }
            base.Draw(gameTime);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XNA3
{
    public class MouseComponent : DrawableGameComponent
    {
#if(!XBOX360)
        private MouseState lastState, currentState;
#endif
        private GamePadState lastpadState, currentPadState;
        private PlayerIndex padIndex;
        private Vector2 position;
        private Texture2D cursorTex;
        private Color cursorColor;
        private SpriteBatch spriteBatch;
        private int mouseSpeed = 25;
        private int mouseSpeedSlow = 10;
        Game theGame;

        public MouseComponent(Game game, ref Texture2D theTexture)
            : base(game)
        {
            //Default the gamepad to player 1, this can be changed
            cursorTex = theTexture;
            padIndex = PlayerIndex.One;
            cursorColor = Color.White;
            ContentManager content = new ContentManager(Game.Services);
            //this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            theGame = game;

        }

        public Vector2 getPosition()
        {
            return position;
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePointer(GamePad.GetState(padIndex));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch =
                (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            //this.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            //this.spriteBatch.Draw(this.cursorTex, this.position, this.cursorColor);
            spriteBatch.Draw(this.cursorTex, this.position, this.cursorColor);
            //this.spriteBatch.End();
            base.Draw(gameTime);
        }

        /*protected override void LoadContent (GameTime gameTime)
        {
            if (loadAllContent)
            {
                //Load our own resources
                
            }
            base.LoadContent(loadAllContent);

        }*/
        private ButtonState _leftButton;

        public ButtonState LeftButton
        {
            get { return _leftButton; }
            set { _leftButton = value; }
        }
        private ButtonState _rightButton;

        public ButtonState RightButton
        {
            get { return _rightButton; }
            set { _rightButton = value; }
        }
        public Texture2D PointerTexture
        {
            get { return cursorTex; }
            set { cursorTex = value; }
        }

        public Color PointerColor
        {
            get { return cursorColor; }
            set { cursorColor = value; }
        }

        public PlayerIndex PadIndex
        {
            get { return padIndex; }
            set { padIndex = value; }
        }

        public void UpdatePointer(GamePadState gamePadState)
        {

            int speed = mouseSpeed;

            if (currentPadState != null)

                lastpadState = currentPadState;

            currentPadState = gamePadState;

            if (currentPadState != lastpadState)
            {

                if (gamePadState.Triggers.Right > 0)

                    speed = mouseSpeedSlow;



                this.position.X = this.position.X + (gamePadState.ThumbSticks.Left.X * speed);

                this.position.Y = this.position.Y + (-gamePadState.ThumbSticks.Left.Y * speed);

                LeftButton = gamePadState.Buttons.A;

                RightButton = gamePadState.Buttons.B;

            }

            #region Physical Mouse (Not for 360)

#if(!XBOX360)



            //Check if mouse is inside our window

            if (currentState != null)

                lastState = currentState;

            currentState = Mouse.GetState();

            if (currentState != lastState)
            {

                int w, h;

                w = theGame.Window.ClientBounds.Width;

                h = theGame.Window.ClientBounds.Height;

                if (currentState.X > 0 && currentState.Y > 0 && currentState.X < w && currentState.Y < h)
                {

                    this.position.X = currentState.X;

                    this.position.Y = currentState.Y;

                    LeftButton = currentState.LeftButton;

                    RightButton = currentState.RightButton;

                }

            }

#endif

            #endregion

        }

    }
}

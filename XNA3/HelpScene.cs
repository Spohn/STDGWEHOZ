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
    public class HelpScene : GameScene
    {

        public HelpScene(Game game, Texture2D background)
            : base(game)
        {
            Components.Add(new ImageComponent(game, background, ImageComponent.DrawMode.Center));
        }
    }
}

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
    public class AudioComponent : Microsoft.Xna.Framework.GameComponent
    {
        private AudioEngine ae;
        private WaveBank wb;
        public SoundBank sb;

        public AudioComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            ae = new AudioEngine("Content\\audio.xgs");
            wb = new WaveBank(ae, "Content\\Wave Bank.xwb");
            if (wb != null)
            {
                sb = new SoundBank(ae, "Content\\Sound Bank.xsb");
            }
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            ae.Update();
            base.Update(gameTime);
        }

        public void PlayCue(string cue)
        {
            sb.PlayCue(cue);
        }
        
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical1
{
    /// <summary>
    /// The FrameRateCounter (FPS Counter), as provided by the University
    /// It works by comparing the time between the current and previous frame
    /// </summary>
    class FrameRateCounter : DrawableGameComponent
    {
        int frameRate, frameCounter, secondsPassed;
        public FrameRateCounter(Game game) : base(game)
        {
            frameRate = 0;
            frameCounter = 0;
            secondsPassed = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (secondsPassed != gameTime.TotalGameTime.Seconds)
            {
                frameRate = frameCounter;
                secondsPassed = gameTime.TotalGameTime.Seconds;
                frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;
        }

        public int FrameRate
        {
            get { return frameRate; }
        }
    }
}

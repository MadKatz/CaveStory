using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    class AnimatedSprite : Sprite
    {
        private int frame_time;
        private int num_frames;
        private int current_frame;
        private int elapsed_time; // Elapsed time since last frame change
        public AnimatedSprite(Game1 game, String filename, int source_x, int source_y, int source_width, int source_height, int fps, int num_frames)
            : base(game, filename, source_x, source_y, source_width, source_height)
        {
            frame_time = (1000 / fps);
            this.num_frames = num_frames;
            current_frame = 0;
            elapsed_time = 0;

        }

        public override void Update(GameTime gameTime)
        {
            elapsed_time += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed_time > frame_time)
            {
                current_frame++;
                elapsed_time = 0;
                if (current_frame < num_frames)
                {
                    source_Rect.X += source_Rect.Width; // watch out for non-uniform sprite sheet
                }
                else
                {
                    source_Rect.X -= source_Rect.Width * (num_frames - 1); //watch out for off by 1 error
                    current_frame = 0;
                }
            }
        }
    }
}

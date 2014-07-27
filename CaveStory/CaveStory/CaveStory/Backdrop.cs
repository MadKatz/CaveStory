using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    class Backdrop
    {
        public Backdrop()
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, float layerdepth)
        {
        }
    }

    class FixedBackdrop : Backdrop
    {
        private Texture2D image;

        public FixedBackdrop(Game1 game, string path)
        {
            image = game.LoadImage(path);
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, float layerdepth)
        {
            for (int x = 0; x < graphics.PreferredBackBufferWidth; x += Constants.BACKGROUNDSIZE)
            {
                for (int y = 0; y < graphics.PreferredBackBufferHeight; y += Constants.BACKGROUNDSIZE)
                {
                    Rectangle destination_rectangle = new Rectangle(x, y, Constants.BACKGROUNDSIZE, Constants.BACKGROUNDSIZE);
                    spriteBatch.Draw(image, destination_rectangle, null, Color.White, 0.0f, new Vector2(), SpriteEffects.None, layerdepth);
                }
            }
        }
    }
}

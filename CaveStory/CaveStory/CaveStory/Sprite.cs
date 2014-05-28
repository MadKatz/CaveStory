using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    class Sprite
    {
        protected Texture2D textureImage;
        protected Rectangle source_Rect;

        public Sprite(Game1 game, String filename, int source_x, int source_y, int width, int height)
        {
            textureImage = game.LoadImage(filename);
            source_Rect.X = source_x;
            source_Rect.Y = source_y;
            source_Rect.Width = width;
            source_Rect.Height = height;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            Rectangle destination_Rect = new Rectangle(x, y, source_Rect.Width, source_Rect.Height);
            spriteBatch.Draw(textureImage, destination_Rect, source_Rect, Color.White);
        }
    }
}

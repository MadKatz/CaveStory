using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    struct Map //Should this be a class?
    {
        private Sprite[,] background_tiles;
        private Tile[,] tiles;
        private Backdrop backdrop;

        public List<CollisionTile> getCollidingTiles(Rectangle rectangle)
        {
            int first_row = rectangle.Top / Constants.TILESIZE;
            int last_row = rectangle.Bottom / Constants.TILESIZE;
            int first_col = rectangle.Left / Constants.TILESIZE;
            int last_col = rectangle.Right / Constants.TILESIZE;
            List<CollisionTile> collisionTiles = new List<CollisionTile>();
            for (int row = first_row; row <= last_row; row++)
            {
                for (int col = first_col; col <= last_col; col++)
                {
                    collisionTiles.Add(new CollisionTile(row, col, tiles[row, col].tile_type));
                }
            }
            return collisionTiles;
        }

        public void Update(GameTime gameTime)
        {
            for (int row = 0; row < tiles.GetLength(0); row++) // location of ++ should not matter where it is within a for loop.
            {
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    if (tiles[row, col].sprite != null)
                    {
                        tiles[row, col].sprite.Update(gameTime);
                    }
                }
            }
        }
        public void DrawBackGround(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, float layerdepth)
        {
            backdrop.Draw(spriteBatch, graphics, layerdepth);
        }
        public void DrawBackLayer(SpriteBatch spriteBatch, float layerdepth)
        {
            for (int row = 0; row < background_tiles.GetLength(0); row++)
            {
                for (int col = 0; col < background_tiles.GetLength(1); col++)
                {
                    if (background_tiles[row, col] != null)
                    {
                        background_tiles[row, col].Draw(spriteBatch, col * Constants.TILESIZE, row * Constants.TILESIZE, layerdepth);
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, float layerdepth)
        {
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    if (tiles[row, col].sprite != null)
                    {
                        tiles[row, col].sprite.Draw(spriteBatch, col * Constants.TILESIZE, row * Constants.TILESIZE, layerdepth);
                    }
                }
            }
        }

        public Map CreateTestMap(Game1 game)
        {
            Map map = new Map();

            map.backdrop = new FixedBackdrop(game, @"Images\bkBlue");
            const int num_rows = 15;
            const int num_cols = 20;
            map.tiles = new Tile[num_rows, num_cols];
            map.background_tiles = new Sprite[num_rows, num_cols];
            Sprite sprite = new Sprite(game, @"Images\PrtCave", Constants.TILESIZE, 0, Constants.TILESIZE, Constants.TILESIZE);
            Tile tile = new Tile(sprite, TileType.WALL_TILE);
            const int row = 11;
            for (int row_1 = 0; row_1 < row; row_1++)
            {
                map.tiles[row_1, 0] = tile;
                map.tiles[row_1, num_cols - 1] = tile;
            }
            for (int col = 0; col < num_cols; col++)
            {
                map.tiles[row, col] = tile;
            }
            map.tiles[10, 5] = tile;
            map.tiles[9, 4] = tile;
            map.tiles[8, 3] = tile;
            map.tiles[7, 2] = tile;
            map.tiles[10, 3] = tile;

            Sprite chain_top = new Sprite(game, @"Images\PrtCave", 11 * Constants.TILESIZE, 2 * Constants.TILESIZE, Constants.TILESIZE, Constants.TILESIZE);
            Sprite chain_middle = new Sprite(game, @"Images\PrtCave", 12 * Constants.TILESIZE, 2 * Constants.TILESIZE, Constants.TILESIZE, Constants.TILESIZE);
            Sprite chain_bottom = new Sprite(game, @"Images\PrtCave", 13 * Constants.TILESIZE, 2 * Constants.TILESIZE, Constants.TILESIZE, Constants.TILESIZE);

            map.background_tiles[8, 2] = chain_top;
            map.background_tiles[9, 2] = chain_middle;
            map.background_tiles[10, 2] = chain_bottom;

            return map;
        }

        public enum TileType
        {
            AIR_TILE,
            WALL_TILE,
        }

        public struct CollisionTile
        {
            public int row;
            public int col;
            public TileType tile_type;

            public CollisionTile(int row, int col, TileType tile_type)
            {
                this.row = row;
                this.col = col;
                this.tile_type = tile_type;
            }
        }

        private struct Tile
        {
            public TileType tile_type;
            public Sprite sprite;
            public Tile(Sprite sprite, TileType tile_type = TileType.AIR_TILE)
            {
                this.tile_type = tile_type;
                this.sprite = sprite;
            }
        }
    }
}
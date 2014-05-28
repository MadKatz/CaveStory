using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveStory
{
    struct Map
    {
        private Tile[,] tiles;

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

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    if (tiles[row, col].sprite != null)
                    {
                        tiles[row, col].sprite.Draw(spriteBatch, col * Constants.TILESIZE, row * Constants.TILESIZE);
                    }
                }
            }
        }

        public Map CreateTestMap(Game1 game)
        {
            Map map = new Map();

            const int num_rows = 15;
            const int num_cols = 20;
            map.tiles = new Tile[num_rows, num_cols];
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
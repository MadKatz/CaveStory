﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CaveStory
{
    class Player
    {
        private Dictionary<SpriteState, Sprite> sprite = new Dictionary<SpriteState, Sprite>();
        private HorizontalFacing horizontalFacing;
        private VerticalFacing verticalFacing;
        private Jump jump;

        //Collision Rectangle-X on Quote Texture (32x32 pixels)
        //6 pixels in
        //10 pixels down
        //20 pixels in width
        //12 pixels in height
        private Rectangle CollisionX = new Rectangle(6, 10, 20, 12);
        //Collision Rectangle-Y on Quote Texture (32x32 pixels)
        //10 pixels in
        //2 pixels down
        //12 pixels in width
        //30 pixels in height
        private Rectangle CollisionY = new Rectangle(10, 2, 12, 30);

        private int x;
        private int y;
        private float velocity_x;
        private float velocity_y;
        private float acceleration_x;
        private bool onground;

        public Player(Game1 game, int x, int y)
        {
            this.x = x;
            this.y = y;
            acceleration_x = 0.0f;
            velocity_x = 0.0f;
            velocity_y = 0.0f;
            horizontalFacing = HorizontalFacing.LEFT;
            verticalFacing = VerticalFacing.HORIZONTAL;
            onground = false;
            InitializeSprites(game);
            jump = new Jump();
        }

        public void Update(GameTime gameTime, Map map)
        {
            sprite[GetSpriteState()].Update(gameTime);
            jump.Update(gameTime);
            UpdateX(gameTime, map);
            UpdateY(gameTime, map);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite[GetSpriteState()].Draw(spriteBatch, x, y);
        }

        public void StartMovingLeft()
        {
            acceleration_x = -Constants.WALKING_ACCELERATION;
            horizontalFacing = HorizontalFacing.LEFT;
        }

        public void StartMovingRight()
        {
            acceleration_x = Constants.WALKING_ACCELERATION;
            horizontalFacing = HorizontalFacing.RIGHT;
        }

        public void StopMoving()
        {
            acceleration_x = 0.0f;
        }

        public void LookUp()
        {
            verticalFacing = VerticalFacing.UP;
        }

        public void LookDown()
        {
            verticalFacing = VerticalFacing.DOWN;
        }

        public void LookHorizontal()
        {
            verticalFacing = VerticalFacing.HORIZONTAL;
        }

        public void StartJump()
        {
            if (OnGround())
            {
                jump.Reset();
                //give ourselves an initial velocity up
                velocity_y = -Constants.JUMPSPEED;
            }
            //else if we are mid jump
            else if (velocity_y < 0.0f)
            {
                jump.Reactive();
            }
        }

        public void StopJump()
        {
            jump.Deactive();
        }

        private void InitializeSprites(Game1 game)
        {
            // for every motion_type
            // for every horizontal_facing
            // for every vertical_facing
            // create a spriteState
            // call initializeSprite with spriteState
            foreach (var motion_type in (MotionType[])Enum.GetValues(typeof(MotionType)))
            {
                foreach (var horizontal_facing in (HorizontalFacing[])Enum.GetValues(typeof(HorizontalFacing)))
                {
                    foreach (var vertical_facing in (VerticalFacing[])Enum.GetValues(typeof(VerticalFacing)))
                    {
                        InitializeSprite(game, new SpriteState(motion_type, horizontal_facing, vertical_facing));
                    }
                }
            }
        }

        private void InitializeSprite(Game1 game, SpriteState spriteState)
        {
            int source_y = spriteState.horizontal_facing == HorizontalFacing.LEFT ? Constants.CHARACTERFRAME * Constants.TILESIZE : (1 + Constants.CHARACTERFRAME) * Constants.TILESIZE;
            // CHARACTERFRAME * TITLESIZE if going left
            // otherwise (CHARACTERFRAME + 1) * TITLESIZE
            int source_x = 0;
            switch (spriteState.motion_type)
            {
                case MotionType.STANDING:
                    source_x = Constants.STANDFRAME * Constants.TILESIZE;
                    break;
                case MotionType.WALKING:
                    source_x = Constants.WALKFRAME * Constants.TILESIZE;
                    break;
                case MotionType.JUMPING:
                    source_x = Constants.JUMPFRAME * Constants.TILESIZE;
                    break;
                case MotionType.FALLING:
                    source_x = Constants.FALLFRAME * Constants.TILESIZE;
                    break;
                default:
                    break;
            }
            source_x = spriteState.verticalFacing == VerticalFacing.UP ? source_x + Constants.UPFRAMEOFFSET * Constants.TILESIZE : source_x;
            if (spriteState.motion_type == MotionType.WALKING)
            {
                sprite[spriteState] = new AnimatedSprite(game, Constants.SPRITEFILEPATH, source_x, source_y, Constants.TILESIZE, Constants.TILESIZE, Constants.FPS, Constants.NUMWALKFRAME);
            }
            else
            {
                if (spriteState.verticalFacing == VerticalFacing.DOWN)
                {
                    source_x = spriteState.motion_type == MotionType.STANDING ? Constants.BACKFRAME * Constants.TILESIZE : Constants.DOWNFRAME * Constants.TILESIZE;
                }
                sprite[spriteState] = new Sprite(game, Constants.SPRITEFILEPATH, source_x, source_y, Constants.TILESIZE, Constants.TILESIZE);
            }
        }

        private SpriteState GetSpriteState()
        {
            MotionType motion;
            if (OnGround())
            {
                motion = acceleration_x == 0.0f ? MotionType.STANDING : MotionType.WALKING;
            }
            else
            {
                motion = velocity_y < 0.0f ? MotionType.JUMPING : MotionType.FALLING;
            }
            return new SpriteState(motion, horizontalFacing, verticalFacing);
        }

        private bool OnGround()
        {
            return onground;
        }

        private Rectangle LeftCollision(int delta)
        {
            Debug.Assert(delta <= 0);
            return new Rectangle(x + CollisionX.Left + delta, y + CollisionX.Top, CollisionX.Width / 2 - delta, CollisionX.Height);
        }
        private Rectangle RightCollision(int delta)
        {
            Debug.Assert(delta >= 0);
            return new Rectangle(x + CollisionX.Left + (CollisionX.Width / 2), y + CollisionX.Top, CollisionX.Width / 2 + delta, CollisionX.Height);
        }
        private Rectangle TopCollision(int delta)
        {
            Debug.Assert(delta <= 0);
            return new Rectangle(x + CollisionY.Left, y + CollisionY.Top + delta, CollisionY.Width, CollisionY.Height / 2 - delta);
        }
        private Rectangle BottomCollision(int delta)
        {
            Debug.Assert(delta >= 0);
            return new Rectangle(x + CollisionY.Left, y + CollisionY.Top + (CollisionY.Height / 2), CollisionY.Width, CollisionY.Height / 2 + delta);
        }

        private void UpdateX(GameTime gameTime, Map map)
        {
            velocity_x += acceleration_x * gameTime.ElapsedGameTime.Milliseconds;
            if (acceleration_x < 0.0f)
            {
                velocity_x = Math.Max(velocity_x, -Constants.MAXSPEEDX);
            }
            else if (acceleration_x > 0.0f)
            {
                velocity_x = Math.Min(velocity_x, Constants.MAXSPEEDX);
            }
            else if (OnGround())
            {
                velocity_x *= Constants.SLOWDOWNFACTOR;
            }

            // Calculate delta
            int delta = (int)Math.Round(velocity_x * gameTime.ElapsedGameTime.Milliseconds);
            if (delta > 0)
            {
                // Check collision in the direction of delta
                CollisionInfo info = GetWallCollisionInfo(map, RightCollision(delta));
                // React to collision
                if (info.collided)
                {
                    x = info.col * Constants.TILESIZE - CollisionX.Right;
                    velocity_x = 0.0f;
                }
                else
                {
                    x += delta;
                }

                // Check collision in other direction
                info = GetWallCollisionInfo(map, LeftCollision(0));
                if (info.collided)
                {
                    x = info.col * Constants.TILESIZE + CollisionX.Right;
                }
            }
            else
            {
                // Check collision in the direction of delta
                CollisionInfo info = GetWallCollisionInfo(map, LeftCollision(delta));
                // React to collision
                if (info.collided)
                {
                    x = info.col * Constants.TILESIZE + CollisionX.Right;
                    velocity_x = 0.0f;
                }
                else
                {
                    x += delta;
                }

                // Check collision in other direction
                info = GetWallCollisionInfo(map, RightCollision(0));
                if (info.collided)
                {
                    x = info.col * Constants.TILESIZE - CollisionX.Right;
                }
            }
        }

        private void UpdateY(GameTime gameTime, Map map)
        {
            // Update Velocity
            if (!jump.Active())
            {
                velocity_y = Math.Min(velocity_y + Constants.GRAVITY * gameTime.ElapsedGameTime.Milliseconds, Constants.MAXSPEEDY);
            }
            // Calculate delta
            int delta = (int)Math.Round(velocity_y * gameTime.ElapsedGameTime.Milliseconds);
            if (delta > 0)
            {
                // Check collision in direction of delta
                CollisionInfo info = GetWallCollisionInfo(map, BottomCollision(delta));
                // React to collision
                if (info.collided)
                {
                    y = info.row * Constants.TILESIZE - CollisionY.Bottom;
                    velocity_y = 0.0f;
                    onground = true;
                }
                else
                {
                    y += delta;
                    onground = false;
                }

                // Check collision in other direction
                info = GetWallCollisionInfo(map, TopCollision(0));
                if (info.collided)
                {
                    y = info.row * Constants.TILESIZE + CollisionY.Height;
                }
            }
            else
            {
                CollisionInfo info = GetWallCollisionInfo(map, TopCollision(delta));
                // React to collision
                if (info.collided)
                {
                    y = info.row * Constants.TILESIZE + CollisionY.Height;
                    velocity_y = 0.0f;
                }
                else
                {
                    y += delta;
                    onground = false;
                }

                // Check collision in other direction
                info = GetWallCollisionInfo(map, BottomCollision(0));
                if (info.collided)
                {
                    y = info.row * Constants.TILESIZE - CollisionY.Bottom;
                    onground = true;
                }
            }
        }

        private CollisionInfo GetWallCollisionInfo(Map map, Rectangle rectangle)
        {
            CollisionInfo info = new CollisionInfo(false, 0, 0);
            List<Map.CollisionTile> tiles = new List<Map.CollisionTile>(map.getCollidingTiles(rectangle));
            foreach (var tile in tiles)
            {
                if (tile.tile_type == Map.TileType.WALL_TILE)
                {
                    info = new CollisionInfo(true, tile.row, tile.col);
                    break;
                }
            }
            return info;
        }

        private enum MotionType
        {
            STANDING,
            WALKING,
            JUMPING,
            FALLING,
        }
        private enum HorizontalFacing
        {
            LEFT,
            RIGHT
        }

        private enum VerticalFacing
        {
            UP,
            DOWN,
            HORIZONTAL
        }
        private struct SpriteState
        {
            public MotionType motion_type;
            public HorizontalFacing horizontal_facing;
            public VerticalFacing verticalFacing;
            public SpriteState(MotionType motion_type = MotionType.STANDING, HorizontalFacing horizontal_facing = HorizontalFacing.LEFT, VerticalFacing verticalFacing = VerticalFacing.HORIZONTAL)
            {
                this.motion_type = motion_type;
                this.horizontal_facing = horizontal_facing;
                this.verticalFacing = verticalFacing;
            }
        }

        private struct CollisionInfo
        {
            public bool collided;
            public int row;
            public int col;
            public CollisionInfo(Boolean collided, int row, int col)
            {
                this.collided = collided;
                this.row = row;
                this.col = col;
            }
        }
        // had to use class instead of struct due to "Structs cannot contain explicit parameterless constructors" error.
        private class Jump
        {
            private int time_remaining_ms;
            private bool active;

            public Jump()
            {
                time_remaining_ms = 0;
                active = false;
            }
            public void Reset()
            {
                time_remaining_ms = Constants.JUMPTIME;
                Reactive();
            }
            public void Reactive()
            {
                active = time_remaining_ms > 0;
            }
            public void Deactive()
            {
                active = false;
            }
            public bool Active()
            {
                return active;
            }
            public void Update(GameTime gameTime)
            {
                if (active)
                {
                    time_remaining_ms -= gameTime.ElapsedGameTime.Milliseconds;
                    if (time_remaining_ms <= 0)
                    {
                        active = false;
                    }
                }
            }
        }
    }
}

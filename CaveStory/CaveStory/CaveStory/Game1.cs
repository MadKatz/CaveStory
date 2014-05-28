using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CaveStory
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Input input = new Input();
        KeyboardState oldstate;
        Dictionary<String, Texture2D> spritesheets = new Dictionary<string, Texture2D>();
        List<Keys> playerKeys;
        Player player;
        Map map;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            oldstate = Keyboard.GetState();
            playerKeys = new List<Keys>()
            {
                Keys.Escape,
                Keys.Left,
                Keys.Right,
                Keys.Up,
                Keys.Down,
                Keys.Z,
            };

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            player = new Player(this, (Window.ClientBounds.Width / 2) - Constants.TILESIZE, (Window.ClientBounds.Height / 2) - Constants.TILESIZE);
            map = map.CreateTestMap(this);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            input.BeginNewFrame();
            UpdateInput();

            if (input.WasKeyPressed(Keys.Escape))
            {
                this.Exit();
            }

            // Player Horizontal movement
            if (input.IsKeyHeld(Keys.Left) && input.IsKeyHeld(Keys.Right))
            {
                player.StopMoving();
            }
            else if (input.IsKeyHeld(Keys.Left))
            {
                player.StartMovingLeft();
            }
            else if (input.IsKeyHeld(Keys.Right))
            {
                player.StartMovingRight();
            }
            else
            {
                player.StopMoving();
            }

            // Player Jump movement
            if (input.WasKeyPressed(Keys.Z))
            {
                player.StartJump();
            }
            else if (input.WasKeyReleased(Keys.Z))
            {
                player.StopJump();
            }

            // Looking
            if (input.IsKeyHeld(Keys.Up) && input.IsKeyHeld(Keys.Down))
            {
                player.LookHorizontal();
            }
            else if (input.IsKeyHeld(Keys.Up))
            {
                player.LookUp();
            }
            else if (input.IsKeyHeld(Keys.Down))
            {
                player.LookDown();
            }
            else
            {
                player.LookHorizontal();
            }


            player.Update(gameTime, map);
            map.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public Texture2D LoadImage(String filename)
        {
            if (!spritesheets.ContainsKey(filename))
                spritesheets.Add(filename, Content.Load<Texture2D>(filename));
            return spritesheets[filename];
        }

        private void UpdateInput()
        {
            KeyboardState newState = Keyboard.GetState();
            if (newState != oldstate)
            {
                foreach (Keys key in playerKeys)
                {
                    if (newState.IsKeyDown(key))
                    {
                        if (!oldstate.IsKeyDown(key))
                        {
                            //pressed
                            input.KeyDownEvent(key);
                        }
                    }
                    else if (oldstate.IsKeyDown(key))
                    {
                        // released
                        input.KeyUpEvent(key);
                    }
                }
            }
            oldstate = newState;
        }
    }
}

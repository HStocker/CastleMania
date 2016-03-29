using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CastleMania
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CastleMania : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool active = true;
        GameState state;
        Playing playing;
        Initializer init;

        public CastleMania()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 832;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            init = new Initializer(spriteBatch, this);
            state = init;
            // TODO: use this.Content to load your game content here
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            active = false;
            base.OnDeactivated(sender, args);
        }
        protected override void OnActivated(object sender, EventArgs args)
        {
            active = true;
            base.OnActivated(sender, args);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void changeGameState(string newState)
        {
            switch (newState)
            {
                case "playing":
                    {
                        playing = new Playing(spriteBatch, this);
                        state.leaving();
                        state = playing;
                        state.entering();
                        break;
                    }
                case "initialize":
                    {
                        init = new Initializer(spriteBatch, this);
                        state.leaving();
                        state = init;
                        state.entering();
                        break;
                    }
            }

        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (active)
            {
                state.update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Playing.getColor("Purple"));

            state.draw();

            base.Draw(gameTime);
        }
    }
}

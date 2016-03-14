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

namespace BreakoutHD
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Paddle paddle;
        Ammo ammo;

        Rectangle screenRectangle;
        Texture2D backgroundTexture;

        int bricksWide = 10;
        int bricksHigh = 5;
        Texture2D brickImage;
        Brick[,] bricks;

        Texture2D winjee;

        bool gamewon;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            screenRectangle = new Rectangle(
            0,
            0,
            graphics.PreferredBackBufferWidth,
            graphics.PreferredBackBufferHeight);

        }

        
        protected override void Initialize()
        {
            //FOR DEBUGGING:
            //graphics.IsFullScreen = false;

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Window.Title = "Breakout HD";
            IsMouseVisible = false;

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTexture = Content.Load<Texture2D>("background");

            Texture2D tempTexture = Content.Load<Texture2D>("paddle");
            paddle = new Paddle(tempTexture, screenRectangle);

            tempTexture = Content.Load<Texture2D>("ammo");
            ammo = new Ammo(tempTexture, screenRectangle);

            brickImage = Content.Load<Texture2D>("brick");

            winjee = Content.Load<Texture2D>("backgroundwin");

            StartGame();
        }

        private void StartGame()
        {
            paddle.SetInStartPosition();
            ammo.SetInStartPosition(paddle.GetBounds());

            bricks = new Brick[bricksWide, bricksHigh];

            for (int y = 0; y < bricksHigh; y++)
            {
                Color tint = Color.White;

                switch (y)
                {
                    case 0:
                        tint = Color.Crimson;
                        break;
                    case 1:
                        tint = Color.Gold;
                        break;
                    case 2:
                        tint = Color.LightGreen;
                        break;
                    case 3:
                        tint = Color.Cyan;
                        break;
                    case 4:
                        tint = Color.RoyalBlue;
                        break;
                }

                for (int x = 0; x < bricksWide; x++)
                {
                    bricks[x, y] = new Brick(
                        brickImage,
                        new Rectangle(
                            x * brickImage.Width,
                            y * brickImage.Height,
                            brickImage.Width,
                            brickImage.Height),
                        tint);
                }
            }
        }

        protected override void UnloadContent()
        {
        }

        
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            paddle.Update();
            ammo.Update();

            foreach (Brick brick in bricks)
            {
                brick.CheckCollision(ammo);
            }
            ammo.PaddleCollision(paddle.GetBounds());

            if (ammo.OffBottom())
                StartGame();


            if(ammo.victory())
            {
                gamewon = true;
            }

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (gamewon)
            {
                spriteBatch.Draw(winjee, screenRectangle, Color.White);
            }

            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);

            foreach (Brick brick in bricks)
                brick.Draw(spriteBatch);

            paddle.Draw(spriteBatch);
            ammo.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

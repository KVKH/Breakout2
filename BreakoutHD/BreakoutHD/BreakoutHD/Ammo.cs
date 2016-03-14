using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakoutHD

{
    class Ammo
    {
        Vector2 motion;
        Vector2 position;
        Rectangle bounds;
        
        float ammoSpeed = 4;

        const float ammoStartSpeed = 8f;

        bool collided;
        
        Texture2D texture;
        Rectangle screenBounds;

        public int brickcount = 50;

        public Rectangle Bounds
        {
            get
            {
                bounds.X = (int)position.X;
                bounds.Y = (int)position.Y;
                return bounds;
            }
        }
        
        public Ammo(Texture2D texture, Rectangle screenBounds)
        {
            bounds = new Rectangle(0, 0, texture.Width, texture.Height);
            this.texture = texture;
            this.screenBounds = screenBounds;
        }
        public void Update()
        {
            collided = false;
            position += motion * ammoSpeed;
            CheckWallCollision();
        }
        
        private void CheckWallCollision()
        {
            if (position.X < 0)
            {
                position.X = 0;
                motion.X *= -1;
            }
            if (position.X + texture.Width > screenBounds.Width)
            {
                position.X = screenBounds.Width - texture.Width;
                motion.X *= -1;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
                motion.Y *= -1;
            }
        }
        
        public void SetInStartPosition(Rectangle paddleLocation)
        {
            Random rand = new Random();

            motion = new Vector2(rand.Next(2, 6), -rand.Next(2, 6));
            motion.Normalize();

            ammoSpeed = ammoStartSpeed;

            position.Y = paddleLocation.Y - texture.Height;
            position.X = paddleLocation.X + (paddleLocation.Width - texture.Width) / 2;
        }

        public bool OffBottom()
        {
            if (position.Y > screenBounds.Height)
            {
                brickcount = 50;
                return true;
            }
            return false;
        }
        
        public bool victory()
        {
            if (brickcount == 0)
            {
                brickcount = 50;
                return true;
            }
            else
                return false;
        }

        public void PaddleCollision(Rectangle paddleLocation)
        {
            Rectangle ammoLocation = new Rectangle(
                (int)position.X,
                (int)position.Y,
                texture.Width,
                texture.Height);

            if (paddleLocation.Intersects(ammoLocation))
            {
                position.Y = paddleLocation.Y - texture.Height;
                motion.Y *= -1;
            }
        }

        public void Deflection(Brick brick)
        {
            brickcount--;

            if (!collided)
            {
                motion.Y *= -1;
                collided = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
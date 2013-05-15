using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Collision
{
    public class Sprite //maior bizu ( so que LEU) do codigo
    {
        protected Texture2D textureImage;
        public Point frameSize;
        public Point currentFrame;
        public Point sheetSize;
        int timeSinceLastFrame = 0;
        const int defaultMillisecondsPerFrame = 100000000;
        protected Vector2 speed;
        public Vector2 position;
        protected float angle;

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, Vector2 speed, float angle)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.angle = angle;
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float angle)
        {
            // TODO: Complete member initialization
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.angle = angle;
        }

        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (position.X < -frameSize.X || position.X > clientRect.Width ||
                position.Y < -frameSize.Y + 380 || position.Y > clientRect.Height)
            {
                return true;
            }
            return false;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y), Color.White, angle, new Vector2 (frameSize.X/2 , frameSize.Y/2), 1f, SpriteEffects.None, 0);
        }

        public Vector2 GetPosition
        {
            get
            {
                return position;
            }
        }

        public float GetAngle
        {
            get
            {
                return angle;
            }
        }
	}        
}

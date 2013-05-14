using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Collision
{
    class Player: Sprite
    {
        public int intSpeed = 5;
        public int hp;
        public int totalhp;
        public int xp;
        public int xpToNextLevel;
        
        public Player(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize,
            float angle, int totalhp, int hp, int xp, int xpToNextLevel)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, angle)
        {
            this.totalhp = totalhp;
            this.hp = hp;
            this.xp = xp;
            this.xpToNextLevel = xpToNextLevel;
        }

        public Vector2 direction
        {
            get 
            {
                Vector2 inputDirection = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    inputDirection.X--;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    inputDirection.X++;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    inputDirection.Y--;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    inputDirection.Y++;
                }

                return inputDirection * intSpeed;
            }
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {

            position += direction;
            MouseState currMouseState = Mouse.GetState();
            if (currMouseState.Y < position.Y)
                angle = -(float)Math.Atan(((double)currMouseState.X - (double)position.X) / ((double)currMouseState.Y - (double)position.Y));
            else if (currMouseState.Y > position.Y)           
                angle = (float)Math.PI - (float)Math.Atan(((double)currMouseState.X - (double)position.X) / ((double)currMouseState.Y - (double)position.Y));
            
        }
    }
}

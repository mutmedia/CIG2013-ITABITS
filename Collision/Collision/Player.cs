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
        public int intSpeed = 10;
        public int hp;
        public int totalhp;
        public int xp;
        public int xpToNextLevel;
        public int level = 0;
        const float PI = 3.1415f;
        SpriteManager spriteManager;
        
        public Player(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize,
            float angle, int totalhp, int hp, int xp, int xpToNextLevel, SpriteManager spriteManager)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, angle)
        {
            this.totalhp = totalhp;
            this.hp = hp;
            this.xp = xp;
            this.xpToNextLevel = xpToNextLevel;
            this.spriteManager = spriteManager;
        }

        public Vector2 direction
        {
            get 
            {
                Vector2 inputDirection = Vector2.Zero;


                // Anda na direcao da seta
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

                //Anda igual ao moller
                //if (Keyboard.GetState().IsKeyDown(Keys.W))
                //{
                //    inputDirection = new Vector2((float)Math.Cos((double)(angle - PI/2)), (float)Math.Sin((double)(angle - PI/2)));
                //}
                //if (Keyboard.GetState().IsKeyDown(Keys.S))
                //{
                //    inputDirection = new Vector2((float)Math.Cos((double)(angle + PI/2)), (float)Math.Sin((double)(angle + PI/2)));
                //}
                //if (Keyboard.GetState().IsKeyDown(Keys.A))
                //{
                //    inputDirection = new Vector2((float)Math.Cos((double)(angle)), (float)Math.Sin((double)(angle)));
                //}
                //if (Keyboard.GetState().IsKeyDown(Keys.D))
                //{
                //    inputDirection = new Vector2((float)Math.Cos((double)(angle + PI)), (float)Math.Sin((double)(angle + PI)));
                //}

                return inputDirection * intSpeed;
            }
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {

            if(spriteManager.playerCanMove)
                position += direction;
            MouseState currMouseState = Mouse.GetState();
            if (currMouseState.Y < position.Y)
                angle = -(float)Math.Atan(((double)currMouseState.X - (double)position.X) / ((double)currMouseState.Y - (double)position.Y));
            else if (currMouseState.Y > position.Y)           
                angle = (float)Math.PI - (float)Math.Atan(((double)currMouseState.X - (double)position.X) / ((double)currMouseState.Y - (double)position.Y));
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y), Color.White, angle, new Vector2(frameSize.X / 2, frameSize.Y / 2), 1f, SpriteEffects.None, 1);
        }
    }
}

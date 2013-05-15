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

namespace Collision
{
    public class Sword : Sprite
    {
        SpriteManager spriteManager;
        bool isAttacking = false;
        int attackcounter = 0;
        static int attackspeed = 20; //the lower the faster
        public int tipDamage;
        public int midDamage;

        public Sword(Texture2D textureImage, Vector2 position, Point frameSize,
            Point currentFrame, Point sheetSize, float angle, int tipDamage, int midDamage, SpriteManager spriteManager)
            : base(textureImage, position, frameSize, currentFrame, sheetSize, angle)
        {
            this.spriteManager = spriteManager;
            this.midDamage = midDamage;
            this.tipDamage = tipDamage;
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Vector2 player_position = spriteManager.GetPlayerPosition();
            float player_angle = spriteManager.GetPlayerAngle();
            MouseState mouseState = Mouse.GetState();

            position.X = player_position.X + 64 * (float)Math.Cos((double)player_angle);
            position.Y = player_position.Y + 64 * (float)Math.Sin((double)player_angle);
            if(!isAttacking)
                angle = player_angle + 0.2f;
            if (mouseState.LeftButton == ButtonState.Pressed && !isAttacking && attackcounter >= attackspeed)
            {
                isAttacking = true;
                attackcounter = 0;                
            }
            if (isAttacking)
            {
                if (attackcounter < 5)
                    angle = player_angle + 0.2f - (float)((attackcounter +1)*0.2);
                else if (attackcounter < 10)
                    angle = player_angle + 0.2f + (float)((attackcounter - 7) * 0.2);
                else
                    isAttacking = false;
            }
            attackcounter++;
        }

        public bool getIsAttacking
        {
            get
            {
                return isAttacking;
            }
        }

       
        public Vector2 collisionPoint_middle
        {
            get
            {
                return new Vector2(position.X + (float)(0.5 * (double)frameSize.Y * Math.Sin((double)angle)),
                    position.Y - (float)(0.5 * (double)frameSize.Y * Math.Cos((double)angle)));                
            }
        }

        public Vector2 collisionPoint_tip
        {
            get
            {
                return new Vector2(position.X + (float)((double)frameSize.Y * Math.Sin((double)angle)),
                    position.Y - (float)((double)frameSize.Y * Math.Cos((double)angle)));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y), Color.White, angle, new Vector2(frameSize.X / 2, frameSize.Y), 1f, SpriteEffects.None, 1);
        }
    }
}

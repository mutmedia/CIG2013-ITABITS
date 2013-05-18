using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Collision
{
    class Enemy: Sprite
    {
        SpriteManager spriteManager;
        public int totalhp;
        public int hp;
        bool isAttacking = false;
        public bool isFollowing = false;
        int attackCounter = 0;
        int attackSpeed;
        int attackRange;
        public int damage;
        public int xp;
        public int weight;
        
        
        public Enemy(Texture2D textureImage, Vector2 position, Point frameSize,
            Point currentFrame, Point sheetSize, float angle, float depth, SpriteManager spriteManager, int weight,
            Vector2 speed, int totalhp, int hp, int attackSpeed, int attackRange, int damage, int xp)
            : base(textureImage, position, frameSize, currentFrame, sheetSize, speed, angle, depth)
        {
            this.spriteManager = spriteManager;
            this.totalhp = totalhp;
            this.hp = hp;
            this.attackSpeed = attackSpeed;
            this.attackRange = attackRange;
            this.damage = damage;
            this.xp = xp;
            this.weight = weight;
        }

        public Vector2 direction
        {
            get 
            {               
                return speed;
            }
        }

        public bool GetIsAttacking
        {
            get
            {
                return isAttacking;
            }
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Vector2 player = spriteManager.GetPlayerPosition();
            float FOV = spriteManager.getFOV();
            
            //calculo do angulo de rotacao
            if (player.Y < position.Y)
                angle = -(float)Math.Atan(((double)player.X - (double)position.X) / ((double)player.Y - (double)position.Y));
            else if (player.Y > position.Y)
                angle = (float)Math.PI - (float)Math.Atan(((double)player.X - (double)position.X) / ((double)player.Y - (double)position.Y));
            
            //andar quando no campo de visao e parar quando muito perto
           float player_distance = (float)Math.Sqrt((player.X - position.X) * (player.X - position.X) + (player.Y - position.Y) * (player.Y - position.Y));


           if (player_distance < FOV)
               isFollowing = true;
            
           if (isFollowing && player_distance > spriteManager.player.frameSize.X/2 + frameSize.X/2 + attackRange/2 && !isAttacking)
           {
                if(position.Y < player.Y)
                      position.Y += direction.Y;

                if(position.Y > player.Y) 
                      position.Y -= direction.Y;
                                       
                if(position.X < player.X)
                     position.X += direction.X;

                if(position.X > player.X)
                     position.X -= direction.X;
           }

           if (player_distance < spriteManager.player.frameSize.X/2 + frameSize.X/2 + attackRange && !isAttacking && attackCounter > attackSpeed)
           {
               isAttacking = true;
               attackCounter = 0;
           }
           if (isAttacking)
           {
               if (attackCounter < 5)
               {
                   if (position.Y < player.Y)
                       position.Y += attackRange / 4;

                   if (position.Y > player.Y)
                       position.Y -= attackRange / 4;

                   if (position.X < player.X)
                       position.X += attackRange / 4;

                   if (position.X > player.X)
                       position.X -= attackRange / 4;
               }
               else if (attackCounter < 10)
               {
                   if (position.Y < player.Y)
                       position.Y -= attackRange / 4;

                   if (position.Y > player.Y)
                       position.Y += attackRange / 4;

                   if (position.X < player.X)
                       position.X -= attackRange / 4;

                   if (position.X > player.X)
                       position.X += attackRange / 4;
               }
               else
                   isAttacking = false;
           }

           attackCounter++;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y), Color.White, angle, new Vector2(frameSize.X / 2, frameSize.Y / 2), 1f, SpriteEffects.None, 0.5f);
        }
    }


}

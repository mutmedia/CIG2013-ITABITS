using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Collision
{
    public class Player: Sprite
    {
        public int intSpeed = 10;
        public int hp;
        public int totalhp;
        public int xp;
        public int xpToNextLevel;
        public int level = 0;
        public Rectangle rectangle;
        const float PI = 3.1415f;
        SpriteManager spriteManager;
        MapManager mapManager;
        public bool changedMap = false;
        public bool changedMap_Up = false;
        public bool changedMap_Down = false;
        public bool changedMap_Right = false;
        public bool changedMap_Left = false;
        
        
        public Player(Texture2D textureImage, Vector2 position, Point frameSize,
            Point currentFrame, Point sheetSize, float angle, int totalhp, int hp,
            int xp, int xpToNextLevel, float depth, SpriteManager spriteManager, MapManager mapManager)
            : base(textureImage, position, frameSize, currentFrame, sheetSize, angle, depth)
        {
            this.totalhp = totalhp;
            this.hp = hp;
            this.xp = xp;
            this.xpToNextLevel = xpToNextLevel;
            this.spriteManager = spriteManager;
            this.mapManager = mapManager;
        }

        

        public bool canMoveUp
        {
            get
            {
                foreach ( Rectangle r in mapManager.currentMap.wallCollisionRectangle)
                {
                    if (topCollisionRectangle.Intersects(r))
                        return false;
                }
                return true;
            }
        }

        public bool canMoveDown
        {
            get
            {
                foreach (Rectangle r in mapManager.currentMap.wallCollisionRectangle)
                {
                    if (downCollisionRectangle.Intersects(r))
                        return false;
                }
                return true;
            }
        }

        public bool canMoveLeft
        {
            get
            {
                foreach (Rectangle r in mapManager.currentMap.wallCollisionRectangle)
                {
                    if (leftCollisionRectangle.Intersects(r))
                        return false;
                }
                return true;
            }
        }

        public bool canMoveRight
        {
            get
            {
                foreach (Rectangle r in mapManager.currentMap.wallCollisionRectangle)
                {
                    if (rightCollisionRectangle.Intersects(r))
                        return false;
                }
                return true;
            }
        }

        public Vector2 direction
        {
            get 
            {
                Vector2 inputDirection = Vector2.Zero;


                // Anda na direcao da seta
                if (Keyboard.GetState().IsKeyDown(Keys.A) && canMoveLeft)
                {
                    inputDirection.X--;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D) &&  canMoveRight)
                {
                    inputDirection.X++;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) && canMoveUp)
                {
                    inputDirection.Y--;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) && canMoveDown)
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

        public void teleport()
        {
            foreach (Rectangle r in mapManager.currentMap.portalRectangle)
            {
                if (r.Intersects(topCollisionRectangle))
                {
                    position = new Vector2(position.X, (mapManager.currentMap.width - 2) * mapManager.currentMap.portalTile.Height + 140);
                    changedMap = true;
                    changedMap_Up = true;
                }
                if (r.Intersects(downCollisionRectangle))
                {
                    position = new Vector2(position.X, 2 * mapManager.currentMap.portalTile.Height + 140);
                    changedMap = true;
                    changedMap_Down = true;
                }
                if (r.Intersects(leftCollisionRectangle))
                {
                    position = new Vector2((mapManager.currentMap.height - 2) * mapManager.currentMap.portalTile.Height + 64, position.Y);
                    changedMap = true;
                    changedMap_Left = true;

                }
                if (r.Intersects(rightCollisionRectangle))
                {
                    position = new Vector2(2 * mapManager.currentMap.portalTile.Height + 64, position.Y);
                    changedMap = true;
                    changedMap_Right = true;

                }
            }
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
                        
            changedMap = false;
            changedMap_Up = false;
            changedMap_Down = false;
            changedMap_Right = false;
            changedMap_Left = false;
            position += direction;
            MouseState currMouseState = Mouse.GetState();
            if (currMouseState.Y < position.Y)
                angle = -(float)Math.Atan(((double)currMouseState.X - (double)position.X) / ((double)currMouseState.Y - (double)position.Y));
            else if (currMouseState.Y > position.Y)           
                angle = (float)Math.PI - (float)Math.Atan(((double)currMouseState.X - (double)position.X) / ((double)currMouseState.Y - (double)position.Y));
            teleport();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y), Color.White, angle, new Vector2(frameSize.X / 2, frameSize.Y / 2), 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(textureImage, topCollisionRectangle, Color.Black);
        }
    }
}

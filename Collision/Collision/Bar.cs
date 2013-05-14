using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Collision
{
    class Bar
    {
        Texture2D barTexture;
        Texture2D backgroundTexture;
        public Vector2 position;
        public float barPercentage;
        int height;
        int width;
        Color barColor;
        Color backgroundColor;
        float depth;

        public Bar(Vector2 position, int height, int width,
            float barPercentage, Color barColor, Color backgroundColor, float depth)
        {
            this.position = position;
            this.height = height;
            this.width = width;
            this.barPercentage = barPercentage;
            this.barColor = barColor;
            this.backgroundColor = backgroundColor;
            this.depth = depth;
        }


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (barTexture == null || backgroundTexture == null)
            {
                barTexture = new Texture2D(graphicsDevice, 1, 1);
                barTexture.SetData(new[] { barColor }); //nao entendi
                backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
                backgroundTexture.SetData(new[] { backgroundColor });
            }
            spriteBatch.Draw(barTexture, new Rectangle((int)(position.X), (int)(position.Y), (int)(barPercentage*width), (int)(height)), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, depth -0.5f);
            spriteBatch.Draw(backgroundTexture, new Rectangle((int)(position.X), (int)(position.Y), (int)(width), (int)(height)),  null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None,depth -1);
        }
    }
}

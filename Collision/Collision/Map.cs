using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;


namespace Collision
{
    class Map
    {
        string textPath;
        char[][] tiles;
        int height;
        int width;
        Texture2D wallTile;
        Texture2D groundTile;
        Texture2D portalTile;
        const char wallChar = '#';
        const char groundChar = '.';
        const char portalChar = '@';
        public Rectangle wallCollisionRectangle = new Rectangle();
        public Rectangle portalRectangle = new Rectangle();
        

        public Map(string textPath, Texture2D wallTile, Texture2D groundTile, Texture2D portalTile)
        {
            this.textPath = textPath;
            this.wallTile = wallTile;
            this.groundTile = groundTile;
            this.portalTile = portalTile;
        }
        
        public void Initialize()
        {
            using (StreamReader reader = new StreamReader(textPath))
            {
                height = int.Parse(reader.ReadLine());
                width = int.Parse(reader.ReadLine());
                tiles = new char[width][];

                for (int i = 0; i < width; i++)
                    tiles[i] = reader.ReadLine().ToCharArray();

                for (int i = 0; i < width; i++)
			    {
			        for (int j = 0; j < height; j++)
			        {
			        
			        }
			    } 
            }
        } 

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < width; i++)
			{
			    for (int j = 0; j < height; j++)
			    {
                    if (tiles[i][j] == groundChar)
                        spriteBatch.Draw(groundTile, new Vector2(64 +groundTile.Width * j,140 + groundTile.Height * i), Color.White);
                    if (tiles[i][j] == wallChar)
                        spriteBatch.Draw(wallTile, new Vector2(64 + wallTile.Width * j, 140 + wallTile.Height * i), Color.White);
                    if (tiles[i][j] == portalChar)
                        spriteBatch.Draw(portalTile, new Vector2(64 + portalTile.Width * j, 140 + portalTile.Height * i), Color.White);
			    }
			} 
        }




    }
}

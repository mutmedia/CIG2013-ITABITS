using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;


namespace Collision
{
    public class Map
    {
        string textPath;
        char[][] tiles;
        public int height;
        public int width;
        public Texture2D wallTile;
        public Texture2D groundTile;
        public Texture2D portalTile;
        const char wallChar = '#';
        const char groundChar = '.';
        const char portalChar = '@';
        public List<Rectangle> wallCollisionRectangle =new List<Rectangle>();
        public List<Rectangle> portalRectangle = new List<Rectangle>();
        public List<Rectangle> groundRectangle = new List<Rectangle>();
        public const int VERTICAL_REPOSITION = 140;
        public const int HORIZONTAL_REPOSITION = 64;
        bool started = true;
        

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
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (tiles[i][j] == wallChar)
                        wallCollisionRectangle.Add(new Rectangle(HORIZONTAL_REPOSITION + wallTile.Width * j, VERTICAL_REPOSITION + wallTile.Height * i, wallTile.Width, wallTile.Height));

                    if (tiles[i][j] == portalChar)
                        portalRectangle.Add(new Rectangle(HORIZONTAL_REPOSITION + portalTile.Width * j, VERTICAL_REPOSITION + portalTile.Height * i, portalTile.Width, portalTile.Height));

                    if (tiles[i][j] == groundChar)
                        groundRectangle.Add(new Rectangle(HORIZONTAL_REPOSITION + groundTile.Width * j, VERTICAL_REPOSITION + groundTile.Height * i, groundTile.Width, groundTile.Height));
                }
            } 
        } 

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < width; i++)
			{
			    for (int j = 0; j < height; j++)
			    {
                    //if (tiles[i][j] == groundChar)
                    //    spriteBatch.Draw(groundTile, new Vector2(HORIZONTAL_REPOSITION + groundTile.Width * j, VERTICAL_REPOSITION + groundTile.Height * i), Color.White);
                    //if (tiles[i][j] == wallChar)
                    //    spriteBatch.Draw(wallTile, new Vector2(HORIZONTAL_REPOSITION + wallTile.Width * j, VERTICAL_REPOSITION + wallTile.Height * i), Color.White);
                    //if (tiles[i][j] == portalChar)
                    //    spriteBatch.Draw(portalTile, new Vector2(HORIZONTAL_REPOSITION + portalTile.Width * j, VERTICAL_REPOSITION + portalTile.Height * i), Color.White);
                    if (started)
                    {
                        if (tiles[i][j] == wallChar)
                            wallCollisionRectangle.Add(new Rectangle(HORIZONTAL_REPOSITION + wallTile.Width * j, VERTICAL_REPOSITION + wallTile.Height * i, wallTile.Width, wallTile.Height));

                        if (tiles[i][j] == portalChar)
                            portalRectangle.Add(new Rectangle(HORIZONTAL_REPOSITION + portalTile.Width * j, VERTICAL_REPOSITION + portalTile.Height * i, portalTile.Width, portalTile.Height));

                        if (tiles[i][j] == groundChar)
                            groundRectangle.Add(new Rectangle(HORIZONTAL_REPOSITION + groundTile.Width * j, VERTICAL_REPOSITION + groundTile.Height * i, groundTile.Width, groundTile.Height));
                        started = false;
                    }
                }
			}

            foreach (Rectangle r in groundRectangle)
                spriteBatch.Draw(groundTile, r, Color.White);
            foreach (Rectangle r in wallCollisionRectangle)
                spriteBatch.Draw(wallTile, r, Color.White);
            foreach (Rectangle r in portalRectangle)
                spriteBatch.Draw(portalTile, r, Color.White);

        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Collision
{
    public class MiniMap
    {
        MapManager mapManager;
        Texture2D minimapTexture;
        Texture2D pixel;
        public bool[,] boolmaze;
        List<Vector3> visibleMap = new List<Vector3>();
        int textureFrame;

        public MiniMap(Texture2D minimapTexture, MapManager mapManager)
        {
            this.minimapTexture = minimapTexture;
            this.mapManager = mapManager;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            boolmaze = new bool[mapManager.floorSize, mapManager.floorSize];

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.Black });

            for (int i = 0; i < mapManager.floorSize; i++)
                for (int j = 0; j < mapManager.floorSize; j++)
                {
                    if (mapManager.mapMatrix[i, j] == -11)
                        boolmaze[i, j] = true;
                    else
                        boolmaze[i, j] = false;
                }
            visibleMap.Clear();
            visibleMap.Add(new Vector3(1700 + mapManager.currentRoom.X * minimapTexture.Width / 15, 800 + mapManager.currentRoom.Y * minimapTexture.Height, textureFrame));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, new Vector2(1700, 800), new Rectangle(1700, 800, minimapTexture.Width * mapManager.floorSize/15, minimapTexture.Height * mapManager.floorSize), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            foreach(Vector3 item in visibleMap)
                spriteBatch.Draw(minimapTexture, new Vector2(item.X, item.Y), new Rectangle((int)(item.Z * minimapTexture.Width / 15), 0, minimapTexture.Width / 15, minimapTexture.Height), Color.Gray, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(minimapTexture, new Vector2(1700 + mapManager.currentRoom.X * minimapTexture.Width/15, 800 + mapManager.currentRoom.Y * minimapTexture.Height), new Rectangle(textureFrame * minimapTexture.Width / 15, 0, minimapTexture.Width / 15, minimapTexture.Height), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public void Update()
        {
            if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 2)
                textureFrame = 12;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 3)
                textureFrame = 14;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 5)
                textureFrame = 11;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 6)
                textureFrame = 3;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 7)
                textureFrame = 13;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 6)
                textureFrame = 3;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 10)
                textureFrame = 1;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 14)
                textureFrame = 4;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 15)
                textureFrame = 5;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 21)
                textureFrame = 2;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 30)
                textureFrame = 7;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 35)
                textureFrame = 6;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 42)
                textureFrame = 9;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 70)
                textureFrame = 8;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 105)
                textureFrame = 10;
            else if (mapManager.mapMatrix[mapManager.currentRoom.Y, mapManager.currentRoom.X] == 210)
                textureFrame = 0;
                
                visibleMap.Add(new Vector3(1700 + mapManager.currentRoom.X * minimapTexture.Width/15, 800 + mapManager.currentRoom.Y * minimapTexture.Height, textureFrame));
        }
    }
}

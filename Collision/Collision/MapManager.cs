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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MapManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public SpriteBatch spriteBatch;
        Map map2;
        Map map3;
        Map map5;
        Map map6;
        Map map7;
        Map map10;
        Map map14;
        Map map15;
        Map map21;
        Map map30;
        Map map35;
        Map map42;
        Map map70;
        Map map105;
        Map map210;
        public Map currentMap;
        public MiniMap miniMap;
        public int[,] mapMatrix;
        public Point currentRoom;
        public int floorSize = 3;
        public bool mapCleared = true;

        public MapManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }


        public void updateMapClearance()
        {
            mapCleared = true;
            for (int i = 0; i < floorSize; i++)
            {
                for (int j = 0; j < floorSize; j++)
                {
                    mapCleared = mapCleared && miniMap.boolmaze[i,j];
                }
            }

            if (mapCleared)
            {
                floorSize++;
                generateMapMatrix();
                if (floorSize % 2 == 0)
                {
                    currentRoom = new Point(floorSize / 2, floorSize / 2);
                }
                else
                {
                    currentRoom = new Point((floorSize - 1) / 2, (floorSize - 1) / 2);
                }
                currentMap = map210;
                ((Game1)Game).spriteManager.player.changedMap = true;
                miniMap.Initialize(((Game1)Game).GraphicsDevice);
            }

        }
        

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            GraphicsDevice graphicsDevice = new GraphicsDevice();
            
            Texture2D groundTile = Game.Content.Load<Texture2D>(@"Tiles/groundTile");
            Texture2D wallTile = Game.Content.Load<Texture2D>(@"Tiles/wallTile");
            Texture2D portalTile = Game.Content.Load<Texture2D>(@"Tiles/portalTile");
            Texture2D miniMapTexture = Game.Content.Load<Texture2D>(@"Images/UI/minimap");

            map2 = new Map("Content\\maps\\map2.txt", wallTile, groundTile, portalTile);
            map2.Initialize();
            map3 = new Map("Content\\maps\\map3.txt", wallTile, groundTile, portalTile);
            map3.Initialize();
            map5 = new Map("Content\\maps\\map5.txt", wallTile, groundTile, portalTile);
            map5.Initialize();
            map6 = new Map("Content\\maps\\map6.txt", wallTile, groundTile, portalTile);
            map6.Initialize();
            map7 = new Map("Content\\maps\\map7.txt", wallTile, groundTile, portalTile);
            map7.Initialize();
            map10 = new Map("Content\\maps\\map10.txt", wallTile, groundTile, portalTile);
            map10.Initialize();
            map14 = new Map("Content\\maps\\map14.txt", wallTile, groundTile, portalTile);
            map14.Initialize();
            map15 = new Map("Content\\maps\\map15.txt", wallTile, groundTile, portalTile);
            map15.Initialize();
            map21 = new Map("Content\\maps\\map21.txt", wallTile, groundTile, portalTile);
            map21.Initialize();
            map30 = new Map("Content\\maps\\map30.txt", wallTile, groundTile, portalTile);
            map30.Initialize();
            map35 = new Map("Content\\maps\\map35.txt", wallTile, groundTile, portalTile);
            map35.Initialize();
            map42 = new Map("Content\\maps\\map42.txt", wallTile, groundTile, portalTile);
            map42.Initialize();
            map70 = new Map("Content\\maps\\map70.txt", wallTile, groundTile, portalTile);
            map70.Initialize();
            map105 = new Map("Content\\maps\\map105.txt", wallTile, groundTile, portalTile);
            map105.Initialize();
            map210 = new Map("Content\\maps\\map210.txt", wallTile, groundTile, portalTile);
            map210.Initialize();

            generateMapMatrix();

            miniMap = new MiniMap(miniMapTexture, this);

            miniMap.Initialize(Game.GraphicsDevice);
        }

        public void generateMapMatrix()
        {
            /*       2
             *     7   3
             *       5 
             *       
             * 
             * -1 askmap
             * -11 NULL
             */

            bool askmap = true;
            int[] vetorengrenado = new int[4];
            mapMatrix = new int[floorSize, floorSize];
            for (int i = 0; i < floorSize; i++)
            {
                for (int j = 0; j < floorSize; j++)
                {
                    mapMatrix[i, j] = -11;
                }
            }

            if (floorSize % 2 == 0)
            {
                mapMatrix[floorSize / 2, floorSize / 2] = 210;
                mapMatrix[floorSize / 2 - 1, floorSize / 2] = -1;
                mapMatrix[floorSize / 2, floorSize / 2 - 1] = -1;
                mapMatrix[floorSize / 2 + 1, floorSize / 2] = -1;
                mapMatrix[floorSize / 2, floorSize / 2 + 1] = -1;
            }
            else
            {
                mapMatrix[(floorSize - 1) / 2, (floorSize - 1) / 2] = 210;
                mapMatrix[(floorSize - 1) / 2 - 1, (floorSize - 1) / 2] = -1;
                mapMatrix[(floorSize - 1) / 2, (floorSize - 1) / 2 - 1] = -1;
                mapMatrix[(floorSize - 1) / 2 + 1, (floorSize - 1) / 2] = -1;
                mapMatrix[(floorSize - 1) / 2, (floorSize - 1) / 2 + 1] = -1;
            }

            for (int i = 0; i < 4; i++)
            {
                vetorengrenado[i] = 1;
            }

            while (askmap)
            {
                for (int i = 0; i < floorSize; i++)
                {
                    for (int j = 0; j < floorSize; j++)
                    {


                        if (mapMatrix[i, j] == -1)
                        {
                            mapMatrix[i, j] = 1;
                            if (j < floorSize - 1)
                                if (mapMatrix[i, j + 1] % 7 == 0)
                                    vetorengrenado[1] = 3;
                            if (i < floorSize - 1)
                                if (mapMatrix[i + 1, j] % 2 == 0)
                                    vetorengrenado[2] = 5;
                            if (j > 0)
                                if (mapMatrix[i, j - 1] % 3 == 0)
                                    vetorengrenado[3] = 7;
                            if (i > 0)
                                if (mapMatrix[i - 1, j] % 5 == 0)
                                    vetorengrenado[0] = 2;

                            if (j < floorSize - 1)
                                if (mapMatrix[i, j + 1] < 0 && ((Game1)Game).rnd.Next(2) == 1)
                                {
                                    vetorengrenado[1] = 3;
                                    mapMatrix[i, j + 1] = -1;
                                }

                            if (j > 0)
                                if (mapMatrix[i, j - 1] < 0 && ((Game1)Game).rnd.Next(2) == 1)
                                {
                                    vetorengrenado[3] = 7;
                                    mapMatrix[i, j - 1] = -1;
                                }

                            if (i > 0)
                                if (mapMatrix[i - 1, j] < 0 && ((Game1)Game).rnd.Next(2) == 1)
                                {
                                    vetorengrenado[0] = 2;
                                    mapMatrix[i - 1, j] = -1;
                                }

                            if (i < floorSize - 1)
                                if (mapMatrix[i + 1, j] < 0 && ((Game1)Game).rnd.Next(2) == 1)
                                {
                                    vetorengrenado[2] = 5;
                                    mapMatrix[i + 1, j] = -1;
                                }

                            for (int k = 0; k < 4; k++)
                                mapMatrix[i, j] *= vetorengrenado[k];

                            if (i == 0 && mapMatrix[i, j] % 2 == 0)
                                mapMatrix[i, j] /= 2;
                            if (i == floorSize - 1 && mapMatrix[i, j] % 5 == 0)
                                mapMatrix[i, j] /= 5;
                            if (j == 0 && mapMatrix[i, j] % 7 == 0)
                                mapMatrix[i, j] /= 7;
                            if (j == floorSize - 1 && mapMatrix[i, j] % 3 == 0)
                                mapMatrix[i, j] /= 3;

                            for (int a = 0; a < 4; a++)
                                vetorengrenado[a] = 1;
                        }
                    }
                }



                askmap = false;

                for (int i = 0; i < floorSize; i++)
                {
                    for (int j = 0; j < floorSize; j++)
                    {
                        if (mapMatrix[i, j] == -1)
                            askmap = true;
                    }
                }
            }

            if (floorSize % 2 == 0)
            {
                currentRoom = new Point(floorSize / 2, floorSize / 2);
            }
            else
            {
                currentRoom = new Point((floorSize - 1) / 2, (floorSize - 1) / 2);
            }

            currentMap = map210;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            if(((Game1)Game).spriteManager.player.changedMap_Up)
                currentRoom.Y--;
            
            if(((Game1)Game).spriteManager.player.changedMap_Down)
                currentRoom.Y++;

            if(((Game1)Game).spriteManager.player.changedMap_Right)
                currentRoom.X++;

            if (((Game1)Game).spriteManager.player.changedMap_Left)
                currentRoom.X--;

            if(((Game1)Game).spriteManager.player.changedMap)
            {
                if (mapMatrix[currentRoom.Y, currentRoom.X] == 2)
                    currentMap = map2;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 3)
                    currentMap = map3;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 5)
                    currentMap = map5;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 6)
                    currentMap = map6;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 7)
                    currentMap = map7;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 6)
                    currentMap = map6;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 10)
                    currentMap = map10;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 14)
                    currentMap = map14;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 15)
                    currentMap = map15;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 21)
                    currentMap = map21;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 30)
                    currentMap = map30;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 35)
                    currentMap = map35;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 42)
                    currentMap = map42;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 70)
                    currentMap = map70;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 105)
                    currentMap = map105;
                else if (mapMatrix[currentRoom.Y, currentRoom.X] == 210)
                    currentMap = map210;
            }

            if (((Game1)Game).spriteManager.player.changedMap)
            {
                miniMap.Update();
            }

            updateMapClearance();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            currentMap.Draw(gameTime, spriteBatch);

            if (miniMap.boolmaze[currentRoom.Y, currentRoom.X] && !((Game1)Game).spriteManager.startLevelUpAnimation)
            {
                currentMap.portalAngle -= 0.25f;
            }

            miniMap.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}

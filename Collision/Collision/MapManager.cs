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
        Map testMap;
        public Map currentMap;

        public MapManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            Texture2D groundTile = Game.Content.Load<Texture2D>(@"Tiles/groundTile");
            Texture2D wallTile = Game.Content.Load<Texture2D>(@"Tiles/wallTile");
            Texture2D portalTile = Game.Content.Load<Texture2D>(@"Tiles/portalTile");

            testMap = new Map("C:\\Users\\Gustavo\\Documents\\GitHub\\CIG2013\\Collision\\Collision\\Content\\Maps\\maptest.txt", wallTile, groundTile, portalTile);
            testMap.Initialize();
            currentMap = testMap;
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

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            
            currentMap.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}

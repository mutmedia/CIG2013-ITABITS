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
    public class MenuManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public SpriteBatch spriteBatch;

        Button playNowButton;
        Sprite mainMenu;
        Texture2D blackPixel;
        

        int time = 1;
        int frameTime = 10;

        public MenuManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            blackPixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            blackPixel.SetData(new[] { Color.Black });

            mainMenu = new Sprite(Game.Content.Load<Texture2D>(@"Images/UI/Main Menu"),
                new Vector2(960, 540), new Point(1920, 1080), new Point(0,0),
                new Point(1, 2), 0.0f);
            playNowButton = new Button(Game.Content.Load<Texture2D>(@"Images/UI/Playnow"),
                new Vector2(960, 800), new Point(1147, 411), new Point(0, 0),
                new Point(2, 4), 0.0f);


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
            if (((Game1)Game).menuActive)
            {
                if (playNowButton.buttonPressed)
                    ((Game1)Game).menuActive = false;
            }
            if (((Game1)Game).gameOver)
            {
                mainMenu.currentFrame.Y = 1;
                playNowButton.currentFrame.X = 1;
                if (playNowButton.buttonPressed)
                    ((Game1)Game).gameOver = false;
            }

            if (!playNowButton.buttonHovered && time % frameTime == frameTime - 1)
            {
                playNowButton.currentFrame.Y = ((playNowButton.currentFrame.Y + 1) % 4);
                time = 0;
            }


            time++;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (((Game1)Game).menuActive || ((Game1)Game).gameOver)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                mainMenu.Draw(gameTime, spriteBatch);
                playNowButton.Draw(gameTime, spriteBatch);

                base.Draw(gameTime);
                spriteBatch.End();
            }

        }
    }
}
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

        Sprite playNowButton;
        Sprite mainMenu;
        Texture2D blackPixel;
        Rectangle mousePosition;
        Rectangle playNowButtonPosition;

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

            mainMenu = new Sprite(Game.Content.Load<Texture2D>(@"Images/Main Menu"),
                new Vector2(960, 540), new Point(1920, 1080), 0, new Point(0,0),
                new Point(1, 2), 0.0f);
            playNowButton = new Sprite(Game.Content.Load<Texture2D>(@"Images/Playnow"),
                new Vector2(960, 800), new Point(1147, 411), 0, new Point(0, 0),
                new Point(2, 4), 0.0f);
            playNowButtonPosition = new Rectangle(386, 595, 1148, 412);


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
            MouseState mouseState = Mouse.GetState();
            mousePosition = new Rectangle(mouseState.X, mouseState.Y, 5, 5);

            if (((Game1)Game).menuActive)
            {
                if (mousePosition.Intersects(playNowButtonPosition) && mouseState.LeftButton == ButtonState.Pressed)
                    ((Game1)Game).menuActive = false;
            }
            if (((Game1)Game).gameOver)
            {
                mainMenu.currentFrame.Y = 1;
                playNowButton.currentFrame.X = 1;
                if (mousePosition.Intersects(playNowButtonPosition) && mouseState.LeftButton == ButtonState.Pressed)
                    ((Game1)Game).gameOver = false;
            }

            if (!mousePosition.Intersects(playNowButtonPosition) && time % frameTime == frameTime - 1)
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
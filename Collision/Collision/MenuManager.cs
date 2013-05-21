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

        public bool pauseMenuActive = false;
        public bool statMenuActive = false;

        Button playNowButton;
        Sprite mainMenu;
        Texture2D blackPixel;
        Button addStrButton;
        Button addAgiButton;
        Button addVitButton;
        Button resumeButton;
        Button exitButton;
        Button statsButton;
        int str = 0;
        int agi = 0;
        int vit = 0;

        public Sprite statSpriteDecimal;
        public Sprite statSpriteUnit;
        public Sprite strSpriteDecimal;
        public Sprite strSpriteUnit;
        public Sprite vitSpriteDecimal;
        public Sprite vitSpriteUnit;
        public Sprite agiSpriteDecimal;
        public Sprite agiSpriteUnit;

        int time = 1;
        int frameTime = 10;

        public MenuManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public void UpdateNumericSprite()
        {
            statSpriteUnit.currentFrame.X = ((Game1)Game).spriteManager.player.statsToAdd % 10;
            statSpriteDecimal.currentFrame.X = (int)(((Game1)Game).spriteManager.player.statsToAdd % 100 / 10);
            strSpriteUnit.currentFrame.X = str % 10;
            strSpriteDecimal.currentFrame.X = (int)(str % 100 / 10);
            agiSpriteUnit.currentFrame.X = agi % 10;
            agiSpriteDecimal.currentFrame.X = (int)(agi % 100 / 10);
            vitSpriteUnit.currentFrame.X = vit % 10;
            vitSpriteDecimal.currentFrame.X = (int)(vit % 100 / 10);
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            blackPixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            blackPixel.SetData(new[] { Color.Black });

            Texture2D number_texture = Game.Content.Load<Texture2D>(@"Images/UI/numberz");
            Texture2D exitButton_texture = Game.Content.Load<Texture2D>(@"Images/UI/exitbutton");
            Texture2D statsButton_texture = Game.Content.Load<Texture2D>(@"Images/UI/statsbutton");
            Texture2D resumeButton_texture = Game.Content.Load<Texture2D>(@"Images/UI/resumebutton");

            mainMenu = new Sprite(Game.Content.Load<Texture2D>(@"Images/UI/Main Menu"),
                new Vector2(960, 540), new Point(1920, 1080), new Point(0,0),
                new Point(2, 3), 0.0f, 1f);
            playNowButton = new Button(Game.Content.Load<Texture2D>(@"Images/UI/Playnow"),
                new Vector2(960, 800), new Point(1147, 411), new Point(0, 0),
                new Point(2, 4), 0.0f, 1f);
            addStrButton = new Button(Game.Content.Load<Texture2D>(@"Images/UI/addLvl"),
                new Vector2(1220, 360), new Point(146, 106), new Point(0, 0),
                new Point(1, 3), 0.0f, 1f);
            addAgiButton = new Button(Game.Content.Load<Texture2D>(@"Images/UI/addLvl"),
                new Vector2(1220, 530), new Point(146, 106), new Point(0, 0),
                new Point(1, 3), 0.0f, 1f);
            addVitButton = new Button(Game.Content.Load<Texture2D>(@"Images/UI/addLvl"),
                new Vector2(1220, 700), new Point(146, 106), new Point(0, 0),
                new Point(1, 3), 0.0f, 1f);
            exitButton = new Button(exitButton_texture, new Vector2(1000, 900), new Point(exitButton_texture.Width / 2 -1, exitButton_texture.Height),
                new Point(0, 0), new Point(1, 2), 0.0f, 1f);
            resumeButton = new Button(resumeButton_texture, new Vector2(1000, 360), new Point(resumeButton_texture.Width / 2 -1, resumeButton_texture.Height),
                new Point(0, 0), new Point(1, 2), 0.0f, 1f);
            statsButton = new Button(statsButton_texture, new Vector2(1000, 630), new Point(statsButton_texture.Width / 2 -1, statsButton_texture.Height),
                new Point(0, 0), new Point(1, 2), 0.0f, 1f);

            statSpriteUnit = new Sprite(number_texture, new Vector2(1200, 890), new Point(number_texture.Width / 10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);
            statSpriteDecimal = new Sprite(number_texture, new Vector2(1150, 890), new Point(number_texture.Width / 10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);
            strSpriteUnit = new Sprite(number_texture, new Vector2(1000, 360), new Point(number_texture.Width / 10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);
            strSpriteDecimal = new Sprite(number_texture, new Vector2(950, 360), new Point(number_texture.Width / 10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);
            vitSpriteUnit = new Sprite(number_texture, new Vector2(1000, 700), new Point(number_texture.Width / 10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);
            vitSpriteDecimal = new Sprite(number_texture, new Vector2(950, 700), new Point(number_texture.Width / 10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);
            agiSpriteUnit = new Sprite(number_texture, new Vector2(1000, 530), new Point(number_texture.Width / 10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);
            agiSpriteDecimal = new Sprite(number_texture, new Vector2(950, 530), new Point(number_texture.Width / 10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);

        
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
            if (((Game1)Game).menuActive && !pauseMenuActive)
            {
                if (playNowButton.buttonPressed)
                    ((Game1)Game).menuActive = false;
                if (!playNowButton.buttonHovered && time % frameTime == frameTime - 1)
                {
                    playNowButton.currentFrame.Y = ((playNowButton.currentFrame.Y + 1) % 4);
                    time = 0;
                }
            }
            if (((Game1)Game).gameOver)
            {
                mainMenu.currentFrame = new Point(0, 1);
                playNowButton.currentFrame.X = 1;
                if (!playNowButton.buttonHovered && time % frameTime == frameTime - 1)
                {
                    playNowButton.currentFrame.Y = ((playNowButton.currentFrame.Y + 1) % 4);
                    time = 0;
                }
                if (playNowButton.buttonPressed)
                    ((Game1)Game).gameOver = false;
            }
            if (((Game1)Game).spriteManager.playerLeveledUp && ((Game1)Game).mapManager.miniMap.boolmaze[((Game1)Game).mapManager.currentRoom.X, ((Game1)Game).mapManager.currentRoom.Y])
            {
                statMenuActive = true;
            }
            
            if(statMenuActive)
            {
                mainMenu.currentFrame = new Point(0, 2);
                UpdateNumericSprite();
                if (addAgiButton.buttonHovered && agi < 33)
                    addAgiButton.currentFrame.Y = 1;
                else if (agi == 33)
                    addAgiButton.currentFrame.Y = 2;
                else
                    addAgiButton.currentFrame.Y = 0;
                if (addAgiButton.buttonPressed && agi < 33 && ((Game1)Game).spriteManager.player.statsToAdd > 0)
                {
                    ((Game1)Game).spriteManager.player.statsToAdd--;
                    agi++;
                    ((Game1)Game).spriteManager.player.intSpeed++;
                    ((Game1)Game).spriteManager.galho.attackspeed--;
                    ((Game1)Game).spriteManager.cano.attackspeed--;
                    ((Game1)Game).spriteManager.crayon.attackspeed--;
                    ((Game1)Game).spriteManager.peixe.attackspeed--;
                    ((Game1)Game).spriteManager.serra.attackspeed--;
                    ((Game1)Game).spriteManager.piroca.attackspeed--;
                    ((Game1)Game).spriteManager.jedi.attackspeed--;
                    ((Game1)Game).spriteManager.currentSword.attackspeed--;
                }

                if (addStrButton.buttonHovered && str < 33)
                    addStrButton.currentFrame.Y = 1;
                else if (str == 33)
                    addStrButton.currentFrame.Y = 2;
                else
                    addStrButton.currentFrame.Y = 0;
                if (addStrButton.buttonPressed && str < 33 && ((Game1)Game).spriteManager.player.statsToAdd > 0)
                {
                    ((Game1)Game).spriteManager.player.statsToAdd--;
                    str++;
                    ((Game1)Game).spriteManager.player.damage++;
                }

                if (addVitButton.buttonHovered && vit < 33)
                    addVitButton.currentFrame.Y = 1;
                else if (vit == 33)
                    addVitButton.currentFrame.Y = 2;
                else
                    addVitButton.currentFrame.Y = 0;
                if (addVitButton.buttonPressed && vit < 33 && ((Game1)Game).spriteManager.player.statsToAdd > 0)
                {
                    ((Game1)Game).spriteManager.player.statsToAdd--;
                    vit++;
                    ((Game1)Game).spriteManager.player.totalhp =(int)(((Game1)Game).spriteManager.player.totalhp*1.2);
                    ((Game1)Game).spriteManager.player.hp = ((Game1)Game).spriteManager.player.totalhp;
                }

                if (((Game1)Game).spriteManager.player.statsToAdd == 0)
                     ((Game1)Game).spriteManager.playerLeveledUp = false;

            }

            

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ((Game1)Game).menuActive = true;
                pauseMenuActive = true;
                ((Game1)Game).spriteManager.playerLeveledUp = false;
                mainMenu.currentFrame =  new Point(1, 0);
                statMenuActive = false;
            }
            if (pauseMenuActive)
            {
                if (resumeButton.buttonHovered)
                    resumeButton.currentFrame.X = 1;
                else
                    resumeButton.currentFrame.X = 0;

                if (exitButton.buttonHovered)
                    exitButton.currentFrame.X = 1;
                else
                    exitButton.currentFrame.X = 0;

                if (statsButton.buttonHovered)
                    statsButton.currentFrame.X = 1;
                else
                    statsButton.currentFrame.X = 0;

                if (exitButton.buttonPressed)
                    ((Game1)Game).Exit();
                if (resumeButton.buttonPressed)
                {
                    ((Game1)Game).menuActive = false;
                    pauseMenuActive = false;
                }

                if (statsButton.buttonPressed)
                {
                    pauseMenuActive = false;
                    statMenuActive = true;
                    mainMenu.currentFrame = new Point(0, 2);
                    ((Game1)Game).menuActive = false;
                }
            }


            time++;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (((Game1)Game).menuActive || ((Game1)Game).gameOver || statMenuActive)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                mainMenu.Draw(gameTime, spriteBatch);
                if (!pauseMenuActive && !statMenuActive)
                {
                    if (((((Game1)Game).menuActive || ((Game1)Game).gameOver)))
                        playNowButton.Draw(gameTime, spriteBatch);
                }
                if (statMenuActive)
                {
                    if (((Game1)Game).spriteManager.player.statsToAdd > 0)
                    {
                        addAgiButton.Draw(gameTime, spriteBatch);
                        addStrButton.Draw(gameTime, spriteBatch);
                        addVitButton.Draw(gameTime, spriteBatch);
                    }
                    statSpriteUnit.Draw(gameTime, spriteBatch);
                    statSpriteDecimal.Draw(gameTime, spriteBatch);
                    strSpriteUnit.Draw(gameTime, spriteBatch);
                    strSpriteDecimal.Draw(gameTime, spriteBatch);
                    agiSpriteUnit.Draw(gameTime, spriteBatch);
                    agiSpriteDecimal.Draw(gameTime, spriteBatch);
                    vitSpriteUnit.Draw(gameTime, spriteBatch);
                    vitSpriteDecimal.Draw(gameTime, spriteBatch);
                }
                if (pauseMenuActive)
                {
                    resumeButton.Draw(gameTime, spriteBatch);
                    statsButton.Draw(gameTime, spriteBatch);
                    exitButton.Draw(gameTime, spriteBatch);
                }

                base.Draw(gameTime);
                spriteBatch.End();
            }

        }
    }
}
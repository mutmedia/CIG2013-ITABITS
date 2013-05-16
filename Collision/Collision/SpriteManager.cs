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
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public SpriteBatch spriteBatch;
        int time = 0;
        
        Player player;

        Bar playerHealthBar;
        Bar playerXpBar;

        public Sword galho;
        public Sword cano;
        public Sword crayon;
        public Sword lampada;
        public Sword serra;
        public Sword espada;
        public Sword jedi;
        public Sword piroca;
        int currentSwordNum = 0;

        public Sprite levelSpriteUnit;
        public Sprite levelSpriteDecimal;
        public Sprite levelUpSprite;
        public bool startLevelUpAnimation = false;
        public int levelupAnimationFrameTime = 4;
                
        public bool playerCanMove = true;

        List<Enemy> enemyList = new List<Enemy>();
        List<Bar> enemyHealthBarList = new List<Bar>();
        List<Sprite> bloodList = new List<Sprite>();

        //ENEMY INFO
        int enemySpawnMin = 100;
        int enemySpawnMax = 150;
        const int ENEMY_HEALTHBAR_HEIGHT = 10;
        //OGRE
            const int OGRESPEED = 6;
            const int OGREATTACKSPEED = 50;
            const int OGRELIFE = 20;
            const int OGREATTACKRANGE = 16;
            const int OGREDAMAGE = 1;
            const int OGREXP = 10;
        /* CALCULO DA VIDA DOS INIMIGOS 
         * Um ataque com o meio da espada da 10 de dano
         * e o com a ponta da 5, assim, o padrao de unidade
         * de vida eh 10 */

        //PLAYER INFO
            const int PLAYERTOTALHP = 10000000;
            const int XPTOLEVEL1 = 5;

        int nextSpawnNumber = 0;

        const float FIELD_OF_VIEW = 500;

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        private void ResetSpawnNumber()
        {
            nextSpawnNumber = ((Game1)Game).rnd.Next(enemySpawnMin,
                                                   enemySpawnMax);
        }

        public float getFOV()
        {
            return FIELD_OF_VIEW;
        }

        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }

        public float GetPlayerAngle()
        {
            return player.GetAngle;
        }

        public float GetPlayerHpPercentage()
        {
            return (float)player.hp / (float)player.totalhp;
        }

        private void SpawnOgre()
        {
            for (int i = 0; i < nextSpawnNumber; i++)
            {
                Vector2 speed = Vector2.Zero;
                Vector2 position = Vector2.Zero;
                Point frameSize = new Point(128, 128);

                position = new Vector2(((Game1)Game).rnd.Next(0,
                                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                                    - (frameSize.X) / 2), ((Game1)Game).rnd.Next(0,
                                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                                    - (frameSize.Y) / 2));

                speed =new Vector2 (OGRESPEED, OGRESPEED);

                //Create the Sprite
                enemyList.Add(new Enemy(Game.Content.Load<Texture2D>(@"images/ogre"), position,
                     new Point(128, 128), 1, new Point(0, 0), new Point(1, 1),
                     speed, 0, OGRELIFE, OGRELIFE, OGREATTACKSPEED, OGREATTACKRANGE, OGREDAMAGE, OGREXP, 0.9f,  this));

                enemyHealthBarList.Add(new Bar(position, ENEMY_HEALTHBAR_HEIGHT, 100, 1, Color.Red, Color.DarkRed, 0));

                if (enemyList[i].IsOutOfBounds(Game.Window.ClientBounds) && i > 0)
                {
                    enemyList.RemoveAt(i);
                    enemyHealthBarList.RemoveAt(i);
                    i--;
                }

                if (distance_between_points(position, player.GetPosition) <= FIELD_OF_VIEW && i > 0)
                {
                    enemyList.RemoveAt(i);
                    enemyHealthBarList.RemoveAt(i);
                    i--;
                }
            }
                
        }

        private float distance_between_points(Vector2 thefirst_vector, Vector2 thesecond_vector)
        {
            return (float)Math.Sqrt((double)((thefirst_vector.X - thesecond_vector.X) * (thefirst_vector.X - thesecond_vector.X)) + (double)((thefirst_vector.Y - thesecond_vector.Y) * (thefirst_vector.Y - thesecond_vector.Y)));
        }

        private void updateEnemyInteraction(GameTime gameTime)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                Enemy enemy = enemyList[i];
                Bar enemyHealthBar = enemyHealthBarList[i];
                Vector2 position = enemy.GetPosition;
                

                if (distance_between_points(enemy.GetPosition, currentSword.collisionPoint_middle) <= 64 && currentSword.getIsAttacking)
                {
                    enemy.hp -= currentSword.midDamage;
                }

                if (distance_between_points(enemy.GetPosition, currentSword.collisionPoint_tip) <= 64 && currentSword.getIsAttacking)
                {
                    enemy.hp -= currentSword.tipDamage;
                }
                if (distance_between_points(enemy.GetPosition, player.GetPosition) <= player.frameSize.X + enemy.frameSize.X/2 && enemy.GetIsAttacking)
                {
                    player.hp -= enemy.damage;
                }

                if (enemy.hp <= 0)
                {
                    bloodList.Add(new Sprite(Game.Content.Load<Texture2D>(@"Images/sangue"), enemyList[i].GetPosition,
                        new Point(128, 128), new Point(0, 0), new Point(1, 1), ((Game1)Game).rnd.Next(), -1f));
                    if (bloodList.Count() % 2000 == 1999)
                        bloodList.RemoveAt(0);
                    enemyList.RemoveAt(i);
                    enemyHealthBarList.RemoveAt(i);
                    i--;
                    player.xp += enemy.xp;
                }

                enemy.Update(gameTime, Game.Window.ClientBounds);
                enemyHealthBar.position.X = position.X - enemy.frameSize.X / 2 + 16;
                enemyHealthBar.position.Y = position.Y + enemy.frameSize.Y / 2 + 5;
                enemyHealthBar.barPercentage = ((float)enemy.hp) / ((float)enemy.totalhp);
                  
            }

            if (enemyList.Count == 0)
            {
                SpawnOgre();
                ResetSpawnNumber();
                enemySpawnMin += 5;
                enemySpawnMax += 5;
            }
        }

        public void GameOverUpdate()
        {
            if (((Game1)Game).gameOver)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList.RemoveAt(i);
                    enemyHealthBarList.RemoveAt(i);
                }
                enemySpawnMin = 10;
                enemySpawnMax = 15;
                player.level = 0;
                player.xp = 0;
                player.xpToNextLevel = XPTOLEVEL1;
                player.totalhp = PLAYERTOTALHP;
                galho.midDamage = 2;
                galho.tipDamage = 1;
                player.hp = player.totalhp;
            }
        }

        public void PlayerLevelUpdate()
        {
            Bar xpBar = playerXpBar;

            if (player.xp >= player.xpToNextLevel)
            {
                player.xp -= player.xpToNextLevel;
                player.xpToNextLevel += 5;
                player.totalhp += 10;
                player.level++;
                player.hp = player.totalhp;
                if (player.level % 5 == 0 && player.level < 40)
                    currentSwordNum++;
                startLevelUpAnimation = true;
                if (startLevelUpAnimation)
                    player.hp = player.totalhp;
            }

            playerXpBar.barPercentage = (float)player.xp / (float)player.xpToNextLevel;
        }

        public Sword currentSword
        {
            get
            {
                switch (currentSwordNum % 8)
                {
                    case 0:
                        return galho;
                    case 1:
                        return cano;
                    case 2:
                        return crayon;
                    case 3:
                        return lampada;
                    case 4:
                        return serra;
                    case 5:
                        return espada;
                    case 6:
                        return jedi;
                    case 7:
                        return piroca;
                    default:
                        return null;
                }
            }
        }

        public void UpdateLevelSprite()
        {
            levelSpriteUnit.currentFrame.X = player.level % 10;
            levelSpriteDecimal.currentFrame.X = (int)(player.level / 10);
        }

        public void LevelUpAnimationUpdate()
        {
            levelUpSprite.position = player.GetPosition;
            if (startLevelUpAnimation && time % levelupAnimationFrameTime == 0)
            {
                
                levelUpSprite.currentFrame.X = (levelUpSprite.currentFrame.X + 1) % levelUpSprite.sheetSize.X;
                if (levelUpSprite.currentFrame.X == 0)
                {
                    levelUpSprite.currentFrame.Y = (levelUpSprite.currentFrame.Y + 1) % levelUpSprite.sheetSize.Y;
                    if (levelUpSprite.currentFrame.Y == 0)
                    {
                        startLevelUpAnimation = false;
                        levelUpSprite.currentFrame = new Point(0,0);
                    }
                }
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            Texture2D player_texture = Game.Content.Load<Texture2D>(@"Images/character");
            Texture2D galho_texture = Game.Content.Load<Texture2D>(@"Images/sword/galho");
            Texture2D cano_texture = Game.Content.Load<Texture2D>(@"Images/sword/cano");
            Texture2D crayon_texture = Game.Content.Load<Texture2D>(@"Images/sword/crayon");
            Texture2D lampada_texture = Game.Content.Load<Texture2D>(@"Images/sword/lampada (melhorar)");
            Texture2D serra_texture = Game.Content.Load<Texture2D>(@"Images/sword/serra");
            Texture2D espada_texture = Game.Content.Load<Texture2D>(@"Images/sword/espada");
            Texture2D jedi_texture = Game.Content.Load<Texture2D>(@"Images/sword/jedi");
            Texture2D piroca_texture = Game.Content.Load<Texture2D>(@"Images/sword/piroca");
            Texture2D number_texture = Game.Content.Load<Texture2D>(@"Images/UI/numberz");
            Texture2D levelup_texture = Game.Content.Load<Texture2D>(@"Images/levelup");

            player = new Player(
                player_texture,
                new Vector2((int)960, (int)540), new Point(player_texture.Width, player_texture.Height), new Point(0, 0),
                new Point(1, 1), 0.0f, PLAYERTOTALHP, PLAYERTOTALHP, 0, XPTOLEVEL1, 1, this);

            galho = new Sword(
                galho_texture, Vector2.Zero, new Point(galho_texture.Width, galho_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 1, 2, 1, this);
            cano = new Sword(
                cano_texture, Vector2.Zero, new Point(cano_texture.Width, cano_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 3, 4, 1, this);
            crayon = new Sword(
                crayon_texture, Vector2.Zero, new Point(crayon_texture.Width, crayon_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 5, 6, 1, this);
            lampada = new Sword(
                lampada_texture, Vector2.Zero, new Point(lampada_texture.Width, lampada_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 7, 8, 1, this);
            serra = new Sword(
                serra_texture, Vector2.Zero, new Point(serra_texture.Width, serra_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 9, 10, 1, this);
            espada = new Sword(
                espada_texture, Vector2.Zero, new Point(espada_texture.Width, espada_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 11, 12, 1, this);
            jedi = new Sword(
                jedi_texture, Vector2.Zero, new Point(jedi_texture.Width, jedi_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 13, 14, 1, this);
            piroca = new Sword(
                piroca_texture, Vector2.Zero, new Point(piroca_texture.Width, piroca_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 15, 16, 1, this);

            levelSpriteUnit = new Sprite(number_texture, new Vector2(1350, 80), new Point(number_texture.Width/10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);
            levelSpriteDecimal = new Sprite(number_texture, new Vector2(1300, 80), new Point(number_texture.Width/10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);

            levelUpSprite = new Sprite(levelup_texture, Vector2.Zero, new Point(levelup_texture.Width / 5, levelup_texture.Height / 6),
                new Point(0, 0), new Point(5, 6), 0.0f, 0.5f);

            playerHealthBar = new Bar(new Vector2(20, 50), 60, 500, 1, Color.Red, Color.DarkRed, 1.3f);
            playerXpBar = new Bar(new Vector2(1400, 50), 60, 500, 1, Color.Green, Color.DarkGreen, 1.3f);

        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            ResetSpawnNumber();
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (!((Game1)Game).menuActive && !((Game1)Game).gameOver)
            {
                // TODO: Add your update code here
                time++;
                
                player.Update(gameTime, Game.Window.ClientBounds);

                currentSword.Update(gameTime, Game.Window.ClientBounds);

                updateEnemyInteraction(gameTime);

                playerHealthBar.barPercentage = GetPlayerHpPercentage();

                PlayerLevelUpdate();

                UpdateLevelSprite();

                LevelUpAnimationUpdate();

                if (player.hp <= 0)
                    ((Game1)Game).gameOver = true;


                base.Update(gameTime);
            }
            else
            {
                GameOverUpdate();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            
            if (!((Game1)Game).menuActive && !((Game1)Game).gameOver)
            {

                player.Draw(gameTime, spriteBatch);

                currentSword.Draw(gameTime, spriteBatch);

                foreach (Sprite s in enemyList)
                    s.Draw(gameTime, spriteBatch);

                foreach (Sprite s in bloodList)
                    s.Draw(gameTime, spriteBatch);

                foreach (Bar b in enemyHealthBarList)
                    b.Draw(gameTime, spriteBatch, Game.GraphicsDevice);

                playerHealthBar.Draw(gameTime, spriteBatch, Game.GraphicsDevice);

                playerXpBar.Draw(gameTime, spriteBatch, Game.GraphicsDevice);

                if(startLevelUpAnimation)
                    levelUpSprite.Draw(gameTime, spriteBatch);

                levelSpriteUnit.Draw(gameTime, spriteBatch);
                levelSpriteDecimal.Draw(gameTime, spriteBatch);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
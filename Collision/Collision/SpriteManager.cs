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

        public bool lastMapState = false;
        public bool currentMapState = false;
        public bool newGame = true;
        
        public Player player;

        Bar playerHealthBar;
        Bar playerXpBar;

        public Sword galho;
        public Sword cano;
        public Sword crayon;
        public Sword peixe;
        public Sword serra;
        public Sword espada;
        public Sword jedi;
        public Sword piroca;
        int currentSwordNum = 0;

        public Sprite levelSpriteUnit;
        public Sprite levelSpriteDecimal;
        public Sprite levelUpSprite;
        public Sprite aura;
        public int auraFrametime = 7;
        public int auraCurrentFrame = 0;
        public bool startLevelUpAnimation = false;
        public int levelupAnimationFrameTime = 4;
        public bool playerLeveledUp = false;

        List<Enemy> enemyList = new List<Enemy>();
        List<Enemy> enemyTypeList = new List<Enemy>();
        List<Bar> enemyHealthBarList = new List<Bar>();
        List<Sprite> bloodList = new List<Sprite>();

        //ENEMY INFO
        int spawnWeight = 10;
        const int ENEMY_HEALTHBAR_HEIGHT = 10;
        
        //PORINGER
        Enemy poringer;
        Texture2D poringer_texture;
            const int PORINGERSPEED = 20;
            const int PORINGERATTACKSPEED = 10;
            const int PORINGERLIFE = 5;
            const int PORINGERATTACKRANGE = 1;
            const int PORINGERDAMAGE = 1;
            const int PORINGERXP = 10000;
            const int PORINGERWEIGHT = 10;
        //GOBLIN
        Enemy goblin;
        Texture2D goblin_texture;
            const int GOBLINSPEED = 10;
            const int GOBLINATTACKSPEED = 50;
            const int GOBLINLIFE = 20;
            const int GOBLINATTACKRANGE = 10;
            const int GOBLINDAMAGE = 10;
            const int GOBLINXP = 5;
            const int GOBLINWEIGHT = 50;
        //OGRE
        Enemy ogre;
        Texture2D ogre_texture;
            const int OGRESPEED = 5;
            const int OGREATTACKSPEED = 50;
            const int OGRELIFE = 30;
            const int OGREATTACKRANGE = 15;
            const int OGREDAMAGE = 20;
            const int OGREXP = 10;
            const int OGREWEIGHT = 100;
        //TROLL
        Enemy troll;
        Texture2D troll_texture;
            const int TROLLSPEED = 5;
            const int TROLLATTACKSPEED = 100;
            const int TROLLLIFE = 50;
            const int TROLLATTACKRANGE = 8;
            const int TROLLDAMAGE = 30;
            const int TROLLXP = 20;
            const int TROLLWEIGHT = 100;
        //BAIACU
        Enemy baiacu;
        Texture2D baiacu_texture;
            const int BAIACUSPEED = 1;
            const int BAIACUATTACKSPEED = 150;
            const int BAIACULIFE = 10000;
            const int BAIACUATTACKRANGE = 13;
            const int BAIACUDAMAGE = 80;
            const int BAIACUXP = 0;
            const int BAIACUWEIGHT = 10;

        //PLAYER INFO
            const int PLAYERTOTALHP = 1000;
            const int XPTOLEVEL1 = 10;

        const float FIELD_OF_VIEW = 500;

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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

        private float distance_between_points(Vector2 thefirst_vector, Vector2 thesecond_vector)
        {
            return (float)Math.Sqrt((double)((thefirst_vector.X - thesecond_vector.X) * (thefirst_vector.X - thesecond_vector.X)) + (double)((thefirst_vector.Y - thesecond_vector.Y) * (thefirst_vector.Y - thesecond_vector.Y)));
        }

        private void updateEnemyInteraction(GameTime gameTime)
        {
            currentMapState = player.changedMap;
            
            for (int i = 0; i < enemyList.Count; i++)
            {
                Enemy enemy = enemyList[i];
                Bar enemyHealthBar = enemyHealthBarList[i];
                Vector2 position = enemy.GetPosition;

                if (distance_between_points(enemy.GetPosition, currentSword.collisionPoint_middle) <= enemy.frameSize.X/2 && currentSword.getIsAttacking)
                {
                    enemy.hp -= (player.damage + currentSword.midDamage);
                }

                if (distance_between_points(enemy.GetPosition, currentSword.collisionPoint_tip) <= enemy.frameSize.X/2 && currentSword.getIsAttacking)
                {
                    enemy.hp -= (player.damage + currentSword.tipDamage);
                }
                if (distance_between_points(enemy.GetPosition, player.GetPosition) <= player.frameSize.X/2 + enemy.frameSize.X/2 && enemy.GetIsAttacking)
                {
                    player.hp -= enemy.damage;
                }

                if (enemy.hp <= 0)
                {
                    bloodList.Add(new Sprite(Game.Content.Load<Texture2D>(@"Images/sangue"), enemy.GetPosition,
                        new Point(64, 64), new Point(0, 0), new Point(1, 1), ((Game1)Game).rnd.Next(), -1f));
                    if (bloodList.Count() % 2000 == 1999)
                        bloodList.RemoveAt(0);
                    enemyList.RemoveAt(i);
                    enemyHealthBarList.RemoveAt(i);                    
                    i--;
                    player.xp += enemy.xp;
                }

                enemy.Update(gameTime, Game.Window.ClientBounds);
                enemyHealthBar.position.X = position.X - enemy.frameSize.X / 2;
                enemyHealthBar.position.Y = position.Y + enemy.frameSize.Y*1.1f/ 2 ;
                enemyHealthBar.barPercentage = ((float)enemy.hp) / ((float)enemy.totalhp);
                  
            }

            if (enemyList.Count() == 0)
                ((Game1)Game).mapManager.miniMap.boolmaze[((Game1)Game).mapManager.currentRoom.Y, ((Game1)Game).mapManager.currentRoom.X] = true;

            if( ((Game1)Game).mapManager.miniMap.boolmaze[((Game1)Game).mapManager.currentRoom.Y, ((Game1)Game).mapManager.currentRoom.X] && currentMapState)
                bloodList.Clear();
            if (!((Game1)Game).mapManager.miniMap.boolmaze[((Game1)Game).mapManager.currentRoom.Y, ((Game1)Game).mapManager.currentRoom.X] && currentMapState)
            {
                SpawnEnemy();
            }
        }

        public void GameOverUpdate()
        {
            if (((Game1)Game).gameOver)
            {
                enemyList.Clear();
                enemyHealthBarList.Clear();
                bloodList.Clear();
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

            if (player.xp >= player.xpToNextLevel && player.level < 99)
            {
                player.xp -= player.xpToNextLevel;

                
                player.level++;
                player.statsToAdd++;
                player.hp = player.totalhp;
                if (player.level % 5 == 0 && player.level < 40)
                    currentSwordNum++;
                startLevelUpAnimation = true;
                if (startLevelUpAnimation)
                    player.hp = player.totalhp;
            }

            playerXpBar.barPercentage = (float)player.xp / (float)player.xpToNextLevel;
        }

        public void generateEnemyList()
        {   
            int currentWeight = 0;
            int nextSpawn = 0;
            enemyList.Clear();
            while (currentWeight < spawnWeight)
            {
                nextSpawn = ((Game1)Game).rnd.Next(enemyTypeList.Count() );
                if (enemyTypeList[nextSpawn].weight + currentWeight <= spawnWeight)
                {
                    
                    if (enemyTypeList[nextSpawn] == ogre)
                       enemyList.Add( new Enemy(ogre_texture, Vector2.Zero, new Point(ogre_texture.Width, ogre_texture.Height), new Point(0, 0), new Point(0, 0),
                            0.0f, 1f, this, OGREWEIGHT, new Vector2(OGRESPEED, OGRESPEED), OGRELIFE, OGRELIFE, OGREATTACKSPEED, OGREATTACKRANGE, OGREDAMAGE, OGREXP, ((Game1)Game).mapManager));
                    else if (enemyTypeList[nextSpawn] == troll)
                       enemyList.Add( new Enemy(troll_texture, Vector2.Zero, new Point(troll_texture.Width, troll_texture.Height), new Point(0, 0), new Point(0, 0),
                            0.0f, 1f, this, TROLLWEIGHT, new Vector2(TROLLSPEED, TROLLSPEED), TROLLLIFE, TROLLLIFE, TROLLATTACKSPEED, TROLLATTACKRANGE, TROLLDAMAGE, TROLLXP, ((Game1)Game).mapManager));
                    else if(enemyTypeList[nextSpawn] == poringer)
                        enemyList.Add (new Enemy(poringer_texture, Vector2.Zero, new Point(poringer_texture.Width, poringer_texture.Height), new Point(0, 0), new Point(0, 0),
                            0.0f, 1f, this, PORINGERWEIGHT, new Vector2(PORINGERSPEED, PORINGERSPEED), PORINGERLIFE, PORINGERLIFE, PORINGERATTACKSPEED, PORINGERATTACKRANGE, PORINGERDAMAGE, PORINGERXP, ((Game1)Game).mapManager));
                    else if (enemyTypeList[nextSpawn] == baiacu)
                        enemyList.Add(new Enemy(baiacu_texture, Vector2.Zero, new Point(baiacu_texture.Width, baiacu_texture.Height), new Point(0, 0), new Point(0, 0),
                            0.0f, 1f, this, BAIACUWEIGHT, new Vector2(BAIACUSPEED, BAIACUSPEED), BAIACULIFE, BAIACULIFE, BAIACUATTACKSPEED, BAIACUATTACKRANGE, BAIACUDAMAGE, BAIACUXP, ((Game1)Game).mapManager));
                    else if (enemyTypeList[nextSpawn] == goblin)
                        enemyList.Add(new Enemy(goblin_texture, Vector2.Zero, new Point(goblin_texture.Width, goblin_texture.Height), new Point(0, 0), new Point(0, 0),
                            0.0f, 1f, this, GOBLINWEIGHT, new Vector2(GOBLINSPEED, GOBLINSPEED), GOBLINLIFE, GOBLINLIFE, GOBLINATTACKSPEED, GOBLINATTACKRANGE, GOBLINDAMAGE, GOBLINXP, ((Game1)Game).mapManager));



                    currentWeight += enemyTypeList[nextSpawn].weight;
                }
            }
        }

        private void SpawnEnemy()
        {
            
            generateEnemyList();
            enemyHealthBarList.Clear();
            
            for (int i = 0; i < enemyList.Count(); i++)
            {
                Vector2 randposition = Vector2.Zero;
                do
                {
                    randposition = new Vector2(((Game1)Game).rnd.Next(0,
                                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                                    - (enemyList[i].frameSize.X) / 2), ((Game1)Game).rnd.Next(0,
                                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                                    - (enemyList[i].frameSize.Y) / 2));

                    //Create the Sprite
                    enemyList[i].position = randposition;
                } while (distance_between_points(randposition, player.GetPosition) <= FIELD_OF_VIEW && i > 0 || !enemyList[i].canSpawn);
                
                enemyHealthBarList.Add(new Bar(Vector2.Zero, ENEMY_HEALTHBAR_HEIGHT, enemyList[i].frameSize.X, 1, Color.Red, Color.DarkRed, 0));
               
            }

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
                        return peixe;
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
            levelSpriteDecimal.currentFrame.X = (int)(player.level % 100 / 10);
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
                        playerLeveledUp = true;
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
            Texture2D peixe_texture = Game.Content.Load<Texture2D>(@"Images/sword/peixe");
            Texture2D serra_texture = Game.Content.Load<Texture2D>(@"Images/sword/serra");
            Texture2D espada_texture = Game.Content.Load<Texture2D>(@"Images/sword/espada");
            Texture2D jedi_texture = Game.Content.Load<Texture2D>(@"Images/sword/jedi");
            Texture2D piroca_texture = Game.Content.Load<Texture2D>(@"Images/sword/piroca");
            Texture2D number_texture = Game.Content.Load<Texture2D>(@"Images/UI/numberz");
            Texture2D levelup_texture = Game.Content.Load<Texture2D>(@"Images/levelup");
            Texture2D aura_texture = Game.Content.Load<Texture2D>(@"Images/aura");

            ogre_texture = Game.Content.Load<Texture2D>(@"Images/Enemies/Ogre");
            troll_texture = Game.Content.Load<Texture2D>(@"Images/Enemies/Troll");
            poringer_texture = Game.Content.Load<Texture2D>(@"Images/Enemies/poringer");
            baiacu_texture = Game.Content.Load<Texture2D>(@"Images/Enemies/baiacu");
            goblin_texture = Game.Content.Load<Texture2D>(@"Images/Enemies/goblin");

            //Player
            player = new Player(
                player_texture,
                new Vector2((int)960, (int)540), new Point(player_texture.Width, player_texture.Height), new Point(0, 0),
                new Point(1, 1), 0.0f, PLAYERTOTALHP, PLAYERTOTALHP, 0, XPTOLEVEL1, 1, this, ((Game1)Game).mapManager);

            //Swords
            galho = new Sword(
                galho_texture, Vector2.Zero, new Point(galho_texture.Width, galho_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 1, 2, 1, this);
            cano = new Sword(
                cano_texture, Vector2.Zero, new Point(cano_texture.Width, cano_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 3, 4, 1, this);
            crayon = new Sword(
                crayon_texture, Vector2.Zero, new Point(crayon_texture.Width, crayon_texture.Height), new Point(0, 0),
                new Point(1, 1), 0, 5, 6, 1, this);
            peixe = new Sword(
                peixe_texture, Vector2.Zero, new Point(peixe_texture.Width, peixe_texture.Height), new Point(0, 0),
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
            // UI
            levelSpriteUnit = new Sprite(number_texture, new Vector2(1350, 80), new Point(number_texture.Width/10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);
            levelSpriteDecimal = new Sprite(number_texture, new Vector2(1300, 80), new Point(number_texture.Width/10, number_texture.Height),
                new Point(0, 0), new Point(10, 1), 0.0f, 1f);

            levelUpSprite = new Sprite(levelup_texture, Vector2.Zero, new Point(levelup_texture.Width / 5, levelup_texture.Height / 6),
                new Point(0, 0), new Point(5, 6), 0.0f, 0.5f);

            playerHealthBar = new Bar(new Vector2(20, 50), 60, 500, 1, Color.Red, Color.DarkRed, 1.3f);
            playerXpBar = new Bar(new Vector2(1400, 50), 60, 500, 1, Color.Green, Color.DarkGreen, 1.3f);

            aura = new Sprite(aura_texture, Vector2.Zero, new Point(aura_texture.Width / 4, aura_texture.Height), new Point(0, 0), new Point(1, 4), 0.0f, -0.5f);

            //Enemies
            //Very Small Size
            poringer = new Enemy(poringer_texture, Vector2.Zero, new Point(poringer_texture.Width, poringer_texture.Height), new Point(0, 0), new Point(0, 0),
                0.0f, 1f, this, PORINGERWEIGHT, new Vector2(PORINGERSPEED, PORINGERSPEED), PORINGERLIFE, PORINGERLIFE, PORINGERATTACKSPEED, PORINGERATTACKRANGE, PORINGERDAMAGE, PORINGERXP, ((Game1)Game).mapManager);
            for (int i = 0; i < 10; i++)
                enemyTypeList.Add(poringer);

            //Small Size
            goblin = new Enemy(goblin_texture, Vector2.Zero, new Point(goblin_texture.Width, goblin_texture.Height), new Point(0, 0), new Point(0, 0),
                0.0f, 1f, this, GOBLINWEIGHT, new Vector2(GOBLINSPEED, GOBLINSPEED), GOBLINLIFE, GOBLINLIFE, GOBLINATTACKSPEED, GOBLINATTACKRANGE, GOBLINDAMAGE, GOBLINXP, ((Game1)Game).mapManager);
            for (int i = 0; i < 5; i++)
                enemyTypeList.Add(goblin);

            //Medium Size
            ogre = new Enemy( ogre_texture, Vector2.Zero, new Point(ogre_texture.Width, ogre_texture.Height), new Point(0, 0), new Point(0,0),
                0.0f, 1f, this, OGREWEIGHT, new Vector2(OGRESPEED, OGRESPEED), OGRELIFE, OGRELIFE, OGREATTACKSPEED, OGREATTACKRANGE, OGREDAMAGE, OGREXP, ((Game1)Game).mapManager);
            for (int i = 0; i < 2; i++)
                enemyTypeList.Add(ogre);

            //Big Size
            troll = new Enemy(troll_texture, Vector2.Zero, new Point(troll_texture.Width, troll_texture.Height), new Point(0, 0), new Point(0, 0),
                0.0f, 1f, this, TROLLWEIGHT, new Vector2(TROLLSPEED, TROLLSPEED), TROLLLIFE, TROLLLIFE, TROLLATTACKSPEED, TROLLATTACKRANGE, TROLLDAMAGE, TROLLXP, ((Game1)Game).mapManager);
            for (int i = 0; i < 1; i++)
                enemyTypeList.Add(troll);
            
            //Boss Size
            baiacu = new Enemy(baiacu_texture, Vector2.Zero, new Point(baiacu_texture.Width, baiacu_texture.Height), new Point(0, 0), new Point(0, 0),
                0.0f, 1f, this, BAIACUWEIGHT, new Vector2(BAIACUSPEED, BAIACUSPEED), BAIACULIFE, BAIACULIFE, BAIACUATTACKSPEED, BAIACUATTACKRANGE, BAIACUDAMAGE, BAIACUXP, ((Game1)Game).mapManager);
            for (int i = 0; i < 1; i++)
                enemyTypeList.Add(baiacu);

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
            if (!((Game1)Game).menuActive && !((Game1)Game).gameOver && (!playerLeveledUp || !((Game1)Game).mapManager.miniMap.boolmaze[((Game1)Game).mapManager.currentRoom.Y, ((Game1)Game).mapManager.currentRoom.X]) && !((Game1)Game).menuManager.pauseMenuActive && !((Game1)Game).menuManager.statMenuActive)
            {
                // TODO: Add your update code here
                if (newGame)
                {
                    SpawnEnemy();
                    newGame = false;
                }
                
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
                if (player.level == 99)
                {
                    aura.position = player.position;
                    aura.angle += 0.4f;
                    if (auraCurrentFrame % auraFrametime == 0)
                        aura.currentFrame.X = ((aura.currentFrame.X + 1) % 4);
                    auraCurrentFrame++;
                }


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

            if (!((Game1)Game).menuActive && !((Game1)Game).gameOver && (!playerLeveledUp || !((Game1)Game).mapManager.miniMap.boolmaze[((Game1)Game).mapManager.currentRoom.Y, ((Game1)Game).mapManager.currentRoom.X]) && !((Game1)Game).menuManager.pauseMenuActive && !((Game1)Game).menuManager.statMenuActive)
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

                if (player.level == 99)
                    aura.Draw(gameTime, spriteBatch);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
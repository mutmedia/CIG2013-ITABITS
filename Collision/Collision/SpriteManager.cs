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
        Player player;
        Bar playerHealthBar;
        Bar playerXpBar;
        Sword sword;

        List<Enemy> enemyList = new List<Enemy>();
        List<Bar> enemyHealthBarList = new List<Bar>();

        //ENEMY INFO
        int enemySpawnMin = 5;
        int enemySpawnMax = 10;
        const int ENEMY_HEALTHBAR_HEIGHT = 10;
        //OGRE
            const int OGRESPEED = 3;
            const int OGREATTACKSPEED = 50;
            const int OGRELIFE = 20;
            const int OGREATTACKRANGE = 16;
            const int OGREDAMAGE = 1;
            const int OGREXP = 1;
        /* CALCULO DA VIDA DOS INIMIGOS 
         * Um ataque com o meio da espada da 10 de dano
         * e o com a ponta da 5, assim, o padrao de unidade
         * de vida eh 10 */

        //PLAYER INFO
            const int PLAYERTOTALHP = 100;
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
                     speed, 0, OGRELIFE, OGRELIFE, OGREATTACKSPEED, OGREATTACKRANGE, OGREDAMAGE, OGREXP,  this));

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

                if (distance_between_points(enemy.GetPosition, sword.collisionPoint_middle) <= 64 && sword.getIsAttacking)
                {
                    enemy.hp -= sword.midDamage;
                }

                if (distance_between_points(enemy.GetPosition, sword.collisionPoint_tip) <= 64 && sword.getIsAttacking)
                {
                    enemy.hp -= sword.tipDamage;
                }
                if (distance_between_points(enemy.GetPosition, player.GetPosition) <= player.frameSize.X + enemy.frameSize.X/2 && enemy.GetIsAttacking)
                {
                    player.hp -= enemy.damage;
                }
                if (enemy.hp <= 0)
                {
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

        public void playerLevelUpdate()
        {
            Bar xpBar = playerXpBar;

            if (player.xp >= player.xpToNextLevel)
            {
                player.xp -= player.xpToNextLevel;
                player.xpToNextLevel += 5;
                player.totalhp += 10;
                sword.midDamage++;
                sword.tipDamage++;
                player.hp = player.totalhp;
            }

            playerXpBar.barPercentage = (float)player.xp / (float)player.xpToNextLevel;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            player = new Player(
                Game.Content.Load<Texture2D>(@"Images/character"),
                new Vector2((int)960, (int)540), new Point(128, 128), 1, new Point(0, 0),
                new Point(1, 1), 0.0f, PLAYERTOTALHP, PLAYERTOTALHP, 0, XPTOLEVEL1);
            
            Texture2D sword_texture = Game.Content.Load<Texture2D>(@"Images/sword");

            sword = new Sword(
                sword_texture, Vector2.Zero, new Point(sword_texture.Width, sword_texture.Height), 0, new Point(0, 0),
                new Point(1, 1), 0, this);

            playerHealthBar = new Bar(new Vector2(20, 50), 60, 500, 1, Color.Red, Color.DarkRed, 1.0f);

            playerXpBar = new Bar(new Vector2(1400, 50), 60, 500, 1, Color.Green, Color.DarkGreen, 1.0f);


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
            if(!((Game1)Game).menuActive)
            {
                // TODO: Add your update code here
                player.Update(gameTime, Game.Window.ClientBounds);

                sword.Update(gameTime, Game.Window.ClientBounds);

                updateEnemyInteraction(gameTime);

                playerHealthBar.barPercentage = GetPlayerHpPercentage();

                playerLevelUpdate();

                if (player.hp <= 0)
                    Game.Exit();

                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!((Game1)Game).menuActive)
            {

                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                player.Draw(gameTime, spriteBatch);

                sword.Draw(gameTime, spriteBatch);

                foreach (Sprite s in enemyList)
                    s.Draw(gameTime, spriteBatch);

                foreach (Bar b in enemyHealthBarList)
                    b.Draw(gameTime, spriteBatch, Game.GraphicsDevice);

                playerHealthBar.Draw(gameTime, spriteBatch, Game.GraphicsDevice);

                playerXpBar.Draw(gameTime, spriteBatch, Game.GraphicsDevice);

                spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }
}
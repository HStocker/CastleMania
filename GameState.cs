using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace CastleMania
{
    class Playing : GameState
    {

        Unpaused unpaused;
        Paused paused;
        CastleMania ingame;
        UI ui;
        GameState state;
        Scene scene;
        SpriteFont output;
        SpriteBatch spriteBatch;
        ingameMenu menu;

        Texture2D spriteSheet;

        public int[] currentCorner = { 600, 500 };
        public int tileWidth = 26;
        public int tileHeight = 44;
        int[] screenDim = { 50, 20 };
        int[,] drawArray;
        int[,] seenArray;

        int[] textScroll = new int[] { -4, -2, -1, 0, 0, 1, 2, 4, 2, 1, 0, 0, -1, -2 };
        int scrollTimer = 0;
        int textScrollIndex = 0;

        int frameCount = 0;
        int frames = 0;
        int frameTimer = 0;
        int swing = 0;
        int swingTimer = 0;

        public Player player;
        public List<Enemy> enemies = new List<Enemy>();
        public List<Item> items = new List<Item>();
        public List<Particle> particles = new List<Particle>();
        public List<Projectile> projectiles = new List<Projectile>();
        public List<StaticObject> staticObjects = new List<StaticObject>();

        private bool collide;
        public bool collisionMarkers = false;

        Color[] colors = { Playing.getColor("LightGray"), Playing.getColor("DarkGray"), Playing.getColor("Brown"), Playing.getColor("LightGray"), Playing.getColor("Gray"), Playing.getColor("Blue") };

        public Playing(SpriteBatch spriteBatch, CastleMania ingame)
        {
            this.ingame = ingame;
            output = ingame.Content.Load<SpriteFont>("Output18pt");
            this.spriteBatch = spriteBatch;
            menu = new ingameMenu(ingame, output, this);
            scene = new Scene(this);

            spriteSheet = ingame.Content.Load<Texture2D>("FrogSprite");
            player = new Player(new int[] { 500, 500 }, scene, this);
            ui = new UI(player);

            unpaused = new Unpaused(this, scene, player);
            paused = new Paused(this, menu);
            
            state = unpaused;

        }
        public void update(GameTime gameTime)
        {
            state.update(gameTime);
        }

        public void draw()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            
            drawTiles();
            if (player.crawling) { spriteBatch.Draw(spriteSheet, new Vector2(player.coords[0] - (player.facing * 16), player.coords[1] + 40), player.getSpriteRectangle(), Color.White, 0f, new Vector2(5, 5), new Vector2(2, 2), SpriteEffects.None, 0f); }
            else spriteBatch.Draw(spriteSheet, new Vector2(player.coords[0], player.coords[1]), player.getSpriteRectangle(), Color.White, 0f, new Vector2(5, 5), new Vector2(2, 2), SpriteEffects.None, 0f); 

            spriteBatch.End();
        }

        public void drawTiles()
        {
            //Debug.Print(Convert.ToString(currentCorner[0] / 64 + "," + currentCorner[1] / 64));
            for (int i = 0; i < 22; i++)
            { //13 X 20 screen size
                for (int j = 0; j < 15; j++)
                {
                    if (scene.includesTile(new int[] { i + (currentCorner[0] / 64), j + (currentCorner[1] / 64) }) && scene.getTile(new int[] { i + (currentCorner[0] / 64), j + (currentCorner[1] / 64) }).getNum() == 1)
                    {
                        spriteBatch.Draw(spriteSheet, new Vector2((64 * i) - (currentCorner[0] % 64), (64 * j) - (currentCorner[1] % 64)), new Rectangle(0, 128, 32, 32), Color.White, 0f, new Vector2(5, 5), new Vector2(2, 2), SpriteEffects.None, 0f);
                        spriteBatch.DrawString(output, Convert.ToString(i + "," + j), new Vector2((64 * i) - (currentCorner[0] % 64), (64 * j) - (currentCorner[1] % 64)), Color.Red);
                    }
                    else if (scene.includesTile(new int[] { i + (currentCorner[0] / 64), j + (currentCorner[1] / 64) }))
                    {
                        spriteBatch.Draw(spriteSheet, new Vector2((i*64) - (currentCorner[0] % 64), (j*64) - (currentCorner[1] % 64)), new Rectangle(32, 128, 32, 32), Color.White, 0f, new Vector2(5, 5), new Vector2(2, 2), SpriteEffects.None, 0f);
                        spriteBatch.DrawString(output, Convert.ToString(i + "," + j), new Vector2((64 * i) - (currentCorner[0] % 64), (64 * j) - (currentCorner[1] % 64)), Color.Red);
                    }
                }
            }
        }

        public static Color getColor(string color)
        {
            switch (color)
            {

                case "Black": return new Color(20, 12, 28);
                case "DarkBlue": return new Color(48, 52, 109);
                case "Green": return new Color(52, 101, 36);
                case "LightGreen": return new Color(109, 170, 44);
                case "White": return new Color(222, 218, 214);
                case "Red": return new Color(208, 70, 72);
                case "Blue": return new Color(89, 125, 206);
                case "Yellow": return new Color(218, 212, 94);
                case "Brown": return new Color(133, 76, 48);
                case "Salmon": return new Color(210, 170, 153);
                case "LightGray": return new Color(133, 149, 151);
                case "Gray": return new Color(117, 113, 97);
                case "Sienna": return new Color(210, 125, 44);
                case "LightBlue": return new Color(109, 194, 202);
                case "DarkGray": return new Color(78, 74, 78);
                case "Purple": return new Color(68, 36, 52);
                default: return new Color(255, 255, 255);
            }
        }

        public string getTag() { return "playing"; }


        public void entering()
        {

        }

        public void leaving()
        {

        }
    }
    interface GameState
    {
        string getTag();
        void update(GameTime gameTime);
        void draw();
        void entering();
        void leaving();
    }
}

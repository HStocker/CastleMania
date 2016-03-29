using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CastleMania
{
    class Scene
    {
        int[] dimensions;
        Constructor constructor;
        Playing playing;
        //public List<Event> events = new List<Event>();
        //TextBox textBox;
        //int[,] triggerArray;
        //public string currentArea;
        int level = 1;

        public Scene(Playing playing)
        {

            this.playing = playing;
            this.constructor = new Constructor(string.Format("Level{0}.csv", level));
            dimensions = constructor.getDimensions();


            //string[] ioArray = File.ReadAllText(string.Format("Content//Level{0}Event.csv", level)).Split('\n');
            //triggerArray = new int[(ioArray[0].Length + 1) / 2, ioArray.GetLength(0)];
            //for (int i = 0; i < ioArray.Length; i++)
            //{
            //    string[] lineSplit = ioArray[i].Split(',');
            //    for (int j = 0; j < lineSplit.Length; j++)
            //    {
            //        triggerArray[j, i] = Convert.ToInt16(lineSplit[j]);
            //    }
            //}

        }
        public bool collides(Rectangle coordinates)
        {

            //List<Tile> tiles = new List<Tile>();
            var tiles = from Tile tile in constructor.getTileArray()
                        where coordinates.Intersects(new Rectangle(tile.getCoords()[0], tile.getCoords()[1], 64, 64))
                        select tile;

            Debug.Print(Convert.ToString((tiles.ToArray().Length != 0)+","+tiles.ToArray().Length+","+coordinates.X+","+coordinates.Y));
            return tiles.ToList().FindAll(a => a.getNum() == 1).Count != 0;
        }

        public List<Tile> getCollisionTiles(Rectangle coordinates) 
        {
            //List<Tile> tiles = new List<Tile>();
            var tiles = from Tile tile in constructor.getTileArray()
                    where coordinates.Intersects(new Rectangle(tile.getCoords()[0], tile.getCoords()[1], 64, 64))
                    select tile;

            return tiles.ToList().FindAll(a => a.getNum() == 1).ToList();
        }

        public Tile[,] getArray() { return constructor.getTileArray(); }
        public int[] getDimensions() { return dimensions; }
        public bool includesTile(int[] coordinates) { return constructor.includesTile(coordinates); }
        public Tile getTile(int[] coordinates) { return constructor.getTile(coordinates); }
        public Tile getTile(int x, int y) { return constructor.getTile(x, y); }

    }

    class Constructor
    {
        Tile[,] tileArray;
        string currentLevel;
        string[] tileTypes = { "floor", "wall", "hatch", "window", "wall", "floor" };

        public Constructor(string currentLevel)
        {
            this.currentLevel = currentLevel;
            this.build();
        }

        public void build()
        {
            string[] ioArray = File.ReadAllText(string.Format("Content//{0}", this.currentLevel)).Split('\n');
            tileArray = new Tile[(ioArray[0].Length + 1) / 2, ioArray.GetLength(0)];
            for (int i = 0; i < ioArray.Length; i++)
            {
                string[] lineSplit = ioArray[i].Split(',');
                for (int j = 0; j < lineSplit.Length; j++)
                {
                    int tileNumber = Convert.ToInt16(lineSplit[j]);
                    tileArray[j, i] = new Tile(tileNumber, tileTypes[tileNumber], j*64, i*64);
                    tileArray[j, i].setTag(tileTypes[tileNumber]);
                }
            }
        }

        public int[] getDimensions()
        {
            int[] temp = new int[2];
            temp[0] = this.tileArray.GetLength(0);
            temp[1] = this.tileArray.GetLength(1);
            return temp;
        }

        public bool includesTile(int x, int y)
        {
            //Debug.Print("x:"+Convert.ToString(x)+"\ny:"+Convert.ToString(y)+"\ndim0:"+Convert.ToString(tileArray.GetLength(0))+"\ndim1:"+Convert.ToString(tileArray.GetLength(1)));
            if (x < 0 || x >= this.tileArray.GetLength(0)) { return false; }
            if (y < 0 || y >= this.tileArray.GetLength(1)) { return false; }
            return true;
        }
        public bool includesTile(int[] coords)
        {
            //Debug.Print("x:"+Convert.ToString(x)+"\ny:"+Convert.ToString(y)+"\ndim0:"+Convert.ToString(tileArray.GetLength(0))+"\ndim1:"+Convert.ToString(tileArray.GetLength(1)));
            if (coords[0] < 0 || coords[0] >= this.tileArray.GetLength(0)) { return false; }
            if (coords[1] < 0 || coords[1] >= this.tileArray.GetLength(1)) { return false; }
            return true;
        }
        public Tile[,] getTileArray() { return this.tileArray; }
        public Tile getTile(int x, int y) { return tileArray[x, y]; }
        public Tile getTile(int[] coordinates) { return tileArray[coordinates[0], coordinates[1]]; }

    }

    class Tile
    {
        string tileType;
        int tileNum;
        public bool solid;
        private int[] coords;
        string tag;
        Rectangle locationRectangle;

        public Tile(int tileNum, string tileType, int x, int y)
        {
            this.tileNum = tileNum;
            this.tileType = tileType;
            this.coords = new int[] { x, y };
            this.locationRectangle = new Rectangle(x, y, 64, 64);
        }
        public Rectangle getLocation() { return locationRectangle; }

        public int[] getCoords() { return coords; }
        public int getNum() { return this.tileNum; }
        public void setNum(int num) { this.tileNum = num; }
        public string getType() { return this.tileType; }
        public void setTag(string tag) { this.tag = tag; }
        public string getTag() { return this.tag; }
        public int distance(Tile inTile) { return Math.Abs(this.coords[0] - inTile.coords[0]) + Math.Abs(this.coords[1] - inTile.coords[1]); }
        }
    public static class REPLACEME
    {
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = rectA.X - rectB.X;
            float depthY = rectA.Y - rectB.Y;
            return new Vector2(depthX, depthY);
        }
    }
}

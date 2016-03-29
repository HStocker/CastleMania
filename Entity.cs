using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace CastleMania
{
    class Player : Drawable
    {
        Scene scene;
        Playing playing;
        public List<string> inventory = new List<string>();
        int[] velocity = new int[] { 0, 0 };
        int[] maxVelocity = new int[] { 4, 4};
        int[] decay = new int[] { 0, 0 };

        public int gravityTimer = 0;
        public int moveTimer = 0;

        public bool isOnGround = true;
        public bool jumping = false;
        public bool moving = false;
        public bool running = false;
        public bool crawling = false;
        public bool walking = false;
        public int facing = 1;
        private float previousBottom;

        public Player(int[] coords, Scene scene, Playing playing)
        {
            this.playing = playing;
            inventory.Add("fist");
            this.coords = coords;
            this.scene = scene;
            this.setSpriteRectangle(new Rectangle(0,0, 32, 64));
        }
        public void update(GameTime gameTime)
        {
            if (moving) { moveTimer += gameTime.ElapsedGameTime.Milliseconds; }
            else if(velocity[0] == 0) { moveTimer = 0; this.setSpriteRectangle(new Rectangle(0, 32 + (32 * -facing), 32, 64)); }

            if (jumping)
            {   //implement iterating through scene.getCollisionTiles()
                //check position of edges of blocks for rounding off
                gravityTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (!scene.collides(new Rectangle(playing.currentCorner[0] + coords[0] + velocity[0], playing.currentCorner[1] + coords[1] + velocity[1], 32, 64)))
                {
                    playing.currentCorner = new int[] { playing.currentCorner[0] + velocity[0], playing.currentCorner[1] - velocity[1] };
                }
                else { jumping = false; }
                if (gravityTimer > 40) { velocity[1] -= 2; gravityTimer = 0; }
                if (velocity[1] < -15) { velocity[1] = -15; }

            }

            if (moving && !jumping)
            {
                //do collision in here
                playing.currentCorner[0] += velocity[0];
            }
            else if(!jumping)
            {
                if (velocity[0] > 0) velocity[0] -= 1; //stop animation
                else if (velocity[0] < 0) velocity[0] += 1;  //stop animation
            }

            //return bool - if no collision, jumping = true;
            this.adjustCoords(scene.getCollisionTiles(new Rectangle(playing.currentCorner[0] + coords[0] + velocity[0], playing.currentCorner[1] + coords[1] + velocity[1], 64, 128))); 
        }
        public Rectangle getHitBox()
        {
            return new Rectangle(coords[0], coords[1], 64, 128);
        }

        public void jump() 
        {
            jumping = true;
            velocity[1] = 15;
        }

        public void walkLeft()
        {
            facing = -1;
            if (velocity[0] > -3) { velocity[0] -= 1; } //turn around animation
            if (velocity[0] < -3) { velocity[0] += 1; }
            //if (!moving) { velocity[0] = -3; }
            moving = true;
            walking = true;
            running = crawling = false;

            if (moveTimer > 625) { moveTimer = 0; }
            else if (moveTimer > 495) { this.setSpriteRectangle(new Rectangle(128, 64, 32, 64)); }
            else if (moveTimer > 330) { this.setSpriteRectangle(new Rectangle(96, 64, 32, 64)); }
            else if (moveTimer > 165) { this.setSpriteRectangle(new Rectangle(64, 64, 32, 64)); }
            else  { this.setSpriteRectangle(new Rectangle(32, 64, 32, 64)); }
        }
        public void walkRight()
        {
            facing = 1;
            if (velocity[0] < 3) { velocity[0] += 1; } // turn around animation
            if (velocity[0] > 3) { velocity[0] -= 1; }
            //if (!moving) { velocity[0] = 3; }
            moving = true;
            walking = true;
            running = crawling = false;

            if (moveTimer > 625) { moveTimer = 0; }
            else if (moveTimer > 495) { this.setSpriteRectangle(new Rectangle(128, 0, 32, 64)); }
            else if (moveTimer > 330) { this.setSpriteRectangle(new Rectangle(96, 0, 32, 64)); }
            else if (moveTimer > 165) { this.setSpriteRectangle(new Rectangle(64, 0, 32, 64)); }
            else { this.setSpriteRectangle(new Rectangle(32, 0, 32, 64)); }
        }
        public void runLeft()
        {
            facing = -1;
            if (velocity[0] > -8) { velocity[0] -= 1; }
            //if (!running) { velocity[0] = -8; }
            running = true;
            moving = true;
            walking = crawling = false;

            if (moveTimer > 330) { moveTimer = 0; }
            else if (moveTimer > 165) { this.setSpriteRectangle(new Rectangle(288, 64, 64, 64)); }
            else { this.setSpriteRectangle(new Rectangle(224, 64, 64, 64)); }

        }
        public void runRight() 
        {
            facing = 1;
            if (velocity[0] < 8) { velocity[0] += 1; }
            //if (!running) { velocity[0] = 8; }
            running = true;
            moving = true;
            walking = crawling = false;

            if (moveTimer > 330) { moveTimer = 0; }
            else if (moveTimer > 165) { this.setSpriteRectangle(new Rectangle(288, 0, 64, 64)); }
            else { this.setSpriteRectangle(new Rectangle(224, 0, 64, 64)); }
        }
        public void crawlLeft()
        {
            facing = -1;
            if (velocity[0] > -2) { velocity[0] -= 1; }
            if (velocity[0] < -2) { velocity[0] += 1; }
            //if (!running) { velocity[0] = 8; }
            crawling = true;
            moving = true;
            walking = running = false;

            if (moveTimer > 330) { moveTimer = 0; }
            else if (moveTimer > 165) { this.setSpriteRectangle(new Rectangle(160, 64, 64, 32)); }
            else { this.setSpriteRectangle(new Rectangle(160, 96, 64, 32)); } 
        }
        public void crawlRight()
        {
            facing = 1;
            if (velocity[0] < 2) { velocity[0] += 1; }
            if (velocity[0] > 2) { velocity[0] -= 1; }
            //if (!running) { velocity[0] = 8; }
            crawling = true;
            moving = true;
            walking = running = false;

            if (moveTimer > 330) { moveTimer = 0; }
            else if (moveTimer > 165) { this.setSpriteRectangle(new Rectangle(160, 0, 64, 32)); }
            else { this.setSpriteRectangle(new Rectangle(160, 32, 64, 32)); } 
        }
        public void duck() { }

        public void adjustCoords(List<Tile> collisionList)
        {
            isOnGround = false;
            Rectangle bounds = new Rectangle(playing.currentCorner[0] + coords[0], playing.currentCorner[1] + coords[1],64,128);
            foreach (Tile tile in collisionList)
            {

                Rectangle tileBounds = tile.getLocation();
                Vector2 depth = REPLACEME.GetIntersectionDepth(bounds, tileBounds);
                Debug.Print(Convert.ToString(depth.X + "," + depth.Y + "," + bounds.X + "," + bounds.Y + "," + tile.getLocation().X + "," + tile.getLocation().Y));
                if (depth != Vector2.Zero)
                {
                    float absDepthX = Math.Abs(depth.X);
                    float absDepthY = Math.Abs(depth.Y);
                    //Debug.Print(Convert.ToString(depth.X + "," + depth.Y));

                    // Resolve the collision along the shallow axis.
                    if (absDepthY < absDepthX)
                    {
                        // If we crossed the top of a tile, we are on the ground.
                        if (previousBottom <= tileBounds.Top)
                            isOnGround = true;

                        // Ignore platforms, unless we are on the ground.
                        if (isOnGround)
                        {
                            // Resolve the collision along the Y axis.
                            playing.currentCorner[1] += (int)depth.Y;

                            // Perform further collisions with the new bounds.
                            bounds = new Rectangle(playing.currentCorner[0] + coords[0], playing.currentCorner[1] + coords[1], 64, 128);
                        }
                    }
                    else
                    {
                        // Resolve the collision along the X axis.
                        playing.currentCorner[0] += (int)depth.X;

                        // Perform further collisions with the new bounds.
                        bounds = new Rectangle(playing.currentCorner[0] + coords[0], playing.currentCorner[1] + coords[1], 64, 128);
                    }
                }
                //int overlapx = (tile.getCoords()[0]) - (coords[0] + playing.currentCorner[0]);
                //int overlapy = (tile.getCoords()[1]) - (coords[0] + playing.currentCorner[1]);
                //if (Math.Abs(overlapy) < Math.Abs(overlapx)) { playing.currentCorner[1] -= overlapy; jumping = false; }
                //else { playing.currentCorner[0] -= overlapx;  }
            }
            previousBottom = bounds.Bottom;
        }
    }
    class Enemy : Drawable
    {
    }
    class Item : Drawable
    {
    }
    class Projectile : Drawable
    {
    }
    class Particle : Drawable
    {
    }
    class StaticObject : Drawable
    {
    }

    class Drawable
    {
        private string tag;
        public double time = 0;
        public int[] coords;
        public int level;
        public double rotation;
        public Rectangle spriteRectangle;
        //use level for animated sprites

        public bool toBeDeleted = false;
        public bool escaped = false;

        public void setTag(string tag) { this.tag = tag; }
        public string getTag() { return this.tag; }
        public Rectangle getSpriteRectangle() { return spriteRectangle; }
        public void setSpriteRectangle(Rectangle spriteRectangle) { this.spriteRectangle = spriteRectangle; }
        public Rectangle locationRectangle { get { return locationRectangle; } set { locationRectangle = value; } }

        public bool collides(Drawable other)
        {
            return this.locationRectangle.Intersects(other.locationRectangle);
        }
        public bool collides(Rectangle other)
        {
            return this.locationRectangle.Intersects(other);
        }
    }

    public class PrioQueue
    {
        int total_size;
        SortedDictionary<int, Queue> storage;

        public PrioQueue()
        {
            this.storage = new SortedDictionary<int, Queue>();
            this.total_size = 0;
        }

        public int Size() { return total_size; }

        public bool IsEmpty()
        {
            return (total_size == 0);
        }

        public object Dequeue()
        {
            if (IsEmpty())
            {
                throw new Exception("Please check that priorityQueue is not empty before dequeing");
            }
            else
                foreach (Queue q in storage.Values)
                {
                    // we use a sorted dictionary
                    if (q.Count > 0)
                    {
                        total_size--;
                        return q.Dequeue();
                    }
                }

            Debug.Assert(false, "not supposed to reach here. problem with changing total_size");

            return null; // not supposed to reach here.
        }

        // same as above, except for peek.

        public object Peek()
        {
            if (IsEmpty())
                throw new Exception("Please check that priorityQueue is not empty before peeking");
            else
                foreach (Queue q in storage.Values)
                {
                    if (q.Count > 0)
                        return q.Peek();
                }

            Debug.Assert(false, "not supposed to reach here. problem with changing total_size");

            return null; // not supposed to reach here.
        }

        public object Dequeue(int prio)
        {
            total_size--;
            return storage[prio].Dequeue();
        }

        public void Enqueue(object item, int prio)
        {
            if (!storage.ContainsKey(prio))
            {
                storage.Add(prio, new Queue());
            }
            storage[prio].Enqueue(item);
            total_size++;

        }
    }
}

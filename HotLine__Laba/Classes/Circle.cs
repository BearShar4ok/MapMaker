using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HotLine__Laba.Classes
{
    public interface ICollidable
    {
        void Update(Vector2 mapPos);
        bool Intersects(ICollidable collider);
    }
    class Circle : ICollidable
    {
        public Rectangle rec;
        public Vector2 basePos;
        public Circle(int x, int y, int width, int height)
        {
            x -= width / 2;
            y -= height / 2;
            Center = new Point(x + width / 2, y + height / 2);
            Size = new Point(width, height);
            rec = new Rectangle(x, y, width, height);
            basePos = new Vector2(x,y);
        }
        public Point Size { get; private set; }
        public Point Center { get; private set; }
        // public int Radius { get; private set; }
        public void Update(Vector2 mapPos)
        {
            rec.X = (int)(basePos.X + mapPos.X);
            rec.Y = (int)(basePos.Y + mapPos.Y);
        }
        public bool Intersects(ICollidable colliderObj)
        {
            if (colliderObj is Circle)
            {

            }
            if (colliderObj is Collider) DoRectangleCircleOverlap((colliderObj as Collider).rec);
            return false;
        }
        private bool DoRectangleCircleOverlap(Rectangle rect)
        {
            int Radius = Size.X / 2;
            // Get the rectangle half width and height
            float rW = (rect.Width) / 2;
            float rH = (rect.Height) / 2;

            // Get the positive distance. This exploits the symmetry so that we now are
            // just solving for one corner of the rectangle (memory tell me it fabs for 
            // floats but I could be wrong and its abs)
            float distX = Math.Abs(Center.X - (rect.Left + rW));
            float distY = Math.Abs(Center.Y - (rect.Top + rH));

            if (distX >= Radius + rW || distY >= Radius + rH)
            {
                // Outside see diagram circle E
                return false;
            }
            if (distX < rW || distY < rH)
            {
                // Inside see diagram circles A and B
                return true; // touching
            }

            // Now only circles C and D left to test
            // get the distance to the corner
            distX -= rW;
            distY -= rH;

            // Find distance to corner and compare to circle radius 
            // (squared and the sqrt root is not needed
            if (distX * distX + distY * distY < Radius * Radius)
            {
                // Touching see diagram circle C
                return true;
            }
            return false;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace HotLine__Laba.Classes.Connectors
{
    class Target
    {

        private Vector2 position;
        private Rectangle bound;
        public Vector2 Position
        {
            get
            {
                return position;
            }

        }
        public Rectangle Bound
        {
            get
            {
                return bound;
            }
        }
        public Target()
        {

        }
        public void Update(Vector2 pos, Rectangle bound)
        {
            position = pos;
            this.bound = bound;
        }
        
    }
}

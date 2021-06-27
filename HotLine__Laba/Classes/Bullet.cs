using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace HotLine__Laba.Classes
{
    class Bullet
    {
        private Texture2D texture;
        public Rectangle rectangle;
        private Vector2 position;
        private int speed;
        double x;
        double y;
        float rotation;
        int animeStop;
        public Rectangle bound;
       
        public Bullet(Vector2 pos, float rot,int X,int Y)
        {
            speed = 24;
            position = pos;
            rectangle = new Rectangle(0, 0, 48, 48);
            bound= new Rectangle((int)pos.X, (int)pos.Y, 48, 48);
            texture = null;
            rotation = rot-(float)(Math.PI*1.5);
            x = X-position.X;
            y = Y-position.Y;
            double a = (Math.Sqrt(x * x + y * y)) / speed;
            x /= a;
            y /= a;
            animeStop = 0;

        }
        public void LoadContent(ContentManager content)
        {
            texture=content.Load<Texture2D>("splBullet_0");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, rotation, new Vector2(16, 16),0.17f, SpriteEffects.None, 0);
        }
        public void Update( )
        {

            bound.X = (int)position.X;
            bound.Y = (int)position.Y;
            if (animeStop<=0)
            {
                position.X += (int)x*2;
                position.Y += (int)y * 2;
                animeStop = 5;
            }
            else
            {
                animeStop--;
            }
        }

    }
}

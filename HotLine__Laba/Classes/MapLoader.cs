using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HotLine__Laba.Classes
{
    class MapLoader
    {
        Vector2 position;
        Rectangle bound;
        Map newMap;
        string textureName;
        Vector2 mappos;
        Vector2 correctPos;
        Texture2D texture;
        public MapLoader(string texture, Vector2 pos,Vector2 mappos)
        {
            textureName = texture;
            this.mappos = mappos;
            position = pos;
            texture = null;
        }
        public void LoadContent(ContentManager content)
        {
           newMap=new Map( content.Load<Texture2D>(textureName),mappos);
            texture=content.Load<Texture2D>("EnemyStay");

        }
        public Map Update(Vector2 pos, Rectangle boundBox)
        {
            
            correctPos = position + pos;
            KeyboardState key = Keyboard.GetState();
            if (boundBox.Intersects(bound) && key.IsKeyDown(Keys.E)) ;
            {
                newMap.OpenNow = true;
            }
            return newMap;
        }
        public void Draw(SpriteBatch brush)
        {
            brush.Draw(texture, correctPos, Color.White);
        }




    }
}

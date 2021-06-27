using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace HotLine__Laba.Classes
{
    class Map
    {
        private Texture2D map;
        private int width;
        private int height;
        private Vector2 position;
        public bool OpenNow { get; set; }
        public Map(Vector2 pos)
        {
            map = null;
            position = pos;
            OpenNow = false;
        }
        public void LoadContent(ContentManager content, GraphicsDevice gd)
        {
            UpLoader.Load();
            UpLoader.makingMap();
            width = UpLoader.width;
            height = UpLoader.height;
            // width и height передавать через json
            Color[] frame = new Color[width * height];   //заполнить из известной даты
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    byte A = Convert.ToByte(255);
                    frame[y * width + x] = new Color(UpLoader.image2d[x, y].R, UpLoader.image2d[x, y].G, UpLoader.image2d[x, y].B, A);
                }
            }
            Texture2D textureCurrent = new Texture2D(gd, width, height);
            textureCurrent.SetData(frame);
            map = textureCurrent;
            //map = content.Load<Texture2D>("unknown");

        }
        public void Update(Vector2 position)
        {
            this.position = position;
            foreach (var item in UpLoader.new_dict[Colisions.wall])
            {
                if (item is ICollidable)
                {
                    (item as ICollidable).Update(position);
                }
            }
        }
        public void Draw(SpriteBatch brushe)
        {
            brushe.Draw(map, position, Color.White);
            foreach (var item in UpLoader.new_dict[Colisions.wall])
            {
                if (item is Collider)
                {
                    brushe.Draw(map, ((Collider)item).rec, Color.Black);
                }
                else
                {
                    brushe.Draw(map, ((Circle)item).rec, Color.Black);
                }
            }
        }
    }
}

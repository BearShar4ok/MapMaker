using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HotLine__Laba.Classes;
using HotLine__Laba.Classes.Connectors;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
namespace HotLine__Laba.Classes
{
    class CircleCollider
    {
        public Rectangle rec;
        public Vector2 pos;
        public CircleCollider(Rectangle rec)
        {
            this.pos = new Vector2(rec.X, rec.Y);
            this.rec = rec;
        }
        public void Update(Vector2 mapPos)
        {
            rec.X = (int)(pos.X + mapPos.X);
            rec.Y = (int)(pos.Y + mapPos.Y);
        }
    }
}

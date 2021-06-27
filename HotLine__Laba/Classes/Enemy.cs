using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace HotLine__Laba.Classes
{
    class Enemy
    {
        static private Random random = new Random();
        private Texture2D bathit;
        private Texture2D texture;
        private Texture2D texture2;
        private Texture2D die1;
        private Texture2D die2;
        private int numOfDieTexture;
        private Vector2 position;
        private int speed;
        private Vector napravlenie;
        public PlayerStatus status;
        public int interval;
        private Texture2D stay;
        private Vector2 origin2;
        private int currentFrame;
        private int spriteWidth;
        private int spriteHeight;
        private int spriteWidth2;
        private int spriteHeight2;
        private bool rightOrLeft;
        public Rectangle sourceRectangle;
        int animeitPause;
        private int spriteWidth1;
        private int spriteHeight1;
        private float rotation;
 
        public Rectangle boundBox;
        int agrRadius;
        bool flag = true;
        private Vector2 correctPos;

        public Enemy(Vector2 pos)
        {
            napravlenie = Vector.righ;
            texture = null;
            texture2 = null;
            die1 = null;
            die2 = null;
            stay = null;
            bathit = null;
            position = pos;
       
            speed = 1;
            status = PlayerStatus.hit;
            agrRadius = 16;
            origin2 = new Vector2(45 / 2, 29 / 2);
            spriteHeight = 32;
            spriteWidth = 45;
            spriteHeight1 = 33;
            spriteWidth1 = 26;
            spriteHeight2 = 40;
            spriteWidth2 = 51;
            currentFrame = 0;
            animeitPause = 3;
            rotation = 0f;

            rightOrLeft = true;
            interval = 10;
            correctPos = Vector2.Zero;
            boundBox = new Rectangle((int)correctPos.X, (int)correctPos.Y, 40,40);

        }
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("EnemyLeft");
            texture2 = content.Load<Texture2D>("EnemyRight");
            stay = content.Load<Texture2D>("EnemyStay");
            die1 = content.Load<Texture2D>("EnemyDie1");
            die2 = content.Load<Texture2D>("EnemyDie2");
            bathit = content.Load<Texture2D>("enemyHit");
        }
        public void Draw(SpriteBatch brushe)
        {
            switch (status)
            {
                case PlayerStatus.stay:
                        brushe.Draw(stay, correctPos, sourceRectangle, Color.White,
                        rotation, origin2, 1, SpriteEffects.None, 0);
                    break;
                case PlayerStatus.go:
                    if (rightOrLeft)
                    {
                        brushe.Draw(texture, correctPos, sourceRectangle, Color.White,
                        rotation, origin2, 1, SpriteEffects.None, 0);
                    }
                    else
                    {
                        brushe.Draw(texture2, correctPos, sourceRectangle, Color.White,
                        rotation, origin2, 1, SpriteEffects.None, 0);

                    }
                    break;
                case PlayerStatus.die:
                    if (flag)
                    {
                        numOfDieTexture = random.Next(0, 2);
                        flag = false;

                    }
                    if (numOfDieTexture == 0)
                    {
                        brushe.Draw(die1, correctPos, Color.White);
                    }
                    else
                    {
                        brushe.Draw(die2, correctPos, Color.White);
                    }




                    break;
                case PlayerStatus.hit:
                    brushe.Draw(bathit, correctPos, sourceRectangle, Color.White,
                      rotation, origin2, 1, SpriteEffects.None, 0);
                
                    break;
                default:
                    break;
            }
        }
        public void Update(Vector2 pos,Vector2 target,Rectangle bound)
        {
            boundBox.X = (int)correctPos.X;
            boundBox.Y = (int)correctPos.Y;
            correctPos = position + pos;
            if (PlayerStatus.die!=status)
            {
                
                if (PlayerStatus.hit == status)
                {
                    sourceRectangle = new Rectangle(spriteWidth2 * currentFrame, 0, spriteWidth2, spriteHeight2);
                    if (animeitPause <= 0)
                    {
                        if (currentFrame < 5)
                        {
                            currentFrame++;

                        }
                        else
                        {
                            currentFrame = 0;
                            status = PlayerStatus.stay;
                            interval = 100;

                        }
                        animeitPause = 3;
                    }
                    animeitPause--;
                }
                else
                {
                    int rangeX = (int)correctPos.X - (int)target.X;
                    int rangeY = (int)correctPos.Y - (int)target.Y;
                    if (rangeX < 0)
                    {
                        rangeX *= -1;
                    }
                    if (rangeY < 0)
                    {
                        rangeY *= -1;
                    }
                    #region направления
                    if (correctPos.X > target.X && correctPos.Y > target.Y && Math.Sqrt(rangeY + rangeX) < agrRadius)
                    {
                        napravlenie = Vector.upLeft;
                        status = PlayerStatus.go;
                        position.X -= speed;
                        position.Y -= speed;
                        rotation = (float)(Math.PI * 1.25);

                    }
                    else if (correctPos.X < target.X && correctPos.Y > target.Y && Math.Sqrt(rangeY + rangeX) < agrRadius)
                    {
                        napravlenie = Vector.upRight;
                        status = PlayerStatus.go;
                        position.X += speed;
                        position.Y -= speed;
                        rotation = (float)(Math.PI * 1.75);

                    }
                    else if (correctPos.X < target.X && correctPos.Y < target.Y && Math.Sqrt(rangeY + rangeX) < agrRadius)
                    {
                        napravlenie = Vector.downRight;
                        status = PlayerStatus.go;
                        position.X += speed;
                        position.Y += speed;
                        rotation = (float)(Math.PI / 4);
                    }
                    else if (correctPos.X > target.X && correctPos.Y < target.Y && Math.Sqrt(rangeY + rangeX) < agrRadius)
                    {
                        napravlenie = Vector.downLeft;
                        status = PlayerStatus.go;
                        position.X -= speed;
                        position.Y += speed;
                        rotation = (float)(Math.PI * 0.75);

                    }
                    else if (correctPos.X < target.X && Math.Sqrt(rangeY + rangeX) < agrRadius)
                    {
                        napravlenie = Vector.righ;
                        status = PlayerStatus.go;
                        position.X += speed;
                        rotation = 0f;
                    }
                    else if (correctPos.X > target.X && Math.Sqrt(rangeY + rangeX) < agrRadius)
                    {
                        napravlenie = Vector.left;
                        status = PlayerStatus.go;
                        position.X -= speed;
                        rotation = (float)(Math.PI);

                    }
                    else if (correctPos.Y < target.Y && Math.Sqrt(rangeY + rangeX) < agrRadius)
                    {
                        napravlenie = Vector.down;
                        status = PlayerStatus.go;

                        rotation = (float)(Math.PI / 2);
                        position.Y += speed;
                    }
                    else if (correctPos.Y > target.Y && Math.Sqrt(rangeY + rangeX) < agrRadius)
                    {
                        napravlenie = Vector.up;
                        status = PlayerStatus.go;
                        rotation = (float)(Math.PI * 1.5);
                        position.Y -= speed;
                    }
                    else
                    {
                        status = PlayerStatus.stay;
                    }
                    #endregion
                    /// if ()
                    /// {
                    ///     interval = 20;
                    ///     status = PlayerStatus.hit;
                    ///     //status = PlayerStatus.stay;
                    /// }

                    if (status == PlayerStatus.go)
                    {
                        if (rightOrLeft)
                        {
                            sourceRectangle = new Rectangle(spriteWidth * currentFrame, 0, spriteWidth, spriteHeight);
                            if (animeitPause <= 0)
                            {
                                if (currentFrame < 9)
                                {
                                    currentFrame++;

                                }
                                else
                                {
                                    currentFrame = 0;
                                    rightOrLeft = false;
                                }

                                animeitPause = 3;
                            }
                            animeitPause--;
                        }
                        else
                        {
                            sourceRectangle = new Rectangle(spriteWidth * currentFrame, 0, spriteWidth, spriteHeight);
                            if (animeitPause <= 0)
                            {
                                if (currentFrame < 9)
                                {
                                    currentFrame++;

                                }
                                else
                                {
                                    currentFrame = 0;
                                    rightOrLeft = true;
                                }
                                animeitPause = 3;
                            }
                            animeitPause--;
                        }
                    }
                    interval--;
                    if (status == PlayerStatus.stay)
                    {
                        sourceRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
                    }
                }
            }
            
        }
    }
}

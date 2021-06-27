using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace HotLine__Laba.Classes
{
    enum PlayerStatus
    {
        stay,go,die,hit,shoot
    }
    enum Vector
    {
        up,down,left,righ,upRight,upLeft,downRight,downLeft
    }
    class Player
    {
        static private Random random = new Random();
        private int numOfPatrons;
        private Texture2D bathit;
        private Texture2D texture;
        private Texture2D texture2;
        private Texture2D die1;
        private Texture2D die2;
        private int numOfDieTexture;
        private Vector2 position;
        private int speed;
        private Vector napravlenie;
        int interval;
        public PlayerStatus status;
        
        private Texture2D stay;
        private Vector2 origin2;
        public int currentFrame;
        private int spriteWidth;
        private int spriteHeight;
        private int spriteWidth2;
        private int spriteHeight2;
        private bool rightOrLeft;
        private Rectangle sourceRectangle;
        int animeitPause;
        private Vector2 mapPosition;
        private int spriteWidth1;
        private int spriteHeight1;
        private float rotation;
        public int health;
        bool isUp = false;
        bool isDown = false;
        bool isLeft = false;
        bool isRight = true;
        bool flag = true;
        private bool freecam;
        Texture2D shoot;
        ContentManager manager;
        public List<Bullet> bullets = new List<Bullet>();
        public Rectangle SourceRectangle {
            get 
            {
                return sourceRectangle;
            }
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
        public Rectangle Bound
        {
            get;
            set;
        }
      
     
        public Player(Vector2 pos)
        {
            health = 2;
            napravlenie = Vector.righ;
            texture = null;
            texture2 = null;
            die1 = null;
            shoot = null;
            die2 = null;
            stay = null;
            bathit = null;
            position = pos;
            mapPosition = Vector2.Zero;
            speed = 3;
            status = PlayerStatus.go;
            freecam = false;
            origin2 = new Vector2(15, 18);
            spriteHeight = 29;
            spriteWidth = 45;
            spriteHeight1 = 33;
            spriteWidth1 = 26;
            spriteHeight2 = 40;
            spriteWidth2 = 53;
            currentFrame = 0;
            animeitPause = 3;
            rotation = 0f;
            rightOrLeft = true;
            interval = 0;
            Bound = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            numOfPatrons = 32;

        }
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("right2");
            texture2 = content.Load<Texture2D>("right");
            stay = content.Load<Texture2D>("Stay_Player");
            die1 = content.Load<Texture2D>("die1");
            die2 = content.Load<Texture2D>("die2");
            bathit = content.Load<Texture2D>("bathit");
            shoot = content.Load<Texture2D>("Shoot");
            manager = content;
        }
        public void Draw(SpriteBatch brushe)
        {
            foreach (var item in bullets)
            {
                item.Update();
            }
            switch (status)
            {
                case PlayerStatus.stay:
                    brushe.Draw(stay, position, sourceRectangle, Color.White,
                       rotation, origin2, 1.16f, SpriteEffects.None, 0);
                    break;
                case PlayerStatus.go:
                    if (rightOrLeft)
                    {
                        brushe.Draw(texture, position, sourceRectangle, Color.White,
                        rotation, origin2, 1.16f, SpriteEffects.None, 0);
                    }
                    else
                    {
                        brushe.Draw(texture2, position, sourceRectangle, Color.White,
                        rotation, origin2, 1.16f, SpriteEffects.None, 0);
                        
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
                        brushe.Draw(die1, position, Color.White);
                    }
                    else
                    {
                        brushe.Draw(die2, position, Color.White);
                    }
                   



                    break;
                case PlayerStatus.hit:
                    brushe.Draw(bathit, position, sourceRectangle, Color.White,
                      rotation, origin2, 1.16f, SpriteEffects.None, 0);

                    break;
                case PlayerStatus.shoot:
                    brushe.Draw(shoot, position, sourceRectangle, Color.White,
                      rotation, origin2, 1.16f, SpriteEffects.None, 0);
                    
                    break;
                default:
                    break;

            }
            foreach (var item in bullets)
            {
                item.Draw(brushe);
            }
           

        }
        public Vector2 Update()
        {
           
            KeyboardState keyboardState = Keyboard.GetState();

            if (status != PlayerStatus.die)
            {
                var mouse = Mouse.GetState();
                if (mouse.X != position.X)
                {
                    float angle = (float)Math.Atan((mouse.Y - position.Y) / (mouse.X - position.X));

                    if (mouse.X < position.X)
                    {
                        angle += (float)Math.PI;
                    }
                    rotation = angle;
                }
                if (status !=PlayerStatus.hit)
                {
                    if (status == PlayerStatus.shoot)
                    {
                        sourceRectangle = new Rectangle(50 * currentFrame, 0, 50, 30);
                        if (animeitPause <= 0)
                        {
                            if (currentFrame < 1)
                            {
                                currentFrame++;

                            }
                            else
                            {
                                currentFrame = 0;
                                status = PlayerStatus.stay;
                                interval = 50;
                                Bullet b = new Bullet(position, rotation, mouse.X, mouse.Y);
                                b.LoadContent(manager);
                                bullets.Add(b);
                                numOfPatrons--;
                            }
                            animeitPause = 5;
                        }
                        animeitPause--;
                    }
                    else
                    {
                        if (keyboardState.IsKeyDown(Keys.W))
                        {
                            status = PlayerStatus.go;
                            if (freecam)
                            {
                                position.Y -= speed;
                            }
                            else
                            {
                                mapPosition.Y += speed;
                            }

                            isDown = true;
                            isUp = false;


                        }
                        else if (keyboardState.IsKeyDown(Keys.S))
                        {
                            status = PlayerStatus.go;
                            if (freecam)
                            {
                                position.Y += speed;
                            }
                            else
                            {
                                mapPosition.Y -= speed;
                            }
                            isDown = false;
                            isUp = true;

                        }
                        else
                        {
                            status = PlayerStatus.go;
                            isDown = false;
                            isUp = false;
                        }


                        if (keyboardState.IsKeyDown(Keys.A))
                        {
                            status = PlayerStatus.go;
                            if (freecam)
                            {
                                position.X -= speed;
                            }
                            else
                            {
                                mapPosition.X += speed;
                            }
                            isLeft = true;
                            isRight = false;

                        }
                        else if (keyboardState.IsKeyDown(Keys.D))
                        {
                            status = PlayerStatus.go;
                            if (freecam)
                            {
                                position.X += speed;
                            }
                            else
                            {
                                mapPosition.X -= speed;
                            }
                            isRight = true;
                            isLeft = false;

                        }
                        else
                        {
                            status = PlayerStatus.go;
                            isRight = false;
                            isLeft = false;
                        }

                        //тесты
                        if (keyboardState.IsKeyDown(Keys.L))
                        {
                            status = PlayerStatus.die;
                        }
                        //тесты


                        if (keyboardState.IsKeyDown(Keys.Space) && interval <= 0)
                        {
                            status = PlayerStatus.hit;
                            currentFrame = 0;

                        }
                        if (keyboardState.IsKeyDown(Keys.E) && interval <= 0 && numOfPatrons > 0)
                        {
                            status = PlayerStatus.shoot;
                            currentFrame = 0;

                        }

                        if (status == PlayerStatus.go)
                        {
                            if (isUp && isRight)
                            {
                                napravlenie = Vector.upRight;



                            }
                            else if (isUp && isLeft)
                            {
                                napravlenie = Vector.upLeft;


                            }
                            else if (isDown && isLeft)
                            {
                                napravlenie = Vector.downLeft;

                            }
                            else if (isDown && isRight)
                            {
                                napravlenie = Vector.downRight;

                            }
                            else if (isUp)
                            {
                                napravlenie = Vector.up;
                            }
                            else if (isDown)
                            {
                                napravlenie = Vector.down;


                            }
                            else if (isRight)
                            {
                                napravlenie = Vector.righ;

                            }
                            else if (isLeft)
                            {
                                napravlenie = Vector.left;


                            }
                            else
                            {
                                status = PlayerStatus.stay;
                            }
                        }//выбор направления

                       






                        System.Diagnostics.Debug.WriteLine(rotation);
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


                        if (status == PlayerStatus.stay)
                        {
                            sourceRectangle = new Rectangle(0, spriteHeight1 * currentFrame, spriteWidth1, spriteHeight1);
                            if (animeitPause <= 0)
                            {
                                if (currentFrame < 5)
                                {
                                    currentFrame++;

                                }
                                else
                                {
                                    currentFrame = 0;

                                }
                                animeitPause = 20;
                            }
                            animeitPause--;
                        }
                        interval--;
                    }
                    
                }
                else
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
                            interval = 50;

                        }
                        animeitPause = 3;
                    }
                    animeitPause--;
                }






            }
            
            return mapPosition;
        }
    }
}

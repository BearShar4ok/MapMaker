using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HotLine__Laba.Classes;
using HotLine__Laba.Classes.Connectors;
namespace HotLine__Laba
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private Map map;
        private Enemy enemy = new Enemy(new Vector2(200,200));
        private Target playerT = new Target();
        private Vector2 position;
        private MouseState lastMouseState;
       
        //MapLoader loader;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(new Vector2((_graphics.PreferredBackBufferWidth-40)/2, (_graphics.PreferredBackBufferHeight-26)/2));
            //loader = new MapLoader("die1", new Vector2(100, 100), new Vector2(0, 0));
            map = new Map(Vector2.Zero);
            base.Initialize();
           
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            //map.LoadContent(Content);
            enemy.LoadContent(Content);
            map.LoadContent(Content, GraphicsDevice);
            //loader.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            // Exit();

            // TODO: Add your update logic here
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X != lastMouseState.X ||
                currentMouseState.Y != lastMouseState.Y)
                position = new Vector2(currentMouseState.X, currentMouseState.Y);
            lastMouseState = currentMouseState;
            Vector2 pos = player.Update();
            playerT.Update(player.Position,player.SourceRectangle);
            enemy.Update(pos,playerT.Position,playerT.Bound);
            //Map m=loader.Update(pos, playerT.Bound);
            //if (m.OpenNow)
            //{
            //    map = m;
            //}
            map.Update(pos);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            map.Draw(_spriteBatch);
            enemy.Draw(_spriteBatch);
            player.Draw(_spriteBatch);
            //loader.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using MyGame.TestGame.Factories;
//using MyGame.TestGame.Physics;
using MyGame.TestGame.Physics.Integrators;
using MyGame.TestGame.Systems;

namespace MyGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;
        //Texture2D texture;
        private GameManager gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 1024;
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            gameManager = new GameManager();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            gameManager.AddSystem(new SpriteRendererSystem(gameManager, this));
            gameManager.AddSystem(new PlayerInputSystem(gameManager));
            gameManager.AddSystem(new SimplePhysicsSystem(gameManager, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), new VerletNoVelocityIntegrator(this, 0.01f)));
            //gameManager.AddSystem(new SimplePhysicsSystem(gameManager, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), new ForwardEulerIntegrator(this)));
            gameManager.Initialize();


            var centerPos = new Vector3(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, 0);

            GellyFactory.CreateControllableCube(centerPos, gameManager, 320);


            //new SpringComponent(80, dampness, eastBall, westBall);


            //new SpringComponent(800, dampness, northBall, westBall);
            //new SpringComponent(80, dampness, northBall, southBall);

            //new Line2DComponent(eastBall, northBall, Color.Red);
            //new Line2DComponent(eastBall, westBall, Color.Red);
            //new Line2DComponent(eastBall, southBall, Color.Red);
            //new Line2DComponent(StationaryBall, eastBall, Color.Red);
            //new Line2DComponent(StationaryBall, westBall, Color.Red);
            //new Line2DComponent(northBall, westBall, Color.Red);
            //new Line2DComponent(southBall, westBall, Color.Red);
            //new Line2DComponent(southBall, northBall, Color.Red);

            //new Line2DComponent(StationaryBall, northBall, Color.Red);
            //new Line2DComponent(StationaryBall, southBall, Color.Red);
            //new Line2DComponent(southBall, northBall, Color.Red);
            //new SimpleRigidbodyComponent(ball);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //_spriteBatch = new SpriteBatch(GraphicsDevice);

            //texture = Content.Load<Texture2D>("character1_without_arm");
            // TODO: use this.Content to load your game content here
        }
        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            gameManager.Update(gameTime);
            //

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            gameManager.Draw();
            //// TODO: Add your drawing code here
            //_spriteBatch.Begin();
            //_spriteBatch.Draw(texture, destinationRectangle: new Rectangle(-(texture.Width/4),0,texture.Width, texture.Height), Color.White);
            //_spriteBatch.Draw(texture, destinationRectangle: new Rectangle(-(texture.Width/6),0,texture.Width, texture.Height), Color.White);
            ////_spriteBatch.Draw(texture, destinationRectangle: new Rectangle(50,50,300,300),null, Color.White, 90f, new Vector2(texture.Width/2,texture.Height/2),SpriteEffects.None, layerDepth: 1);
            ////_spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            ////_spriteBatch.Draw(texture, new Vector2(100,0), Color.White);
            //_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

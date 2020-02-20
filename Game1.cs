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

            base.Initialize();
            gameManager.AddSystem(new SpriteRendererSystem(gameManager, this));
            gameManager.AddSystem(new PlayerInputSystem(gameManager));
            gameManager.AddSystem(new CollisionSystem(gameManager));
            //gameManager.AddSystem(new SimplePhysicsSystem(gameManager, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), new VerletNoVelocityIntegrator(this, 0.01f)));
            gameManager.AddSystem(new SimplePhysicsSystem(gameManager, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), new ForwardEulerIntegrator(this)));
            gameManager.Initialize();


            var centerPos = new Vector3(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, 0);

            //JellyFactory.CreateControllabelJellyCube(centerPos, gameManager, 320);
            //JellyFactory.CreateControllableTriangle(centerPos, gameManager, 20, Color.CornflowerBlue);
            JellyFactory.CreateControllableCube(centerPos, gameManager, 60, Color.White);
            JellyFactory.CreateCube(new Vector3(400, 400, 0), gameManager, 120, Color.White);
            JellyFactory.CreateCube(new Vector3(400, 600, 0), gameManager, 120, Color.White, rotation: (2*MathHelper.Pi/4));
            JellyFactory.CreateCube(new Vector3(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight * 1.7f, 0), gameManager, 1500, Color.White, false);
            JellyFactory.CreateCube(new Vector3(_graphics.PreferredBackBufferWidth * 1.5f, _graphics.PreferredBackBufferHeight / 2, 0), gameManager, 1500, Color.White, false);
            JellyFactory.CreateCube(new Vector3(-_graphics.PreferredBackBufferWidth * 0.5f, _graphics.PreferredBackBufferHeight / 2, 0), gameManager, 1500, Color.White, false);
            JellyFactory.CreateCube(new Vector3(_graphics.PreferredBackBufferWidth / 2, -_graphics.PreferredBackBufferHeight * 0.7f, 0), gameManager, 1500, Color.White, false);
            //JellyFactory.CreateNonCollidingCube(new Vector3(420, 400, 0), gameManager, 20, Color.Red);

            //JellyFactory.CreateRandomShape(new Vector3(400,400,0), gameManager, 60, Color.White);
            //JellyFactory.CreateRandomShape(new Vector3(800,400,0), gameManager, 60, Color.White);
            //JellyFactory.CreateRandomShape(new Vector3(400,800,0), gameManager, 60, Color.White);
            //JellyFactory.CreateRandomShape(new Vector3(200,600,0), gameManager, 60, Color.White);
            //JellyFactory.CreateRandomShape(new Vector3(700,300,0), gameManager, 60, Color.White);



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

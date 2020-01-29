using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
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

            //Entity player = new Entity(gameManager);
            //var playerRect = new Rectangle(586 / 3, 586 / 4, 200, 400);
            //var playerSpawn = new Vector3(200, 1024 - playerRect.Height, 0);
            //new TransformComponent(player, playerSpawn);
            //new SpriteComponent(player, "Man",0.5f,Color.White,new Vector2(0.5f,0.5f), 0, SpriteEffects.None, playerRect);
            //new PlayerInputComponent(player);
            //new BoxColliderComponent(player, new Rectangle((int)playerSpawn.X, (int)playerSpawn.Y, playerRect.Width, playerRect.Height));
            //new SimpleRigidbodyComponent(player);
            //var background = new Entity(gameManager);
            //new SpriteComponent(background, "stars",0, new Vector2(2,2));

            var centerPos = new Vector3(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, 0);
            var StationaryBall = new Entity(gameManager, centerPos);
            new SpriteComponent(StationaryBall, "ball",1,Color.Red,Vector2.One*0.5f);
            new RigidBodyComponent(StationaryBall,1, SimulationObjectType.Passive);
            new BoxColliderComponent(StationaryBall, new Rectangle(0, 0, 68, 68));
            new PlayerInputComponent(StationaryBall);


            var dist = _graphics.PreferredBackBufferWidth / 4;
            var southballPos = centerPos + -Vector3.Down * dist;
            var southBall = new Entity(gameManager, southballPos) ;
            new SpriteComponent(southBall, "ball", 1, Color.Red, Vector2.One * 1f);
            new BoxColliderComponent(southBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(southBall, 5f, SimulationObjectType.Active);

            var eastBallPos = centerPos + Vector3.Right * dist;
            var eastBall = new Entity(gameManager,eastBallPos);
            new SpriteComponent(eastBall, "ball", 1, Color.Red, Vector2.One * 1f);
            new BoxColliderComponent(eastBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(eastBall, 5f, SimulationObjectType.Active);

            var westBallPos = centerPos + Vector3.Right * -dist;
            var westBall = new Entity(gameManager, westBallPos);
            new SpriteComponent(westBall, "ball", 1, Color.Red, Vector2.One * 1f);
            new BoxColliderComponent(westBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(westBall, 5f, SimulationObjectType.Active);

            var northBallPos = centerPos + Vector3.Down * dist;
            var northBall = new Entity(gameManager, northBallPos);
            new SpriteComponent(northBall, "ball",1,Color.Red,Vector2.One*1f);
            new BoxColliderComponent(northBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(northBall, 5f, SimulationObjectType.Active);

            var northEastBallPos = eastBallPos + .5f * (northBallPos - eastBallPos);
            var northEastBall = new Entity(gameManager, northEastBallPos);
            new SpriteComponent(northEastBall, "ball",1,Color.Red,Vector2.One*1f);
            new BoxColliderComponent(northEastBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(northEastBall, 5f, SimulationObjectType.Active);

            var northwestBallPos = westBallPos + .5f * (northBallPos - westBallPos);
            var northwestBall = new Entity(gameManager, northwestBallPos);
            new SpriteComponent(northwestBall, "ball",1,Color.Red,Vector2.One*1f);
            new BoxColliderComponent(northwestBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(northwestBall, 5f, SimulationObjectType.Active);

            var southEastBallPos = eastBallPos+ .5f * (southballPos - eastBallPos);
            var southEastBall = new Entity(gameManager, southEastBallPos);
            new SpriteComponent(southEastBall, "ball",1,Color.Red,Vector2.One*1f);
            new BoxColliderComponent(southEastBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(southEastBall, 5f, SimulationObjectType.Active);

            var southWestBallPos = westBallPos + .5f * (southballPos - westBallPos);
            var southWestBall = new Entity(gameManager, southWestBallPos);
            new SpriteComponent(southWestBall, "ball",1,Color.Red,Vector2.One*1f);
            new BoxColliderComponent(southWestBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(southWestBall, 5f, SimulationObjectType.Active);


            var dampness = 50f;
            var innerDampness = dampness * 2;
            var stiffness = 800;
            var innerStiffness = stiffness * 2;
            //cetner to vertices
            new SpringComponent(innerStiffness, innerDampness, StationaryBall, northBall);
            new SpringComponent(innerStiffness, innerDampness, StationaryBall, southBall);
            new SpringComponent(innerStiffness, innerDampness, StationaryBall, eastBall);
            new SpringComponent(innerStiffness, innerDampness, StationaryBall, westBall);
            new SpringComponent(innerStiffness, innerDampness, StationaryBall, northwestBall);
            new SpringComponent(innerStiffness, innerDampness, StationaryBall, southWestBall);
            new SpringComponent(innerStiffness, innerDampness, StationaryBall, northEastBall);
            new SpringComponent(innerStiffness, innerDampness, StationaryBall, southEastBall);

            //Edges
            new SpringComponent(stiffness, dampness, eastBall, northEastBall);
            new SpringComponent(stiffness, dampness, eastBall, southEastBall);
            new SpringComponent(stiffness, dampness, northEastBall, northBall);
            new SpringComponent(stiffness, dampness, northwestBall, northBall);
            new SpringComponent(stiffness, dampness, northwestBall, westBall);
            new SpringComponent(stiffness, dampness, westBall, southWestBall);
            new SpringComponent(stiffness, dampness, southWestBall, southBall);
            new SpringComponent(stiffness, dampness, southBall, southEastBall);
            //crossesection
            new SpringComponent(stiffness, dampness, southWestBall,northwestBall);
            new SpringComponent(stiffness, dampness, southEastBall,northEastBall);
            new SpringComponent(stiffness, dampness, southEastBall,southWestBall);
            new SpringComponent(stiffness, dampness, northwestBall,northEastBall);
            


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

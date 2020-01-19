using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Systems
{
    public class SpriteRendererSystem : BaseSystem
    {
        private readonly Game game;
        private SpriteBatch spriteBatch;
        private Dictionary<string, Texture2D> TextureDictionary = new Dictionary<string, Texture2D>();

        public SpriteRendererSystem(IManager manager, Game game) : base(manager)
        {
            this.game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public override void Draw()
        {
            //Not sure if needed
            game.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (int i = 0; i < SpriteComponent.Instances.Count; i++)
            {
                var sprite = SpriteComponent.Instances[i];
                //TODO: Do we want to do it like this!??
                var transform = sprite.Entity.GetComponent<TransformComponent>();


                var texture = TextureDictionary[sprite.TextureName];
                spriteBatch.Draw(texture, position: transform.Position,
                    sourceRectangle: sprite.SourceRectangle,
                    color: sprite.Color, 
                    rotation: sprite.Rotation,
                    origin: transform.Position,//TODO: CHECK THIS,
                    scale: sprite.Scale,
                    effects: sprite.SpriteEffects,
                    layerDepth: sprite.Layer
                   );
            }
            spriteBatch.End();
        }

        public override void Initialize()
        {
            base.Initialize();
            var content = this.game.Content;
            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);

            TextureDictionary.Add("Man", content.Load<Texture2D>("character1_without_arm"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using MyGame.TestGame.Components.SpriteComponents;
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
        //public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness)
        //{
        //    // calculate the distance between the two vectors
        //    float distance = Vector2.Distance(point1, point2);

        //    // calculate the angle between the two vectors
        //    float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

        //    DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        //}

        public override void Draw()
        {
            //Not sure if needed
            game.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            for (int i = 0; i < SpriteComponent.Instances.Count; i++)
            {
                var sprite = SpriteComponent.Instances[i];
                //TODO: Do we want to do it like this!??
                //var transform = sprite.Entity.GetComponent<TransformComponent>()?.Position ?? Vector3.Zero;
                //Quick fix...
                var trans2d = new Vector2(sprite.Entity.Position.X, sprite.Entity.Position.Y);

                // calculate the distance between the two vectors

                var texture = TextureDictionary[sprite.TextureName];
                sprite.SourceRectangle = sprite.SourceRectangle ?? new Rectangle(0, 0, texture.Width, texture.Height);

                spriteBatch.Draw(texture, position: trans2d,
                    sourceRectangle: sprite.SourceRectangle,
                    color: sprite.Color,
                    rotation: sprite.Rotation,
                    origin: new Vector2(sprite.SourceRectangle.Value.Width/2, sprite.SourceRectangle.Value.Height/2),
                    scale: sprite.Scale,
                    effects: sprite.SpriteEffects,
                    layerDepth: sprite.Layer
                   );
            }
            for (int i = 0; i < LineComponentBase.Instances.Count; i++)
            {
                var comp = LineComponentBase.Instances[i];
                comp.CalculateLine(out var from, out var distance, out var angle);
                // calculate the angle between the two vectors
                spriteBatch.Draw(TextureDictionary["dot"],
                    from,
                    null,
                    comp.color,
                    angle,
                    Vector2.Zero,
                    new Vector2(distance, 8f),
                    SpriteEffects.None,
                    0f
                    );

            }
            spriteBatch.End();
        }

        public override void Initialize()
        {
            base.Initialize();
            var content = this.game.Content;
            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);

            var pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

            TextureDictionary.Add("dot", pixel);
            TextureDictionary.Add("Man", content.Load<Texture2D>("character1_without_arm"));
            TextureDictionary.Add("stars", content.Load<Texture2D>("stars"));
            TextureDictionary.Add("white", content.Load<Texture2D>("white"));
            TextureDictionary.Add("ball", content.Load<Texture2D>("circle-128"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public class SpriteComponent : BaseComponent<SpriteComponent>
    {
        public string TextureName { get; set; }
        public Color Color { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public Rectangle? SourceRectangle { get; set; }// Think this should be used by spritemaps etc
        public SpriteEffects SpriteEffects { get; set; }
        public int Layer { get; set; }

        public SpriteComponent(IEntity entity, string textureName, int layer, Color color, Vector2 scale, float rotation, SpriteEffects effects, Rectangle? sourceRectangle = null) : base(entity)
        {
            TextureName = textureName;
            Color = color;
            Scale = scale;
            Rotation = rotation;
            SourceRectangle = sourceRectangle;
            SpriteEffects = effects;
            Layer = layer;
        }
        public SpriteComponent(IEntity entity, string textureName) : this(entity, textureName, 0, Color.White, new Vector2(1, 1), 0, SpriteEffects.None, null) { }
            
    }
}

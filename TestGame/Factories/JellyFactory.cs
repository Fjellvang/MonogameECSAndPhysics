﻿using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using MyGame.TestGame.Components.ColliderComponents;
using MyGame.TestGame.Components.SpriteComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Factories
{
    public static class JellyFactory
    {
        public static IEntity CreateControllableTriangle(Vector2 center, IManager manager, float scale = 1, Color color = default)
        {
            var entity = new Entity(manager, center);

            var pi = Math.PI;
            var top = -new Vector3((float)Math.Cos(pi / 2), (float)Math.Sin(pi / 2), 0) * 2 * scale;
            var downRight = -new Vector3((float)Math.Cos(pi*5/4), (float)Math.Sin(pi*5/4), 0) * 2 * scale;
            var downLeft = -new Vector3((float)Math.Cos(pi*7/4), (float)Math.Sin(pi*7/4), 0) * 2 * scale;
            new LineRelativeToEntityComponent(entity, top.ToVector2(), downRight.ToVector2(), color);
            new LineRelativeToEntityComponent(entity, downRight.ToVector2(), downLeft.ToVector2(), color);
            new LineRelativeToEntityComponent(entity, downLeft.ToVector2(), top.ToVector2(), color);
            new PlayerInputComponent(entity);
            var col = new PolygonCollider(entity, new Vector2[] { top.ToVector2(), downRight.ToVector2(), downLeft.ToVector2() });
            new RigidBodyComponent(entity, 1, 200,((top + downLeft + downRight) / 2).ToVector2(), SimulationObjectType.Active, col);
            return entity;
        }
        public static IEntity CreateControllableCube(Vector2 center, IManager manager, float scale = 1, Color color = default)
        {
            var entity = new Entity(manager, center);
            var width = Vector3.Right * scale;
            var height = Vector3.Up * scale;
            var halfwidth = (width / 2).ToVector2();
            var halfheight = (height / 2).ToVector2();
            var half = halfwidth + halfheight;
            new LineRelativeToEntityComponent(entity, Vector2.Zero - (half), width.ToVector2() - (half), color);
            new LineRelativeToEntityComponent(entity, width.ToVector2() - half, (width * 0.5f + height * 0.5f).ToVector2(), color);
            new LineRelativeToEntityComponent(entity, (width + height).ToVector2() - half, height.ToVector2() - half, color);
            new LineRelativeToEntityComponent(entity, height.ToVector2() - half, Vector2.Zero - half, color);
            //var entity = CreateCube(center, manager, scale, color);
            new PlayerInputComponent(entity);
            //var width = Vector3.Right * scale;
            //var height = Vector3.Up * scale;

            var col = new BoxCollider(entity, width.ToVector2(), height.ToVector2());
            new RigidBodyComponent(entity, 20, 20000, new Vector2((width.X + height.X) /2, (width.Y + height.Y) / 2), SimulationObjectType.Active, col);
            return entity;
        }
        public static IEntity CreateNonCollidingCube(Vector2 center, IManager manager, float scale = 1, Color color = default)
        {
            var entity = new Entity(manager, center);
            var width = Vector3.Right * scale;
            var height = Vector3.Up * scale;
            var halfwidth = (width / 2).ToVector2();
            var halfheight = (height / 2).ToVector2();
            var half = halfwidth + halfheight;
            new LineRelativeToEntityComponent(entity, Vector2.Zero - (half), width.ToVector2() - (half), color);
            new LineRelativeToEntityComponent(entity, width.ToVector2() - half, (width * 0.5f + height * 0.5f).ToVector2(), color);
            new LineRelativeToEntityComponent(entity, (width + height).ToVector2() - half, height.ToVector2() - half, color);
            new LineRelativeToEntityComponent(entity, height.ToVector2() - half, Vector2.Zero - half, color);
            return entity;
        }
        public static IEntity CreateCube(Vector2 center, IManager manager, float scale = 1, Color color = default, bool rig = true, float rotation = 0)
        {
            var entity = new Entity(manager, center, rotation);
            var width = Vector3.Right * scale;
            var height = Vector3.Up * scale;
            var halfwidth = (width / 2).ToVector2();
            var halfheight = (height / 2).ToVector2();
            var half = halfwidth + halfheight;
            new LineRelativeToEntityComponent(entity, Vector2.Zero - (half), width.ToVector2() - (half), color);
            new LineRelativeToEntityComponent(entity, width.ToVector2() - half, (width * 0.5f + height * 0.5f).ToVector2(), color);
            new LineRelativeToEntityComponent(entity, (width + height).ToVector2() - half, height.ToVector2() - half, color);
            new LineRelativeToEntityComponent(entity, height.ToVector2() - half, Vector2.Zero - half, color);


            var col = new BoxCollider(entity, width.ToVector2(), height.ToVector2());
            if (rig)
            {
                new RigidBodyComponent(entity, 200, 20000, new Vector2((width.X + height.X) / 2, (width.Y + height.Y) / 2), SimulationObjectType.Active, col);
            }
            else
            {
                new RigidBodyComponent(entity, 0, 0, new Vector2((width.X + height.X) / 2, (width.Y + height.Y) / 2), SimulationObjectType.Passive, col);
            }
            return entity;
        }
        public static IEntity CreateRandomShape(Vector2 center, IManager manager, float scale = 1, Color color = default)
        {
            var entity = new Entity(manager, center);
            var width = Vector2.UnitX * scale;
            var max = (float)Math.PI;
            //var current = 0f;
            var first = Vector2.Zero;
            var next = width;
            for (int i = 0; i < 5; i++)
            {
                var angle = 0.5f + (float)manager.GetRandom.NextDouble() * (max - 0.5f);
                //Is this overrotation ???
                next = Vector2.Transform(next, Matrix.CreateRotationZ(angle));
                new LineRelativeToEntityComponent(entity, first, next, color);
                max -= angle;
                first = next;
            }
            new LineRelativeToEntityComponent(entity, first, Vector2.Zero, color);
            return entity;
        }
        public static IEntity CreateControllabelJellyCube(Vector2 centerPos, IManager gameManager, float width = 320)
        {

            var StationaryBall = new Entity(gameManager, centerPos);
            //new SpriteComponent(StationaryBall, "ball",1,Color.Red,Vector2.One*0.5f);
            new RigidBodyComponent(StationaryBall,200, 20000, Vector2.Zero, SimulationObjectType.Passive, null);
            //new BoxColliderComponent(StationaryBall, new Rectangle(0, 0, 68, 68));
            new PlayerInputComponent(StationaryBall);


            var dist = width;
            var southballPos = centerPos + Vector2.UnitY * dist;
            var southBall = new Entity(gameManager, southballPos) ;
            //new SpriteComponent(southBall, "ball", 1, Color.Red, Vector2.One * 1f);
            //new BoxColliderComponent(southBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(southBall, 5f, 200, Vector2.Zero, SimulationObjectType.Active, null);

            var eastBallPos = centerPos + Vector2.UnitX * dist;
            var eastBall = new Entity(gameManager,eastBallPos);
            //new SpriteComponent(eastBall, "ball", 1, Color.Red, Vector2.One * 1f);
            //new BoxColliderComponent(eastBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(eastBall, 5f, 200, Vector2.Zero, SimulationObjectType.Active, null);

            var westBallPos = centerPos + Vector2.UnitX * -dist;
            var westBall = new Entity(gameManager, westBallPos);
            //new SpriteComponent(westBall, "ball", 1, Color.Red, Vector2.One * 1f);
            //new BoxColliderComponent(westBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(westBall, 5f, 200, Vector2.Zero, SimulationObjectType.Active, null);

            var northBallPos = centerPos + -Vector2.UnitY * dist;
            var northBall = new Entity(gameManager, northBallPos);
            //new SpriteComponent(northBall, "ball",1,Color.Red,Vector2.One*1f);
            //new BoxColliderComponent(northBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(northBall, 5f, 200, Vector2.Zero, SimulationObjectType.Active, null);

            var northEastBallPos = eastBallPos + .5f * (northBallPos - eastBallPos);
            var northEastBall = new Entity(gameManager, northEastBallPos);
            //new SpriteComponent(northEastBall, "ball",1,Color.Red,Vector2.One*1f);
            //new BoxColliderComponent(northEastBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(northEastBall, 5f, 200, Vector2.Zero, SimulationObjectType.Active, null);

            var northwestBallPos = westBallPos + .5f * (northBallPos - westBallPos);
            var northwestBall = new Entity(gameManager, northwestBallPos);
            //new SpriteComponent(northwestBall, "ball",1,Color.Red,Vector2.One*1f);
            //new BoxColliderComponent(northwestBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(northwestBall, 5f, 200, Vector2.Zero, SimulationObjectType.Active, null);

            var southEastBallPos = eastBallPos+ .5f * (southballPos - eastBallPos);
            var southEastBall = new Entity(gameManager, southEastBallPos);
            //new SpriteComponent(southEastBall, "ball",1,Color.Red,Vector2.One*1f);
            //new BoxColliderComponent(southEastBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(southEastBall, 5f, 200, Vector2.Zero, SimulationObjectType.Active, null);

            var southWestBallPos = westBallPos + .5f * (southballPos - westBallPos);
            var southWestBall = new Entity(gameManager, southWestBallPos);
            //new SpriteComponent(southWestBall, "ball",1,Color.Red,Vector2.One*1f);
            //new BoxColliderComponent(southWestBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(southWestBall, 5f, 200, Vector2.Zero, SimulationObjectType.Active, null);


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
            //corners
            new SpringComponent(stiffness, dampness, southBall, westBall, null,false);
            new SpringComponent(stiffness, dampness, westBall, northBall, null,false);
            new SpringComponent(stiffness, dampness, northBall, eastBall, null,false);
            new SpringComponent(stiffness, dampness, eastBall, southBall, null,false);

            new SpringComponent(stiffness, dampness, southBall, northBall, null,false);
            new SpringComponent(stiffness, dampness, eastBall, westBall, null,false);
            new SpringComponent(stiffness, dampness, southEastBall, northwestBall, null,false);
            new SpringComponent(stiffness, dampness, southWestBall, northEastBall, null,false);
            return StationaryBall;
        }
    }
}

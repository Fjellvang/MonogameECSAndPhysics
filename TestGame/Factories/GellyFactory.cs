using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Factories
{
    public static class GellyFactory
    {
        public static IEntity CreateControllableCube(Vector3 centerPos, IManager gameManager, float width = 1280/4)
        {

            var StationaryBall = new Entity(gameManager, centerPos);
            //new SpriteComponent(StationaryBall, "ball",1,Color.Red,Vector2.One*0.5f);
            new RigidBodyComponent(StationaryBall,1, SimulationObjectType.Passive);
            new BoxColliderComponent(StationaryBall, new Rectangle(0, 0, 68, 68));
            new PlayerInputComponent(StationaryBall);


            var dist = width;
            var southballPos = centerPos + -Vector3.Down * dist;
            var southBall = new Entity(gameManager, southballPos) ;
            //new SpriteComponent(southBall, "ball", 1, Color.Red, Vector2.One * 1f);
            new BoxColliderComponent(southBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(southBall, 5f, SimulationObjectType.Active);

            var eastBallPos = centerPos + Vector3.Right * dist;
            var eastBall = new Entity(gameManager,eastBallPos);
            //new SpriteComponent(eastBall, "ball", 1, Color.Red, Vector2.One * 1f);
            new BoxColliderComponent(eastBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(eastBall, 5f, SimulationObjectType.Active);

            var westBallPos = centerPos + Vector3.Right * -dist;
            var westBall = new Entity(gameManager, westBallPos);
            //new SpriteComponent(westBall, "ball", 1, Color.Red, Vector2.One * 1f);
            new BoxColliderComponent(westBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(westBall, 5f, SimulationObjectType.Active);

            var northBallPos = centerPos + Vector3.Down * dist;
            var northBall = new Entity(gameManager, northBallPos);
            //new SpriteComponent(northBall, "ball",1,Color.Red,Vector2.One*1f);
            new BoxColliderComponent(northBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(northBall, 5f, SimulationObjectType.Active);

            var northEastBallPos = eastBallPos + .5f * (northBallPos - eastBallPos);
            var northEastBall = new Entity(gameManager, northEastBallPos);
            //new SpriteComponent(northEastBall, "ball",1,Color.Red,Vector2.One*1f);
            new BoxColliderComponent(northEastBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(northEastBall, 5f, SimulationObjectType.Active);

            var northwestBallPos = westBallPos + .5f * (northBallPos - westBallPos);
            var northwestBall = new Entity(gameManager, northwestBallPos);
            //new SpriteComponent(northwestBall, "ball",1,Color.Red,Vector2.One*1f);
            new BoxColliderComponent(northwestBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(northwestBall, 5f, SimulationObjectType.Active);

            var southEastBallPos = eastBallPos+ .5f * (southballPos - eastBallPos);
            var southEastBall = new Entity(gameManager, southEastBallPos);
            //new SpriteComponent(southEastBall, "ball",1,Color.Red,Vector2.One*1f);
            new BoxColliderComponent(southEastBall, new Rectangle(0, 0, 68, 68));
            new RigidBodyComponent(southEastBall, 5f, SimulationObjectType.Active);

            var southWestBallPos = westBallPos + .5f * (southballPos - westBallPos);
            var southWestBall = new Entity(gameManager, southWestBallPos);
            //new SpriteComponent(southWestBall, "ball",1,Color.Red,Vector2.One*1f);
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

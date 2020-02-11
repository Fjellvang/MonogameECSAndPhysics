using Microsoft.Xna.Framework;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using MyGame.TestGame.Components.ColliderComponents;
using MyGame.TestGame.Factories;
using MyGame.TestGame.Physics.ForceGenerators;
using MyGame.TestGame.Physics.Integrators;
using System.Collections.Generic;

namespace MyGame.TestGame.Systems
{
    public class SimplePhysicsSystem : BaseSystem
    {
        float toSpawn = 2.0f;
        float spawned = 0;
        //TODO: find better soultion
        List<IForceGenerator> forceGenerators = new List<IForceGenerator>();
        public Integrator Integrator{ get; set; }
        public SimplePhysicsSystem(IManager manager, Rectangle? bounds, Integrator integrator) : base(manager)
        {
            //this.bounds = bounds ?? new Rectangle(0, 0, 1280, 1280);
            Integrator = integrator;
        }

        public override void Initialize()
        {
            //forceGenerators.Add(new Gravity(500));
            //forceGenerators.Add(new Medium(2f));
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < SpringComponent.Instances.Count; i++)
            {
                var spring = SpringComponent.Instances[i].spring;
                
                spring.ApplyForce(null); // passes null, as spring stores reference internally...
            }

            for (int i = 0; i < RigidBodyComponent.Instances.Count; i++)
            {
                var rig = RigidBodyComponent.Instances[i];
                //var transform = rig.Entity.GetComponent<TransformComponent>();

                if (rig.ObjectType != SimulationObjectType.Active)
                {
                    continue;
                }

                //applyforces like gravity and drag etc
                for (int j = 0; j < forceGenerators.Count; j++)
                {
                    forceGenerators[j].ApplyForce(rig);
                }

                //find acceleration
                //todo: maybe this needs another loop ????
                var accleration = rig.ResultantForce / rig.Mass; //Todo, get a prop for this ?
                //TODO: SATISFY CONSTRAINTS

                //TODO: Consider if we need to move translation out of integration ?
                Integrator.Integrate(accleration, rig);

                var collider = rig.Entity.GetComponent<ColliderBaseComponent>();

                for (int j = 0; j < ColliderBaseComponent.Instances.Count; j++)
                {
                    var other = ColliderBaseComponent.Instances[j];
                    if (collider.Entity == other.Entity)
                    {
                        continue;
                    }
                    if (collider.CollidesWith(rig.CurrentPosition.ToVector2(), rig.Entity.Rotation, other, out var x))
                    {
                        //TODO: This is not real physics...

                        var test = collider.FindBestCollisionEdge(x.Value.Axis);
                        var test2 = other.FindBestCollisionEdge(-x.Value.Axis);
                        var points = collider.CalculateContactManifold(test, test2, x.Value.Axis);
                        if (points.Count>=2)
                        {
                            //var middle =rig.Entity.Position.ToVector2() + (points[1] - points[0]);
                            if (this.spawned <= 0f)
                            {
                                this.spawned = toSpawn;
                                JellyFactory.CreateNonCollidingCube(new Vector3(points[0], 0), this.Manager, 10, Color.Red);
                                JellyFactory.CreateNonCollidingCube(new Vector3(points[1], 0), this.Manager, 10, Color.Blue);
                            }
                        }
                        rig.CurrentPosition = rig.Entity.Position;
                    }
                }

                spawned -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                rig.UpdateEntityPosition();

                rig.ResetForces();
                //TODO: will these two lines end up being identical ???
                //rig.Update(gameTime);

                //rig.Entity.Position = rig.SimulationObject.CurrentPosition;
                //rig.SimulationObject.CurrentPosition = rig.Entity.Position;

                //Reset forces on the object..
            }
        }
    }
}

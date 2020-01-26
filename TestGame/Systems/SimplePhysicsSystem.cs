using Microsoft.Xna.Framework;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using MyGame.TestGame.Physics.ForceGenerators;
using MyGame.TestGame.Physics.Integrators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Systems
{
    public class SimplePhysicsSystem : BaseSystem
    {
        Rectangle bounds; //TODO: find something better
        //TODO: find better soultion
        List<IForceGenerator> forceGenerators = new List<IForceGenerator>();
        public Integrator Integrator{ get; set; }
        public SimplePhysicsSystem(IManager manager, Rectangle? bounds, Integrator integrator) : base(manager)
        {
            this.bounds = bounds ?? new Rectangle(0, 0, 1280, 1280);
            Integrator = integrator;
        }

        public override void Initialize()
        {
            forceGenerators.Add(new Gravity());
            forceGenerators.Add(new Medium(0.1f));
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < SpringComponent.Instances.Count; i++)
            {
                var spring = SpringComponent.Instances[i].spring;
                
                spring.ApplyForce(null); // passes null, as spring stores reference internally...
            }

            for (int i = 0; i < SimpleRigidbodyComponent.Instances.Count; i++)
            {
                var rig = SimpleRigidbodyComponent.Instances[i];
                var transform = rig.Entity.GetComponent<TransformComponent>();

                if (rig.SimulationObject.ObjectType != Physics.SimulationObjectType.Active)
                {
                    continue;
                }

                //applyforces like gravity and drag etc
                for (int j = 0; j < forceGenerators.Count; j++)
                {
                    forceGenerators[j].ApplyForce(rig.SimulationObject);
                }

                //find acceleration
                //todo: maybe this needs another loop ????
                var accleration = rig.SimulationObject.ResultantForce / rig.SimulationObject.Mass; //Todo, get a prop for this ?

                Integrator.Integrate(accleration, rig.SimulationObject);

                //TODO: will these two lines end up being identical ???
                rig.SimulationObject.Update(gameTime);
                transform.Position = rig.SimulationObject.CurrentPosition;

                //Reset forces on the object..
                rig.SimulationObject.ResetForces();
            }
        }
    }
}

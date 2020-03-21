using Microsoft.Xna.Framework;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using MyGame.TestGame.Components.ColliderComponents;
using MyGame.TestGame.Factories;
using MyGame.TestGame.Physics.ForceGenerators;
using MyGame.TestGame.Physics.Integrators;
using System;
using System.Collections.Generic;

namespace MyGame.TestGame.Systems
{
    public class SimplePhysicsSystem : BaseSystem
    {
        //TODO: find better soultion
        List<IForceGenerator> forceGenerators = new List<IForceGenerator>();
        Queue<Collision> collisions = new Queue<Collision>();
        public Integrator Integrator{ get; set; }
        public SimplePhysicsSystem(IManager manager, Rectangle? bounds, Integrator integrator) : base(manager)
        {
            //this.bounds = bounds ?? new Rectangle(0, 0, 1280, 1280);
            Integrator = integrator;
        }

        public override void Initialize()
        {
            forceGenerators.Add(new Gravity(50));
            //forceGenerators.Add(new Medium(2f));
        }
        private void AddCollision(ColliderBaseComponent A, ColliderBaseComponent B, MTV mtv)
        {
            var collision = new Collision(A, B, mtv);
            if (collisions.Contains(collision)) { return; }
            collisions.Enqueue(collision);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < SpringComponent.Instances.Count; i++)
            {
                var spring = SpringComponent.Instances[i].spring;
                
                spring.ApplyForce(null); // passes null, as spring stores reference internally...
            }
            //TODO: add collision check bere 
            for (int i = 0; i < RigidBodyComponent.Instances.Count; i++)
            {
                var rig = RigidBodyComponent.Instances[i];
                var collider = rig.Entity.GetComponent<ColliderBaseComponent>();
                for (int j = i+1; j < RigidBodyComponent.Instances.Count; j++)
                {
                    var other = RigidBodyComponent.Instances[j];
                    if (collider.CollidesWith(rig.CurrentPosition, rig.NextRotation, other.Collider, out var mtv))
                    {
                        AddCollision(rig.Collider, other.Collider, mtv.Value);
                    }
                }
            }
            while(collisions.Count > 0)
            {
                var collision = collisions.Dequeue();
                var colliderA = collision.A;
                var colliderB = collision.B;
                colliderA.AttachedRigidBody(out var rig);
                var mtv = collision.MTV;
                var A = colliderA.FindBestCollisionEdge(mtv.Axis, rig.CurrentPosition);
                var B = colliderB.FindBestCollisionEdge(-mtv.Axis, colliderB.Entity.Transform.Position);
                var points = colliderA.CalculateContactManifold(A, B, mtv.Axis);
                for (int l = 0; l < points.Count; l++)
                {
                    ApplyImpulse(colliderA, colliderB, mtv, points[l], points.Count);
                }
                var penetration = -mtv.Axis * (mtv.Magnitude * 1.5f);
                //if (colliderB.AttachedRigidBody(out var otherrig))
                //{
                //    penetration *= .5f;
                //    otherrig.CurrentPosition -= penetration;
                //}
                //colliderA.AttachedRigidBody(out var rig);
                rig.CurrentPosition += penetration;
                //TODO: REFACTOR
                //for (int j = 0; j < ColliderBaseComponent.Instances.Count; j++)
                //{
                //    var other2 = ColliderBaseComponent.Instances[j];
                //    if (rig.Entity.Id == other2.Entity.Id)
                //    {
                //        continue;
                //    }
                //    if (rig.Collider.CollidesWith(rig.CurrentPosition, rig.NextRotation, other2, out var mtv2))
                //    {
                //        AddCollision(rig, other, mtv2.Value);
                //    }
                //}
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
                var accleration = rig.ResultantForce * rig.InvMass; //Todo, get a prop for this ?
                var angularAcceleration = rig.ResultantAngularForce * rig.InvInertia; //Todo, get a prop for this ?
                //TODO: SATISFY CONSTRAINTS

                //TODO: Consider if we need to move translation out of integration ?
                Integrator.Integrate(accleration, angularAcceleration, rig);
                rig.NextRotation = Matrix.CreateRotationZ(rig.CurrentAngle); 


                rig.UpdateEntityPosition();

                rig.ResetForces();
                //TODO: will these two lines end up being identical ???
                //rig.Update(gameTime);

                //rig.Entity.Position = rig.SimulationObject.CurrentPosition;
                //rig.SimulationObject.CurrentPosition = rig.Entity.Position;

                //Reset forces on the object..
            }
        }
        private static void ApplyImpulse(ColliderBaseComponent colA, ColliderBaseComponent colB, MTV? mtv, Vector2 pointOfCollision, int numberOfCollisionPoints)
        {
            colA.AttachedRigidBody(out var rig);
            colB.AttachedRigidBody(out var otherRig);

            var normal = -mtv.Value.Axis;
            var body1Contact = (pointOfCollision - rig.CurrentPosition);
            var body2Contact = (pointOfCollision - rig.CurrentPosition);

            //var relativevelocity = otherVel + body2Contact.Cross(otherAngularVelocity) - 
            //    rig.CurrentVelocity - body1Contact.Cross(rig.CurrentAngularVelocity);
            var relativevelocity = otherRig.CurrentVelocity -
                rig.CurrentVelocity;
            var velocityAlongNormal = Vector2.Dot(relativevelocity, normal);
            if (velocityAlongNormal < 10f) //TODO: Find a fitting constant here.
            {
                return;
            }
            //rig.AddForceAtPoint(-rig.CurrentVelocity, pointOfimpact);

            float fCr = .2f; // Coefficient of restitution. 1 to 0, betyder halvvejs mellem elastisk og ind elastisk. TODO: Gem det i objekter og tag mindste..

            var body1RadCrossN = body1Contact.Cross(normal);
            var body2RadCrossN = body2Contact.Cross(normal);

            var body1Inertia = body1RadCrossN * body1RadCrossN * rig.InvInertia;
            var body2Inertia = body2RadCrossN * body2RadCrossN * otherRig.InvInertia;

            float jNormal = -(1f + fCr) * velocityAlongNormal;

            jNormal /= (rig.InvMass + otherRig.InvMass) + body1Inertia + body2Inertia;
            jNormal /= numberOfCollisionPoints;

            //apply impulse..
            var impulse = jNormal * normal;
            //rig.CurrentVelocity -= impulse * rig.InvMass;
            var vel1 = impulse * rig.InvMass;
            var angle = rig.InvInertia * body1Contact.Cross(impulse);
            
            var collisionTanget = normal.Cross(normal.Cross(relativevelocity)); // wonder bout order of operations???
            collisionTanget.Normalize();
            var Vrt = relativevelocity.Dot(collisionTanget);
            if (false && Math.Abs(Vrt) > 0)
            {
                //apply friction
                var mub = .001f; //Friction coefficient. TODO: Get this properly instead of this arbitraty value
                var newVel = ((impulse) + ((mub * jNormal) * collisionTanget)) / rig.Mass;
                var newAngle = (body1Contact.Cross((impulse) + (mub * jNormal) * collisionTanget)) * rig.InvInertia;
                
                //IS THIS REALLY RIGHT??? anyway refactor heavly.
                rig.CurrentVelocity -= newVel;
                rig.CurrentAngularVelocity -= newAngle;
                otherRig.CurrentVelocity += newVel;
                otherRig.CurrentAngularVelocity += newAngle;
            }
            else
            {
                rig.CurrentVelocity -= vel1;
                rig.CurrentAngularVelocity -= angle;
                otherRig.CurrentVelocity += impulse * otherRig.InvMass;
                otherRig.CurrentAngularVelocity += otherRig.InvInertia * body2Contact.Cross(impulse);
            }

            
        }

        private static void ApplyImpulseNoRotation(RigidBodyComponent rig, ColliderBaseComponent other, MTV? mtv)
        {
            Vector2 otherVel = Vector2.Zero;
            float otherMass = 1;
            if (other.AttachedRigidBody(out var otherRig))
            {
                otherVel = otherRig.CurrentVelocity;
                otherMass = otherRig.Mass;
            }
            var relativevelocity = rig.CurrentVelocity - otherVel;
            var vrn = Vector2.Dot(relativevelocity, -mtv.Value.Axis);
            //rig.AddForceAtPoint(-rig.CurrentVelocity, pointOfimpact);

            var fCr = .5f; // Coefficient of restitution. 1 to 0, betyder halvvejs mellem elastisk og ind elastisk.

            var imnpulse = (-(1f + fCr) * (vrn)) /
                           ((Vector2.Dot(-mtv.Value.Axis, -mtv.Value.Axis)) *
                             (1f / rig.Mass + 1f / otherMass));
            rig.CurrentVelocity += (imnpulse * -mtv.Value.Axis) / rig.Mass;
            if (otherRig != null)
            {
                otherRig.CurrentVelocity -= (imnpulse * -mtv.Value.Axis) / otherRig.Mass;
            }
        }
    }
    public struct Collision
    {
        public ColliderBaseComponent A { get; }
        public ColliderBaseComponent B { get; }
        public MTV MTV { get; }
        public Collision(ColliderBaseComponent a, ColliderBaseComponent b, MTV mtv)
        {
            A = a;
            B = b;
            MTV = mtv;
        }

        public override bool Equals(object obj)
        {
            return obj is Collision collision &&
                   A.Entity.Id == collision.A.Entity.Id &&
                   B.Entity.Id == collision.B.Entity.Id &&
                   EqualityComparer<MTV>.Default.Equals(MTV, collision.MTV);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(A, B, MTV);
        }
    }
}

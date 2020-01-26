//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MyGame.TestGame.Physics
//{
//    /// <summary>
//    /// Active are affected by by forces, passive arent
//    /// </summary>
//    public enum SimulationObjectType { Passive, Active}
//    public abstract class SimulationObject
//    {
//        public float Mass { get; set; }
//        public SimulationObjectType ObjectType { get; set; }
//        public Vector3 CurrentPosition { get; set; }
//        public Vector3 PreviousPosition { get; set; }
//        public Vector3 CurrentVelocity { get; set; }
//        /// <summary>
//        /// All forces acting on the object summed up.
//        /// </summary>
//        public Vector3 ResultantForce { get; set; }
//        public SimulationObject(float mass, SimulationObjectType objectType)
//        {
//            this.Mass = mass;
//            this.ObjectType = objectType;
//            CurrentPosition = Vector3.Zero;
//            PreviousPosition = CurrentPosition;
//            CurrentVelocity = Vector3.Zero;
//        }

//        /// <summary>
//        /// Typically being called at beginning or end of each timestep
//        /// </summary>
//        public void ResetForces()
//        {
//            this.ResultantForce = Vector3.Zero;
//        }

//        public abstract void Update(GameTime time);
//    }
//}

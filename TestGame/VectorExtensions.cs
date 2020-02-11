using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame
{
    public static class VectorExtensions
    {
        public static Vector2 ToVector2(this Vector3 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static Vector2 Cross(this Vector2 vec, float scalar)
        {
            return new Vector2(vec.Y * scalar, vec.X * -scalar);
        }

        public static float Dot(this Vector2 thisVec, Vector2 thatVec)
        {
            return Vector2.Dot(thisVec, thatVec);
        }
        public static Vector2 ToLeftTurnedNormal(this Vector2 vector2)
        {
            return new Vector2(-vector2.Y, vector2.X);
        }
        public static Vector2 ToRightTurnedNormal(this Vector2 vector2)
        {
            return new Vector2(vector2.Y, -vector2.X);
        }
    }
}

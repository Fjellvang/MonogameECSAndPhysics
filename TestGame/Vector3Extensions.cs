using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame
{
    public static class Vector3Extensions
    {
        public static Vector2 ToVector2(this Vector3 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }
    }
}

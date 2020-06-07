using EasyModMine.BuiltInGameLogic;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace EasyModMine.RenderEngine
{
    class EngineMath
    {
        public static float ToRadians(float angle)
        {
            return angle * 0.0174532925f;
        }

        public static Vector2 RotateVector(Vector2 v, float degrees)
        {
            double a = degrees * 0.0174532925;
            double sin = Math.Sin(a);
            double cos = Math.Cos(a);

            float tx = v.X;
            float ty = v.Y;
            v.X = ((float)cos * tx) - ((float)sin * ty);
            v.Y = ((float)sin * tx) + ((float)cos * ty);
            return v;
        }
    }
}

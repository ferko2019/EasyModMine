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
    }
}

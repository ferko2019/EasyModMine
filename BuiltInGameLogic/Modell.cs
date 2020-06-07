using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using EasyModMine.RenderEngine;

namespace EasyModMine.BuiltInGameLogic
{
    class Modell
    {
        public Vector3[] vertices;
        public Vector2[] uv;
        public Vector3[] normal;
        public Texture2D[] textures;
        public Color[] colors;
        public string location = "";
    }
}

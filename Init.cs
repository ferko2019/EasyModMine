using EasyModMine.BuiltInGameLogic;
using EasyModMine.RenderEngine;
using OpenTK;
using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using _3DVoxelEngine.GameLogic;

namespace EasyModMine
{
    class Init
    {
        static void Main(string[] args)
        {
            Modell mesh = LoadResources.ReadModells(Directory.GetCurrentDirectory() + @"\Mods\Default\Modells\cube.obj");//LoadObj.LoadModellFromObj(Directory.GetCurrentDirectory()+@"\Mods\Default\Modells\cube.obj");

            GameWindow window = new GameWindow(640,480);
            OpenGlRenderer renderer = new OpenGlRenderer(window);
            GameObject obj = new GameObject("test",Vector3.Zero,Vector3.Zero,Vector3.One,mesh);
            foreach (var item in mesh.uv)
            {
                Console.WriteLine(item);
            }
            renderer.UpdateBuffers(mesh.vertices, mesh.uv, mesh.normal);
            window.Run(1/ DisplayDevice.Default.RefreshRate);
        }
    }
}

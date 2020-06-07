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
using EasyModMine.Mods.Default.Scripts;
using OpenTK.Graphics.OpenGL;

namespace EasyModMine
{
    class Init
    {
        static void Main(string[] args)
        {
            Modell mesh = LoadResources.ReadModell(Directory.GetCurrentDirectory() + @"\Mods\Default\Modells\earth.obj");//LoadObj.LoadModellFromObj(Directory.GetCurrentDirectory()+@"\Mods\Default\Modells\cube.obj");
            GameWindow window = new GameWindow(640,480);
            OpenGlRenderer renderer = new OpenGlRenderer(window);
            renderer.ImmediateRendering = false;
            renderer.Quad = false;
            GameObject obj = new GameObject("test",new Vector3(1,3,2),Vector3.Zero,Vector3.One,mesh);
            = LevelGenerator.GenerateTerrain3D(100, 100,100, .05f, 0,.5f);
            Console.WriteLine(level.Length);
            Modell combined = CombineModells.Combine(CombineModells.GameObjectToModellArray(ChunksToGameObjects(NearChunck(level.ToArray(),renderer.player_pos,50)).ToArray()));
            Console.WriteLine(combined.vertices.Length);
            Console.WriteLine(combined.uv.Length);
            Console.WriteLine(combined.normal.Length);
            renderer.UpdateBuffers(combined.vertices, combined.uv, combined.normal);
            window.Run(1/ DisplayDevice.Default.RefreshRate);
        }

        public static List<GameObject>[] NearChunck(List<GameObject>[] chunks,Vector3 player_pos,float max_distance)
        {
            List<List<GameObject>> final_chunks = new List<List<GameObject>>();
            foreach (List<GameObject> chunk in chunks)
            {
                if(Vector3.Distance(player_pos,chunk[0].position) < max_distance)
                {
                    final_chunks.Add(chunk);
                }
            }
            return final_chunks.ToArray();
        }

        public static List<GameObject> ChunksToGameObjects(List<GameObject>[] chunks)
        {
            List<GameObject> g_objs = new List<GameObject>();

            foreach (List<GameObject> objects in chunks)
            {
                g_objs.AddRange(objects);
            }
            return g_objs;
        }
    }
}

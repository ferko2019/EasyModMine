using EasyModMine.BuiltInGameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using _3DVoxelEngine.GameLogic;
using System.IO;
using EasyModMine.RenderEngine;

namespace EasyModMine.Mods.Default.Scripts
{
    class LevelGenerator
    {
        public static GameObject[] GenerateTerrainOld(int width = 100,int height = 100,float scale = 1,int seed = 0,float divide = 100)
        {
            List<GameObject> gameObjects = new List<GameObject>();  
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    string type = "none";
                    float y = 0;
                    Modell cube = LoadResources.ReadModell(Directory.GetCurrentDirectory() + @"\Mods\Default\Modells\cube.obj");
                    GameObject currentBlock = new GameObject(x + "." + z + "." + type, new Vector3(x, y, z), Vector3.Zero, new Vector3(1,1,1), cube);
                    //Console.WriteLine(new Vector3(x, y, z));
                    gameObjects.Add(currentBlock);
                }
            }
            return gameObjects.ToArray();
        }

        public static List<GameObject>[] GenerateTerrain3D(int width = 100, int height = 100,int depth = 100, float scale = 1, int seed = 0, float treshold = 100)
        {
            List<List<GameObject>> gameObjects = new List<List<GameObject>>();
            for (int c_x = 0; c_x < width; c_x += 16)
            {
                for (int c_z = 0; c_z < depth; c_z += 16)
                {
                    for (int c_y = 0; c_y < height; c_y += 16)
                    {
                        List<GameObject> chunk = new List<GameObject>();
                        for (int x = c_x; x < c_x+16; x++)
                        {
                            for (int y = c_y; y < c_y+16; y++)
                            {
                                for (int z = c_z; z < c_z+16; z++)
                                {
                                    float noise = Perlin3D(x * scale, y * scale, z * scale);
                                    //Console.WriteLine(noise);
                                    if (noise >= treshold)
                                    {
                                        Modell cube = LoadResources.ReadModell(Directory.GetCurrentDirectory() + @"\Mods\Default\Modells\cube.obj");
                                        //Console.WriteLine(noise);
                                        string type = "none";
                                        GameObject currentBlock = new GameObject(x + "." + y + "." + z + "." + type, new Vector3(x, y, z), Vector3.Zero, new Vector3(.5f, .5f, .5f), cube);
                                        //Console.WriteLine(new Vector3(x, y, z));
                                        chunk.Add(currentBlock);
                                    }
                                }
                            }
                        }
                        gameObjects.Add(chunk);
                    }
                }
            }
            return gameObjects.ToArray();
        }

        public static float Perlin3D(float x_f,float y_f,float z_f)
        {
            double x = (double)x_f;
            double y = (double)y_f;
            double z = (double)z_f;

            double ab = Noise.perlin(x, y, 0);
            double bc = Noise.perlin(y, z, 0);
            double ac = Noise.perlin(x, z, 0);

            double ba = Noise.perlin(y, x, 0);
            double cb = Noise.perlin(z, y, 0);
            double ca = Noise.perlin(z, x, 0);

            double abc = ab + bc + ac + ba + cb + ca;
            return (float)abc / 6f;
        }

        public static void UpdateLevel(OpenGlRenderer renderer,List<GameObject>[] level)
        {
            //Console.WriteLine(renderer.level.Length);
            Modell combined = CombineModells.Combine(CombineModells.GameObjectToModellArray(Init.ChunksToGameObjects(Init.NearChunck(level.ToArray(), renderer.player_pos, 50)).ToArray()));
            //Console.WriteLine(combined.vertices.Length);
            //Console.WriteLine(combined.uv.Length);
            //Console.WriteLine(combined.normal.Length);
            //renderer.UpdateBuffers(combined.vertices, combined.uv, combined.normal);
        }
    }
}
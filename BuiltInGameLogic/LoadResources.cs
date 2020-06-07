using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Security.Cryptography;
using EasyModMine.RenderEngine;
using EasyModMine.BuiltInGameLogic;

namespace _3DVoxelEngine.GameLogic
{
    class LoadResources
    {
        //public string GameData = Directory.GetCurrentDirectory()+@"\GameData";

        public string resourceName = "";
        public static string[] ReadScripts(string path)
        {
            string[] scripts = Directory.GetFiles(path);
            string[] Scripts = new string[scripts.Length];
            for (int i = 0; i < scripts.Length; i++)
            {
                Scripts[i] = File.ReadAllText(scripts[i]);
            }
            return Scripts;
        }

        public static Modell ReadModell(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("File was not exist");
            }
            string[] lines = File.ReadAllLines(path);
            List<Vector3> localVertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<int> uvIndex = new List<int>();
            List<int> faces = new List<int>();
            List<Vector3> normal = new List<Vector3>();
            List<int> normalIndexes = new List<int>();
            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    if (line[0] == 'v' && line[1] != 'n' && line[1] != 't')
                    {
                        int end = 0;
                        bool finished = false;
                        foreach (char chr in line)
                        {
                            if (chr == 'v' && !finished)
                            {
                                end++;
                            }
                            else if (chr == ' ' && !finished)
                            {
                                end++;
                            }
                            else
                            {
                                finished = true;
                            }
                        }
                        string[] data = line.Substring(end, line.Length - end).Split(' ');
                        localVertices.Add(new Vector3(float.Parse(data[0].Replace('.', ',')), float.Parse(data[1].Replace('.', ',')), float.Parse(data[2].Replace('.', ','))));
                    }
                    else if (line[0] == 'f')
                    {
                        string[] data = line.Substring(2, line.Length - 2).Split(' ');
                        foreach (string vert in data)
                        {
                            string[] v_index = vert.Split('/');
                            faces.Add(int.Parse(v_index[0]));
                            uvIndex.Add(int.Parse(v_index[1]));
                            normalIndexes.Add(int.Parse(v_index[2]));
                        }
                    }
                    else if (line[0] == 'v' && line[1] == 't')
                    {
                        int end = 0;
                        bool finished = false;
                        foreach (char chr in line)
                        {
                            if (chr == 'v' && !finished)
                            {
                                end++;
                            }
                            else if (chr == 't' && !finished)
                            {
                                end++;
                            }
                            else if (chr == ' ' && !finished)
                            {
                                end++;
                            }
                            else
                            {
                                finished = true;
                            }
                        }
                        string[] data = line.Substring(end, line.Length - end).Split(' ');
                        uv.Add(new Vector2(float.Parse(data[0].Replace('.', ',')), float.Parse(data[1].Replace('.', ','))));
                    }
                    else if (line[0] == 'v' && line[1] == 'n')
                    {
                        int end = 0;
                        bool finished = false;
                        foreach (char chr in line)
                        {
                            if (chr == 'v' && !finished)
                            {
                                end++;
                            }
                            else if (chr == ' ' && !finished)
                            {
                                end++;
                            }
                            else if (chr == 'n' && !finished)
                            {
                                end++;
                            }
                            else
                            {
                                finished = true;
                            }
                        }
                        string[] data = line.Substring(end, line.Length - end).Split(' ');
                        normal.Add(new Vector3(float.Parse(data[0].Replace('.', ',')), float.Parse(data[1].Replace('.', ',')), float.Parse(data[2].Replace('.', ','))));
                    }
                }
            }
            List<Vector3> finalVertices = new List<Vector3>();
            List<Vector3> finalNormals = new List<Vector3>();
            List<Vector2> finalUV = new List<Vector2>();
            foreach (int vert_index in faces)
            {
                finalVertices.Add(localVertices[vert_index - 1]);
            }

            foreach (int uv_index in uvIndex)
            {
                finalUV.Add(EngineMath.RotateVector(uv[uv_index - 1], 180));
            }

            foreach (int normal_index in normalIndexes)
            {
                finalNormals.Add(normal[normal_index - 1]);
            }
            Modell modell = new Modell() { vertices = finalVertices.ToArray(), uv = finalUV.ToArray(), normal = finalNormals.ToArray(), location = path };
            return modell;
        }

        public static Texture2D ReadTexture(string path, bool pixelated = false)
        {
            string fpath = path;
            Bitmap bmp = new Bitmap(fpath);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                pixelated ? (int)TextureMinFilter.Nearest : (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                pixelated ? (int)TextureMagFilter.Nearest : (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            return new Texture2D(id, new Vector2(bmp.Width, bmp.Height));
        }

        public static void ReadAudio()
        {

        }
    }
}
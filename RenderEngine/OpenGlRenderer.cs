using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using EasyModMine.RenderEngine;
using EasyModMine.BuiltInGameLogic;
using System.IO;
using _3DVoxelEngine.GameLogic;
using OpenTK.Input;
using EasyModMine.Mods.Default.Scripts;

namespace EasyModMine.RenderEngine
{
    class OpenGlRenderer
    {
        //Game window
        public GameWindow window;

        //Vertex bufer

        public int VEB;
        public Vector3[] vertexBuffer = new Vector3[0];

        //Uv buffer

        public int UVB;
        public Vector2[] uvBuffer = new Vector2[0];

        //Normal buffer

        public int NOB;
        public Vector3[] normalBuffer = new Vector3[0];

        //Textures

        public Texture2D[] textures = new Texture2D[0];

        //Colors

        public Color[] colors = new Color[0];

        //Render settings
        public bool ImmediateRendering = false;
        public bool Quad = false;

        //Player data

        public Vector3 player_pos;
        public Vector2 lastMousePos;

        Input input = new Input();

        //Level

        public List<GameObject>[] level;

        int time = 0;

        public OpenGlRenderer(GameWindow window)
        {
            this.window = window;
            Start();
        }
        void Start()
        {
            window.Load += loaded;
            window.Resize += resize;
            window.RenderFrame += renderFrame;
            window.Title = "OpenGL Render";
        }

        private void resize(object sender, EventArgs e)
        {
            // Set viewport size
            GL.Viewport(0,0,window.Width,window.Height);
            // Set prespective projection matrix
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView(EngineMath.ToRadians(45), (float)window.Width / (float)window.Height, .01f, 1000);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        void loaded(object o, EventArgs e)
        {
            input.lastPos = new Vector2(Mouse.GetCursorState().X, Mouse.GetCursorState().Y);
            //texture = LoadResources.ReadTexture(Directory.GetCurrentDirectory() + @"\Mods\Default\Textures\earth.jpg");
            //Change clear color
            GL.ClearColor(Color.Black);
            //Render polygons in a correct order
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.TextureCoordArray);
            //Setup buffers

            //Vertex buffer

            VEB = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VEB);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * vertexBuffer.Length),
                vertexBuffer, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Uv buffer

            UVB = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, UVB);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(Vector2.SizeInBytes * uvBuffer.Length),
                uvBuffer, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Normal buffer

            NOB = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, UVB);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * normalBuffer.Length),
                normalBuffer, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            level = LevelGenerator.GenerateTerrain3D(20, 20, 20, .05f, 0, .5f);
            //Console.WriteLine(renderer.level.Length);
            Modell combined = CombineModells.Combine(CombineModells.GameObjectToModellArray(Init.ChunksToGameObjects(Init.NearChunck(level.ToArray(), player_pos, 9999)).ToArray()));
            //Console.WriteLine(combined.vertices.Length);
            //Console.WriteLine(combined.uv.Length);
            //Console.WriteLine(combined.normal.Length);
            
            UpdateBuffers(combined.vertices, combined.uv, combined.normal, combined.textures, combined.colors);
        }

        void renderFrame(object o,FrameEventArgs e)
        {
            Vector2 mouseDelta = input.MouseDelta();
            float sensitivity = .1f;
            //GL.Rotate(mouseDelta.Y * sensitivity, 1, 0, 0);
            GL.Rotate(mouseDelta.X * sensitivity, 0, 1, 0);
            // Clear screen
            Vector3 move = Input.GetMoveDir(.1f);
            GL.Translate(move);
            player_pos += move;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //Render image

            //Enable thing before use
            GL.Enable(EnableCap.TextureCoordArray);
            //Setup blending
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Immedaiate test

            PrimitiveType type = PrimitiveType.Triangles;
            if (Quad)
                type = PrimitiveType.Quads;

            if (ImmediateRendering)
            {
                GL.Begin(type);

                for (int i = 0; i < vertexBuffer.Length; i++)
                {
                    GL.Normal3(normalBuffer[i]);
                    GL.TexCoord2(uvBuffer[i]);
                    GL.BindTexture(TextureTarget.Texture2D, textures[i].ID);
                    GL.Enable(EnableCap.Texture2D);
                    GL.Color4(colors[i]);
                    GL.Vertex3(vertexBuffer[i]);
                }

                GL.End();
            }
            else
            {
                //Enable arrays

                GL.EnableClientState(ArrayCap.VertexArray);
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                //GL.EnableClientState(ArrayCap.NormalArray);

                //GL.Color3(Color.Red);

                //Set vertex pointer
                GL.BindBuffer(BufferTarget.ArrayBuffer, VEB);
                GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, 0);

                //Set uv pointer

                GL.BindBuffer(BufferTarget.ArrayBuffer, UVB);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vector2.SizeInBytes, 0);

                //Set normal pointer

                /*GL.BindBuffer(BufferTarget.ArrayBuffer, NOB);
                GL.NormalPointer(NormalPointerType.Float, Vector3.SizeInBytes, 0);*/

                //Draw
                GL.Color3(Color.White);
                GL.DrawArrays(type, 0, vertexBuffer.Length);
            }

            time++;
            // Swap buffers
            GL.Flush();
            window.SwapBuffers();
            if(time >= 120)
            {
                time = 0;
                //LevelGenerator.UpdateLevel(this,level);
            }
        }

        public void UpdateBuffers(Vector3[] vertices,Vector2[] uvs,Vector3[] normals,Texture2D[] textures,Color[] colors)
        {
            //Vertex buffer

            vertexBuffer = vertices;
            GL.BindBuffer(BufferTarget.ArrayBuffer, VEB);
            GL.BufferSubData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)0, (int)(Vector3.SizeInBytes * vertexBuffer.Length), vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Uv buffer

            uvBuffer = uvs;
            GL.BindBuffer(BufferTarget.ArrayBuffer, UVB);
            GL.BufferSubData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)0, (int)(Vector2.SizeInBytes * uvBuffer.Length), uvBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Normal buffer

            normalBuffer = normals;
            GL.BindBuffer(BufferTarget.ArrayBuffer, NOB);
            GL.BufferSubData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)0, (int)(Vector3.SizeInBytes * normalBuffer.Length), normalBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            Console.WriteLine("Buffers updated");

            this.textures = textures;
            this.colors = colors;
        }
    }
}
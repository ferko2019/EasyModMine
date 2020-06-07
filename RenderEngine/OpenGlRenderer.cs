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

        //Texture

        public Texture2D texture = new Texture2D(0,Vector2.Zero);


        //Render settings
        public bool ImmediateRendering = false;
        public bool Quad = false;

        //Player data

        public Vector3 player_pos;

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
            texture = LoadResources.ReadTexture(Directory.GetCurrentDirectory() + @"\Mods\Default\Textures\earth.jpg");
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
        }

        void renderFrame(object o,FrameEventArgs e)
        {
            // Clear screen
            Vector3 move = Input.GetMoveDir(.1f);
            GL.Translate(move);
            player_pos += move;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //Render image

            //Enable thing before use
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
            GL.Enable(EnableCap.Texture2D);
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

            // Swap buffers
            GL.Flush();
            window.SwapBuffers();
        }

        public void UpdateBuffers(Vector3[] vertices,Vector2[] uvs,Vector3[] normals)
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
        }
    }
}
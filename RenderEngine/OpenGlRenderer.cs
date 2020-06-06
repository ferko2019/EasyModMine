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
            texture = LoadAsset.LoadTexture(Directory.GetCurrentDirectory() + @"\Mods\Default\Textures\earth.jpg");
            GL.Translate(0, 0, -5);
            //Change clear color
            GL.ClearColor(Color.Black);
            //Render polygons in a correct order
            GL.Enable(EnableCap.DepthTest);
            //Setup buffers

            //Vertex buffer

            VEB = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VEB);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (int)Vector3.SizeInBytes * (int)vertexBuffer.Length, vertexBuffer, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Uv buffer

            UVB = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, UVB);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (int)Vector2.SizeInBytes * (int)uvBuffer.Length, uvBuffer, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Normal buffer

            NOB = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, UVB);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (int)Vector3.SizeInBytes * (int)normalBuffer.Length, normalBuffer, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        void renderFrame(object o,FrameEventArgs e)
        {
            // Clear screen
            //GL.LoadIdentity();
            //GL.Rotate(1, 1, 0, 0);
            //GL.Rotate(1, 1, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Render image

            //Enable thing before use
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.EnableClientState(ArrayCap.VertexArray);
            //GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            //Setup blending

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

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
            //GL.Color4(Color.FromArgb(255,Color.White));
            GL.DrawArrays(PrimitiveType.Triangles,0,vertexBuffer.Length);

            // Swap buffers
            GL.Flush();
            window.SwapBuffers();
        }

        public void UpdateBuffers(Vector3[] vertices,Vector2[] uvs,Vector3[] normals)
        {
            //Vertex buffer

            vertexBuffer = vertices;
            GL.BindBuffer(BufferTarget.ArrayBuffer, VEB);
            GL.BufferSubData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)0, Vector3.SizeInBytes * vertexBuffer.Length, vertexBuffer);

            //Uv buffer

            uvBuffer = uvs;
            GL.BindBuffer(BufferTarget.ArrayBuffer, UVB);
            GL.BufferSubData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)0, Vector2.SizeInBytes * uvBuffer.Length, uvBuffer);

            //Normal buffer

            normalBuffer = normals;
            GL.BindBuffer(BufferTarget.ArrayBuffer, NOB);
            GL.BufferSubData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)0, Vector3.SizeInBytes * normalBuffer.Length, normalBuffer);

            Console.WriteLine("Buffers updated");
        }
    }
}
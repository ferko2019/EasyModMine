using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyModMine.BuiltInGameLogic
{
    class Input
    {
        public Vector2 lastPos;

        public static Vector3 GetMoveDir(float amount,Key forward = Key.W,Key backward = Key.S,Key left = Key.A,
            Key right = Key.D,Key up = Key.Q,Key down = Key.E)
        {
            Vector3 dir = Vector3.Zero;
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(forward))
                dir.Z += amount;
            if (state.IsKeyDown(backward))
                dir.Z -= amount;
            if (state.IsKeyDown(left))
                dir.X += amount;
            if (state.IsKeyDown(right))
                dir.X -= amount;
            if (state.IsKeyDown(up))
                dir.Y -= amount;
            if (state.IsKeyDown(down))
                dir.Y += amount;
            return dir;
        }

        public Vector2 MouseDelta()
        {
            MouseState state = Mouse.GetCursorState();
            float delta_x = state.X - lastPos.X;
            float delta_y = state.Y - lastPos.Y;

            /*Console.WriteLine(lastPos);
            Console.WriteLine(new Vector2(state.X,state.Y));
            Console.WriteLine(new Vector2(delta_x, delta_y));*/
            lastPos = new Vector2(state.X,state.Y);
            return new Vector2(delta_x, delta_y);
        }
    }
}

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
    }
}

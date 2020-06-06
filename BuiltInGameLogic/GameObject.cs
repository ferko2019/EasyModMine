using EasyModMine.BuiltInGameLogic;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyModMine.BuiltInGameLogic
{
    class GameObject
    {
        public string name = "null";
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public Modell modell;

        public GameObject(string name,Vector3 position,Vector3 rotation,Vector3 scale,Modell modell)
        {
            this.name = name;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.modell = modell;
        }
    }
}

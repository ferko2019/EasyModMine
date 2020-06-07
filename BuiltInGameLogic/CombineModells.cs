using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyModMine.BuiltInGameLogic
{
    class CombineModells
    {
        public static Modell Combine(Modell[] modells)
        {
            Modell final_modell = new Modell();
            int count = 0;
            foreach (var item in modells)
            {
                count += item.vertices.Length;
            }
            final_modell.vertices = new Vector3[count];
            final_modell.uv = new Vector2[count];
            final_modell.normal = new Vector3[count];
            int pos = 0;
            foreach (Modell modell in modells)
            {
                for (int i = 0; i < modell.vertices.Length; i++)
                {
                    final_modell.vertices[i + pos] = modell.vertices[i];
                }
                pos += modell.vertices.Length;
            }
            pos = 0;
            foreach (Modell modell in modells)
            {
                for (int i = 0; i < modell.normal.Length; i++)
                {
                    final_modell.normal[i + pos] = modell.normal[i];
                }
                pos += modell.normal.Length;
            }
            pos = 0;
            foreach (Modell modell in modells)
            {
                for (int i = 0; i < modell.uv.Length; i++)
                {
                    final_modell.uv[i + pos] = modell.uv[i];
                }
                pos += modell.uv.Length;
            }
            return final_modell;
        }

        public static Modell[] GameObjectToModellArray(GameObject[] objects)
        {
            List<Modell> modells = new List<Modell>();
            foreach (var item in objects)
            {
                modells.Add(item.ToTransformed());
            }
            return modells.ToArray();
        }
    }
}

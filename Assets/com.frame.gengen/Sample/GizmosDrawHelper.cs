using UnityEngine;

namespace JackFrame.GenGen.Sample {

    public static class GizmosDrawHelper {

        public static void DrawCube(Vector3 pos, Vector3 size, Color color) {
            Gizmos.color = color;
            Gizmos.DrawCube(new Vector3(pos.x * size.x, pos.y * size.y), size);
        }

    }

}
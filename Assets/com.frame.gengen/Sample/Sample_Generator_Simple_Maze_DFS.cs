using System;
using UnityEngine;

namespace JackFrame.GenGen.Sample {

    public class Sample_Generator_Simple_Maze_DFS : MonoBehaviour {

        GGSimpleMazeDFSGenerator generator;

        void Start() {
            generator = new GGSimpleMazeDFSGenerator();
            generator.Input(35, 37, 0, 0);
            generator.GenInstant();
        }

        void Update() {

            if (Input.GetKeyDown(KeyCode.Space)) {
                generator.GenByStep();
            }

        }

        void OnDrawGizmos() {

            if (generator == null) {
                return;
            }

            ReadOnlySpan<int> res = generator.GetMap();
            if (res.Length == 0) {
                return;
            }

            var size = generator.Size;

            Vector2 cubeSize = new Vector2(0.3f, 0.3f);

            Vec2Int pos;
            Color color = Color.black;
            for (int i = 0; i < res.Length; i += 1) {
                int value = res[i];
                pos = Vec2Int.FromArrayIndex(i, size.x);
                Vector2 drawPos = new Vector2(pos.x, pos.y);
                if (value == GGSimpleMazeDFSGenerator.NODE_WALL) {
                    color = Color.black;
                } else if (value == GGSimpleMazeDFSGenerator.NODE_ROAD) {
                    color = Color.white;
                }
                GizmosDrawHelper.DrawCube(drawPos, cubeSize, color);
            }

            color = Color.green;
            pos = generator.CurPos;
            GizmosDrawHelper.DrawCube(new Vector2(pos.x, pos.y), cubeSize, color);

        }

    }

}
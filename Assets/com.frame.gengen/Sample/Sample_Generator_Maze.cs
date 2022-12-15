using System;
using UnityEngine;

namespace JackFrame.GenGen.Sample {

    public class Sample_Generator_Maze : MonoBehaviour {

        GGMazeDFSGenerator generator;

        void Start() {
            generator = new GGMazeDFSGenerator();
            generator.Input(19, 19, 0, 0);
            // generator.GenInstant();
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

            // ReadOnlySpan<int> res = generator.GetResult();
            ReadOnlySpan<int> res = generator.GetResult();
            if (res.Length == 0) {
                return;
            }

            var size = generator.Size;

            Vector2 cubeSize = new Vector2(0.3f, 0.3f);

            Vec2Int pos;
            Color color = Color.black;
            for (int i = 0; i < res.Length; i += 1) {
                int value = res[i];
                pos = generator.GetPos(i);
                Vector2 drawPos = new Vector2(pos.x, pos.y);
                if (value == 0) {
                    color = Color.black;
                } else {
                    color = Color.white;
                }
                GizmosDrawHelper.DrawCube(drawPos, cubeSize, color);
            }

            color = Color.green;
            pos = generator.GetCurPos();
            GizmosDrawHelper.DrawCube(new Vector2(pos.x, pos.y), cubeSize, color);

        }

    }

}
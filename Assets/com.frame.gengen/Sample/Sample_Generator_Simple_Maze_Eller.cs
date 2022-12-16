using System;
using UnityEngine;

namespace JackFrame.GenGen.Sample {

    public class Sample_Generator_Simple_Maze_Eller : MonoBehaviour {

        GGSimpleMazeEllerGenerator generator;

        [SerializeField] Vector2Int size;
        [SerializeField] int setTypeCount;

        void Start() {
            generator = new GGSimpleMazeEllerGenerator();
            generator.Input(size.x, size.y, setTypeCount);
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

            GizmosDrawHelper.DrawMaze(res, generator.Cursor, generator.Size, new Vector2(0.3f, 0.3f));

        }

    }

}
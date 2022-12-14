using System;
using UnityEngine;

namespace JackFrame.GenGen.Sample {

    public class Sample : MonoBehaviour {

        void Awake() {

            Console.SetOut(new UnityTextWriter());

        }

        void OnDrawGizmos() {

            // Draw Perlin
            float c = 0;
            Vector2Int board = new Vector2Int(20, 20);
            Vector2 size = new Vector2(0.1f, 0.1f);
            for (int x = 0; x < board.x; x += 1) {
                for (int y = 0; y < board.y; y += 1) {
                    float nx = (float)x / board.x;
                    float ny = (float)y / board.y;
                    c = GGPerlinHelper.Noise(nx, ny);
                    Gizmos.color = new Color(c, c, c, 1);
                    Vector3 pos = new Vector3(x * size.x, y * size.y, 0);
                    Gizmos.DrawCube(pos, size);
                }
            }

        }

    }

}
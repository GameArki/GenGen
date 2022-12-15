using System;
using System.Collections.Generic;

namespace JackFrame.GenGen {

    // Simple Maze Eller
    // BottomLeft To TopRight
    /*
        1. x 向右逐次加入随机集合
        2. 相同集的横向墙打通
        3. 每个集合至少有一个向上的通路

    */
    public class GGSimpleMazeEllerGenerator {

        struct Node {
            public Vec2Int pos;
            public int setIndex;
        }

        public const int NODE_WALL = 0;
        public const int NODE_ROAD = 1;

        Random rd;

        int[] map;
        int setTypeCount;
        Node[] curLine;
        Node[] nextLine;

        int width;
        int height;

        Vec2Int cursor;

        bool isDone;

        public GGSimpleMazeEllerGenerator() { }

        public void Input(int width, int originalHeight, int setTypeCount, int seed = 0) {

            if (seed == 0) {
                this.rd = new Random();
            } else {
                this.rd = new Random(seed);
            }

            this.width = width;
            this.height = originalHeight;

            this.map = new int[width * originalHeight];
            
            this.setTypeCount = setTypeCount;

            this.cursor = Vec2Int.zero;

            this.curLine = new Node[width];
            this.nextLine = new Node[width];

            this.isDone = false;

        }

        public void GenByTimes(int times, bool isExtends) {
            if (isExtends) {
                height += times;
                isDone = false;
            }
            for (int i = 0; i < times; i += 1) {
                GenByStep();
            }
        }

        public void GenByStep() {

            if (isDone) {
                return;
            }

            // Cur Line
            var cur_line = curLine;
            var next_line = nextLine;

            if (cursor.x < width) {
                // 1. x 向右逐次加入随机集合
                var node = new Node();
                node.pos = cursor;
                node.setIndex = rd.Next(setTypeCount);
                cur_line[cursor.x] = node;
                cursor.x += 2;

            } else {
                // 2. 相同集的横向墙打通
                for (int i = 0; i < width - 1; i += 1) {
                    var cur_node = cur_line[i];
                    var next_node = cur_line[i + 1];
                    if (cur_node.setIndex == next_node.setIndex) {
                        var pos = cur_node.pos;
                        pos.x += 1;
                        map[pos.y * width + pos.x] = NODE_ROAD;
                    }
                }
                // 3. 每个集合至少有一个向上的通路
                for (int i = 0; i < width; i += 1) {
                    var cur_node = cur_line[i];
                    var next_node = new Node();
                    next_node.pos = cur_node.pos;
                    next_node.pos.y += 1;
                    next_node.setIndex = cur_node.setIndex;
                    next_line[i] = next_node;
                }
                // 4. 重置
                cursor.x = 0;
                cursor.y += 2;
                var tmp = cur_line;
                cur_line = next_line;
                next_line = tmp;
            }

        }

        int GetIndex(int x, int y) {
            return y * width + x;
        }

    }

}
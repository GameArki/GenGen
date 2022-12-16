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
            public int setIndex;
        }

        public const int NODE_WALL = 0;
        public const int NODE_ROAD = 1;

        Random rd;

        int[] map;
        int setTypeCount;

        int width;
        int height;
        public Vec2Int Size => new Vec2Int(width, height);

        enum Status { None, CurLine_Gen, NextLine_GenAtLeastOne, NextLine_GenGap, NextLine_MergeVertical, LastLine_MergeAll, Done }
        Status status;

        Node[] curLine;
        Node[] nextLine;

        int cursorX;
        int cursorY;
        public Vec2Int Cursor => new Vec2Int(cursorX, cursorY);

        int nextCursorX;

        public GGSimpleMazeEllerGenerator() {
            this.status = Status.None;
        }

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

            this.cursorX = 0;
            this.cursorY = 0;

            this.curLine = new Node[width];
            this.nextLine = new Node[width];

            status = Status.CurLine_Gen;

        }

        public void GenInstant() {
            while (status != Status.Done) {
                GenByStep();
            }
        }

        public void GenByStep() {

            if (status == Status.Done) {
                return;
            }

            // Status
            // 1. 向右逐次加入随机集合
            // 2. 相同集的横向墙打通
            // 3. 每个集合至少有一个随机向上的通路
            // 4. 随机补充下一行的空白
            // 5. 如果下一行的同 index 的节点是相同 set，则向上打通
            // 如果下一行是最后一行，则左右全打通
            // 如果下一行不是最后一行，重复 1

            // Cur Line
            var cur_line = curLine;
            var next_line = nextLine;

            if (status == Status.CurLine_Gen) {
                CurLine_Gen();
            } else if (status == Status.NextLine_GenAtLeastOne) {
                NextLine_GenAtLeastOne();
            } else if (status == Status.LastLine_MergeAll) {
                LastLine_MergeAll();
            }

        }

        // 从左到右, 生成随机 set
        // 并打通左边相同 set 的墙
        void CurLine_Gen() {

            var node = new Node();
            node.setIndex = rd.Next(setTypeCount);
            curLine[cursorX] = node;

            var index = GetIndex(cursorX, cursorY);
            map[index] = NODE_ROAD;

            if (cursorX - 2 >= 0) {
                var left = curLine[cursorX - 2];
                if (left.setIndex == node.setIndex) {
                    var wallIndex = GetIndex(cursorX - 1, cursorY);
                    map[wallIndex] = NODE_ROAD;
                }
            }

            if (cursorX + 2 >= width) {
                cursorX = 0;
                if (cursorY + 2 >= height) {
                    status = Status.LastLine_MergeAll;
                } else {
                    cursorY += 2;
                    status = Status.NextLine_GenAtLeastOne;
                }
            } else {
                cursorX += 2;
            }

        }

        // 相同 set 向下延伸至少一格
        void NextLine_GenAtLeastOne() {

            var curNode = curLine[nextCursorX];
            int curSet = curNode.setIndex;

            var nextNode = new Node();
            nextNode.setIndex = rd.Next(setTypeCount);
            nextLine[nextCursorX] = nextNode;

            // 相同 set 打通
            if (curSet == nextNode.setIndex) {
                var roadIndex = GetIndex(nextCursorX, cursorY - 1);
                map[roadIndex] = NODE_ROAD;
            }

            bool hasRightNode = TryGetCurLineRightNode(out int setIndex);
            if (hasRightNode) {
                if (setIndex != curSet) {

                    // 如果右边不是相同 set 检测从左边界到该节点的下一行是否有相同 set
                    // 如果没有, 挑一个生成并打通
                    int leftBoardIndex = -1;
                    for (int i = nextCursorX - 2; i >= 0; i -= 2) {
                        var node = nextLine[i];
                        if (node.setIndex == setIndex) {
                            leftBoardIndex = i;
                            break;
                        }
                    }

                    if (leftBoardIndex != -1) {
                        int genIndex = rd.Next(leftBoardIndex, nextCursorX);
                        nextLine[genIndex].setIndex = setIndex;
                    }

                }
            } else {
                status = Status.NextLine_MergeVertical;
            }

        }

        bool TryGetCurLineRightNode(out int setIndex) {
            if (nextCursorX + 2 < width) {
                var node = curLine[nextCursorX + 2];
                setIndex = node.setIndex;
                return true;
            } else {
                setIndex = -1;
                return false;
            }
        }

        // 最后一行
        void LastLine_MergeAll() {
            if (cursorX + 1 < width) {
                int wallIndex = GetIndex(cursorX + 1, cursorY);
                map[wallIndex] = NODE_ROAD;

                if (cursorX + 2 >= width) {
                    cursorX = 0;
                    status = Status.Done;
                } else {
                    cursorX += 2;
                }
            } else {
                cursorX = 0;
                status = Status.Done;
            }
        }

        int GetIndex(int x, int y) {
            return y * width + x;
        }

        Vec2Int GetCursor() {
            return new Vec2Int(cursorX, cursorY);
        }

        public ReadOnlySpan<int> GetMap() {
            return map;
        }

    }

}
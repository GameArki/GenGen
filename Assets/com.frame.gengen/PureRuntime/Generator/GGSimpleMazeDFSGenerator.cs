using System;
using System.Collections.Generic;

namespace JackFrame.GenGen {

    // Simple Maze (DFS)
    // From BottomLeft(0, 0) To TopRight(width, height)
    public class GGSimpleMazeDFSGenerator {

        Random rd;

        // ==== Start ====
        int[] map;
        int maxCount;

        Vec2Int size;
        public Vec2Int Size => size;

        // ==== Process ====
        int visitedCount;
        Stack<Vec2Int> visitedIndexStack;

        // ==== Result ====
        bool isDone;
        public bool IsDone => isDone;

        // ==== Temp ====
        Vec2Int curPos;
        public Vec2Int CurPos => curPos;
        
        List<Vec2Int> tmpDirList;

        public GGSimpleMazeDFSGenerator() { }

        public void Input(int width, int height, int startX, int startY) {

            this.map = new int[width * height];
            this.size = new Vec2Int() { x = width, y = height };
            this.rd = new Random();

            this.visitedCount = 0;
            this.maxCount = width * height;

            this.visitedIndexStack = new Stack<Vec2Int>();

            this.isDone = false;

            this.tmpDirList = new List<Vec2Int>(4);

            GenFirstStep(startX, startY);

        }

        // 一次性生成
        public void GenInstant() {
            while (!isDone) {
                GenByStep();
            }
        }

        void GenFirstStep(int x, int y) {
            int index = GetIndex(x, y);
            map[index] = 2;
            visitedIndexStack.Push(new Vec2Int(x, y));
            visitedCount += 1;
            curPos = new Vec2Int(x, y);
        }

        public void GenByStep() {

            if (isDone) {
                return;
            }

            // src = [0, 0, 0 ......]
            // 0  : 未访问
            // 1  : 已访问
            // 2  : 路
            // 3  : 障碍

            // 1. 随机选择一个方向
            // 2. 如果方向上未访问过，则标记为路，然后继续往前走
            // 3. 如果方向上已经访问过，则选择另一个方向
            // 4. 如果所有方向都已经访问过，则回退一步

            var dirList = tmpDirList;
            dirList.Clear();
            Vec2Int up = new Vec2Int(curPos.x, curPos.y + 2);
            Vec2Int down = new Vec2Int(curPos.x, curPos.y - 2);
            Vec2Int left = new Vec2Int(curPos.x - 2, curPos.y);
            Vec2Int right = new Vec2Int(curPos.x + 2, curPos.y);
            if (GetDirValue(up) == 0) {
                dirList.Add(up);
            }
            if (GetDirValue(down) == 0) {
                dirList.Add(down);
            }
            if (GetDirValue(left) == 0) {
                dirList.Add(left);
            }
            if (GetDirValue(right) == 0) {
                dirList.Add(right);
            }

            // shuffle
            for (int i = 0; i < dirList.Count; i += 1) {
                int index = rd.Next(0, dirList.Count);
                var tmp = dirList[i];
                dirList[i] = dirList[index];
                dirList[index] = tmp;
            }

            var visited = visitedIndexStack;
            if (dirList.Count == 0) {
                if (IsAllVisited() || visited.Count == 0) {
                    isDone = true;
                    visited.Clear();
                    return;
                }
                // 4. 如果所有方向都已经访问过，则回退一步
                _ = visited.Pop();
                curPos = visited.Peek();
                return;
            } else {
                // 1. 随机选择一个方向
                var nextPos = dirList[0];
                var arr = map;
                // 2. 方向上未访问过，标记为路
                // 3. 经过的中间点标记为路
                var diff = nextPos - curPos;
                var passPos = curPos + diff / 2;
                arr[GetIndex(nextPos)] = 2;
                arr[GetIndex(passPos)] = 2;
                visited.Push(nextPos);
                curPos = nextPos;
            }

        }

        bool IsAllVisited() {
            return visitedCount >= maxCount;
        }

        int GetIndex(int x, int y) {
            return x + y * size.x;
        }

        int GetIndex(Vec2Int pos) {
            return pos.x + pos.y * size.x;
        }

        public Vec2Int GetPos(int index) {
            return new Vec2Int() { x = index % size.x, y = index / size.x };
        }

        int GetDirValue(int x, int y) {
            if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
                return 3;
            }
            return map[x + y * size.x];
        }

        int GetDirValue(Vec2Int pos) {
            return GetDirValue(pos.x, pos.y);
        }

        public ReadOnlySpan<int> GetMap() {
            return map;
        }

    }

}
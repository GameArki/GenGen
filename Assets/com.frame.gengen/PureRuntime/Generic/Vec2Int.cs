namespace JackFrame.GenGen {

    public struct Vec2Int {
        
        public static Vec2Int zero => new Vec2Int() { x = 0, y = 0 };

        public int x;
        public int y;

        public Vec2Int(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public Vec2Int(int v) {
            this.x = v;
            this.y = v;
        }

        public Vec2Int(Vec2Int v) {
            this.x = v.x;
            this.y = v.y;
        }

        public static Vec2Int operator +(Vec2Int a, Vec2Int b) {
            return new Vec2Int(a.x + b.x, a.y + b.y);
        }

        public static Vec2Int operator -(Vec2Int a, Vec2Int b) {
            return new Vec2Int(a.x - b.x, a.y - b.y);
        }

        public static Vec2Int operator *(Vec2Int a, int b) {
            return new Vec2Int(a.x * b, a.y * b);
        }

        public static Vec2Int operator /(Vec2Int a, int b) {
            return new Vec2Int(a.x / b, a.y / b);
        }

    }
}
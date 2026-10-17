using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnionFind
{
    class Program
    {
        static void Main(string[] args)
        {
            var uf = new UnionFind(5);

            uf.Merge(0, 1);
            uf.Merge(1, 2);

            Console.WriteLine(uf.Same(0, 2));
            Console.WriteLine(uf.Same(0, 3));
            Console.WriteLine(uf.Size(0));

        }
    }

    /// <summary>
    /// 2つの値を持つクラス。
    /// ソート時に値の比較が可能。
    /// 3つの値なら、Pair<型1, Pair<型2, 型3>> で可能。
    /// </summary>
    /// <typeparam name="T1">1つ目の値の型</typeparam>
    /// <typeparam name="T2">2つ目の値の型</typeparam>
    class Pair<T1, T2> : IComparable
        where T1 : IComparable
        where T2 : IComparable
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }

        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        public int CompareTo(object obj)
        {
            var o = (Pair<T1, T2>)obj;
            var c1 = First.CompareTo(o.First);

            return (c1 != 0) ? c1 : Second.CompareTo(o.Second);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", First, Second);
        }
    }

    /// <summary>
    /// (X, Y) 座標を偏角で比較できるクラス。
    /// </summary>
    class XYAngle : IComparable
    {
        public int Index { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
        public double Angle { get; set; }

        public XYAngle(int index, long x, long y)
        {
            Index = index;
            X = x;
            Y = y;
            Angle = Math.Atan2(y, x);
        }

        public int CompareTo(object obj)
        {
            var a = this;
            var b = (XYAngle)obj;
            bool ah = (a.Y < 0 || (a.Y == 0 && a.X < 0));
            bool bh = (b.Y < 0 || (b.Y == 0 && b.X < 0));
            if (ah != bh) return bh ? -1 : 1;
            return -Math.Sign(a.X * b.Y - a.Y * b.X);
        }

        public override string ToString()
        {
            return string.Format("{0}({1},{2}): Angle={1}", Index, X, Y, Angle);
        }
    }

    class Point<T>
    {
        public T X { get; set; }
        public T Y { get; set; }

        public Point(T x, T y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// 長方形を定義するクラス。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Rect<T>
    {
        public T Left { get; set; }
        public T Right { get; set; }
        public T Top { get; set; }
        public T Bottom { get; set; }

        public Rect(T left, T right, T top, T bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// 反時計回りに90度回転する。
        /// </summary>
        public void Rotate()
        {
            T newLeft = Bottom;
            T newRight = Top;
            T newTop = (T)(-(dynamic)Left);
            T newBottom = (T)(-(dynamic)Right);
            Left = newLeft;
            Right = newRight;
            Top = newTop;
            Bottom = newBottom;
        }
    }

    /// <summary>
    /// Binary Indexed Tree(Fenwick Tree)
    /// 0スタート
    /// </summary>
    class BIT
    {
        public List<long> Data { get; set; }

        public BIT(int n)
        {
            Data = new List<long>();
            Data.AddRange(new long[n + 1]);
        }

        public void Add(int i, long x)
        {
            i++;
            while (i < Data.Count)
            {
                Data[i] += x;
                i += i & -i;
            }
        }

        public long Sum(int[] args)
        {
            if (args.Length == 1)
            {
                int r = args[0];
                long s = 0;
                while (r != 0)
                {
                    s += Data[r];
                    r -= r & -r;

                }
                return s;
            }
            else
            {
                int l = args[0];
                int r = args[1];
                return Sum(new int[] { r }) - Sum(new int[] { l });
            }
        }

        public int lower_bound(long w)
        {
            int x = 0;
            int n = Data.Count - 1;
            int k = 1 << (int)Math.Log(n, 2);
            while (k != 0)
            {
                if (x + k <= n && Data[x + k] < w)
                {
                    w -= Data[x + k];
                    x += k;
                }
                k >>= 1;
            }
            return x + 1;
        }
    }

    /// <summary>
    /// PriorityQueue
    /// https://yambe2002.hatenablog.com/entry/2015/11/01/114503
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T> where T : IComparable
    {
        private IComparer<T> _comparer = null;
        private int _type = 0;

        private T[] _heap;
        private int _sz = 0;

        private int _count = 0;

        /// <summary>
        /// Priority Queue with custom comparer
        /// </summary>
        public PriorityQueue(int maxSize, IComparer<T> comparer)
        {
            _heap = new T[maxSize];
            _comparer = comparer;
        }

        /// <summary>
        /// Priority queue
        /// </summary>
        /// <param name="maxSize">max size</param>
        /// <param name="type">0: asc, 1:desc</param>
        public PriorityQueue(int maxSize, int type = 0)
        {
            _heap = new T[maxSize];
            _type = type;
        }

        private int Compare(T x, T y)
        {
            if (_comparer != null) return _comparer.Compare(x, y);
            return _type == 0 ? x.CompareTo(y) : y.CompareTo(x);
        }

        public void Push(T x)
        {
            _count++;

            //node number
            var i = _sz++;

            while (i > 0)
            {
                //parent node number
                var p = (i - 1) / 2;

                if (Compare(_heap[p], x) <= 0) break;

                _heap[i] = _heap[p];
                i = p;
            }

            _heap[i] = x;
        }

        public T Pop()
        {
            _count--;

            T ret = _heap[0];
            T x = _heap[--_sz];

            int i = 0;
            while (i * 2 + 1 < _sz)
            {
                //children
                int a = i * 2 + 1;
                int b = i * 2 + 2;

                if (b < _sz && Compare(_heap[b], _heap[a]) < 0) a = b;

                if (Compare(_heap[a], x) >= 0) break;

                _heap[i] = _heap[a];
                i = a;
            }

            _heap[i] = x;

            return ret;
        }

        public int Count()
        {
            return _count;
        }

        public T Peek()
        {
            return _heap[0];
        }

        public bool Contains(T x)
        {
            for (int i = 0; i < _sz; i++) if (x.Equals(_heap[i])) return true;
            return false;
        }

        public void Clear()
        {
            while (this.Count() > 0) this.Pop();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var ret = new List<T>();

            while (this.Count() > 0)
            {
                ret.Add(this.Pop());
            }

            foreach (var r in ret)
            {
                this.Push(r);
                yield return r;
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[_sz];
            int i = 0;

            foreach (var r in this)
            {
                array[i++] = r;
            }

            return array;
        }
    }

    /// <summary>
    /// https://zenn.dev/student_blog/articles/csharp-union-find
    /// </summary>
    class UnionFind
    {
        private readonly int[] _parentOrSize;

        public UnionFind(int n)
        {
            _parentOrSize = new int[n];
            //Array.Fill(_parentOrSize, -1);
            for (int i = 0; i < n; i++)
            {
                _parentOrSize[i] = -1;
            }
        }

        public int Leader(int x)
        {
            if (_parentOrSize[x] < 0)
            {
                return x;
            }

            return _parentOrSize[x] = Leader(_parentOrSize[x]);
        }

        public bool Merge(int a, int b)
        {
            int x = Leader(a);
            int y = Leader(b);

            if (x == y)
            {
                return false;
            }

            if (-_parentOrSize[x] < -_parentOrSize[y])
            {
                int w = x;
                x = y;
                y = w;
            }

            _parentOrSize[x] += _parentOrSize[y];
            _parentOrSize[y] = x;

            return true;
        }

        public bool Same(int a, int b)
        {
            return Leader(a) == Leader(b);
        }

        public int Size(int x)
        {
            return -_parentOrSize[Leader(x)];
        }
    }
}

namespace BlockedNQueensAStar
{
    public readonly record struct Pos(int Row, int Col);

    public enum HeuristicType
    {
        H0_Zero,
        H1_Conflict,   // NOT admissible
        H2_Remaining   // admissible (recommended)
    }

    public sealed class HeuristicContext
    {
        public int N { get; }
        public HashSet<Pos> Blocked { get; }
        public HeuristicContext(int n, IEnumerable<Pos> blocked)
        {
            N = n;
            Blocked = new HashSet<Pos>(blocked);
        }
    }

    public sealed class State
    {
        public int[] QueenCols { get; }
        public int Depth { get; }   
        public string Key => ToString(); 

        public State(int[] queenCols, int depth)
        {
            QueenCols = queenCols;
            Depth = depth;
        }

        public State AddQueen(int col)
        {
            var arr = new int[QueenCols.Length];
            Array.Copy(QueenCols, arr, QueenCols.Length);
            arr[Depth] = col;
            return new State(arr, Depth + 1);
        }

        public override string ToString()
        {
            return string.Join(",", QueenCols.Take(Depth).Select(c => c.ToString()));
        }

    }

    internal sealed class Node
    {
        public State State { get; }
        public int G { get; }  
        public int H { get; } 
        public int F => G + H;
        public Node? Parent { get; } 

        public Node(State s, int g, int h, Node? parent)
        {
            State = s; G = g; H = h; Parent = parent;
        }
    }

    public sealed class BlockedNQueensSolver
    {
        private readonly int _n;
        private readonly HashSet<Pos> _blocked;
        private readonly Func<State, HeuristicContext, int> _heuristic;
        private readonly HeuristicContext _ctx;

        public BlockedNQueensSolver(int n, IEnumerable<Pos> blocked, HeuristicType hType)
        {
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));
            _n = n;
            _blocked = new HashSet<Pos>(blocked);
            _ctx = new HeuristicContext(n, _blocked);
            _heuristic = hType switch
            {
                HeuristicType.H0_Zero => Heuristics.Zero,
                HeuristicType.H1_Conflict => Heuristics.Conflict,
                HeuristicType.H2_Remaining => Heuristics.Remaining,
                _ => Heuristics.Remaining
            };
        }

        public int[]? Solve()
        {
            var startCols = new int[_n]; 
            var start = new State(startCols, 0);
            var h0 = _heuristic(start, _ctx);
            var startNode = new Node(start, 0, h0, null);

            var open = new PriorityQueue<Node, (int F, int G, int Tie)>(
                Comparer<(int F, int G, int Tie)>.Create((a, b) =>
                {
                    int cmp = a.F.CompareTo(b.F);
                    if (cmp != 0) return cmp;

                    cmp = a.G.CompareTo(b.G);
                    if (cmp != 0) return cmp;

                    return a.Tie.CompareTo(b.Tie);
                }));

            int tie = 0;
            open.Enqueue(startNode, (startNode.F, startNode.G, tie++));

            var closed = new HashSet<string>();

            while (open.Count > 0)
            {
                var current = open.Dequeue();
                var s = current.State;

                if (s.Depth == _n)
                {
                    return Reconstruct(current);
                }

                if (!closed.Add(s.Key))
                    continue; 

                int row = s.Depth;
                for (int col = 0; col < _n; col++)
                {
                    if (_blocked.Contains(new Pos(row, col)))
                        continue;
                    if (InConflict(row, col, s))
                        continue;

                    var childState = s.AddQueen(col);
                    var g = current.G + 1;
                    var h = _heuristic(childState, _ctx);
                    if (h == int.MaxValue) 
                        continue;
                    var child = new Node(childState, g, h, current);
                    open.Enqueue(child, (child.F, child.G, tie++));
                }
            }

            return null;
        }

        private static bool InConflict(int row, int col, State s)
        {
            for (int r = 0; r < s.Depth; r++)
            {
                int c = s.QueenCols[r];
                if (c == col) return true; 
                if (Math.Abs(row - r) == Math.Abs(col - c)) return true;
            }
            return false;
        }

        private static int[] Reconstruct(Node goal)
        {
            var stack = new Stack<int>();
            for (var n = goal; n != null; n = n.Parent)
            {
                if (n.State.Depth > 0)
                {
                    int row = n.State.Depth - 1;
                    stack.Push(n.State.QueenCols[row]);
                }
            }
            return stack.ToArray(); 
        }
    }

    public static class Heuristics
    {
        public static int Zero(State s, HeuristicContext ctx) => 0;
        public static int Remaining(State s, HeuristicContext ctx)
            => ctx.N - s.Depth;

        public static int Conflict(State s, HeuristicContext ctx)
        {
            int n = ctx.N;
            int placed = s.Depth;
            int remaining = n - placed;
            if (remaining <= 0) return 0;

            var usedCols = new bool[n];
            var usedD1 = new HashSet<int>(); 
            var usedD2 = new HashSet<int>(); 
            for (int r = 0; r < placed; r++)
            {
                int c = s.QueenCols[r];
                usedCols[c] = true;
                usedD1.Add(r + c);
                usedD2.Add(r - c);
            }

            var newQueens = new List<Pos>(remaining);
            for (int r = placed; r < n; r++)
            {
                bool found = false;
                for (int c = 0; c < n; c++)
                {
                    if (usedCols[c]) continue;
                    if (ctx.Blocked.Contains(new Pos(r, c))) continue;
                    if (usedD1.Contains(r + c) || usedD2.Contains(r - c)) continue;
                    usedCols[c] = true;
                    usedD1.Add(r + c);
                    usedD2.Add(r - c);
                    newQueens.Add(new Pos(r, c));
                    found = true;
                    break;
                }
                if (!found)
                {
                    return int.MaxValue;
                }
            }

            int conflicts = 0;
            for (int i = 0; i < newQueens.Count; i++)
            {
                for (int j = i + 1; j < newQueens.Count; j++)
                {
                    var a = newQueens[i];
                    var b = newQueens[j];
                    if (Math.Abs(a.Row - b.Row) == Math.Abs(a.Col - b.Col))
                        conflicts++;
                }
            }

            return remaining + conflicts; 
        }
    }

    public static class BoardPrinter
    {
        public static void Print(int[]? cols, int n, HashSet<Pos>? blocked = null)
        {
            if (cols == null)
            {
                Console.WriteLine("No solution.");
                return;
            }
            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    if (blocked != null && blocked.Contains(new Pos(r, c)))
                    {
                        Console.Write("# ");
                    }
                    else if (r < cols.Length && cols[r] == c)
                    {
                        Console.Write("Q ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            int N = 8;
            var blocked = new List<Pos>
            {
                new Pos(0,0),
                new Pos(3,5),
                new Pos(4,4)
            };

            foreach (HeuristicType ht in Enum.GetValues(typeof(HeuristicType)))
            {
                Console.WriteLine($"\n=== Trying {ht} ===");
                var solver = new BlockedNQueensSolver(N, blocked, ht);
                var sol = solver.Solve();
                BoardPrinter.Print(sol, N, new HashSet<Pos>(blocked));
            }
        }
    }

    internal static class Program
    {
        static void Main(string[] args)
        {
            
            Demo.Run();
            Console.WriteLine("press any key to end!");
            Console.ReadKey();
        }
    }
}

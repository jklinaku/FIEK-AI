using System;
using System.Collections.Generic;
using System.Linq;

class Cage
{
    public int Sum;
    public List<(int, int)> Cells;
    public Cage(int sum, IEnumerable<(int, int)> cells)
    {
        Sum = sum;
        Cells = cells.ToList();
    }
}

class KillerSudokuCSP
{
    private Dictionary<(int, int), HashSet<int>> domains;
    private List<Cage> cages;
    private Dictionary<(int, int), Cage> cellToCage;
    private int[,] grid;

    public KillerSudokuCSP(List<Cage> cages)
    {
        this.cages = cages;
        grid = new int[9, 9];
        domains = new Dictionary<(int, int), HashSet<int>>();
        cellToCage = new Dictionary<(int, int), Cage>();

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                domains[(r, c)] = new HashSet<int>(Enumerable.Range(1, 9));
            }
        }

        foreach (var cage in cages)
        {
            foreach (var cell in cage.Cells)
            {
                cellToCage[cell] = cage;
            }
        }

        foreach (var cage in cages)
        {
            if (cage.Cells.Count == 1)
            {
                var (r, c) = cage.Cells[0];
                Assign((r, c), cage.Sum);
            }
        }
    }

    private bool Assign((int, int) cell, int value)
    {
        grid[cell.Item1, cell.Item2] = value;
        domains[cell] = new HashSet<int> { value };
        return ForwardCheck(cell, value);
    }

    private bool ForwardCheck((int, int) cell, int value)
    {
        int r = cell.Item1, c = cell.Item2;

        for (int i = 0; i < 9; i++)
        {
            if (i != c && domains[(r, i)].Remove(value) && domains[(r, i)].Count == 0)
                return false;
            if (i != r && domains[(i, c)].Remove(value) && domains[(i, c)].Count == 0)
                return false;
        }

        int boxR = (r / 3) * 3;
        int boxC = (c / 3) * 3;
        for (int rr = boxR; rr < boxR + 3; rr++)
        {
            for (int cc = boxC; cc < boxC + 3; cc++)
            {
                if ((rr, cc) != cell && domains[(rr, cc)].Remove(value) && domains[(rr, cc)].Count == 0)
                    return false;
            }
        }

        if (!CheckCageConstraints(cellToCage[cell]))
            return false;

        return true;
    }

    private bool CheckCageConstraints(Cage cage)
    {
        int filledSum = 0;
        int emptyCount = 0;
        HashSet<int> used = new HashSet<int>();

        foreach (var (r, c) in cage.Cells)
        {
            int val = grid[r, c];
            if (val > 0)
            {
                if (used.Contains(val)) return false; 
                used.Add(val);
                filledSum += val;
            }
            else
            {
                emptyCount++;
            }
        }

        if (filledSum > cage.Sum) return false;

        int minPossible = filledSum;
        int maxPossible = filledSum;

        var available = Enumerable.Range(1, 9).Where(x => !used.Contains(x)).ToList();
        available.Sort();

        for (int i = 0; i < emptyCount; i++)
        {
            minPossible += available[i];
            maxPossible += available[available.Count - 1 - i];
        }

        if (minPossible > cage.Sum || maxPossible < cage.Sum) return false;

        return true;
    }

    private (int, int)? SelectUnassigned()
    {
        (int, int)? best = null;
        int minSize = int.MaxValue;

        foreach (var cell in domains.Keys)
        {
            if (grid[cell.Item1, cell.Item2] == 0)
            {
                int size = domains[cell].Count;
                if (size < minSize)
                {
                    minSize = size;
                    best = cell;
                }
            }
        }
        return best;
    }

    public bool Solve()
    {
        var cell = SelectUnassigned();
        if (cell == null)
            return true; 

        var domainValues = domains[cell.Value].ToList();
        foreach (var val in domainValues)
        {
            var backupGrid = (int[,])grid.Clone();
            var backupDomains = domains.ToDictionary(entry => entry.Key, entry => new HashSet<int>(entry.Value));

            if (IsSafe(cell.Value.Item1, cell.Value.Item2, val))
            {
                if (Assign(cell.Value, val))
                {
                    if (Solve())
                        return true;
                }
            }

            grid = backupGrid;
            domains = backupDomains;
        }
        return false;
    }

    private bool IsSafe(int r, int c, int val)
    {
        for (int i = 0; i < 9; i++)
        {
            if (grid[r, i] == val || grid[i, c] == val) return false;
        }
        int boxR = (r / 3) * 3;
        int boxC = (c / 3) * 3;
        for (int rr = boxR; rr < boxR + 3; rr++)
        {
            for (int cc = boxC; cc < boxC + 3; cc++)
            {
                if (grid[rr, cc] == val) return false;
            }
        }
        return CheckCageConstraints(cellToCage[(r, c)]);
    }

    public void PrintGrid()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                Console.Write(grid[r, c] == 0 ? "." : grid[r, c]);
                if (c < 8) Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main()
    {
        var cages = new List<Cage> {
            new Cage(2,   [(0,0)]),
            new Cage(22,  [(0,1), (1,1), (1,2)]),
            new Cage(12,  [(0,2), (0,3), (0,4)]),
            new Cage(22,  [(0,5), (1,4), (1,5), (2,4)]),
            new Cage(23,  [(0,6), (0,7), (0,8), (1,8)]),
            new Cage(8,   [(1,0), (2,0), (3,0)]),
            new Cage(16,  [(1,3), (2,2), (2,3), (3,3)]),
            new Cage(6,   [(1,6), (2,6)]),
            new Cage(18,  [(1,7), (2,7), (3,7)]),
            new Cage(13,  [(2,1), (3,1)]),
            new Cage(16,  [(2,5), (3,4), (3,5)]),
            new Cage(8,   [(2,8), (3,8)]),
            new Cage(5,   [(3,2)]),
            new Cage(20,  [(3,6), (4,6), (5,6), (5,7)]),
            new Cage(12,  [(4,0), (4,1), (5,1)]),
            new Cage(11,  [(4,2), (4,3)]),
            new Cage(21,  [(4,4), (4,5), (5,5)]),
            new Cage(10,  [(4,7), (4,8)]),
            new Cage(7,   [(5,0)]),
            new Cage(6,   [(5,2), (6,2)]),
            new Cage(9,   [(5,3), (6,3)]),
            new Cage(6,   [(5,4)]),
            new Cage(5,   [(5,8)]),
            new Cage(15,  [(6,0), (6,1)]),
            new Cage(4,   [(6,4), (6,5)]),
            new Cage(28,  [(6,6), (6,7), (7,6), (8,6), (8,7), (8,8)]),
            new Cage(17,  [(6,8), (7,7), (7,8)]),
            new Cage(15,  [(7,0), (8,0), (8,1)]),
            new Cage(3,   [(7,1)]),
            new Cage(18,  [(7,2), (7,3), (8,2)]),
            new Cage(4,   [(7,4)]),
            new Cage(2,   [(7,5)]),
            new Cage(21,  [(8,3), (8,4), (8,5)])
        };

        var solver = new KillerSudokuCSP(cages);
        if (solver.Solve())
        {
            Console.WriteLine("Zgjidhja u gjet:");
            solver.PrintGrid();
        }
        else
        {
            Console.WriteLine("Nuk u gjet zgjidhje.");
        }
    }
}

using System;

class LatinSquareSolver
{
    static bool[,] rowUsed;  
    static bool[,] colUsed; 
    static int[,] grid;
    static int n;

    public static void Main(string[] args)
    {
        //************** EDITABLE ******************
        n = 5;
        //************** EDITABLE ******************




        grid = new int[n, n];
        rowUsed = new bool[n, n + 1];
        colUsed = new bool[n, n + 1];

        if (SolveLatinSquare())
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("No solution found.");
        }
        Console.WriteLine("press any key to end!");
        Console.ReadKey();
    }

    static bool SolveLatinSquare()
    {
        for (int depth = 1; depth <= n * n; depth++)
        {
            if (DepthLimitedDFS(0, depth))
                return true;
        }
        return false; 
    }

    static bool DepthLimitedDFS(int index, int remainingDepth)
    {
        if (index == n * n)
        {
            return true;
        }
        if (remainingDepth == 0)
        {
            return false;
        }

        int r = index / n;
        int c = index % n;

        for (int val = 1; val <= n; val++)
        {
            if (!rowUsed[r, val] && !colUsed[c, val])
            {
                grid[r, c] = val;
                rowUsed[r, val] = true;
                colUsed[c, val] = true;

                if (DepthLimitedDFS(index + 1, remainingDepth - 1))
                {
                    return true;  
                }

                // Backtrack: undo the move and try next value
                grid[r, c] = 0;
                rowUsed[r, val] = false;
                colUsed[c, val] = false;
            }
        }

        return false;
    }
}

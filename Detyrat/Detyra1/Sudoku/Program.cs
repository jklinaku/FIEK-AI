using System;
using System.Collections.Generic;

class Program
{
    // Predefined Sudoku puzzles for each difficulty.
    static int[][,] easyPuzzles = new int[][,]
    {
        new int[,] {
            {3,0,6,5,0,0,4,9,0},
            {5,2,0,0,3,0,7,0,0},
            {4,8,0,6,2,9,0,3,1},
            {2,0,3,4,0,0,0,0,7},
            {0,7,4,0,6,0,1,2,0},
            {8,0,0,0,0,2,6,0,3},
            {1,3,0,9,4,7,0,5,6},
            {0,0,2,0,5,0,0,7,4},
            {0,4,5,0,0,6,3,0,9}
        },
        new int[,] {
            {4,9,2,3,0,0,5,1,8},
            {3,8,1,9,0,0,6,2,7},
            {0,7,0,0,0,0,3,4,9},
            {0,2,4,0,9,3,1,0,6},
            {1,0,0,0,2,0,0,0,3},
            {7,0,9,6,1,0,2,5,0},
            {9,4,7,0,0,0,0,3,0},
            {2,1,8,0,0,9,7,6,5},
            {6,5,3,0,0,2,4,9,1}
        },
        new int[,] {
            {0,4,0,0,1,9,6,0,5},
            {8,5,0,4,7,6,0,3,0},
            {6,0,0,0,0,0,0,7,4},
            {0,0,7,6,4,8,2,0,0},
            {2,1,0,0,3,0,0,5,8},
            {0,0,4,1,2,5,3,0,0},
            {4,7,0,0,0,0,0,0,3},
            {0,2,0,7,8,4,0,1,6},
            {1,0,8,5,9,0,0,4,0}
        }
    };

    static int[][,] mediumPuzzles = new int[][,]
    {
        new int[,] {
            {3,0,6,5,0,8,4,0,0},
            {5,2,0,0,0,0,0,0,0},
            {0,8,7,0,0,0,0,3,1},
            {0,0,3,0,1,0,0,8,0},
            {9,0,0,8,6,3,0,0,5},
            {0,5,0,0,9,0,6,0,0},
            {1,3,0,0,0,0,2,5,0},
            {0,0,0,0,0,0,0,7,4},
            {0,0,5,2,0,6,3,0,0}
        },
        new int[,] {
            {7,4,0,0,0,0,0,2,0},
            {8,0,2,0,0,0,1,0,0},
            {6,0,0,3,5,0,8,0,0},
            {5,3,0,0,4,0,0,0,1},
            {0,0,0,9,3,7,0,0,0},
            {9,0,0,0,2,0,0,6,7},
            {0,0,9,0,6,1,0,0,3},
            {0,0,5,0,0,0,9,0,6},
            {0,6,0,0,0,0,0,4,2}
        },
        new int[,] {
            {0,9,0,0,6,7,5,0,8},
            {0,8,1,0,0,0,0,2,0},
            {0,0,6,2,0,0,0,4,0},
            {8,2,0,0,0,0,1,7,0},
            {0,0,5,0,2,0,9,0,0},
            {0,3,9,0,0,0,0,5,4},
            {0,4,0,0,0,6,8,0,0},
            {0,1,0,0,0,0,7,6,0},
            {6,0,3,8,7,0,0,9,0}
        }
    };

    static int[][,] hardPuzzles = new int[][,]
    {
        new int[,] {
            {0,0,0,0,7,0,0,0,0},
            {0,2,0,0,0,0,7,0,0},
            {0,0,7,6,0,0,5,0,0},
            {0,6,3,4,0,0,9,8,0},
            {9,7,0,0,6,0,0,2,5},
            {0,5,1,0,0,2,6,4,0},
            {0,0,8,0,0,7,2,0,0},
            {0,0,2,0,0,0,0,7,0},
            {0,0,0,0,8,0,0,0,0}
        },
        new int[,] {
            {4,9,0,0,0,0,0,0,0},
            {3,0,0,9,0,5,6,0,0},
            {0,0,6,0,8,0,0,0,0},
            {0,0,0,0,0,0,1,7,6},
            {1,0,0,0,2,0,0,0,3},
            {7,3,9,0,0,0,0,0,0},
            {0,0,0,0,5,0,8,0,0},
            {0,0,8,4,0,9,0,0,5},
            {0,0,0,0,0,0,0,9,1}
        },
        new int[,] {
            {0,4,0,8,1,0,0,0,0},
            {8,0,2,0,0,6,1,0,0},
            {0,9,0,0,5,0,0,0,4},
            {0,0,7,0,4,0,2,0,1},
            {0,0,0,0,3,0,0,0,0},
            {9,0,4,0,2,0,3,0,0},
            {4,0,0,0,6,0,0,8,0},
            {0,0,5,7,0,0,9,0,6},
            {0,0,0,0,9,3,0,4,0}
        }
    };

    static void Main()
    {
        Console.WriteLine("Sudoku Solver - Choose a difficulty level and puzzle:");
        Console.WriteLine("1. Easy\n2. Medium\n3. Hard");
        Console.Write("Select difficulty (1-3): ");
        string diffInput = Console.ReadLine();
        if (!int.TryParse(diffInput, out int diffChoice) || diffChoice < 1 || diffChoice > 3)
        {
            Console.WriteLine("Invalid selection. Exiting.");
            return;
        }

        int[][,] selectedList;
        string diffName;
        switch (diffChoice)
        {
            case 1: selectedList = easyPuzzles; diffName = "Easy"; break;
            case 2: selectedList = mediumPuzzles; diffName = "Medium"; break;
            case 3: selectedList = hardPuzzles; diffName = "Hard"; break;
            default: selectedList = easyPuzzles; diffName = "Easy"; break;
        }

        Console.WriteLine($"\nSelected {diffName} difficulty. Choose a puzzle:");
        for (int i = 0; i < selectedList.Length; i++)
        {
            Console.WriteLine($"Puzzle {i + 1}");
        }
        Console.Write($"Select puzzle (1-{selectedList.Length}): ");
        string puzzInput = Console.ReadLine();
        if (!int.TryParse(puzzInput, out int puzzChoice) || puzzChoice < 1 || puzzChoice > selectedList.Length)
        {
            Console.WriteLine("Invalid puzzle number. Exiting.");
            return;
        }

        int[,] board = selectedList[puzzChoice - 1];
        Console.WriteLine($"\nUnsolved {diffName} Puzzle #{puzzChoice}:");
        PrintBoard(board);

        var  solved = SolveSudokuBFS(board);
        if (solved != null)
        {
            Console.WriteLine("\nSolved Sudoku:");
            PrintBoard(solved);
        }
        else
        {
            Console.WriteLine("No solution found for this puzzle.");
        }
        Console.WriteLine("press any key to end!");
        Console.ReadKey();
    }

    static int[,] SolveSudokuBFS(int[,] startBoard)
    {
        Queue<int[,]> queue = new Queue<int[,]>();
        queue.Enqueue(CopyBoard(startBoard));

        while (queue.Count > 0)
        {
            int[,] currentBoard = queue.Dequeue();
            int emptyRow = -1, emptyCol = -1;
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (currentBoard[r, c] == 0) 
                    {
                        emptyRow = r;
                        emptyCol = c;
                        break;
                    }
                }
                if (emptyRow != -1) break;
            }

            if (emptyRow == -1)
            {
                return currentBoard;
            }

            for (int val = 1; val <= 9; val++)
            {
                if (IsSafe(currentBoard, emptyRow, emptyCol, val))
                {
                    int[,] newBoard = CopyBoard(currentBoard);
                    newBoard[emptyRow, emptyCol] = val;
                    queue.Enqueue(newBoard);
                }
            }
        }

        return null;
    }
    static int[,] CopyBoard(int[,] original)
    {
        int[,] copy = new int[9, 9];
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++)
                copy[r, c] = original[r, c];
        return copy;
    }
    static bool IsSafe(int[,] board, int row, int col, int num)
    {
        for (int x = 0; x < 9; x++)
        {
            if (board[row, x] == num || board[x, col] == num)
                return false;
        }
        int startRow = row - row % 3;
        int startCol = col - col % 3;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[startRow + i, startCol + j] == num)
                    return false;
            }
        }
        return true;
    }

    static void PrintBoard(int[,] board)
    {
        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0) Console.WriteLine("-------------------------");
            for (int j = 0; j < 9; j++)
            {
                if (j % 3 == 0) Console.Write("| ");
                Console.Write(board[i, j] == 0 ? ". " : board[i, j] + " ");
            }
            Console.WriteLine("|");
        }
        Console.WriteLine("-------------------------");
    }
}

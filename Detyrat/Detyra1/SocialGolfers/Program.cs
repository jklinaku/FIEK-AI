using System;
using System.Collections.Generic;

public class SocialGolferSolver
{
    //************** EDITABLE ******************
    const int W = 5; //the limit of depth, how many weeks do we want to generate!
    //************** EDITABLE ******************






    //************** NOT EDITABLE ******************
    const int N = 32;              
    const int P = 4;               
    const int G = N / P;
    static bool[,] pairUsed = new bool[N, N];
    static int[,] solution = new int[W, N];
    //************** NOT EDITABLE ******************





    static bool ScheduleWeek(int week)
    {
        if (week == W) return true;

        for (int i = 0; i < N; i++)
            solution[week, i] = -1;

        return AssignGroupsForWeek(week, 0);
    }

    static bool AssignGroupsForWeek(int week, int startIndex)
    {

        int anchor = -1;
        for (int i = startIndex; i < N; i++)
        {
            if (solution[week, i] == -1)  
            {
                anchor = i;
                break;
            }
        }

        if (anchor == -1)
        {
            if (ScheduleWeek(week + 1))
                return true;
            else
                return false;  // backtrack if scheduling further weeks fails
        }

        // Try to form a group with 'anchor' and P-1 other players.
        for (int j = anchor + 1; j < N; j++)
        {
            if (solution[week, j] != -1) continue;  
            for (int k = j + 1; k < N; k++)
            {
                if (solution[week, k] != -1) continue;
                for (int m = k + 1; m < N; m++)
                {
                    if (solution[week, m] != -1) continue;
                    if (pairUsed[anchor, j] || pairUsed[anchor, k] || pairUsed[anchor, m] ||
                        pairUsed[j, k] || pairUsed[j, m] || pairUsed[k, m])
                    {
                        continue;
                    }

                    int groupId = 0;
                    bool[] usedGroupIds = new bool[G];
                    for (int i = 0; i < N; i++)
                    {
                        if (solution[week, i] != -1)
                            usedGroupIds[solution[week, i]] = true;
                    }
                    while (groupId < G && usedGroupIds[groupId]) groupId++;

                    solution[week, anchor] = solution[week, j] = solution[week, k] = solution[week, m] = groupId;

                    pairUsed[anchor, j] = pairUsed[j, anchor] = true;
                    pairUsed[anchor, k] = pairUsed[k, anchor] = true;
                    pairUsed[anchor, m] = pairUsed[m, anchor] = true;
                    pairUsed[j, k] = pairUsed[k, j] = true;
                    pairUsed[j, m] = pairUsed[m, j] = true;
                    pairUsed[k, m] = pairUsed[m, k] = true;

                    if (AssignGroupsForWeek(week, anchor + 1))
                        return true;
                    // **Backtrack**: unassign this group and unmark all pairs, then try a different combination
                    solution[week, anchor] = -1;
                    solution[week, j] = -1;
                    solution[week, k] = -1;
                    solution[week, m] = -1;

                    pairUsed[anchor, j] = pairUsed[j, anchor] = false;
                    pairUsed[anchor, k] = pairUsed[k, anchor] = false;
                    pairUsed[anchor, m] = pairUsed[m, anchor] = false;
                    pairUsed[j, k] = pairUsed[k, j] = false;
                    pairUsed[j, m] = pairUsed[m, j] = false;
                    pairUsed[k, m] = pairUsed[m, k] = false;
                }
            }
        }

        return false;
    }

    public static void Main()
    {
        bool found = ScheduleWeek(0);
        if (found)
        {
            // Print the solution schedule (groups for each week)
            for (int week = 0; week < W; week++)
            {
                printWeek(week);
            }
        }
        else
        {
            Console.WriteLine($"No schedule found for {W} weeks.");
        }
        Console.WriteLine("press any key to end!");
        Console.ReadKey();
    }

    private static void printWeek(int week)
    {
        Console.Write($"Week {week + 1}: ");
        for (int grp = 0; grp < G; grp++)
        {
            Console.Write("Group " + (grp + 1) + " -> {");
            bool first = true;
            for (int player = 0; player < N; player++)
            {
                if (solution[week, player] == grp)
                {
                    Console.Write((first ? "" : ",") + player);
                    first = false;
                }
            }
            Console.Write("}  ");
        }
        Console.WriteLine();
        
    }
}

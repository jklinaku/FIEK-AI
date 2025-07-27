using System;
using System.Collections.Generic;
using System.Linq;

class SocialGolferSolver
{
    //************** EDITABLE ******************
    static int P = 4;   
    static int G = 8;   
    static int W = 5;
    //************** EDITABLE ******************


    static int N = P * G;
    static bool[,] pairUsed = new bool[N, N];
    static int[,] solution = new int[W, N];

    static bool ScheduleWeek(int week)
    {
        if (week == W) return true;

        for (int i = 0; i < N; i++)
            solution[week, i] = -1;

        return AssignGroupsForWeek(week, 0, 0);
    }

    static bool AssignGroupsForWeek(int week, int startIndex, int groupsFormed)
    {
        if (groupsFormed == G)
            return ScheduleWeek(week + 1);

        List<int> available = new();
        for (int i = 0; i < N; i++)
            if (solution[week, i] == -1)
                available.Add(i);

        if (available.Count < P) return false;

        foreach (var group in Combinations(available, P))
        {
            if (!IsValidGroup(group)) continue;

            foreach (var a in group)
                solution[week, a] = groupsFormed;

            MarkPairs(group, true);

            if (AssignGroupsForWeek(week, 0, groupsFormed + 1))
                return true;

            // Backtrack
            foreach (var a in group)
                solution[week, a] = -1;

            MarkPairs(group, false);
        }

        return false;
    }

    static bool IsValidGroup(List<int> group)
    {
        for (int i = 0; i < group.Count; i++)
            for (int j = i + 1; j < group.Count; j++)
                if (pairUsed[group[i], group[j]])
                    return false;
        return true;
    }

    static void MarkPairs(List<int> group, bool value)
    {
        for (int i = 0; i < group.Count; i++)
            for (int j = i + 1; j < group.Count; j++)
            {
                pairUsed[group[i], group[j]] = value;
                pairUsed[group[j], group[i]] = value;
            }
    }
    static IEnumerable<List<int>> Combinations(List<int> list, int k)
    {
        int n = list.Count;
        if (k > n) yield break;

        var indices = new int[k];
        for (int i = 0; i < k; i++) indices[i] = i;

        while (true)
        {
            yield return indices.Select(index => list[index]).ToList();

            int i = k - 1;
            while (i >= 0 && indices[i] == n - k + i)
                i--;

            if (i < 0)
                break;

            indices[i]++;
            for (int j = i + 1; j < k; j++)
                indices[j] = indices[j - 1] + 1;
        }
    }



    static void Main()
    {
        bool found = ScheduleWeek(0);

        if (found)
        {
            for (int week = 0; week < W; week++)
            {
                printWeek(week);
            }
        }
        else
        {
            Console.WriteLine($"No schedule found for {W} weeks with {P} players per group and {G} groups per week.");
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

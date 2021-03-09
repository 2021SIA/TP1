using System;
using System.Collections.Generic;
using TP1.Models;
using TP1.Sokoban;

namespace TP1
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = SokobanFactory.FromFile("Maps/example1.txt");
            var search = new DFS(game);
            var solution = search.GetSolution();

            List<SokobanActions> actions = new List<SokobanActions>();
            while(solution.Parent != null)
            {
                actions.Add((SokobanActions)solution.Action);
                solution = solution.Parent;
            }
            actions.Reverse();
            foreach (var action in actions)
                Console.WriteLine(action);
        }
    }
}

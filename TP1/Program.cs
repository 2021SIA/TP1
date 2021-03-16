using System;
using System.Collections.Generic;
using System.Diagnostics;
using TP1.Models;
using TP1.Sokoban;

namespace TP1
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = SokobanFactory.FromFile("Maps/8x8.txt");
            var search = new BFS(game);
            var sw = new Stopwatch();
            sw.Start();
            var solution = search.GetSolution();
            sw.Stop();

            if(solution == null)
            {
                Console.WriteLine("No solution");
                return;
            }
            List<SokobanState> states = new List<SokobanState>();
            List<SokobanActions> actions = new List<SokobanActions>();
            while(solution.Parent != null)
            {
                actions.Add((SokobanActions)solution.Action);
                states.Add((SokobanState)solution.State);
                solution = solution.Parent;
            }
            actions.Reverse();
            states.Reverse();
            Console.WriteLine($"Solved in: {sw.ElapsedMilliseconds}ms");
            foreach (var action in actions)
            {
                Console.Write(action);
                Console.Write('\t');
            }
            foreach(SokobanState stat in states)
            {
                stat.ToString();
                Console.Write('\n');
            }
        }
    }
}

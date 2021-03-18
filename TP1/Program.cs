using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TP1.Models;
using TP1.Sokoban;
using static TP1.Models.SearchTree;

namespace TP1
{
    
    class Program
    {
        private enum Algorithms
        {
            BFS, DFS, IDDFS, GGS, IDAStar, AStar
        }

        static void Main(FileInfo map, bool show, Algorithms strategy, int heuristic = 1, int depth = 30)
        {
            var game = SokobanFactory.FromFile(map.FullName);

            HeuristicFunction heuristicFunction = heuristic switch
            {
                1 => SokobanHeuristics.Heuristic1,
                2 => SokobanHeuristics.Heuristic2,
                3 => SokobanHeuristics.Heuristic3,
                _ => throw new ArgumentOutOfRangeException(nameof(heuristic))
            };

            SearchTree search = strategy switch
            {
                Algorithms.AStar => new AStar(game, heuristicFunction),
                Algorithms.BFS => new BFS(game),
                Algorithms.DFS => new DFS(game),
                Algorithms.GGS => new GGS(game, heuristicFunction),
                Algorithms.IDAStar => new IDA(game, heuristicFunction),
                Algorithms.IDDFS => new IDDFS(depth, game),
                _ => throw new ArgumentOutOfRangeException(nameof(strategy))
            };

            Console.WriteLine($"Strategy: {strategy}");
            if (strategy >= Algorithms.GGS && strategy <= Algorithms.AStar)
                Console.WriteLine($"Heuristic: {heuristic}");
            Console.WriteLine("Solving");

            var sw = new Stopwatch();
            sw.Start();
            var solution = search.GetSolution(out int expanded, out int frontier);
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
            Console.WriteLine($"Solved in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Moves: {actions.Count}");
            Console.WriteLine($"Nodes expanded: {expanded}");
            Console.WriteLine($"Nodes in frontier: {frontier}");
            Console.WriteLine("Solution:");
            foreach (var action in actions)
            {
                Console.Write(action.ToString().PadRight(8));
            }
            Console.Write('\n');

            if(show)
            {
                Console.WriteLine("Steps:");
                for(int i = 0; i < states.Count; i++)
                {
                    Console.WriteLine($"{i}.");
                    Console.Write(states[i]);
                    Console.WriteLine();
                }
            }
        }
    }
}

using System;
using TP1.Models;
using TP1.Sokoban;

namespace TP1
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = SokobanFactory.FromFile("Maps/example2.txt");
            var search = new DFS(game);
            var solution = search.GetSolution();
        }
    }
}

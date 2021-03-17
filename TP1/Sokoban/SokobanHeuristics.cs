using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TP1.Models;
using static TP1.Models.SearchTree;

namespace TP1.Sokoban
{
    public class SokobanHeuristics
    {
        private static int[,] TrimArray(int rowToRemove, int columnToRemove, int[,] originalArray)
        {
            int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

            for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
            {
                if (i == rowToRemove)
                    continue;

                for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
                {
                    if (k == columnToRemove)
                        continue;

                    result[j, u] = originalArray[i, k];
                    u++;
                }
                j++;
            }

            return result;
        }
        private static int GetMinCost(int[,] matrix)
        {
            if (matrix.Length == 2)
            {
                int sum1 = matrix[0, 0] + matrix[1, 1], sum2 = matrix[0, 1] + matrix[1, 0];
                return sum1 < sum2 ? sum1 : sum2;
            }
            int min = -1, sum;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                sum = matrix[0, i] + GetMinCost(TrimArray(0, i, matrix));
                if (min == -1 || sum < min) { min = sum; }
            }
            return min;
        }

        //Heuristica que utiliza la distancia de las cajas al objetivo mas cercano.
        public static double Heuristic1(State s)
        {
            if (!(s is SokobanState state)) { return 0; }
            double total = 0;
            double minDistance;
            foreach (Point box in state.Boxes)
            {
                minDistance = -1;
                foreach (Point objective in state.Map.Objectives)
                {
                    int distX = box.X - objective.X;
                    int distY = box.Y - objective.Y;
                    double dist = Math.Abs(distX) + Math.Abs(distY);
                    if (minDistance == -1 || dist < minDistance) { minDistance = dist; }
                }
                total += minDistance;
            }
            return total;
        }
        //Heuristica que asigna cada caja a un unico objetivo, y obtiene la suma minima de las distancias.
        public static double Heuristic2(State s)
        {
            if (!(s is SokobanState state)) { return 0; }
            int[,] distances = new int[state.Boxes.Count(), state.Map.Objectives.Count()];
            for (int i = 0; i < state.Boxes.Count(); i++)
            {
                for (int j = 0; j < state.Map.Objectives.Count(); j++)
                {
                    Point box = state.Boxes.ElementAt(i);
                    Point objective = state.Map.Objectives.ElementAt(j);
                    distances[i, j] = Math.Abs(box.X - objective.X) + Math.Abs(box.Y - objective.Y);
                }
            }
            return GetMinCost(distances);
        }
        //Heuristica que utiliza la distancia de las cajas al objetivo mas cercano, sumado a la distancia del jugador a la caja en juego mas cercana.
        public static double Heuristic3(State s)
        {
            if (!(s is SokobanState state)) { return 0; }
            double total = 0;
            double minDistance, minPlayerDistance = -1;
            Point player = state.Player;
            foreach (Point box in state.Boxes)
            {
                minDistance = -1;
                foreach (Point objective in state.Map.Objectives)
                {
                    int distX = box.X - objective.X;
                    int distY = box.Y - objective.Y;
                    int dist = Math.Abs(distX) + Math.Abs(distY);
                    if (minDistance == -1 || dist < minDistance) { minDistance = dist; }
                }
                if (minDistance > 0)
                {
                    int distX = box.X - player.X;
                    int distY = box.Y - player.Y;
                    int playerDist = Math.Abs(distX) + Math.Abs(distY);
                    if (minPlayerDistance == -1 || playerDist < minPlayerDistance)
                    {
                        minPlayerDistance = playerDist;
                    }
                }
                total += minDistance;
            }
            total += minPlayerDistance;
            return total;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static TP1.Models.SearchTree;

namespace TP1.Sokoban
{
    public class SokobanHeuristics
    {
        public static double Heuristic1(Node n)
        {
            double total = 0;
            double minDistance;
            Point player = ((SokobanState)n.State).Player;
            foreach (Point box in ((SokobanState)n.State).Boxes)
            {
                minDistance = -1;
                foreach (Point objective in ((SokobanState)n.State).Map.Objectives)
                {
                    int distX = box.X - objective.X;
                    int distY = box.Y - objective.Y;
                    double dist = Math.Abs(distX) + Math.Abs(distY);
                    //Si la caja esta en juego obtengo la distancia del jugador a la caja tambien.
                    if (dist > 0)
                    {
                        distX = box.X - player.X;
                        distY = box.Y - player.Y;
                        dist += Math.Abs(distX) + Math.Abs(distY);
                    }
                    if (minDistance == -1 || dist < minDistance) { minDistance = dist; }
                }
                total += minDistance;
            }
            return total;
        }
    }
}

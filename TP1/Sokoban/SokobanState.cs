using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TP1.Models;

namespace TP1.Sokoban
{
    public class SokobanState : State
    {
        public SokobanMap Map {get;}
        public Point Player { get; }
        public IEnumerable<Point> Boxes { get => boxes; }
        private readonly ICollection<Point> boxes;

        public SokobanState(SokobanMap map, Point playerPosition, IEnumerable<Point> boxes)
        {
            Map = map;
            Player = playerPosition;
            this.boxes = boxes.ToArray();
        }

        public bool IsValidAction(object action, out SokobanState next)
        {
            Point nextPos, nextAfterPos;
            nextPos = nextAfterPos = Player;
            switch ((SokobanActions)action)
            {
                case SokobanActions.Up:
                    nextPos.Y -=1;
                    nextAfterPos.Y -=2;
                    break;
                case SokobanActions.Down:
                    nextPos.Y +=1;
                    nextAfterPos.Y +=2;
                    break;
                case SokobanActions.Left:
                    nextPos.X -=1;
                    nextAfterPos.X -=2;
                    break;
                case SokobanActions.Right:
                    nextPos.X +=1;
                    nextAfterPos.X +=2;
                    break;
            }
            if(boxes.Contains(nextPos))
            {
                if (!(boxes.Contains(nextAfterPos) || Map.Walls.Contains(nextAfterPos)))
                {
                    next = new SokobanState(Map, nextPos, boxes.Select(box => box == nextPos ? nextAfterPos : box));
                    return true;
                }
                else
                {
                    next = null;
                    return false;
                }
            }
            else if(!Map.Walls.Contains(nextPos))
            {
                next = new SokobanState(Map, nextPos, boxes);
                return true;
            }
            else
            {
                next = null;
                return false;
            }
        }
        
        public bool IsGoal
        {
            get => boxes.All(Map.Objectives.Contains);
        }
        public bool IsDead()
        {
            
            foreach(Point box in boxes){
                // chequea esquina arriba a la derecha
                if(Map.Walls.Contains(new Point(box.X+1, box.Y)) && Map.Walls.Contains(new Point(box.X, box.Y -1))){
                    return true;
                }
                //chequea izquierda arriba
                else if(Map.Walls.Contains(new Point(box.X-1, box.Y)) && Map.Walls.Contains(new Point(box.X, box.Y -1))){
                    return true;
                }
                //chequea izquierda abajo
                else if(Map.Walls.Contains(new Point(box.X-1, box.Y)) && Map.Walls.Contains(new Point(box.X, box.Y +1))){
                    return true;
                }
                //chequea derecha abajo
                else if(Map.Walls.Contains(new Point(box.X+1, box.Y)) && Map.Walls.Contains(new Point(box.X, box.Y +1))){
                    return true;
                }
            }
            
            return false;
        }
        
        public void ToString(){
            int maxX = 0;
            int maxY = 0;
            foreach( Point wall in Map.Walls)
            {
                if(wall.X > maxX) maxX = wall.X;
                if(wall.Y >maxY) maxY = wall.Y;
            }
            string [,] state = new string[maxY+1, maxX+1];
            state[Player.Y, Player.X] = "@";
            foreach( Point wall in Map.Walls)
            {
                state[wall.Y, wall.X]= "#";
            }
            foreach( Point objective in Map.Objectives)
            {
                if(objective == Player)
                    state[objective.Y, objective.X]= "+";
                else 
                    state[objective.Y, objective.X]= ".";
            }
            foreach( Point box in Boxes)
            {
                if(state[box.Y, box.X] == ".")
                    state[box.Y, box.X]= "*";
                else 
                    state[box.Y, box.X]= "$";
            }
            int rowLength = state.GetLength(0);
            int colLength = state.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", state[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            // Console.ReadLine();
        }

        public IDictionary<object, State> PosibleActions()
        {
            var dict = new Dictionary<object, State>();
            foreach (SokobanActions action in Enum.GetValues(typeof(SokobanActions)))
                if (IsValidAction(action, out SokobanState next))
                    dict.Add(action, next);

            return dict;
        }

        public override bool Equals(object obj)
        {
            return this == obj ||
                obj is SokobanState state &&
                Map == state.Map &&
                Player == state.Player &&
                boxes.All(state.boxes.Contains);
            }

        public override int GetHashCode()
        {
            return HashCode.Combine(Player, boxes.Aggregate(17, (sum, b) => unchecked (sum * 23 + b.GetHashCode())));
        }

        public class SokobanMap
        {
            public IEnumerable<Point> Walls { get; }
            public IEnumerable<Point> Objectives { get; }

            public SokobanMap(IEnumerable<Point> walls, IEnumerable<Point> objectives)
            {
                Walls = new HashSet<Point>(walls);
                Objectives = new List<Point>(objectives);
            }

            public override bool Equals(object obj)
            {
                return this == obj ||
                    obj is SokobanMap map &&
                    Walls.All(map.Walls.Contains) &&
                    Objectives.All(map.Objectives.Contains);
            }
           
            public override int GetHashCode()
            {
                return HashCode.Combine(Walls.Aggregate(17, (sum, w) => sum + 23 * w.GetHashCode()), Objectives.Aggregate(17, (sum, o) => sum + 23 * o.GetHashCode()));
            }
        }
    }
}

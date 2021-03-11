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

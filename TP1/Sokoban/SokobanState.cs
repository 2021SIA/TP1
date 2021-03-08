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
        HashSet<Point> boxes;

        public bool IsValidAction(object action, out State next)
        {
            throw new NotImplementedException();
        }
        public bool IsGoal()
        {
            throw new NotImplementedException();
        }

        public IDictionary<object, State> PosibleActions()
        {
            Dictionary<object, State> dict = new Dictionary<object, State>();
            foreach (var action in Enum.GetValues(typeof(SokobanActions)))
                if (IsValidAction(action, out State next))
                    dict.Add(action, next);

            return dict;
        }

        public override bool Equals(object obj)
        {
            if(obj == this)
            {
                return true;
            }
            return obj is SokobanState other && 
                this.Map.Equals(other.Map) && 
                Player.Equals(other.Player) && 
                boxes.All(other.boxes.Contains);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Map, Player, boxes.Aggregate(17, (sum, b) => sum + 23 * b.GetHashCode()));
        }

        public class SokobanMap
        {
            IEnumerable<Point> Walls { get => walls; }
            IEnumerable<Point> Objectives { get => objectives; }

            HashSet<Point> walls;
            List<Point> objectives;

            public SokobanMap(IEnumerable<Point> walls, IEnumerable<Point> objectives)
            {
                this.walls = new HashSet<Point>(walls);
                this.objectives = new List<Point>(objectives);
            }

            public override bool Equals(object obj)
            {
                if (obj == this)
                {
                    return true;
                }
                return obj is SokobanMap other && walls.All(other.walls.Contains) && objectives.All(other.objectives.Contains);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(walls.Aggregate(17, (sum, w) => sum + 23 * w.GetHashCode()), objectives.Aggregate(17, (sum, o) => sum + 23 * o.GetHashCode()));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
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
        }
    }
}

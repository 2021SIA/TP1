using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace TP1.Sokoban
{
    public class SokobanFactory
    {
        public static SokobanState FromFile(string path)
        {
            List<Point> walls = new List<Point>(), boxes = new List<Point>(), goals = new List<Point>();
            Point player = Point.Empty;

            Point pos = Point.Empty;
            using var stream = new FileStream(path, FileMode.Open);
            var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                char c = (char)reader.Read();
                if (c == '\n')
                {
                    pos.X = 0;
                    pos.Y++;
                }
                switch (c)
                {
                    case '#':
                        walls.Add(pos);
                        break;
                    case '@':
                        player = pos;
                        break;
                    case '+':
                        player = pos;
                        goals.Add(pos);
                        break;
                    case '$':
                        boxes.Add(pos);
                        break;
                    case '*':
                        boxes.Add(pos);
                        goals.Add(pos);
                        break;
                    case '.':
                        goals.Add(pos);
                        break;
                    case ' ':
                    case '-':
                    case '_':
                        break;
                }
                if (c != '\r')
                    pos.X++;
            }
            var map = new SokobanState.SokobanMap(walls, goals);
            var state = new SokobanState(map, player, boxes);
            return state;
        }
    }
}

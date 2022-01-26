using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Three_Dimensional_V3
{
    internal class Bullet
    {
        public Point3 pos;
        public PointF direction;
        public bool isKill = false;

        public Bullet(Point3 _pos, PointF _direction)
        {
            pos = _pos;
            direction = _direction;
        }

        public void Move()
        {
            pos.X += Convert.ToSingle(Math.Sin(direction.X)) * 55 * Convert.ToSingle(Math.Cos(direction.Y));
            pos.Z += Convert.ToSingle(Math.Cos(direction.X)) * 55 * Convert.ToSingle(Math.Cos(direction.Y));
            pos.Y -= Convert.ToSingle(Math.Sin(direction.Y)) * 55;
        }
    }
}

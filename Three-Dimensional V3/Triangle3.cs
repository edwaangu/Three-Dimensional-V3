using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Dimensional_V3
{
    internal class Triangle3
    {
        public Point3[] points = new Point3[3];

        public Triangle3(Point3[] _points)
        {
            points = _points;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Dimensional_V3
{
    internal class Rectangle3
    {
        public Point3 pos;
        public Point3 size;

        public Rectangle3(Point3 _pos, Point3 _size)
        {
            pos = new Point3(_pos.X - _size.X, _pos.Y - _size.Y, _pos.Z - _size.Z);
            size = new Point3(_size.X * 2, _size.Y * 2, _size.Z * 2);
        }
    }
}

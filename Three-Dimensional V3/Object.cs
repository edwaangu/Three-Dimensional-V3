using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Dimensional_V3
{
    class Object
    {
        public List<Triangle3> tris = new List<Triangle3>();
        public Point3 pos, rotation;
        public int id;

        public Object(List<Triangle3> _tris, Point3 _pos, Point3 _rotation)
        {
            tris = _tris;
            pos = _pos;
            rotation = _rotation;
            id = Form1.maxid;
            Form1.maxid++;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Dimensional_V3
{
    internal class SortingTriangle3
    {
        public Triangle3 tri;
        public Object obj;
        public bool needsToDraw = true;
        public SortingTriangle3(Triangle3 _tri, Object _obj)
        {
            tri = _tri;
            obj = _obj;
        }
    }
}

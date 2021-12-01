using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Dimensional_V3
{
    internal class Camera
    {
        public float fov;
        public Point3 pos;
        public Camera(float _fov, Point3 _pos)
        {
            fov = _fov;
            pos = _pos;
        }
    }
}

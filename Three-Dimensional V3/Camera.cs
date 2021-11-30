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
        float Z0;
        public Camera(float _fov, Point3 _position)
        {
            fov = _fov;
            position = _position;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Three_Dimensional_V3
{
    internal class Camera
    {
        public float fov;
        public Point3 pos;
        public PointF direction;
        public float maximumRenderDistance;
        public Camera(float _fov, Point3 _pos, PointF _direction, float _maximumRenderDistance)
        {
            fov = _fov;
            pos = _pos;
            direction = _direction;
            maximumRenderDistance = _maximumRenderDistance;
        }
    }
}

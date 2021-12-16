using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Three_Dimensional_V3
{
    internal class Triangle3
    {
        public Point3[] points = new Point3[3];

        public Point3[] oldPoints = new Point3[3];

        public float saidZ;

        public Triangle3(Point3[] _points)
        {
            points = _points;
        }

        void TrianglePointsReset()
        {
            oldPoints = new Point3[3];
            for (int i = 0; i < oldPoints.Length; i++)
            {
                oldPoints[i] = new Point3(0 + points[i].X, 0 + points[i].Y, 0 + points[i].Z);
            }
        }

        void TranslateByObject(Object _obj)
        {
            for (int i = 0; i < oldPoints.Length; i++)
            {
                oldPoints[i].X += _obj.pos.X;
                oldPoints[i].Y += _obj.pos.Y;
                oldPoints[i].Z += _obj.pos.Z;
            }
        }

        void RotateByObject(Object _obj)
        {
            float rad2Deg = Convert.ToSingle(180 / Math.PI);
            for (int i = 0; i < oldPoints.Length; i++)
            {
                oldPoints[i].X -= _obj.pos.X;
                oldPoints[i].Y -= _obj.pos.Y;
                oldPoints[i].Z -= _obj.pos.Z;
                // Rotate by X Axis
                float theta = -_obj.rotation.X / rad2Deg;

                float oldX = 0;
                float oldZ = 0;
                oldX += oldPoints[i].X;
                oldZ += oldPoints[i].Z; // To avoid changing the value of oldpoints

                oldPoints[i].X = Convert.ToSingle((oldX * Math.Cos(theta)) + (oldZ * Math.Sin(theta)));
                oldPoints[i].Z = Convert.ToSingle((oldZ * Math.Cos(theta)) - (oldX * Math.Sin(theta)));

                // Rotate by Y Axis:
                theta = -_obj.rotation.Y / rad2Deg;

                oldZ = 0;
                float oldY = 0;
                oldY += oldPoints[i].Y;
                oldZ += oldPoints[i].Z; // To avoid changing the value of oldpoints

                oldPoints[i].Y = Convert.ToSingle((oldY * Math.Cos(theta)) - (oldZ * Math.Sin(theta)));
                oldPoints[i].Z = Convert.ToSingle((oldZ * Math.Cos(theta)) + (oldY * Math.Sin(theta)));

                // Rotate by Z Axis:
                theta = -_obj.rotation.Z / rad2Deg;

                oldX = 0;
                oldY = 0;
                oldY += oldPoints[i].Y;
                oldX += oldPoints[i].X; // To avoid changing the value of oldpoints

                oldPoints[i].Y = Convert.ToSingle((oldY * Math.Cos(theta)) - (oldX * Math.Sin(theta)));
                oldPoints[i].X = Convert.ToSingle((oldX * Math.Cos(theta)) + (oldY * Math.Sin(theta)));


                oldPoints[i].X += _obj.pos.X;
                oldPoints[i].Y += _obj.pos.Y;
                oldPoints[i].Z += _obj.pos.Z;
            }
        }

        void TranslateByCamera(Camera _cam)
        {
            for (int i = 0; i < oldPoints.Length; i++)
            {
                oldPoints[i].X -= _cam.pos.X;
                oldPoints[i].Y -= _cam.pos.Y;
                oldPoints[i].Z -= _cam.pos.Z;

                // Rotate by Y Axis
                float theta = -_cam.direction.X;

                float oldX = 0;
                float oldZ = 0;
                oldX += oldPoints[i].X;
                oldZ += oldPoints[i].Z; // To avoid changing the value of oldpoints

                oldPoints[i].X = Convert.ToSingle((oldX * Math.Cos(theta)) + (oldZ * Math.Sin(theta)));
                oldPoints[i].Z = Convert.ToSingle((oldZ * Math.Cos(theta)) - (oldX * Math.Sin(theta)));

                // Rotate by X Axis:
                theta = -_cam.direction.Y;

                oldZ = 0;
                float oldY = 0;
                oldY += oldPoints[i].Y;
                oldZ += oldPoints[i].Z; // To avoid changing the value of oldpoints

                oldPoints[i].Y = Convert.ToSingle((oldY * Math.Cos(theta)) - (oldZ * Math.Sin(theta)));
                oldPoints[i].Z = Convert.ToSingle((oldZ * Math.Cos(theta)) + (oldY * Math.Sin(theta)));
            }
        }

        public void setupLayering(Camera _cam, Object _obj, PointF _resolution)
        {
            TrianglePointsReset();
            TranslateByObject(_obj);
            RotateByObject(_obj);
            TranslateByCamera(_cam);

            float toRad = Convert.ToSingle(180 / Math.PI);
            float Z0 = Convert.ToSingle((_resolution.X / 2) / Math.Tan((_cam.fov / 2) / toRad));
            saidZ = 0;
            for (int i = 0; i < oldPoints.Length; i++)
            {

                float theZ = oldPoints[i].Z - Z0;
                float theX = oldPoints[i].X * (Z0 / (Z0 + theZ));
                float theY = oldPoints[i].Y * (Z0 / (Z0 + theZ));
                saidZ += Convert.ToSingle(theZ);
                //saidZ += Convert.ToSingle(Math.Sqrt(Math.Pow(oldPoints[i].X, 2) + Math.Pow(oldPoints[i].Y, 2) + Math.Pow(oldPoints[i].Z, 2)));
            }
            saidZ /= 3;
        }
        
        public bool ShouldBeOnScreen(Camera _cam, Object _obj, PointF _resolution)
        {
            TrianglePointsReset();
            TranslateByObject(_obj);
            RotateByObject(_obj);
            TranslateByCamera(_cam);

            float toRad = Convert.ToSingle(180 / Math.PI);
            float Z0 = Convert.ToSingle((_resolution.X / 2) / Math.Tan((_cam.fov / 2) / toRad));

            Point3 theAveragePosition = new Point3(0, 0, 0);

            saidZ = 0;
            for (int i = 0; i < points.Length; i++)
            {
                float theZ = oldPoints[i].Z - Z0;
                float theX = oldPoints[i].X * (Z0 / (Z0 + theZ));
                float theY = oldPoints[i].Y * (Z0 / (Z0 + theZ));

                if (theX > 30000)
                {
                    theX = 30000;
                }
                if (theX < -30000)
                {
                    theX = -30000;
                }
                if (theY > 30000)
                {
                    theY = 30000;
                }
                if (theY < -30000)
                {
                    theY = -30000;
                }

                theAveragePosition.X += theX;
                theAveragePosition.Y += theY;
                theAveragePosition.Z += oldPoints[i].Z;
                //Console.WriteLine($"Z of point {i} is {oldPoints[i].Z}");
                if (oldPoints[i].Z > 0)
                {
                    if (theX > -_resolution.X / 1.9 && theX < _resolution.X / 1.9 && theY > -_resolution.Y / 1.9 && theY < _resolution.Y / 1.9 && Math.Sqrt((Math.Pow(oldPoints[i].X, 2) + Math.Pow(oldPoints[i].Y, 2) + Math.Pow(oldPoints[i].Z, 2))) < _cam.maximumRenderDistance)
                    {
                        return true;
                    }
                }
            }
            //theAveragePosition.X /= 3;
            //theAveragePosition.Y /= 3;
            //theAveragePosition.Z /= 3;


            //if (theAveragePosition.Z > 0)
            //{
            //    if (theAveragePosition.X > -_resolution.X / 1.9 && theAveragePosition.X < _resolution.X / 1.9 && theAveragePosition.Y > -_resolution.Y / 1.9 && theAveragePosition.Y < _resolution.Y / 1.9)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }

        public PointF[] PointsOnScreen(Camera _cam, Object _obj, PointF _resolution)
        {
            PointF[] thePoints = new PointF[3];

            TrianglePointsReset();
            TranslateByObject(_obj);
            RotateByObject(_obj);
            TranslateByCamera(_cam);

            float toRad = Convert.ToSingle(180 / Math.PI);
            float Z0 = Convert.ToSingle((_resolution.X / 2) / Math.Tan((_cam.fov / 2) / toRad));

            saidZ = 0;
            for(int i = 0;i < thePoints.Length;i++)
            {
                float theZ = oldPoints[i].Z - Z0;
                float theX = oldPoints[i].X * (Z0 / (Z0 + theZ));
                float theY = oldPoints[i].Y * (Z0 / (Z0 + theZ));
                if(theX > 30000)
                {
                    theX = 30000;
                }
                if(theX < -30000)
                {
                    theX = -30000;
                }
                if (theY > 30000)
                {
                    theY = 30000;
                }
                if (theY < -30000)
                {
                    theY = -30000;
                }
                thePoints[i].X = theX;
                thePoints[i].Y = theY;
                saidZ += Convert.ToSingle(theZ);
            }
            saidZ /= 3;
            //Console.WriteLine($"saidZ of tri {thenum} is {saidZ}");

            return thePoints;
        }
    }
}

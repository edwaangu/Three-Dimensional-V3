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

        public Color mainColor;

        public float saidZ;
        public bool isKill = false;

        public bool isCutTriangle = false;

        public Triangle3(Point3[] _points, Color _mainColor)
        {
            points = _points;
            mainColor = _mainColor;
        }

        // Cutoff Triangle
        public void cutoffTriangle(float cutoffZ, List<SortingTriangle3> _tris, Object _obj, int debugI)
        {
            // Hide Triangle3 completely
            if (oldPoints[0].Z < cutoffZ && oldPoints[1].Z < cutoffZ && oldPoints[2].Z < cutoffZ)
            {
                isKill = true;
                //Console.WriteLine($"Triangle {debugI} at coords X:{oldPoints[0].X} Y:{oldPoints[0].Y} Z:{oldPoints[0].Z}, X:{oldPoints[1].X} Y:{oldPoints[1].Y} Z:{oldPoints[1].Z}, was deleted");
                return;
            }

            // Don't change Triangle3 at all
            if (oldPoints[0].Z >= cutoffZ && oldPoints[1].Z >= cutoffZ && oldPoints[2].Z >= cutoffZ)
            {
                isKill = false;
                //Console.WriteLine($"Triangle {debugI} at coords X:{oldPoints[0].X} Y:{oldPoints[0].Y} Z:{oldPoints[0].Z}, X:{oldPoints[1].X} Y:{oldPoints[1].Y} Z:{oldPoints[1].Z},  X:{oldPoints[2].X} Y:{oldPoints[2].Y} Z:{oldPoints[2].Z}, was fully shown");
                return;
            }

            // One point is less than cutoff, other two oldPoints are more than cutoff
            if (oldPoints[0].Z < cutoffZ && oldPoints[1].Z >= cutoffZ && oldPoints[2].Z >= cutoffZ) // If point 0 is less than cutoff
            {
                // Get slopes and new positions
                float slopeY = (oldPoints[1].Y - oldPoints[0].Y) / (oldPoints[1].Z - oldPoints[0].Z);
                float newY1 = slopeY * (cutoffZ - oldPoints[0].Z) + oldPoints[0].Y;

                float slopeY2 = (oldPoints[2].Y - oldPoints[0].Y) / (oldPoints[2].Z - oldPoints[0].Z);
                float newY2 = slopeY2 * (cutoffZ - oldPoints[0].Z) + oldPoints[0].Y;

                float slopeX = (oldPoints[1].X - oldPoints[0].X) / (oldPoints[1].Z - oldPoints[0].Z);
                float newX1 = slopeX * (cutoffZ - oldPoints[0].Z) + oldPoints[0].X;

                float slopeX2 = (oldPoints[2].X - oldPoints[0].X) / (oldPoints[2].Z - oldPoints[0].Z);
                float newX2 = slopeX2 * (cutoffZ - oldPoints[0].Z) + oldPoints[0].X;

                if (oldPoints[2].Z > oldPoints[1].Z)
                {
                    // Triangle 1
                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[2].X, oldPoints[2].Y, oldPoints[2].Z),
                        new Point3(newX2, newY2, cutoffZ),
                        new Point3(newX1, newY1, cutoffZ)
                    };

                    // Triangle 2
                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[2].X, oldPoints[2].Y, oldPoints[2].Z),
                        new Point3(oldPoints[1].X, oldPoints[1].Y, oldPoints[1].Z),
                        new Point3(newX1, newY1, cutoffZ)
                    };
                }
                else
                {
                    // Triangle 1
                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[1].X, oldPoints[1].Y, oldPoints[1].Z),
                        new Point3(newX2, newY2, cutoffZ),
                        new Point3(newX1, newY1, cutoffZ)
                    };

                    // Triangle 2
                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[2].X, oldPoints[2].Y, oldPoints[2].Z),
                        new Point3(oldPoints[1].X, oldPoints[1].Y, oldPoints[1].Z),
                        new Point3(newX2, newY2, cutoffZ)
                    };
                }
                //Console.WriteLine($"Triangle {debugI} at coords X:{oldPoints[0].X} Y:{oldPoints[0].Y} Z:{oldPoints[0].Z}, X:{oldPoints[1].X} Y:{oldPoints[1].Y} Z:{oldPoints[1].Z},  X:{oldPoints[2].X} Y:{oldPoints[2].Y} Z:{oldPoints[2].Z}, had its 0 point cut");
                isKill = true;
                return;
            }
            if (oldPoints[1].Z < cutoffZ && oldPoints[0].Z >= cutoffZ && oldPoints[2].Z >= cutoffZ) // If point 1 is less than cutoff
            {
                float slopeY = (oldPoints[0].Y - oldPoints[1].Y) / (oldPoints[0].Z - oldPoints[1].Z);
                float newY1 = slopeY * (cutoffZ - oldPoints[1].Z) + oldPoints[1].Y;

                float slopeY2 = (oldPoints[2].Y - oldPoints[1].Y) / (oldPoints[2].Z - oldPoints[1].Z);
                float newY2 = slopeY2 * (cutoffZ - oldPoints[1].Z) + oldPoints[1].Y;

                float slopeX = (oldPoints[0].X - oldPoints[1].X) / (oldPoints[0].Z - oldPoints[1].Z);
                float newX1 = slopeX * (cutoffZ - oldPoints[1].Z) + oldPoints[1].X;

                float slopeX2 = (oldPoints[2].X - oldPoints[1].X) / (oldPoints[2].Z - oldPoints[1].Z);
                float newX2 = slopeX2 * (cutoffZ - oldPoints[1].Z) + oldPoints[1].X;

                if (oldPoints[2].Z > oldPoints[0].Z)
                {
                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[2].X, oldPoints[2].Y, oldPoints[2].Z),
                        new Point3(newX2, newY2, cutoffZ),
                        new Point3(newX1, newY1, cutoffZ)
                    };
                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[2].X, oldPoints[2].Y, oldPoints[2].Z),
                        new Point3(oldPoints[0].X, oldPoints[0].Y, oldPoints[0].Z),
                        new Point3(newX1, newY1, cutoffZ)
                    };
                }
                else
                {
                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[0].X, oldPoints[0].Y, oldPoints[0].Z),
                        new Point3(newX2, newY2, cutoffZ),
                        new Point3(newX1, newY1, cutoffZ)
                    };

                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[2].X, oldPoints[2].Y, oldPoints[2].Z),
                        new Point3(oldPoints[0].X, oldPoints[0].Y, oldPoints[0].Z),
                        new Point3(newX2, newY2, cutoffZ)
                    };
                }
                //Console.WriteLine($"Triangle {debugI} at coords X:{oldPoints[0].X} Y:{oldPoints[0].Y} Z:{oldPoints[0].Z}, X:{oldPoints[1].X} Y:{oldPoints[1].Y} Z:{oldPoints[1].Z},  X:{oldPoints[2].X} Y:{oldPoints[2].Y} Z:{oldPoints[2].Z}, had its 1 point cut");
                isKill = true;
                return;
            }
            if (oldPoints[2].Z < cutoffZ && oldPoints[1].Z >= cutoffZ && oldPoints[0].Z >= cutoffZ) // If point 2 is less than cutoff
            {

                float slopeY = (oldPoints[1].Y - oldPoints[2].Y) / (oldPoints[1].Z - oldPoints[2].Z);
                float newY1 = slopeY * (cutoffZ - oldPoints[2].Z) + oldPoints[2].Y;

                float slopeY2 = (oldPoints[0].Y - oldPoints[2].Y) / (oldPoints[0].Z - oldPoints[2].Z);
                float newY2 = slopeY2 * (cutoffZ - oldPoints[2].Z) + oldPoints[2].Y;

                float slopeX = (oldPoints[1].X - oldPoints[2].X) / (oldPoints[1].Z - oldPoints[2].Z);
                float newX1 = slopeX * (cutoffZ - oldPoints[2].Z) + oldPoints[2].X;

                float slopeX2 = (oldPoints[0].X - oldPoints[2].X) / (oldPoints[0].Z - oldPoints[2].Z);
                float newX2 = slopeX2 * (cutoffZ - oldPoints[2].Z) + oldPoints[2].X;

                if (oldPoints[0].Z > oldPoints[1].Z)
                {
                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[0].X, oldPoints[0].Y, oldPoints[0].Z),
                        new Point3(newX2, newY2, cutoffZ),
                        new Point3(newX1, newY1, cutoffZ)
                    };

                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[0].X, oldPoints[0].Y, oldPoints[0].Z),
                        new Point3(oldPoints[1].X, oldPoints[1].Y, oldPoints[1].Z),
                        new Point3(newX1, newY1, cutoffZ)
                    };
                }
                else
                {

                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[1].X, oldPoints[1].Y, oldPoints[1].Z),
                        new Point3(newX2, newY2, cutoffZ),
                        new Point3(newX1, newY1, cutoffZ)
                    };

                    _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                    _tris[_tris.Count - 1].tri.isCutTriangle = true;
                    _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                    {
                        new Point3(oldPoints[0].X, oldPoints[0].Y, oldPoints[0].Z),
                        new Point3(oldPoints[1].X, oldPoints[1].Y, oldPoints[1].Z),
                        new Point3(newX2, newY2, cutoffZ)
                    };
                }
                //Console.WriteLine($"Triangle {debugI} at coords X:{oldPoints[0].X} Y:{oldPoints[0].Y} Z:{oldPoints[0].Z}, X:{oldPoints[1].X} Y:{oldPoints[1].Y} Z:{oldPoints[1].Z},  X:{oldPoints[2].X} Y:{oldPoints[2].Y} Z:{oldPoints[2].Z}, had its 2 point cut");
                isKill = true;
                return;
            }

            // Two oldPoints are less than cutoff, other point is more than cutoff
            if (oldPoints[0].Z >= cutoffZ && oldPoints[1].Z < cutoffZ && oldPoints[2].Z < cutoffZ) // If point 0 is more than cutoff
            {
                // Point 1
                float slopeY = (oldPoints[0].Y - oldPoints[1].Y) / (oldPoints[0].Z - oldPoints[1].Z);
                float newY1 = slopeY * (cutoffZ - oldPoints[1].Z) + oldPoints[1].Y;

                // Point 2
                float slopeY2 = (oldPoints[0].Y - oldPoints[2].Y) / (oldPoints[0].Z - oldPoints[2].Z);
                float newY2 = slopeY2 * (cutoffZ - oldPoints[2].Z) + oldPoints[2].Y;

                // Point 1
                float slopeX = (oldPoints[0].X - oldPoints[1].X) / (oldPoints[0].Z - oldPoints[1].Z);
                float newX1 = slopeX * (cutoffZ - oldPoints[1].Z) + oldPoints[1].X;

                // Point 2
                float slopeX2 = (oldPoints[0].X - oldPoints[2].X) / (oldPoints[0].Z - oldPoints[2].Z);
                float newX2 = slopeX2 * (cutoffZ - oldPoints[2].Z) + oldPoints[2].X;

                _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                _tris[_tris.Count - 1].tri.isCutTriangle = true;
                _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                {
                    new Point3(oldPoints[0].X, oldPoints[0].Y, oldPoints[0].Z),
                    new Point3(newX1, newY1, cutoffZ),
                    new Point3(newX2, newY2, cutoffZ)
                };
                isKill = true;
                //Console.WriteLine($"Triangle {debugI} at coords X:{oldPoints[0].X} Y:{oldPoints[0].Y} Z:{oldPoints[0].Z}, X:{oldPoints[1].X} Y:{oldPoints[1].Y} Z:{oldPoints[1].Z},  X:{oldPoints[2].X} Y:{oldPoints[2].Y} Z:{oldPoints[2].Z}, had its 1 and 2 points cut");
                return;
            }
            if (oldPoints[1].Z >= cutoffZ && oldPoints[0].Z < cutoffZ && oldPoints[2].Z < cutoffZ) // If point 1 is more than cutoff
            {

                // Point 0
                float slopeY = (oldPoints[1].Y - oldPoints[0].Y) / (oldPoints[1].Z - oldPoints[0].Z);
                float newY1 = slopeY * (cutoffZ - oldPoints[0].Z) + oldPoints[0].Y;

                // Point 2
                float slopeY2 = (oldPoints[1].Y - oldPoints[2].Y) / (oldPoints[1].Z - oldPoints[2].Z);
                float newY2 = slopeY2 * (cutoffZ - oldPoints[2].Z) + oldPoints[2].Y;

                // Point 0
                float slopeX = (oldPoints[1].X - oldPoints[0].X) / (oldPoints[1].Z - oldPoints[0].Z);
                float newX1 = slopeX * (cutoffZ - oldPoints[0].Z) + oldPoints[0].X;

                // Point 2
                float slopeX2 = (oldPoints[1].X - oldPoints[2].X) / (oldPoints[1].Z - oldPoints[2].Z);
                float newX2 = slopeX2 * (cutoffZ - oldPoints[2].Z) + oldPoints[2].X;

                _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                _tris[_tris.Count - 1].tri.isCutTriangle = true;
                _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                {
                    new Point3(newX1, newY1, cutoffZ),
                    new Point3(oldPoints[1].X, oldPoints[1].Y, oldPoints[1].Z),
                    new Point3(newX2, newY2, cutoffZ)
                };
                isKill = true;
                //Console.WriteLine($"Triangle {debugI} at coords X:{oldPoints[0].X} Y:{oldPoints[0].Y} Z:{oldPoints[0].Z}, X:{oldPoints[1].X} Y:{oldPoints[1].Y} Z:{oldPoints[1].Z},  X:{oldPoints[2].X} Y:{oldPoints[2].Y} Z:{oldPoints[2].Z}, had its 0 and 2 points cut");
                return;
            }
            if (oldPoints[2].Z >= cutoffZ && oldPoints[1].Z < cutoffZ && oldPoints[0].Z < cutoffZ) // If point 2 is more than cutoff
            {
                // Point 0
                float slopeY2 = (oldPoints[2].Y - oldPoints[0].Y) / (oldPoints[2].Z - oldPoints[0].Z);
                float newY2 = slopeY2 * (cutoffZ - oldPoints[0].Z) + oldPoints[0].Y;

                // Point 1
                float slopeY = (oldPoints[2].Y - oldPoints[1].Y) / (oldPoints[2].Z - oldPoints[1].Z);
                float newY1 = slopeY * (cutoffZ - oldPoints[1].Z) + oldPoints[1].Y;

                // Point 0
                float slopeX2 = (oldPoints[2].X - oldPoints[0].X) / (oldPoints[2].Z - oldPoints[0].Z);
                float newX2 = slopeX2 * (cutoffZ - oldPoints[0].Z) + oldPoints[0].X;

                // Point 1
                float slopeX = (oldPoints[2].X - oldPoints[1].X) / (oldPoints[2].Z - oldPoints[1].Z);
                float newX1 = slopeX * (cutoffZ - oldPoints[1].Z) + oldPoints[1].X;

                _tris.Add(new SortingTriangle3(new Triangle3(new Point3[]{
                        new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f),new Point3(0f, 0f, 0f)
                    }, mainColor), _obj));
                _tris[_tris.Count - 1].tri.isCutTriangle = true;
                _tris[_tris.Count - 1].tri.oldPoints = new Point3[3]
                {
                    new Point3(newX2, newY2, cutoffZ),
                    new Point3(newX1, newY1, cutoffZ),
                    new Point3(oldPoints[2].X, oldPoints[2].Y, oldPoints[2].Z)
                };
                //Console.WriteLine($"Triangle {debugI} at coords X:{oldPoints[0].X} Y:{oldPoints[0].Y} Z:{oldPoints[0].Z}, X:{oldPoints[1].X} Y:{oldPoints[1].Y} Z:{oldPoints[1].Z},  X:{oldPoints[2].X} Y:{oldPoints[2].Y} Z:{oldPoints[2].Z}, had its 0 and 1 points cut");
                isKill = true;
                return;
            }
        }

        float GetDistanceBetweenPoints(Point3 _p1, Point3 _p2)
        {
            return Convert.ToSingle(Math.Sqrt(Math.Pow(_p2.X - _p1.X, 2) + Math.Pow(_p2.Y - _p1.Y, 2) + Math.Pow(_p2.Z - _p1.Z, 2)));
        }

        public void TriangleMaxDist(int maxdist, List<Triangle3> _tris)
        {

            float dist0 = GetDistanceBetweenPoints(points[1], points[2]);
            float dist1 = GetDistanceBetweenPoints(points[0], points[2]);
            float dist2 = GetDistanceBetweenPoints(points[1], points[0]);
            if (dist0 <= maxdist && dist1 <= maxdist && dist2 <= maxdist)
            {
                isKill = false;
            }
            else
            {
                if (dist0 >= dist1 && dist0 >= dist2)
                {
                    if (dist0 > maxdist)
                    {
                        float avgX = (points[1].X + points[2].X) / 2;
                        float avgY = (points[1].Y + points[2].Y) / 2;
                        float avgZ = (points[1].Z + points[2].Z) / 2;

                        _tris.Add(new Triangle3(new Point3[]
                        {
                        new Point3(avgX, avgY, avgZ),
                        new Point3(points[0].X, points[0].Y, points[0].Z),
                        new Point3(points[1].X, points[1].Y, points[1].Z)
                        }, mainColor));
                        _tris[_tris.Count - 1].TriangleMaxDist(maxdist, _tris);
                        _tris.Add(new Triangle3(new Point3[]
                        {
                        new Point3(avgX, avgY, avgZ),
                        new Point3(points[0].X, points[0].Y, points[0].Z),
                        new Point3(points[2].X, points[2].Y, points[2].Z)
                        }, mainColor));
                        _tris[_tris.Count - 1].TriangleMaxDist(maxdist, _tris);
                    }
                }
                else if (dist1 >= dist0 && dist1 >= dist2)
                {
                    if (dist1 > maxdist)
                    {
                        float avgX = (points[1].X + points[2].X) / 2;
                        float avgY = (points[1].Y + points[2].Y) / 2;
                        float avgZ = (points[1].Z + points[2].Z) / 2;

                        _tris.Add(new Triangle3(new Point3[]
                        {
                        new Point3(avgX, avgY, avgZ),
                        new Point3(points[0].X, points[0].Y, points[0].Z),
                        new Point3(points[1].X, points[1].Y, points[1].Z)
                        }, mainColor));
                        _tris[_tris.Count - 1].TriangleMaxDist(maxdist, _tris);
                        _tris.Add(new Triangle3(new Point3[]
                        {
                        new Point3(avgX, avgY, avgZ),
                        new Point3(points[0].X, points[0].Y, points[0].Z),
                        new Point3(points[2].X, points[2].Y, points[2].Z)
                        }, mainColor));
                        _tris[_tris.Count - 1].TriangleMaxDist(maxdist, _tris);
                    }
                }
                else
                {
                    if (dist2 > maxdist)
                    {
                        float avgX = (points[1].X + points[0].X) / 2;
                        float avgY = (points[1].Y + points[0].Y) / 2;
                        float avgZ = (points[1].Z + points[0].Z) / 2;

                        _tris.Add(new Triangle3(new Point3[]
                        {
                        new Point3(avgX, avgY, avgZ),
                        new Point3(points[2].X, points[2].Y, points[2].Z),
                        new Point3(points[1].X, points[1].Y, points[1].Z)
                        }, mainColor));
                        _tris[_tris.Count - 1].TriangleMaxDist(maxdist, _tris);
                        _tris.Add(new Triangle3(new Point3[]
                        {
                        new Point3(avgX, avgY, avgZ),
                        new Point3(points[2].X, points[2].Y, points[2].Z),
                        new Point3(points[0].X, points[0].Y, points[0].Z)
                        }, mainColor));
                        _tris[_tris.Count - 1].TriangleMaxDist(maxdist, _tris);
                    }
                }
                isKill = true;
            }
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

        public void setupLayering(Camera _cam, Object _obj, PointF _resolution, List<SortingTriangle3> _tris, int debugI)
        {
            if (isCutTriangle == false)
            {
                TrianglePointsReset();
                TranslateByObject(_obj);
                RotateByObject(_obj);
                TranslateByCamera(_cam);
            }

            float toRad = Convert.ToSingle(180 / Math.PI);
            float Z0 = Convert.ToSingle((_resolution.X / 2) / Math.Tan((_cam.fov / 2) / toRad));
            PointF[] thePoints = new PointF[3];
            if (isCutTriangle == false)
            {
                cutoffTriangle(2, _tris, _obj, debugI);
            }
            saidZ = 0;
            for (int i = 0; i < oldPoints.Length; i++)
            {

                float theZ = oldPoints[i].Z - Z0;
                float theX = oldPoints[i].X * (Z0 / (Z0 + theZ));
                float theY = oldPoints[i].Y * (Z0 / (Z0 + theZ));
                thePoints[i] = new PointF(theX, theY);
                //saidZ += Convert.ToSingle(theZ);
                saidZ += Convert.ToSingle(Math.Sqrt(Math.Pow(oldPoints[i].X, 2) + Math.Pow(oldPoints[i].Y, 2) + Math.Pow(oldPoints[i].Z, 2)));
                
            }
            // Are all points behind the camera
            if (oldPoints[0].Z < 0 && oldPoints[1].Z < 0 && oldPoints[2].Z < 0)
            {
                isKill = true;
            }

            // Are all points a greater distance away from the camera than the maximum render distance
            if (Math.Sqrt((Math.Pow(oldPoints[0].X, 2) + Math.Pow(oldPoints[0].Y, 2) + Math.Pow(oldPoints[0].Z, 2))) >= _cam.maximumRenderDistance && Math.Sqrt((Math.Pow(oldPoints[1].X, 2) + Math.Pow(oldPoints[1].Y, 2) + Math.Pow(oldPoints[1].Z, 2))) >= _cam.maximumRenderDistance && Math.Sqrt((Math.Pow(oldPoints[2].X, 2) + Math.Pow(oldPoints[2].Y, 2) + Math.Pow(oldPoints[2].Z, 2))) >= _cam.maximumRenderDistance)
            {
                isKill = true;
            }

            // Are all points off left of the screen
            if (thePoints[0].X < -_resolution.X / 1.9 && thePoints[1].X < -_resolution.X / 1.9 && thePoints[2].X < -_resolution.X / 1.9)
            {
                isKill = true;
            }

            // Are all points off right of the screen
            if (thePoints[0].X > _resolution.X / 1.9 && thePoints[1].X > _resolution.X / 1.9 && thePoints[2].X > _resolution.X / 1.9)
            {
                isKill = true;
            }

            // Are all points off up of the screen
            if (thePoints[0].Y < -_resolution.Y / 1.9 && thePoints[1].Y < -_resolution.Y / 1.9 && thePoints[2].Y < -_resolution.Y / 1.9)
            {
                isKill = true;
            }

            // Are all points off down of the screen
            if (thePoints[0].Y > _resolution.Y / 1.9 && thePoints[1].Y > _resolution.Y / 1.9 && thePoints[2].Y > _resolution.Y / 1.9)
            {
                isKill = true;
            }
            saidZ /= 3;
        }
        
        public bool ShouldBeOnScreen(Camera _cam, Object _obj, PointF _resolution)
        {
            if (isCutTriangle == false)
            {
                TrianglePointsReset();
                TranslateByObject(_obj);
                RotateByObject(_obj);
                TranslateByCamera(_cam);
            }

            float toRad = Convert.ToSingle(180 / Math.PI);
            float Z0 = Convert.ToSingle((_resolution.X / 2) / Math.Tan((_cam.fov / 2) / toRad));

            Point3 theAveragePosition = new Point3(0, 0, 0);
            PointF[] thePoints = new PointF[3];

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

                thePoints[i] = new PointF(theX, theY);

                theAveragePosition.X += theX;
                theAveragePosition.Y += theY;
                theAveragePosition.Z += oldPoints[i].Z;
                ////Console.WriteLine($"Z of point {i} is {oldPoints[i].Z}");
                /*if (oldPoints[i].Z > 0)
                {
                    if (theX > -_resolution.X / 1.9 && theX < _resolution.X / 1.9 && theY > -_resolution.Y / 1.9 && theY < _resolution.Y / 1.9)
                    {
                        return true;
                    }
                }*/
            }

            // Are all points behind the camera
            /*
            if (oldPoints[0].Z < 0 && oldPoints[1].Z < 0 && oldPoints[2].Z < 0) {
                return false;
            }

            // Are all points a greater distance away from the camera than the maximum render distance
            if (Math.Sqrt((Math.Pow(oldPoints[0].X, 2) + Math.Pow(oldPoints[0].Y, 2) + Math.Pow(oldPoints[0].Z, 2))) >= _cam.maximumRenderDistance && Math.Sqrt((Math.Pow(oldPoints[1].X, 2) + Math.Pow(oldPoints[1].Y, 2) + Math.Pow(oldPoints[1].Z, 2))) >= _cam.maximumRenderDistance && Math.Sqrt((Math.Pow(oldPoints[2].X, 2) + Math.Pow(oldPoints[2].Y, 2) + Math.Pow(oldPoints[2].Z, 2))) >= _cam.maximumRenderDistance)
            {
                return false;
            }

            // Are all points off left of the screen
            if(thePoints[0].X < -_resolution.X / 1.9 && thePoints[1].X < -_resolution.X / 1.9 && thePoints[2].X < -_resolution.X / 1.9)
            {
                return false;
            }

            // Are all points off right of the screen
            if (thePoints[0].X > _resolution.X / 1.9 && thePoints[1].X > _resolution.X / 1.9 && thePoints[2].X > _resolution.X / 1.9)
            {
                return false;
            }

            // Are all points off up of the screen
            if (thePoints[0].Y < -_resolution.Y / 1.9 && thePoints[1].Y < -_resolution.Y / 1.9 && thePoints[2].Y < -_resolution.Y / 1.9)
            {
                return false;
            }

            // Are all points off down of the screen
            if (thePoints[0].Y > _resolution.Y / 1.9 && thePoints[1].Y > _resolution.Y / 1.9 && thePoints[2].Y > _resolution.Y / 1.9)
            {
                return false;
            }*/


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

            return true;
        }

        public PointF[] PointsOnScreen(Camera _cam, Object _obj, PointF _resolution)
        {
            PointF[] thePoints = new PointF[3];

            if (isCutTriangle == false)
            {
                TrianglePointsReset();
                TranslateByObject(_obj);
                RotateByObject(_obj);
                TranslateByCamera(_cam);
            }

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
                saidZ += Convert.ToSingle(Math.Sqrt(Math.Pow(oldPoints[i].X, 2) + Math.Pow(oldPoints[i].Y, 2) + Math.Pow(oldPoints[i].Z, 2)));
            }
            saidZ /= 3;
            ////Console.WriteLine($"saidZ of tri {thenum} is {saidZ}");

            return thePoints;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Three_Dimensional_V3
{
    internal class Collision
    {
        string type; // "plane, cube, slope"
        Point3 pos, size;
        public Collision(Point3 _pos, Point3 _size, string _type)
        {
            pos = _pos;
            size = _size;
            type = _type;
        }

        public bool colBetween3DRecs(Rectangle3 r1, Rectangle3 r2)
        {

            //RectangleF r1XZRec = new RectangleF(r1Pos.X - r1Size.X, r1Pos.Z - r1Size.Z, r1Size.X * 2, r1Size.Z * 2);
            //RectangleF r2XZRec = new RectangleF(r2Pos.X - r2Size.X, r2Pos.Z - r2Size.Z, r2Size.X * 2, r2Size.Z * 2);

            //RectangleF r1XYRec = new RectangleF(r1Pos.X - r1Size.X, r1Pos.Y - r1Size.Y, r1Size.X * 2, r1Size.Y * 2);
            //RectangleF r2XYRec = new RectangleF(r2Pos.X - r2Size.X, r2Pos.Y - r2Size.Y, r2Size.X * 2, r2Size.Y * 2);
            RectangleF r1XZRec = new RectangleF(r1.pos.X, r1.pos.Z, r1.size.X, r1.size.Z);
            RectangleF r2XZRec = new RectangleF(r2.pos.X, r2.pos.Z, r2.size.X, r2.size.Z);
            RectangleF r1XYRec = new RectangleF(r1.pos.X, r1.pos.Y, r1.size.X, r1.size.Y);
            RectangleF r2XYRec = new RectangleF(r2.pos.X, r2.pos.Y, r2.size.X, r2.size.Y);


            return r1XZRec.IntersectsWith(r2XZRec) && r1XYRec.IntersectsWith(r2XYRec);
        }

        public void checkCollision(Player _p)
        {
            if(type == "plane")
            {
                RectangleF pRec = new RectangleF(_p.pos.X - _p.size.X, _p.pos.Z - _p.size.Z, _p.size.X * 2, _p.size.Z * 2);
                RectangleF colRec = new RectangleF(pos.X - size.X, pos.Z - size.Z, size.X*2, size.Z*2);
                if (_p.pos.Y + _p.size.Y > pos.Y && pRec.IntersectsWith(colRec))
                {
                    _p.pos.Y = pos.Y - _p.size.Y;
                    _p.velocity.Y = 0;
                    _p.hasJumped = false;
                }
            }
            else if(type == "cube")
            {
                // Every single player rect for each side
                Rectangle3 pRecPX = new Rectangle3(new Point3(_p.pos.X + _p.size.X - _p.size.X / 8, _p.pos.Y, _p.pos.Z), new Point3(_p.size.X / 8, _p.size.Y, _p.size.Z));
                Rectangle3 pRecNX = new Rectangle3(new Point3(_p.pos.X - _p.size.X + _p.size.X / 8, _p.pos.Y, _p.pos.Z), new Point3(_p.size.X / 8, _p.size.Y, _p.size.Z));
                Rectangle3 pRecPY = new Rectangle3(new Point3(_p.pos.X, _p.pos.Y + _p.size.Y - _p.size.Y / 8, _p.pos.Z), new Point3(_p.size.X, _p.size.Y / 8, _p.size.Z));
                Rectangle3 pRecNY = new Rectangle3(new Point3(_p.pos.X, _p.pos.Y - _p.size.Y + _p.size.Y / 8, _p.pos.Z), new Point3(_p.size.X, _p.size.Y / 8, _p.size.Z));
                Rectangle3 pRecPZ = new Rectangle3(new Point3(_p.pos.X, _p.pos.Y, _p.pos.Z + _p.size.Z - _p.size.Z / 8), new Point3(_p.size.X, _p.size.Y, _p.size.Z / 8));
                Rectangle3 pRecNZ = new Rectangle3(new Point3(_p.pos.X, _p.pos.Y, _p.pos.Z - _p.size.Z + _p.size.Z / 8), new Point3(_p.size.X, _p.size.Y, _p.size.Z / 8));

                // Every single cube rect for each side
                Rectangle3 colRecPX = new Rectangle3(new Point3(pos.X + size.X - size.X / 8, pos.Y, pos.Z), new Point3(size.X / 8, size.Y, size.Z));
                Rectangle3 colRecNX = new Rectangle3(new Point3(pos.X - size.X + size.X / 8, pos.Y, pos.Z), new Point3(size.X / 8, size.Y, size.Z));
                Rectangle3 colRecPY = new Rectangle3(new Point3(pos.X, pos.Y + size.Y - size.Y / 8, pos.Z), new Point3(size.X, size.Y / 8, size.Z));
                Rectangle3 colRecNY = new Rectangle3(new Point3(pos.X, pos.Y - size.Y + size.Y / 8, pos.Z), new Point3(size.X, size.Y / 8, size.Z));
                Rectangle3 colRecPZ = new Rectangle3(new Point3(pos.X, pos.Y, pos.Z + size.Z - size.Z / 8), new Point3(size.X, size.Y, size.Z / 8));
                Rectangle3 colRecNZ = new Rectangle3(new Point3(pos.X, pos.Y, pos.Z - size.Z + size.Z / 8), new Point3(size.X, size.Y, size.Z / 8));


                // Bottom of player hits top of cube
                /*
                if (colBetween3DRecs(new Point3(_p.pos.X, _p.pos.Y + _p.size.Y - _p.size.Y / 8, _p.pos.Z), new Point3(_p.size.X, _p.size.Y / 8, _p.size.Z), new Point3(pos.X, pos.Y - size.Y / 2, pos.Z), new Point3(size.X, size.Y / 2, size.Z)))
                {
                    _p.pos.Y = pos.Y - size.Y - _p.size.Y;
                    _p.velocity.Y = 0;
                    _p.hasJumped = false;
                }
                // Top of player hits bottom of cube
                else if (colBetween3DRecs(new Point3(_p.pos.X, _p.pos.Y - _p.size.Y + _p.size.Y / 8, _p.pos.Z), new Point3(_p.size.X, _p.size.Y / 8, _p.size.Z), new Point3(pos.X, pos.Y + size.Y - size.Y / 4, pos.Z), new Point3(size.X, size.Y / 4, size.Z)))
                {
                    _p.pos.Y = pos.Y + size.Y + _p.size.Y;
                    _p.velocity.Y = 0;
                }

                // Negative X side of player hits Positive X side of cube
                if (colBetween3DRecs(new Point3(_p.pos.X - _p.size.X + _p.size.X / 8, _p.pos.Y, _p.pos.Z), new Point3(_p.size.X / 8, _p.size.Y, _p.size.Z), new Point3(pos.X + size.X / 2, pos.Y, pos.Z), new Point3(size.X / 2, size.Y, size.Z)))
                {
                    _p.pos.X = pos.X + size.X + _p.size.X;
                }

                // Positive X side of player hits Negative X side of cube
                if (colBetween3DRecs(new Point3(_p.pos.X + _p.size.X - _p.size.X / 8, _p.pos.Y, _p.pos.Z), new Point3(_p.size.X / 8, _p.size.Y, _p.size.Z), new Point3(pos.X - size.X / 2, pos.Y, pos.Z), new Point3(size.X / 2, size.Y, size.Z)))
                {
                    _p.pos.X = pos.X - size.X - _p.size.X;
                }


                // Negative Z side of player hits Positive Z side of cube
                if (colBetween3DRecs(new Point3(_p.pos.X, _p.pos.Y, _p.pos.Z - _p.size.Z + _p.size.Z / 8), new Point3(_p.size.X, _p.size.Y, _p.size.Z / 8), new Point3(pos.X, pos.Y, pos.Z + size.Z / 2), new Point3(size.X, size.Y, size.Z / 2)))
                {
                    _p.pos.Z = pos.Z + size.Z + _p.size.Z;
                }

                // Positive Z side of player hits negative Z side of cube
                if (colBetween3DRecs(new Point3(_p.pos.X, _p.pos.Y, _p.pos.Z + _p.size.Z - _p.size.Z / 8), new Point3(_p.size.X, _p.size.Y, _p.size.Z / 8), new Point3(pos.X, pos.Y, pos.Z - size.Z / 2), new Point3(size.X, size.Y, size.Z / 2)))
                {
                    _p.pos.Z = pos.Z - size.Z - _p.size.Z;
                }*/
            }
        }

    }
}

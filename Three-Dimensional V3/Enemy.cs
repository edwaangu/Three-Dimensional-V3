using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Three_Dimensional_V3
{
    internal class Enemy
    {
        public Point3 pos;
        Point3 velocity = new Point3(0, 0, 0);
        public Point3 size = new Point3(80, 80, 80);
        public string type; //shooter, crawler
        public bool hasJumped = false;


        public Enemy(Point3 _pos, string _type)
        {
            pos = _pos;
            type = _type;
        }

        public bool colBetween3DRecs(Rectangle3 r1, Rectangle3 r2)
        {
            RectangleF r1XZRec = new RectangleF(r1.pos.X, r1.pos.Z, r1.size.X, r1.size.Z);
            RectangleF r2XZRec = new RectangleF(r2.pos.X, r2.pos.Z, r2.size.X, r2.size.Z);
            RectangleF r1XYRec = new RectangleF(r1.pos.X, r1.pos.Y, r1.size.X, r1.size.Y);
            RectangleF r2XYRec = new RectangleF(r2.pos.X, r2.pos.Y, r2.size.X, r2.size.Y);


            return r1XZRec.IntersectsWith(r2XZRec) && r1XYRec.IntersectsWith(r2XYRec);
        }

        public void Move(List<Collision> _cols, Player _p)
        {
            float dirXZ = Convert.ToSingle(Math.Atan2(_p.pos.Z - pos.Z, _p.pos.X - pos.X));
            float distXZ = Convert.ToSingle(Math.Sqrt(Math.Pow(_p.pos.X - pos.X, 2) + Math.Pow(_p.pos.Z - pos.Z, 2)));
            float dirY = Convert.ToSingle(Math.Atan2(pos.Y - pos.Y, distXZ));

            pos.X += velocity.X;
            pos.Y += velocity.Y;
            pos.Z += velocity.Z;

            velocity.X += Convert.ToSingle(Math.Cos(dirXZ));
            velocity.Y += 30f / 60f;
            velocity.Z += Convert.ToSingle(Math.Sin(dirXZ));

            velocity.X *= 0.8f;
            velocity.Z *= 0.8f;

            /** COLLISIONS **/
            // Every single enemy rect for each side
            Rectangle3 pRecPX = new Rectangle3(new Point3(pos.X + size.X - size.X / 8, pos.Y, pos.Z), new Point3(size.X / 8, size.Y, size.Z));
            Rectangle3 pRecNX = new Rectangle3(new Point3(pos.X - size.X + size.X / 8, pos.Y, pos.Z), new Point3(size.X / 8, size.Y, size.Z));
            Rectangle3 pRecPY = new Rectangle3(new Point3(pos.X, pos.Y + size.Y - size.Y / 8, pos.Z), new Point3(size.X, size.Y / 8, size.Z));
            Rectangle3 pRecNY = new Rectangle3(new Point3(pos.X, pos.Y - size.Y + size.Y / 8, pos.Z), new Point3(size.X, size.Y / 8, size.Z));
            Rectangle3 pRecPZ = new Rectangle3(new Point3(pos.X, pos.Y, pos.Z + size.Z - size.Z / 8), new Point3(size.X, size.Y, size.Z / 8));
            Rectangle3 pRecNZ = new Rectangle3(new Point3(pos.X, pos.Y, pos.Z - size.Z + size.Z / 8), new Point3(size.X, size.Y, size.Z / 8));

            foreach (Collision _c in _cols)
            {

                if (_c.type == "plane")
                {
                    RectangleF pRec = new RectangleF(pos.X - size.X, pos.Z - size.Z, size.X * 2, size.Z * 2);
                    RectangleF colRec = new RectangleF(_c.pos.X - _c.size.X, _c.pos.Z - _c.size.Z, _c.size.X * 2, _c.size.Z * 2);
                    if (pos.Y + size.Y > _c.pos.Y && pRec.IntersectsWith(colRec))
                    {
                        pos.Y = _c.pos.Y - size.Y;
                        velocity.Y = 0;
                        hasJumped = false;
                    }
                }
                else if (_c.type == "cube")
                {
                    // Every single cube rect for each side
                    Rectangle3 colRecPX = new Rectangle3(new Point3(_c.pos.X + _c.size.X - _c.size.X / 8, _c.pos.Y, _c.pos.Z), new Point3(_c.size.X / 8, _c.size.Y, _c.size.Z));
                    Rectangle3 colRecNX = new Rectangle3(new Point3(_c.pos.X - _c.size.X + _c.size.X / 8, _c.pos.Y, _c.pos.Z), new Point3(_c.size.X / 8, _c.size.Y, _c.size.Z));
                    Rectangle3 colRecPY = new Rectangle3(new Point3(_c.pos.X, _c.pos.Y + _c.size.Y - _c.size.Y / 8, _c.pos.Z), new Point3(_c.size.X, _c.size.Y / 8, _c.size.Z));
                    Rectangle3 colRecNY = new Rectangle3(new Point3(_c.pos.X, _c.pos.Y - _c.size.Y + _c.size.Y / 8, _c.pos.Z), new Point3(_c.size.X, _c.size.Y / 8, _c.size.Z));
                    Rectangle3 colRecPZ = new Rectangle3(new Point3(_c.pos.X, _c.pos.Y, _c.pos.Z + _c.size.Z - _c.size.Z / 8), new Point3(_c.size.X, _c.size.Y, _c.size.Z / 8));
                    Rectangle3 colRecNZ = new Rectangle3(new Point3(_c.pos.X, _c.pos.Y, _c.pos.Z - _c.size.Z + _c.size.Z / 8), new Point3(_c.size.X, _c.size.Y, _c.size.Z / 8));

                    // Bottom of player hits top of cube
                    if (colBetween3DRecs(pRecPY, colRecNY))
                    {
                        pos.Y = _c.pos.Y - _c.size.Y - size.Y;
                        velocity.Y = 0;
                        hasJumped = false;
                    }

                    // Top of player hits bottom of cube
                    else if (colBetween3DRecs(pRecNY, colRecPY))
                    {
                        pos.Y = _c.pos.Y + _c.size.Y + size.Y;
                        velocity.Y = 0;
                    }

                    else
                    {
                        // Neg X of player hits Pos X of cube
                        if (colBetween3DRecs(pRecNX, colRecPX))
                        {
                            pos.X = _c.pos.X + _c.size.X + size.X;
                        }

                        // Pos X of player hits Neg X of cube
                        if (colBetween3DRecs(pRecPX, colRecNX))
                        {
                            pos.X = _c.pos.X - _c.size.X - size.X;
                        }


                        // Neg Z of player hits Pos Z of cube
                        if (colBetween3DRecs(pRecNZ, colRecPZ))
                        {
                            pos.Z = _c.pos.Z + _c.size.Z + size.Z;
                        }

                        // Pos Z of player hits Neg Z of cube
                        if (colBetween3DRecs(pRecPZ, colRecNZ))
                        {
                            pos.Z = _c.pos.Z - _c.size.Z - size.Z;
                        }
                    }
                }
            }
        }
    }
}

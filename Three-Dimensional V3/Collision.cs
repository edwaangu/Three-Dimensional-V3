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
        public string type; // "plane, cube, slope"
        public Point3 pos, size;
        public bool isMovePlatform;
        public int movePlatformType; // 0 PZ, 1 NZ, 2 PX, 3 NX
        public Collision(Point3 _pos, Point3 _size, string _type, bool _isMovePlatform = false, int _movePlatformType = 0)
        {
            pos = _pos;
            size = _size;
            type = _type;
            isMovePlatform = _isMovePlatform;
            movePlatformType = _movePlatformType;
        }

        public bool colBetween3DRecs(Rectangle3 r1, Rectangle3 r2)
        {
            RectangleF r1XZRec = new RectangleF(r1.pos.X, r1.pos.Z, r1.size.X, r1.size.Z);
            RectangleF r2XZRec = new RectangleF(r2.pos.X, r2.pos.Z, r2.size.X, r2.size.Z);
            RectangleF r1XYRec = new RectangleF(r1.pos.X, r1.pos.Y, r1.size.X, r1.size.Y);
            RectangleF r2XYRec = new RectangleF(r2.pos.X, r2.pos.Y, r2.size.X, r2.size.Y);


            return r1XZRec.IntersectsWith(r2XZRec) && r1XYRec.IntersectsWith(r2XYRec);
        }

        public void checkCollision(Player _p, int movePlatformMode = -1)
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
                if (colBetween3DRecs(pRecPY, colRecNY))
                {
                    _p.pos.Y = pos.Y - size.Y - _p.size.Y;
                    _p.velocity.Y = 0;
                    _p.hasJumped = false;
                    if (isMovePlatform)
                    {
                        if (movePlatformType == 0)
                        {
                            if (movePlatformMode == 0)
                            {
                                _p.pos.Z += 10;
                            }
                            if (movePlatformMode == 2)
                            {
                                _p.pos.Z -= 10;
                            }
                        }
                        if (movePlatformType == 1)
                        {
                            if (movePlatformMode == 0)
                            {
                                _p.pos.Z -= 10;
                            }
                            if (movePlatformMode == 2)
                            {
                                _p.pos.Z += 10;
                            }
                        }
                        if (movePlatformType == 2)
                        {
                            if (movePlatformMode == 0)
                            {
                                _p.pos.X += 10;
                            }
                            if (movePlatformMode == 2)
                            {
                                _p.pos.X -= 10;
                            }
                        }
                        if (movePlatformType == 3)
                        {
                            if (movePlatformMode == 0)
                            {
                                _p.pos.X -= 10;
                            }
                            if (movePlatformMode == 2)
                            {
                                _p.pos.X += 10;
                            }
                        }
                    }
                }

                // Top of player hits bottom of cube
                else if (colBetween3DRecs(pRecNY, colRecPY))
                {
                    _p.pos.Y = pos.Y + size.Y + _p.size.Y;
                    _p.velocity.Y = 0;
                }

                else
                {
                    // Neg X of player hits Pos X of cube
                    if (colBetween3DRecs(pRecNX, colRecPX))
                    {
                        _p.pos.X = pos.X + size.X + _p.size.X;
                    }

                    // Pos X of player hits Neg X of cube
                    if (colBetween3DRecs(pRecPX, colRecNX))
                    {
                        _p.pos.X = pos.X - size.X - _p.size.X;
                    }


                    // Neg Z of player hits Pos Z of cube
                    if (colBetween3DRecs(pRecNZ, colRecPZ))
                    {
                        _p.pos.Z = pos.Z + size.Z + _p.size.Z;
                    }

                    // Pos Z of player hits Neg Z of cube
                    if (colBetween3DRecs(pRecPZ, colRecNZ))
                    {
                        _p.pos.Z = pos.Z - size.Z - _p.size.Z;
                    }
                }
            }
        }

    }
}

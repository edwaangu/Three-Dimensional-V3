using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Dimensional_V3
{
    internal class Player
    {
        public Point3 pos;
        public Point3 velocity;
        public Point3 size;
        public bool hasJumped = false;

        public Player(Point3 _pos)
        {
            size = new Point3(50, 100, 50);
            velocity = new Point3(0f, -25f, 0f);
            pos = _pos;
        }

        public void addVelocities()
        {
            pos.X += velocity.X;
            pos.Y += velocity.Y;
            pos.Z += velocity.Z;

            velocity.Y += 30f / 60f;


            if(hasJumped == false)
            {

                velocity.X *= 0.8f;
                velocity.Z *= 0.8f;
            }
            else
            {

                velocity.X *= 0.95f;
                velocity.Z *= 0.95f;
            }
        }
    }
}

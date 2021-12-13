using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Three_Dimensional_V3
{
    public partial class MainScreen : UserControl
    {
        /**
         * -- Three-Dimensional V3 -- 
         * Made by Ted Angus
         * Created: 2021/11/29 - 2021/11/30
         * 
         * -- TO DO --
         * ~ Proper Z-Layering
         * ~ Object rotations
         * ~ Camera movement
         * ~ Camera rotation
         * ~ Completely in function
         * ~ Usability in other programs
         * 
         * -- VERSION HISTORY --
         * 
         * Version v3.1:
         * - I don't know at this point
         * - Rotation XZ works, rotation Y does not work
         * 
         * Version v3.0.0:
         * ~ Very basic WORKING 3d stuff for once!
         * ~ Basic layering
         * 
        */


        /** MAIN VARIABLES **/
        PointF res; // Resolution

        /** CAMERA RELATED VARIABLES **/
        Camera camera = new Camera(70, new Point3(0, 0, 0), new PointF(Convert.ToSingle(180 / (180 / Math.PI)), 0)); // Camera
        bool[] keys = new bool[256];


        /** SHAPE RELATED VARIABLES **/
        List<Object> objs = new List<Object>(){ 
            new Object(new List<Triangle3>() {
                // Front
                new Triangle3( new Point3[] {
                    new Point3(-50, -50, -50),
                    new Point3(-50, 50, -50),
                    new Point3(50, -50, -50),
                }),
                new Triangle3( new Point3[] {
                    new Point3(50, 50, -50),
                    new Point3(-50, 50, -50),
                    new Point3(50, -50, -50),
                }),
                // Right Side
                
                new Triangle3( new Point3[] {
                    new Point3(50, -50, -50),
                    new Point3(50, 50, -50),
                    new Point3(50, -50, 50),
                }),
                new Triangle3( new Point3[] {
                    new Point3(50, 50, 50),
                    new Point3(50, 50, -50),
                    new Point3(50, -50, 50),
                }),

                // Left Side
                new Triangle3( new Point3[] {
                    new Point3(-50, -50, -50),
                    new Point3(-50, 50, -50),
                    new Point3(-50, -50, 50),
                }),
                new Triangle3( new Point3[] {
                    new Point3(-50, 50, 50),
                    new Point3(-50, 50, -50),
                    new Point3(-50, -50, 50),
                }),

                // Back Side
                new Triangle3( new Point3[] {
                    new Point3(-50, -50, 50),
                    new Point3(-50, 50, 50),
                    new Point3(50, -50, 50),
                }),
                new Triangle3( new Point3[] {
                    new Point3(50, 50, 50),
                    new Point3(-50, 50, 50),
                    new Point3(50, -50, 50),
                }),

                // Top Side
                new Triangle3( new Point3[] {
                    new Point3(-50, -50, -50),
                    new Point3(-50, -50, 50),
                    new Point3(50, -50, -50),
                }),
                new Triangle3( new Point3[] {
                    new Point3(50, -50, 50),
                    new Point3(-50, -50, 50),
                    new Point3(50, -50, -50),
                }),
                
                // Bottom Side
                new Triangle3( new Point3[] {
                    new Point3(-50, 50, -50),
                    new Point3(-50, 50, 50),
                    new Point3(50, 50, -50),
                }),
                new Triangle3( new Point3[] {
                    new Point3(50, 50, 50),
                    new Point3(-50, 50, 50),
                    new Point3(50, 50, -50),
                }),
            }, new Point3(0, 0, 600), new Point3(0, 0, 0))
        };

        /** INIT METHOD **/
        public MainScreen()
        {
            InitializeComponent();
            res = new PointF(this.Width, this.Height); // 800, 450

            foreach (Object obj in objs)
            {
                foreach (Triangle3 tri in obj.tris)
                {
                    tri.PointsOnScreen(camera, obj, res, 0);
                }
            }

            for(int i = 0;i < keys.Length;i++)
            {
                keys[i] = false;
            }
        }
        /** UPDATE METHOD **/
        private void frameUpdate_Tick(object sender, EventArgs e)
        {
            if (keys[87])
            {
                camera.pos.X -= Convert.ToSingle(Math.Sin(camera.direction.X) * 5);
                camera.pos.Z -= Convert.ToSingle(Math.Cos(camera.direction.X) * 5);
            }
            if (keys[65])
            {
                camera.pos.X -= Convert.ToSingle(Math.Sin(camera.direction.X - (90 / (180 / Math.PI))) * 5);
                camera.pos.Z -= Convert.ToSingle(Math.Cos(camera.direction.X - (90 / (180 / Math.PI))) * 5);
            }
            if (keys[68])
            {
                camera.pos.X -= Convert.ToSingle(Math.Sin(camera.direction.X + (90 / (180 / Math.PI))) * 5);
                camera.pos.Z -= Convert.ToSingle(Math.Cos(camera.direction.X + (90 / (180 / Math.PI))) * 5);
            }
            if (keys[83])
            {
                camera.pos.X += Convert.ToSingle(Math.Sin(camera.direction.X) * 5);
                camera.pos.Z += Convert.ToSingle(Math.Cos(camera.direction.X) * 5);
            }
            if (keys[16])
            {
                camera.pos.Y -= 5;
            }
            if (keys[32])
            {
                camera.pos.Y += 5;
            }
            if (keys[37])
            {
                camera.direction.X += Convert.ToSingle(2 / (180 / Math.PI));
            }
            if (keys[39])
            {
                camera.direction.X -= Convert.ToSingle(2 / (180 / Math.PI));
            }
            if (keys[38])
            {
                camera.direction.Y += Convert.ToSingle(2 / (180 / Math.PI));
            }
            if (keys[40])
            {
                camera.direction.Y -= Convert.ToSingle(2 / (180 / Math.PI));
            }
            this.Refresh();
        }

        /** PAINT METHOD **/
        private void MainScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(res.X / 2, res.Y / 2);
            int i = 0;

            foreach(Object obj in objs)
            {
                obj.tris = obj.tris.OrderByDescending(x => x.saidZ).ToList();
                foreach (Triangle3 tri in obj.tris)
                {
                    i += 12;
                    if (tri.ShouldBeOnScreen(camera, obj, res))
                    {
                        e.Graphics.DrawPolygon(new Pen(Color.FromArgb(i, i, i), 3), tri.PointsOnScreen(camera, obj, res, i / 12));
                    }
                    //e.Graphics.DrawPolygon(new Pen(Color.Black, 2), tri.PointsOnScreen(camera, obj, res));
                }
            }
            e.Graphics.ResetTransform();
        }

        /** KEYBOARD **/
        private void MainScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            keys[e.KeyValue] = true;
        }

        private void MainScreen_KeyUp(object sender, KeyEventArgs e)
        {
            keys[e.KeyValue] = false;
        }
    }
}

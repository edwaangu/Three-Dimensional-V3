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
         * Version v3.2:
         * - Hell yeah I did the Y rotations
         * - All rotations work now
         * - Stable for once
         * - Layering issues may still exist though unfortunately
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
        Random randGen = new Random();

        /** CAMERA RELATED VARIABLES **/
        Camera camera = new Camera(70, new Point3(0, 0, 0), new PointF(0, 0), 2000); // Camera
        bool[] keys = new bool[256];

        /** FPS RELATED VARIABLES **/
        int framesSinceLastSecond = 0;
        int accurateSec = -1;
        int lastSec = -1;
        int fps = 0;



        /** SHAPE RELATED VARIABLES **/
        List<Object> objs = new List<Object>(){ 
            /*new Object(new List<Triangle3>() {
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
            }, new Point3(0, 0, 600), new Point3(0, 0, 0))*/
        };

        /** SPHERE MAKER **/
        void Sphere(Point3 location, float radius, float rows, float columns)
        {
            List<Triangle3> tempTris = new List<Triangle3>();
            float rad2Deg = Convert.ToSingle(180 / Math.PI);

            for(float i = -90;i < 90;i +=180 / rows)
            {
                for(float j = 0;j < 360;j += 360 / (columns))
                {
                    float columnAdder = (360 / columns) / rad2Deg;
                    float rowAdder = (180 / rows) / rad2Deg;
                    float dirHorizontal = j / rad2Deg;
                    float dirVertical = i / rad2Deg;
                    Triangle3 tempTri = new Triangle3(new Point3[] { 
                        new Point3(
                            Convert.ToSingle(Math.Cos(dirHorizontal) * radius * Math.Cos(dirVertical)), 
                            Convert.ToSingle(Math.Sin(dirVertical) * radius), 
                            Convert.ToSingle(Math.Sin(dirHorizontal) * radius * Math.Cos(dirVertical))),
                        new Point3(
                            Convert.ToSingle(Math.Cos(dirHorizontal) * radius * Math.Cos(dirVertical + rowAdder)), 
                            Convert.ToSingle(Math.Sin(dirVertical + rowAdder) * radius), 
                            Convert.ToSingle(Math.Sin(dirHorizontal) * radius * Math.Cos(dirVertical + rowAdder))),
                        new Point3(
                            Convert.ToSingle(Math.Cos(dirHorizontal + columnAdder) * radius * Math.Cos(dirVertical + rowAdder)),
                            Convert.ToSingle(Math.Sin(dirVertical + rowAdder) * radius), 
                            Convert.ToSingle(Math.Sin(dirHorizontal + columnAdder) * radius * Math.Cos(dirVertical + rowAdder)))
                    });
                    Triangle3 tempTri2 = new Triangle3(new Point3[] {
                        new Point3(
                            Convert.ToSingle(Math.Cos(dirHorizontal) * radius * Math.Cos(dirVertical)),
                            Convert.ToSingle(Math.Sin(dirVertical) * radius),
                            Convert.ToSingle(Math.Sin(dirHorizontal) * radius * Math.Cos(dirVertical))),
                        new Point3(
                            Convert.ToSingle(Math.Cos(dirHorizontal + columnAdder) * radius * Math.Cos(dirVertical)),
                            Convert.ToSingle(Math.Sin(dirVertical) * radius),
                            Convert.ToSingle(Math.Sin(dirHorizontal + columnAdder) * radius * Math.Cos(dirVertical))),
                        new Point3(
                            Convert.ToSingle(Math.Cos(dirHorizontal + columnAdder) * radius * Math.Cos(dirVertical + rowAdder)),
                            Convert.ToSingle(Math.Sin(dirVertical + rowAdder) * radius),
                            Convert.ToSingle(Math.Sin(dirHorizontal + columnAdder) * radius * Math.Cos(dirVertical + rowAdder)))
                    });

                    tempTris.Add(tempTri);
                    tempTris.Add(tempTri2);
                }
            }

            objs.Add(new Object(tempTris, location, new Point3(0, 0, 0)));
        }
         
        

        /** INIT METHOD **/
        public MainScreen()
        {
            InitializeComponent();

            Sphere(new Point3(0, 0, 600), 300, 50, 50);
            /*for(int i = 0;i < 200;i++)
            {
                objs.Add(new Object(new List<Triangle3>() {
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
            }, new Point3(randGen.Next(-1500, 1500), randGen.Next(-500, 500), randGen.Next(-1500, 1500)), new Point3(0, 0, 0)));
            }*/
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
            framesSinceLastSecond++;
            accurateSec = DateTime.Now.Second;
            if (lastSec != accurateSec)
            {
                lastSec = accurateSec;
                fps = framesSinceLastSecond;
                framesSinceLastSecond = 0;
            }

            if (keys[87])
            {
                camera.pos.X += Convert.ToSingle(Math.Sin(camera.direction.X) * 5);
                camera.pos.Z += Convert.ToSingle(Math.Cos(camera.direction.X) * 5);
            }
            if (keys[65])
            {
                camera.pos.X += Convert.ToSingle(Math.Sin(camera.direction.X - (90 / (180 / Math.PI))) * 5);
                camera.pos.Z += Convert.ToSingle(Math.Cos(camera.direction.X - (90 / (180 / Math.PI))) * 5);
            }
            if (keys[68])
            {
                camera.pos.X += Convert.ToSingle(Math.Sin(camera.direction.X + (90 / (180 / Math.PI))) * 5);
                camera.pos.Z += Convert.ToSingle(Math.Cos(camera.direction.X + (90 / (180 / Math.PI))) * 5);
            }
            if (keys[83])
            {
                camera.pos.X -= Convert.ToSingle(Math.Sin(camera.direction.X) * 5);
                camera.pos.Z -= Convert.ToSingle(Math.Cos(camera.direction.X) * 5);
            }
            if (keys[16])
            {
                camera.pos.Y += 5;
            }
            if (keys[32])
            {
                camera.pos.Y -= 5;
            }
            if (keys[37])
            {
                camera.direction.X += Convert.ToSingle(1 / (180 / Math.PI));
            }
            if (keys[39])
            {
                camera.direction.X -= Convert.ToSingle(1 / (180 / Math.PI));
            }
            if (keys[38])
            {
                camera.direction.Y += Convert.ToSingle(1 / (180 / Math.PI));
            }
            if (keys[40])
            {
                camera.direction.Y -= Convert.ToSingle(1 / (180 / Math.PI));
            }
            this.Refresh();
        }

        /** PAINT METHOD **/
        private void MainScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(res.X / 2, res.Y / 2);
            float i = 0;

            List<SortingTriangle3> trisToSort = new List<SortingTriangle3>();
            foreach(Object obj in objs)
            {
                foreach(Triangle3 tri in obj.tris)
                {
                    trisToSort.Add(new SortingTriangle3(tri, obj));
                }
            }

            trisToSort = trisToSort.OrderByDescending(x => x.tri.saidZ).ToList();

            foreach (SortingTriangle3 tri in trisToSort)
            {
                //sorttri.obj.tris = sorttri.obj.tris.OrderByDescending(x => x.saidZ).ToList();

                if (tri.tri.ShouldBeOnScreen(camera, tri.obj, res))
                {
                    i += 0.05f;
                    e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(Convert.ToInt16(i / 2), Convert.ToInt16(i / 20), Convert.ToInt16(i))), tri.tri.PointsOnScreen(camera, tri.obj, res, i / 12));
                }
            }

            /*
            foreach(Object obj in objs)
            {
                obj.tris = obj.tris.OrderByDescending(x => x.saidZ).ToList();
                i = 0;
                foreach (Triangle3 tri in obj.tris)
                {
                    i += 12;
                    if (tri.ShouldBeOnScreen(camera, obj, res))
                    {
                        e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(i, i, i)), tri.PointsOnScreen(camera, obj, res, i / 12));
                    }
                    //e.Graphics.DrawPolygon(new Pen(Color.Black, 2), tri.PointsOnScreen(camera, obj, res));
                }
            }
            */
            e.Graphics.ResetTransform();

            e.Graphics.DrawString($"FPS: {fps}", DefaultFont, new SolidBrush(Color.Black), new PointF(10, 10));
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

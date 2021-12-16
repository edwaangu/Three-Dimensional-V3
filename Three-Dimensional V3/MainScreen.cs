using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Three_Dimensional_V3
{
    public partial class MainScreen : UserControl
    {
        /**
         * -- Three-Dimensional V3 -- 
         * Made by Ted Angus
         * Created: 2021/11/29 - 2021/12/15
         * 
         * -- TO DO --
         * ~ Hide shapes that aren't visible to the camera (Because other shapes may be covering it)
         * ~ Proper Z-Layering 
         * ~ [PRIORITY] Object rotations
         * ~ Completely in function
         * ~ Convert this program to a reference
         * ~ [PRIORITY] Add more options for shapes (Cones, Pyramids, Planes, 2D Shapes in 3D, etc.)
         * ~ [PRIORITY] Turn the cube into a function so it's adjustable
         * 
         * -- BUGS --
         * ~ [PRIORITY] Triangle disappears with all points off screen even though a line still crosses the screen
         * ~ Triangle covers entire screen when right behind camera
         * ~ Horrible issues at higher FOVS (Probably will never fix)
         * 
         * -- VERSION HISTORY --
         * Version v3.5:
         * - Cones
         * - Added size to cylinders
         * - Plane function
         * - Thoughts of fixing issues
         * 
         * Version v3.4:
         * - Designated Cube Function
         * - Cylinders!
         * 
         * Version v3.3:
         * - Spheres
         * - Fixed "Triangles may flash when appearing for one frame before being sorted with the rest"
         * 
         * Version v3.2:
         * - Y axis Rotations completed
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
        Camera camera = new Camera(70, new Point3(0, 0, 0), new PointF(0, 0), 20000); // Camera
        bool[] keys = new bool[256];

        /** FPS RELATED VARIABLES **/
        int framesSinceLastSecond = 0;
        int accurateSec = -1;
        int lastSec = -1;
        int fps = 0;



        /** SHAPE RELATED VARIABLES **/
        List<Object> objs = new List<Object>();

        /** SHAPE MAKERS **/
        void newSphere(Point3 location, float radius, float rows, float columns)
        {
            // Create a list of triangles to update
            List<Triangle3> tempTris = new List<Triangle3>();

            // Get the conversion from radians to degrees
            float rad2Deg = Convert.ToSingle(180 / Math.PI);

            // A for loop that starts from the bottom to the top of the sphere, increasing by 180 over the amount of rows
            for(float i = -90;i < 90;i +=180 / rows)
            {
                // A for loop that starts and loops around back to the start, increasing by 360 over the amount of columns
                for(float j = 0;j < 360;j += 360 / (columns))
                {
                    // Get the column adder and row adder into variables for simplicity
                    float columnAdder = (360 / columns) / rad2Deg;
                    float rowAdder = (180 / rows) / rad2Deg;

                    // Find the current degree in the horizontal and vertical directions
                    float dirHorizontal = j / rad2Deg;
                    float dirVertical = i / rad2Deg;

                    // Create two new triangles (Assuming 0 is min and 1 is max (up and right is positive): One is (0, 0) (0, 1) (1, 0), and the second is (1, 1) (0, 1) (1, 0)
                    Triangle3 tempTri = new Triangle3(new Point3[] { 
                        new Point3(
                            Convert.ToSingle(Math.Cos(dirHorizontal) * radius * Math.Cos(dirVertical)), // Multiply the horizontal Sine by radius and the Cosine of the vertical
                            Convert.ToSingle(Math.Sin(dirVertical) * radius), // Multiply the Vertical by the radius only
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

            // Create the object
            objs.Add(new Object(tempTris, location, new Point3(0, 0, 0)));
        }

        void newCube(Point3 location, Point3 size, Point3 rotation)
        {
            // Create 12 triangles, 2 for each side
            objs.Add(new Object(new List<Triangle3>() {
                // Front
                new Triangle3( new Point3[] {
                    new Point3(-size.X, -size.Y, -size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }),
                new Triangle3( new Point3[] {
                    new Point3(size.X, size.Y, -size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }),
                // Right Side
                
                new Triangle3( new Point3[] {
                    new Point3(size.X, -size.Y, -size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }),
                new Triangle3( new Point3[] {
                    new Point3(size.X, size.Y, size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }),

                // Left Side
                new Triangle3( new Point3[] {
                    new Point3(-size.X, -size.Y, -size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                }),
                new Triangle3( new Point3[] {
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                }),

                // Back Side
                new Triangle3( new Point3[] {
                    new Point3(-size.X, -size.Y, size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }),
                new Triangle3( new Point3[] {
                    new Point3(size.X, size.Y, size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }),

                // Top Side
                new Triangle3( new Point3[] {
                    new Point3(-size.X, -size.Y, -size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }),
                new Triangle3( new Point3[] {
                    new Point3(size.X, -size.Y, size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }),
                
                // Bottom Side
                new Triangle3( new Point3[] {
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                }),
                new Triangle3( new Point3[] {
                    new Point3(size.X, size.Y, size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                }),
            }, location, rotation));
        }

        void newCylinder(Point3 location, Point3 size, float columns, Point3 rotation)
        {
            // Create a list of temp triangles
            List<Triangle3> tempTris = new List<Triangle3>();

            // Conversion
            float rad2Deg = Convert.ToSingle(180 / Math.PI);

            // A for loop for everycolumn
            for (float i = 0;i < 360;i +=360 / columns)
            {
                float columnAdder = 360 / columns;

                // Rectangles
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * size.X), -size.Y, Convert.ToSingle(Math.Sin(i / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * size.X), -size.Y, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * size.X), size.Y, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * size.Z)),
                }));
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * size.X), size.Y, Convert.ToSingle(Math.Sin(i / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * size.X), -size.Y, Convert.ToSingle(Math.Sin(i / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * size.X), size.Y, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * size.Z)),
                }));

                // Top circle
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * size.X), size.Y, Convert.ToSingle(Math.Sin(i / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * size.X), size.Y, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * size.Z)),
                    new Point3(0, size.Y, 0),
                }));

                // Bottom circle
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * size.X), -size.Y, Convert.ToSingle(Math.Sin(i / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * size.X), -size.Y, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * size.Z)),
                    new Point3(0, -size.Y, 0),
                }));
            }

            objs.Add(new Object(tempTris, location, rotation));
        }

        void newCone(Point3 location, float radius, float height, float columns, Point3 rotation)
        {
            // Create a list of temp triangles
            List<Triangle3> tempTris = new List<Triangle3>();

            // Conversion
            float rad2Deg = Convert.ToSingle(180 / Math.PI);

            // A for loop for everycolumn
            for (float i = 0; i < 360; i += 360 / columns)
            {
                float columnAdder = 360 / columns;

                // Triangles
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * radius), height, Convert.ToSingle(Math.Sin(i / rad2Deg) * radius)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * radius), height, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * radius)),
                    new Point3(0, -height, 0),
                }));

                // Bottom circle
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * radius), height, Convert.ToSingle(Math.Sin(i / rad2Deg) * radius)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * radius), height, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * radius)),
                    new Point3(0, height, 0),
                }));
            }

            objs.Add(new Object(tempTris, location, rotation));
        }

        void newPlane(Point3 location, PointF size, Point3 rotation)
        {
            objs.Add(new Object(new List<Triangle3>()
            {
                new Triangle3(new Point3[]{
                    new Point3(-size.X, 0, -size.Y) ,
                    new Point3(-size.X, 0, size.Y) ,
                    new Point3(size.X, 0, -size.Y) 
                }),
                new Triangle3(new Point3[]{
                    new Point3(size.X, 0, size.Y) ,
                    new Point3(-size.X, 0, size.Y) ,
                    new Point3(size.X, 0, -size.Y)
                }),
            }, location, rotation));
        }
        /** INIT METHOD **/
        public MainScreen()
        {
            // Initialize
            InitializeComponent();

            // Add objects
            newCylinder(new Point3(150, 0, 1000), new Point3(150, 100, 50), 36, new Point3(0, 0, 0));
            newSphere(new Point3(450, 0, 1000), 100, 8, 16);
            newCube(new Point3(-450, 0, 1000), new Point3(100, 100, 100), new Point3(0, 0, 0));
            newCone(new Point3(-150, 0, 1000), 50, 100, 12, new Point3(0, 0, 0));
            newPlane(new Point3(0, 100, 1000), new PointF(700,150), new Point3(0, 0, 0));

            // Set resolution
            res = new PointF(this.Width, this.Height); // 800, 450

            foreach (Object obj in objs)
            {
                foreach (Triangle3 tri in obj.tris)
                {
                    tri.PointsOnScreen(camera, obj, res);
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

            foreach(Object obj in objs)
            {
                //obj.rotation.X++;
                //obj.rotation.Y++;
                //obj.rotation.Z++;
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


            // Add the triangles to an array to be sorted
            List<SortingTriangle3> trisToSort = new List<SortingTriangle3>();
            List<float> objInts = new List<float>();
            foreach(Object obj in objs)
            {
                objInts.Add(0); 
                foreach (Triangle3 tri in obj.tris)
                {
                    trisToSort.Add(new SortingTriangle3(tri, obj));
                }
            }

            // Sort the triangles
            foreach (SortingTriangle3 tri in trisToSort)
            {
                tri.tri.setupLayering(camera, tri.obj, res);
            }
            trisToSort = trisToSort.OrderByDescending(x => x.tri.saidZ).ToList();

            // Draw the triangles
            foreach (SortingTriangle3 tri in trisToSort)
            {
                if (tri.needsToDraw)
                {
                    if (tri.tri.ShouldBeOnScreen(camera, tri.obj, res))
                    {
                        // Color.FromArgb(Convert.ToInt16(i / 2), Convert.ToInt16(i / 20), Convert.ToInt16(i)))
                        e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(Convert.ToInt16(objInts[tri.obj.id] / 20), Convert.ToInt16(objInts[tri.obj.id] / 100), Convert.ToInt16(objInts[tri.obj.id]/10))), tri.tri.PointsOnScreen(camera, tri.obj, res));
                        objInts[tri.obj.id] += 2555 / tri.obj.tris.Count;
                        //e.Graphics.DrawPolygon(new Pen(Color.Black, 2), tri.tri.PointsOnScreen(camera, tri.obj, res, i / 12));
                    }
                }
            }
            e.Graphics.ResetTransform();

            // Get FPS
            e.Graphics.DrawString($"FPS: {fps}", DefaultFont, new SolidBrush(Color.Black), new PointF(10, 10));
        }

        /** KEYBOARD **/
        private void MainScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Update value of key array to true
            keys[e.KeyValue] = true;
        }

        private void MainScreen_KeyUp(object sender, KeyEventArgs e)
        {
            // Update value of key array to false
            keys[e.KeyValue] = false;
        }
    }
}

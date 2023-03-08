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
         * Created: 2021/11/29 - 2022/1/13
         * 
         * -- TO DO --
         * ~ Proper Z-Layering 
         * ~ [PRIORITY] Object rotations
         * ~ Completely in function
         * ~ Convert this program to a reference
         * 
         * -- BUGS --
         * ~ [PRIORITY] Triangle disappears with all points off screen even though a line still crosses the screen
         * ~ Horrible issues at higher FOVS (Probably will never fix)
         * 
         * -- VERSION HISTORY --
         * 
         * Version v3.6: (Long awaited!)
         * - Better Z-Layering (By Splitting triangles)
         * - You can go through shapes now, looks pretty cool
         * - Changed the color engine to revert back to closeness by all layers
         * - Added the Z Buffer into it's own function
         * 
         * - Bug Fixed: Triangle covers entire screen when right behind camera
         * 
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
        Camera camera = new Camera(70, new Point3(0, 0, 0), new PointF(0, 0), 10000); // Camera
        bool[] keys = new bool[256];

        /** FPS RELATED VARIABLES **/
        int framesSinceLastSecond = 0;
        int accurateSec = -1;
        int lastSec = -1;
        int fps = 0;

        // Millisecond differences
        int lastMillis = 0;
        int lastSecond = 0;

        int getMillisSinceLast()
        {
            int theTime;
            if (DateTime.Now.Second < lastSecond)
            {
                if (DateTime.Now.Millisecond < lastMillis)
                {
                    theTime = 60000 + ((DateTime.Now.Second - lastSecond) * 1000) + 1000 + (DateTime.Now.Millisecond - lastMillis);
                }
                else
                {
                    theTime = 60000 + ((DateTime.Now.Second - lastSecond) * 1000) + (DateTime.Now.Millisecond - lastMillis);
                }
            }
            else
            {
                if (DateTime.Now.Millisecond < lastMillis)
                {
                    theTime = ((DateTime.Now.Second - lastSecond) * 1000) + 1000 + (DateTime.Now.Millisecond - lastMillis);
                }
                else
                {
                    theTime = ((DateTime.Now.Second - lastSecond) * 1000) + (DateTime.Now.Millisecond - lastMillis);
                }
            }
            return theTime;
        }

        int objCreateTime;
        int zBufferTime;
        int drawTime;

        /** SHAPE RELATED VARIABLES **/
        List<Object> objs = new List<Object>();
        List<SortingTriangle3> trisToSort = new List<SortingTriangle3>();
        List<float> objInts = new List<float>();

        int test = 0;

        /** SHAPE MAKERS **/
        void newSphere(Point3 location, float radius, float rows, float columns, Color _c)
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
                    }, _c);
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
                    }, _c);

                    tempTris.Add(tempTri);
                    tempTris.Add(tempTri2);
                }
            }

            // Create the object
            objs.Add(new Object(tempTris, location, new Point3(0, 0, 0)));
        }

        void newCube(Point3 location, Point3 size, Point3 rotation, Color _c)
        {
            // Create 12 triangles, 2 for each side
            objs.Add(new Object(new List<Triangle3>() {
                // Front
                new Triangle3( new Point3[] {
                    new Point3(-size.X, -size.Y, -size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }, _c),
                new Triangle3( new Point3[] {
                    new Point3(size.X, size.Y, -size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }, _c),
                // Right Side
                
                new Triangle3( new Point3[] {
                    new Point3(size.X, -size.Y, -size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }, _c),
                new Triangle3( new Point3[] {
                    new Point3(size.X, size.Y, size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }, _c),

                // Left Side
                new Triangle3( new Point3[] {
                    new Point3(-size.X, -size.Y, -size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                }, _c),
                new Triangle3( new Point3[] {
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                }, _c),

                // Back Side
                new Triangle3( new Point3[] {
                    new Point3(-size.X, -size.Y, size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }, _c),
                new Triangle3( new Point3[] {
                    new Point3(size.X, size.Y, size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }, _c),

                // Top Side
                new Triangle3( new Point3[] {
                    new Point3(-size.X, -size.Y, -size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }, _c),
                new Triangle3( new Point3[] {
                    new Point3(size.X, -size.Y, size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }, _c),
                
                // Bottom Side
                new Triangle3( new Point3[] {
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                }, _c),
                new Triangle3( new Point3[] {
                    new Point3(size.X, size.Y, size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                }, _c),
            }, location, rotation));
        }

        void newCylinder(Point3 location, Point3 size, float columns, Point3 rotation, Color _c)
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
                }, _c));
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * size.X), size.Y, Convert.ToSingle(Math.Sin(i / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * size.X), -size.Y, Convert.ToSingle(Math.Sin(i / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * size.X), size.Y, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * size.Z)),
                }, _c));

                // Top circle
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * size.X), size.Y, Convert.ToSingle(Math.Sin(i / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * size.X), size.Y, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * size.Z)),
                    new Point3(0, size.Y, 0),
                }, _c));

                // Bottom circle
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * size.X), -size.Y, Convert.ToSingle(Math.Sin(i / rad2Deg) * size.Z)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * size.X), -size.Y, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * size.Z)),
                    new Point3(0, -size.Y, 0),
                }, _c));
            }

            objs.Add(new Object(tempTris, location, rotation));
        }

        void newCone(Point3 location, float radius, float height, float columns, Point3 rotation, Color _c)
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
                }, _c));

                // Bottom circle
                tempTris.Add(new Triangle3(new Point3[]
                {
                    new Point3(Convert.ToSingle(Math.Cos(i / rad2Deg) * radius), height, Convert.ToSingle(Math.Sin(i / rad2Deg) * radius)),
                    new Point3(Convert.ToSingle(Math.Cos((i + columnAdder) / rad2Deg) * radius), height, Convert.ToSingle(Math.Sin((i + columnAdder) / rad2Deg) * radius)),
                    new Point3(0, height, 0),
                }, _c));
            }

            objs.Add(new Object(tempTris, location, rotation));
        }

        void newPlane(Point3 location, PointF size, Point3 rotation, Color _c)
        {
            objs.Add(new Object(new List<Triangle3>()
            {
                new Triangle3(new Point3[]{
                    new Point3(-size.X, 0, -size.Y) ,
                    new Point3(-size.X, 0, size.Y) ,
                    new Point3(size.X, 0, -size.Y) 
                }, _c),
                new Triangle3(new Point3[]{
                    new Point3(size.X, 0, size.Y) ,
                    new Point3(-size.X, 0, size.Y) ,
                    new Point3(size.X, 0, -size.Y)
                }, _c),
            }, location, rotation));
        }

        /** 3D MANAGER **/
        void SetupZBuffer()
        {
            trisToSort.Clear();
            objInts.Clear();

            // Split triangles
            foreach(Object obj in objs)
            {
                int oldTrisCount = 0;
                oldTrisCount += obj.tris.Count;
                for (int j = 0;j < oldTrisCount;j ++)
                {
                    obj.tris[j].TriangleMaxDist(50, obj.tris, camera, obj);
                }
                for(int i = obj.tris.Count-1; i >= 0; i--)
                {
                    if (obj.tris[i].isKill)
                    {
                        obj.tris.RemoveAt(i);
                    }
                }
            }

            // Add the triangles to an array to be sorted
            foreach (Object obj in objs)
            {
                objInts.Add(0);
                foreach (Triangle3 tri in obj.tris)
                {
                    trisToSort.Add(new SortingTriangle3(tri, obj));
                }
            }

            // Sort the triangles
            for(int i = 0;i < trisToSort.Count;i ++)
            {
                trisToSort[i].tri.setupLayering(camera, trisToSort[i].obj, res, trisToSort, i);
            }

            for(int j = trisToSort.Count-1;j >= 0; j--)
            {
                if (trisToSort[j].tri.isKill)
                {
                    trisToSort.RemoveAt(j);
                }
            }

            trisToSort = trisToSort.OrderByDescending(x => x.tri.saidZ).ToList();
        }

        /** INIT METHOD **/
        public MainScreen()
        {
            // Initialize
            InitializeComponent();

            // Set resolution
            res = new PointF(this.Width, this.Height); // 800, 450

            SetupZBuffer();

            for(int i = 0;i < keys.Length;i++)
            {
                keys[i] = false;
            }
        }
        /** UPDATE METHOD **/
        private void frameUpdate_Tick(object sender, EventArgs e)
        {
            // Framerate
            framesSinceLastSecond++;
            accurateSec = DateTime.Now.Second;
            if (lastSec != accurateSec)
            {
                lastSec = accurateSec;
                fps = framesSinceLastSecond;
                framesSinceLastSecond = 0;
            }

            // Millis differences
            lastMillis = DateTime.Now.Millisecond;
            lastSecond = DateTime.Now.Second;

            // Setup objects
            objs.Clear();
            Form1.maxid = 0;

            // Add objects
            
            //newCylinder(new Point3(150, -100, 1000), new Point3(150, 100, 50), 36, new Point3(test, test, test), Color.Yellow);
            //newSphere(new Point3(450, -100, 1000), 100, 8, 16, Color.Blue);
            //newCube(new Point3(-450, -100, 1000), new Point3(100, 100, 100), new Point3(test, test, test), Color.LimeGreen);
            //newCone(new Point3(-150, -100, 1000), 50, 100, 12, new Point3(test, test, test), Color.Purple);
            newPlane(new Point3(0, 100, 0), new PointF(4000, 4000), new Point3(5, 0, 0), Color.Red);

            objCreateTime = getMillisSinceLast();

            // Camera movement and rotation
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
            test++;
            // Sort objects
            SetupZBuffer();

            zBufferTime = getMillisSinceLast();
            this.Refresh();
        }

        /** PAINT METHOD **/
        private void MainScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(res.X / 2, res.Y / 2);

            // Draw the triangles
            int i = 0;
            int h = 0;
            foreach (SortingTriangle3 tri in trisToSort)
            {
                i++;
                //e.Graphics.FillPolygon(new SolidBrush(tri.tri.mainColor), tri.tri.PointsOnScreen(camera, tri.obj, res));
                e.Graphics.DrawPolygon(new Pen(Color.Black, 2), tri.tri.PointsOnScreen(camera, tri.obj, res));
                h++;
            }
            e.Graphics.ResetTransform();

            // Get FPS
            drawTime = getMillisSinceLast();
            e.Graphics.DrawString($"FPS: {fps}\nTris: {h}/{i}\nCreate Time: {objCreateTime}ms\nZ Buffer Time: {zBufferTime}ms\nDraw Time: {drawTime}ms", DefaultFont, new SolidBrush(Color.Black), new PointF(10, 10));
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

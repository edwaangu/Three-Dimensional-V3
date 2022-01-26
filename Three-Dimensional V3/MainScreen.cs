﻿using System;
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
         * -- Three-Dimensional V3 / Dimensional Escape -- 
         * Made by Ted Angus
         * Created: 2022/1/24 - 2022/1/26
         * 
         * -- CONTROLS --
         * Esc - End program
         * W - Forward
         * A/D - Strafe Left/Right
         * S - Backwards
         * Mouse movement - Turns camera
         * Z - Shoot
         * 
         * -- TO DO --
         * 
         * ~ Killing enemies & enemies that shoot
         * ~ HP, Energy
         * ~ Floating text?
         * ~ Start level creation
         * ~ Final boss?
         * ~ Better graphics
         * ~ Win screen and menu screen
         * ~ Polishing and bug fixes
         * 
         * -- Everything that's finished --
         * ~ Create the game file itself
         * ~ Test movement
         * ~ Gravity
         * ~ First person
         * ~ Collisions to floors
         * ~ Collisions to walls
         * ~ Laser gun shooting
         * ~ Enemy testing, and collisions
         * ~ Cursor movement
         * 
         * 
        */

        /** VARIABLES YOU CAN EDIT FOR VIEWING PLEASURE: **/
        int mode = 0; //0 = the level, 1 = checkerboard
        int triangleSplitMax = 1200; // Lower this if you are experiencing large layering issues, but raise it if you are experiencing fps issues

        /** MAIN VARIABLES **/
        PointF res; // Resolution
        Random randGen = new Random();
        Point prevMouse = new Point(0, 0);
        PointF cursorIncrease = new PointF(0, 0);

        /** CAMERA RELATED VARIABLES **/
        Camera camera = new Camera(90, new Point3(0, 0, -300), new PointF(0, 0), 3000); // Camera
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
        int totalTris = 0;

        /** SHAPE RELATED VARIABLES **/
        List<Object> objs = new List<Object>();
        List<SortingTriangle3> trisToSort = new List<SortingTriangle3>();
        List<float> objInts = new List<float>();

        /** GAME RELATED VARIABLES **/
        Player p = new Player(new Point3(0, -15, 0));

        List<Collision> cols = new List<Collision>();
        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemies = new List<Enemy>();

        int bulletCooldown = 20;

        int test = 0;
        int movePlatformA = 0;
        public int movePlatformAMode = 0;
        int platformMoveStop = 0;

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

        void newCube(Point3 location, Point3 size, Point3 rotation, Color _c, bool missingBottom = false, bool missingTop = false, bool missingLeft = false, bool missingRight = false, bool missingFront = false, bool missingBack = false, bool isMovingPlatform = false, int movePlatformType = 0)
        {
            List<Triangle3> temp = new List<Triangle3>();
            if (!missingFront)
            {
                // Front
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(-size.X, -size.Y, -size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }, _c));
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(size.X, size.Y, -size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }, _c));
            }

            if (!missingRight)
            {
                // Right side
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(size.X, -size.Y, -size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }, _c));
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(size.X, size.Y, size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }, _c));
            }

            if (!missingLeft)
            {
                // Left side
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(-size.X, -size.Y, -size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                }, _c));
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                }, _c));
            }

            if (!missingBack)
            {
                //Back Side
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(-size.X, -size.Y, size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }, _c));
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(size.X, size.Y, size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, -size.Y, size.Z),
                }, _c));
            }

            if (!missingTop)
            {
                // Top side
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(-size.X, -size.Y, -size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }, _c));
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(size.X, -size.Y, size.Z),
                    new Point3(-size.X, -size.Y, size.Z),
                    new Point3(size.X, -size.Y, -size.Z),
                }, _c));
            }

            if (!missingBottom) {
                // Bottom side
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(-size.X, size.Y, -size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                }, _c));
                temp.Add(new Triangle3(new Point3[] {
                    new Point3(size.X, size.Y, size.Z),
                    new Point3(-size.X, size.Y, size.Z),
                    new Point3(size.X, size.Y, -size.Z),
                }, _c));
            }
            // Create 12 triangles, 2 for each side
            objs.Add(new Object(temp, location, rotation));

            cols.Add(new Collision(location, size, "cube", _isMovePlatform: isMovingPlatform, _movePlatformType:movePlatformType));
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
            totalTris = 0;

            // Split triangles
            foreach(Object obj in objs)
            {
                int oldTrisCount = 0;
                oldTrisCount += obj.tris.Count;
                totalTris += obj.tris.Count;
                for (int j = 0;j < oldTrisCount;j ++)
                {
                    obj.tris[j].TriangleMaxDist(triangleSplitMax, obj.tris);
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


            // Setup keys
            for(int i = 0;i < keys.Length;i++)
            {
                keys[i] = false;
            }

            // Add enemies
            if (mode == 1)
            {
                enemies.Add(new Enemy(new Point3(0, -200, 0), "crawler"));
            }
            else
            {
                enemies.Add(new Enemy(new Point3(500, -1700, 15000), "crawler"));
                enemies.Add(new Enemy(new Point3(-500, -1700, 15000), "crawler"));
                enemies.Add(new Enemy(new Point3(1000, -1700, 15000), "crawler"));
                enemies.Add(new Enemy(new Point3(-1000, -1700, 15000), "crawler"));

            }

        }

        /** UPDATE METHOD **/
        private void frameUpdate_Tick(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            prevMouse = new Point(0 + Cursor.Position.X, 0 + Cursor.Position.Y);
            Cursor.Position = new Point(f.DesktopLocation.X + this.Width / 2, f.DesktopLocation.Y + this.Height / 2);
            Cursor.Hide();

            // Bullet counter
            if (bulletCooldown > 0)
            {
                bulletCooldown--;
            }

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
            cols.Clear();

            if (mode == 1)
            {
                for (int i = Convert.ToInt16(p.pos.X / 400f) - 8; i <= Convert.ToInt16(p.pos.X / 400f) + 8; i++)
                {
                    for (int j = Convert.ToInt16(p.pos.Z / 400f) - 8; j <= Convert.ToInt16(p.pos.Z / 400f) + 8; j++)
                    {
                        newCube(new Point3(i * 400, 100 + (i+j) * 100, j * 400), new Point3(200, 50, 200), new Point3(0, 0, 0), (i + j) % 2 == 0 ? Color.Black : Color.LightSlateGray, missingBottom: true, missingFront: true, missingLeft: true);

                    }
                }
            }
            else
            {
                // Base
                newCube(new Point3(0, 150, 3000), new Point3(1000, 150, 4000), new Point3(0, 0, 0), Color.Gray, missingBottom: true, missingLeft: true, missingRight: true, missingFront: true, missingBack: true);

                // Platforms
                newCube(new Point3(0, -200, 2000), new Point3(200, 200, 200), new Point3(0, 0, 0), Color.DarkSlateGray, missingBottom: true);
                newCube(new Point3(200, -500, 3000), new Point3(200, 100, 200), new Point3(0, 0, 0), Color.DarkSlateGray, missingBottom: true);
                newCube(new Point3(-200, -800, 4000), new Point3(200, 100, 200), new Point3(0, 0, 0), Color.DarkSlateGray, missingBottom: true);
                newCube(new Point3(-400, -1100, 5000), new Point3(200, 100, 200), new Point3(0, 0, 0), Color.DarkSlateGray, missingBottom: true);
                newCube(new Point3(300, -1100, 6000), new Point3(200, 100, 200), new Point3(0, 0, 0), Color.DarkSlateGray, missingBottom: true);

                // Ledge
                newCube(new Point3(0, -750, 8000), new Point3(1000, 750, 1000), new Point3(0, 0, 0), Color.Gray, missingBottom: true, missingLeft: true, missingRight: true, missingBack: true);

                // Moving platforms
                if (platformMoveStop <= 0)
                {
                    movePlatformA += movePlatformAMode == 2 ? -5 : 5;
                }
                if(movePlatformA > 1500 && movePlatformAMode == 0)
                {
                    movePlatformAMode = 1;
                    platformMoveStop = 60;
                }
                if (movePlatformA < 0 && movePlatformAMode == 2)
                {
                    movePlatformAMode = 3;
                    platformMoveStop = 60;
                }

                if(movePlatformAMode == 1 || movePlatformAMode == 3)
                {
                    platformMoveStop--;
                    if(platformMoveStop <= 0)
                    {
                        movePlatformAMode++;
                        if(movePlatformAMode >= 4)
                        {
                            movePlatformAMode = 0;
                        }
                    }
                }

                newCube(new Point3(0, -1500, 12500 - movePlatformA), new Point3(200, 100, 200), new Point3(0, 0, 0), Color.DarkSlateGray, missingBottom: true, isMovingPlatform: true, movePlatformType: 1);
                newCube(new Point3(400, -1500, 9500 + movePlatformA), new Point3(200, 100, 200), new Point3(0, 0, 0), Color.DarkSlateGray, missingBottom: true, isMovingPlatform: true, movePlatformType: 0);


                // Ledge 2
                newCube(new Point3(0, -1500, 15000), new Point3(2000, 150, 2000), new Point3(0, 0, 0), Color.SaddleBrown, missingBottom: true, missingLeft: true, missingRight: true, missingBack: true);

            }

            //cols.Add(new Collision(new Point3(0, 0, 0), new Point3(2000, 0, 2000), "plane"));
            //cols.Add(new Collision(new Point3(150, -300, 700), new Point3(100, 100, 100), "cube"));
            //cols.Add(new Collision(new Point3(0, -50, -4100), new Point3(2000, 0, 2000), "plane"));

            foreach (Bullet b in bullets)
            {
                newSphere(b.pos, 10, 4, 8, Color.LimeGreen);
                b.Move();
                foreach(Enemy enemy in enemies)
                {
                    if(b.pos.X > enemy.pos.X - enemy.size.X && b.pos.X < enemy.pos.X + enemy.size.X && b.pos.Y > enemy.pos.Y - enemy.size.Y && b.pos.Y < enemy.pos.Y + enemy.size.Y && b.pos.Z > enemy.pos.Z - enemy.size.Z && b.pos.Z < enemy.pos.Z + enemy.size.Z)
                    {
                        enemy.health--;
                        b.isKill = true;
                    }
                }
                if(Math.Sqrt(Math.Pow(b.pos.X - p.pos.X, 2) + Math.Pow(b.pos.Y - p.pos.Y, 2) + Math.Pow(b.pos.Z - p.pos.Z, 2)) > 2000)
                {
                    b.isKill = true;
                }
            }

            foreach (Enemy enemy in enemies)
            {
                newCube(new Point3(enemy.pos.X, enemy.pos.Y-5, enemy.pos.Z), enemy.size, new Point3(0, 0, 0), enemy.type == "crawler" ? Color.DarkRed : Color.DarkTurquoise);

                newCube(new Point3(enemy.pos.X, enemy.pos.Y - 120, enemy.pos.Z), new Point3(100 * (enemy.health / 5f), 20, 20), new Point3(Convert.ToSingle(-camera.direction.X * (180 / Math.PI)), 0, 0), Color.Green);
            }

            for (int i = bullets.Count-1;i >= 0;i--)
            {
                if (bullets[i].isKill)
                {
                    bullets.RemoveAt(i);
                }
            }

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].health <= 0)
                {
                    enemies.RemoveAt(i);
                }
            }




            objCreateTime = getMillisSinceLast();

            // Movement and rotation
            if (keys[87])
            {
                p.velocity.X += Convert.ToSingle(Math.Sin(camera.direction.X) * (p.hasJumped ? 1.5f : 4));
                p.velocity.Z += Convert.ToSingle(Math.Cos(camera.direction.X) * (p.hasJumped ? 1.5f : 4));
            }
            if (keys[65])
            {
                p.velocity.X += Convert.ToSingle(Math.Sin(camera.direction.X - (90 / (180 / Math.PI))) * (p.hasJumped ? 1.5f : 4));
                p.velocity.Z += Convert.ToSingle(Math.Cos(camera.direction.X - (90 / (180 / Math.PI))) * (p.hasJumped ? 1.5f : 4));
            }
            if (keys[68])
            {
                p.velocity.X += Convert.ToSingle(Math.Sin(camera.direction.X + (90 / (180 / Math.PI))) * (p.hasJumped ? 1.5f : 4));
                p.velocity.Z += Convert.ToSingle(Math.Cos(camera.direction.X + (90 / (180 / Math.PI))) * (p.hasJumped ? 1.5f : 4));
            }
            if (keys[83])
            {
                p.velocity.X -= Convert.ToSingle(Math.Sin(camera.direction.X) * (p.hasJumped ? 1.5f : 4));
                p.velocity.Z -= Convert.ToSingle(Math.Cos(camera.direction.X) * (p.hasJumped ? 1.5f : 4));
            }
            if (keys[32] && p.hasJumped == false)
            {
                p.hasJumped = true;
                p.velocity.Y = -20;
            }

            // Bullet shooting
            if(keys[90] && bulletCooldown <= 0)
            {
                bullets.Add(new Bullet(new Point3(p.pos.X + 0, p.pos.Y + 0, p.pos.Z + 0), camera.direction));
                bulletCooldown = 20;
            }

            // Add velocities to the player
            p.addVelocities();

            foreach (Collision col in cols)
            {
                col.checkCollision(p, col.isMovePlatform ? movePlatformAMode : -1);
            }

            // Check collisions and movement for enemies
            foreach(Enemy enemy in enemies)
            {
                if (Math.Sqrt(Math.Pow(p.pos.X - enemy.pos.X, 2) + Math.Pow(p.pos.Y - enemy.pos.Y, 2) + Math.Pow(p.pos.Z - enemy.pos.Z, 2)) < camera.maximumRenderDistance/1.3f)
                {
                    enemy.Move(cols, p);
                }
            }


            camera.pos.X = p.pos.X;
            camera.pos.Y = p.pos.Y - (p.size.Y / 2);
            camera.pos.Z = p.pos.Z;
            if (keys[16])
            {
                //camera.pos.Y += 5;
            }
            if (keys[32])
            {
                //camera.pos.Y -= 5;
            }

            if (test > 2)
            {
                cursorIncrease.X -= Convert.ToSingle((Cursor.Position.X - prevMouse.X) / (180f / Math.PI) / 7f);
                cursorIncrease.Y += Convert.ToSingle((Cursor.Position.Y - prevMouse.Y) / (180f / Math.PI) / 7f);
            }
            camera.direction.X += cursorIncrease.X;
            camera.direction.Y += cursorIncrease.Y;
            cursorIncrease.X *= 0.6f;
            cursorIncrease.Y *= 0.6f;

            if(p.pos.Y > 3500)
            {
                p.pos = new Point3(0, -3500, 0);
                p.velocity.Y = 0;
            }

            if(p.velocity.Y > 30)
            {
                p.velocity.Y = 30;
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
                e.Graphics.FillPolygon(new SolidBrush(tri.tri.mainColor), tri.tri.PointsOnScreen(camera, tri.obj, res));
                //e.Graphics.DrawPolygon(new Pen(Color.Black, 2), ps);
                h++;
            }
            e.Graphics.ResetTransform();

            // Get FPS
            drawTime = getMillisSinceLast();
            e.Graphics.DrawString($"FPS: {fps}\nTris: {h}/{totalTris}\nCreate Time: {objCreateTime}ms\nZ Buffer Time: {zBufferTime}ms\nDraw Time: {drawTime}ms", DefaultFont, new SolidBrush(Color.Black), new PointF(10, 10));
        }

        /** KEYBOARD **/
        private void MainScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Update value of key array to true
            keys[e.KeyValue] = true;
            if(e.KeyCode == Keys.Escape)
            {
                Cursor.Show();
                Application.Exit();
            }
        }

        private void MainScreen_KeyUp(object sender, KeyEventArgs e)
        {
            // Update value of key array to false
            keys[e.KeyValue] = false;
        }
    }
}

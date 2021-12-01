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
         * Version v3.0.0:
         * ~ Very basic WORKING 3d stuff for once!
         * ~ Basic layering
         * 
        */


        /** MAIN VARIABLES **/
        PointF res; // Resolution

        /** CAMERA RELATED VARIABLES **/
        Camera camera = new Camera(60, new Point3(0, 0, 0)); // Camera


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
                    tri.PointsOnScreen(camera, obj, res);
                }
            }
        }
        /** UPDATE METHOD **/
        private void frameUpdate_Tick(object sender, EventArgs e)
        {
            objs[0].pos.X++;
            objs[0].pos.Z++;
            objs[0].pos.Y ++;
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
                    i += 10;
                    e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(i, i, i)), tri.PointsOnScreen(camera, obj, res));
                    //e.Graphics.DrawPolygon(new Pen(Color.Black, 2), tri.PointsOnScreen(camera, obj, res));
                }
            }
            e.Graphics.ResetTransform();
        }
    }
}

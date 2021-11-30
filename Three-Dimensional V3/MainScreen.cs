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
         * Three-Dimensional V3 (Made by Ted)
         * Created: 2021/11/29
         * 
         * 
         * 
        */


        /** MAIN VARIABLES **/
        PointF res; // Resolution
        bool mode3d = false; // 3D?

        /** CAMERA RELATED VARIABLES **/
        Camera camera = new Camera(45); // Camera


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
        }

        /** UPDATE METHOD **/
        private void frameUpdate_Tick(object sender, EventArgs e)
        {

        }

        /** PAINT METHOD **/
        private void MainScreen_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

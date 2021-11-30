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
         * Three-Dimensional V3 (Ted Angus)
         * Created: 2021/11/29
         * 
         * 
         * 
        */


        /** VARIABLES **/
        PointF res; // Resolution
        Camera camera = new Camera(45); // Camera


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

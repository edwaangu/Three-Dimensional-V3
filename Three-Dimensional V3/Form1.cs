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
    public partial class Form1 : Form
    {
        public static int maxid = 0;
        public Form1()
        {
            InitializeComponent();

            MainScreen ms = new MainScreen();
            this.Controls.Add(ms);
        }
    }
}

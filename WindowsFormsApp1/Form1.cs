using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> imgInput;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog()==DialogResult.OK)
            {
                imgInput = new Image<Bgr, byte>(dialog.FileName);
                pictureBox1.Image = imgInput.Bitmap;
            }
        }
    }
}

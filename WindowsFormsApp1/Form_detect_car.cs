using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WindowsFormsApp1
{
    public partial class Form_detect_car : Form
    {
        Image<Bgr, byte> imgInput;
        VideoCapture capture;
        int cars_number = 0;
        int second = 0;
        int new_cars =0;
        public Form_detect_car()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imgInput = new Image<Bgr, byte>(dialog.FileName);
                    pictureBox1.Image = imgInput.Bitmap;
                }
                else
                {
                    throw new Exception("No file selected");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void detectCarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput != null)
                {
                    Detect(imgInput);
                }
                else
                {
                    throw new Exception("Please file select image");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Detect(Image<Bgr,byte>img)
        {
            try
            {
                Image<Bgr, Byte> imgresize = img.Resize(img.Width/2, img.Height/2, Emgu.CV.CvEnum.Inter.Linear);

                String path = Path.GetFullPath(@"D:\C\DetectCar\data\haarcascade_car.xml");
                //String path = Path.GetFullPath(@"data\haarcascade_car.xml");
                CascadeClassifier cascadeClassifier = new CascadeClassifier(path);
                var imgGray = imgresize.Convert<Gray, byte>().Clone();
                Rectangle[] cars = cascadeClassifier.DetectMultiScale(imgGray, 1.1,5);
                foreach (var car in cars)
                {
                    imgresize.Draw(car, new Bgr(0, 0, 255), 2);
                }
                pictureBox1.Image = imgresize.Bitmap;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private async void videoDectectCarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            try
            {
                OpenFileDialog odf = new OpenFileDialog();
                if (odf.ShowDialog()==DialogResult.OK)
                {
                    capture = new VideoCapture(odf.FileName);
                    while(true)
                    {
                        Mat m = new Mat();
                        capture.Read(m);
                        Detect(m.ToImage<Bgr,byte>());
                        // pictureBox1.Image = m.Bitmap;
                        double fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                        await Task.Delay(1000 / Convert.ToInt32(fps));
                    }
                    
                }
                else
                {
                    throw new Exception("No file selected");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DetectCounting(Image<Bgr, byte> img)
        {
            try
            {
                Image<Bgr, Byte> imgresize = img.Resize(img.Width / 2, img.Height / 2, Emgu.CV.CvEnum.Inter.Linear);

                String path = Path.GetFullPath(@"D:\C\DetectCar\data\haarcascade_car.xml");
                CascadeClassifier cascadeClassifier = new CascadeClassifier(path);
                var imgGray = imgresize.Convert<Gray, byte>().Clone();
                Rectangle[] cars = cascadeClassifier.DetectMultiScale(imgGray, 1.1, 5);
                int aa = 0;
                DateTime now = DateTime.Now;

                foreach (var car in cars)
                {
                    imgresize.Draw(car, new Bgr(0, 0, 255), 2);
                    int cx = car.Width / 2;
                    int cy = car.Height / 2;

                    Point center = new Point(car.X + cx, car.Y + cy);

                    CvInvoke.Circle(imgresize, center, 3, new MCvScalar(0, 0, 255), 3);
                    CvInvoke.Line(imgresize, new Point(0, 250), new Point(1200, 250), new MCvScalar(255, 0, 0),1);
                    
                    //DateTime now = DateTime.Now;
                    /*if(second>=60)
                    {
                        second =1;
                    }*/
                    //aa = center.Y;
                    aa = now.Second;
                    if (center.Y >= 200 && center.Y <= 225)
                    {
                        if (now.Second > second)
                        {
                            second = now.Second + 10;
                            if (second >= 60)
                            {
                                second = 1;
                            }
                                
                            //new_cars = 0;
                            cars_number++;
                            CvInvoke.Line(imgresize, new Point(0, 250), new Point(1200, 250), new MCvScalar(0, 0, 255), 1);

                        }
                        //CvInvoke.Line(imgresize, new Point(25, 550), new Point(1200, 550), new MCvScalar(0, 0, 255), 3);
                        //Console.WriteLine($"cars : {cars_number}");
                    }

                }
                CvInvoke.PutText(imgresize, $"Count cars:{cars_number}-{second}-{aa}", new System.Drawing.Point(10, 270), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1.0, new MCvScalar(0, 0, 255));
                pictureBox1.Image = imgresize.Bitmap;
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            this.Location = new Point(0, 0);
            this.Size = new Size(w, h);
        }
    }
}

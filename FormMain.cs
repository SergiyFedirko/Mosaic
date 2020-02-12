using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mosaic
{
    
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            
        }

        PictureBox[] PB = null;
        int LengthSides = 3;
        Bitmap Picture = null;

        void CreatePictureRegion()
        {
            if (PB != null)
            {
                for (int i = 0; i < PB.Length; i++)
                {
                    PB[i].Dispose();
                }
                PB = null;
            }
            
            
            int num = LengthSides;
            PB = new PictureBox[num * num];
            
            int w = panel1.Width / num;
            int h = panel1.Height / num;

            int countX = 0; 
            int countY = 0;
            for (int i = 0; i < PB.Length; i++)
            {
                PB[i] = new PictureBox();

                PB[i].Width = w;
                PB[i].Height = h;
                PB[i].Left = 0 + countX * PB[i].Width;
                PB[i].Top = 0 + countY * PB[i].Height;

                Point pt = new Point();
                pt.X = PB[i].Left;
                pt.Y = PB[i].Top;
                PB[i].Tag = pt;

                countX++;
                if (countX == num)
                {
                    countX = 0;
                    countY++;
                }


                PB[i].Parent = panel1;
                PB[i].BorderStyle = BorderStyle.None;
                PB[i].SizeMode = PictureBoxSizeMode.StretchImage;
                PB[i].Show();
                PB[i].Click += new EventHandler(PB_Click); 
            }

            
            DrawPicture();
            
        }

        void DrawPicture()
        {
            if (Picture == null) return;
            int countX = 0;
            int countY = 0;
            int num = LengthSides;
            for (int i = 0; i < PB.Length; i++)
            {
                int w = Picture.Width / num;
                int h = Picture.Height / num;
                PB[i].Image = Picture.Clone(new RectangleF(countX * w, countY * h, w, h), Picture.PixelFormat);
                countX++;
                if (countX == LengthSides)
                {
                    countX = 0;
                    countY++;
                }

            }
        }

        void PB_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            
            for (int i = 0; i < PB.Length; i++)
            {
                if (PB[i].Visible == false)
                {
                    if (
                        (pb.Location.X == PB[i].Location.X && 
                         Math.Abs(pb.Location.Y - PB[i].Location.Y) == PB[i].Height ) 
                        ||
                        (pb.Location.Y == PB[i].Location.Y && 
                         Math.Abs(pb.Location.X - PB[i].Location.X) == PB[i].Width)
                        )
                    {
                        Point pt = PB[i].Location;
                        PB[i].Location = pb.Location;
                        pb.Location = pt;

                        for (int j = 0; j < PB.Length; j++)
                        {
                            Point point = (Point)PB[j].Tag;
                            if (PB[j].Location != point)
                            {
                                return;
                            }
                        }

                        for (int m = 0; m < PB.Length; m++)
                        {
                            PB[m].Visible = true;
                            PB[m].BorderStyle = BorderStyle.None;
                        }
                    }

                    break;
                }
            }

            

            
        }

        private void toolStripButtonLoadPicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDlg = new OpenFileDialog();
            ofDlg.Filter = "файли картинок (*.bmp;*.jpg;*.jpeg;)|*.bmp;*.jpg;.jpeg|All files (*.*)|*.*";
            ofDlg.FilterIndex = 1;
            ofDlg.RestoreDirectory = true;

            if (ofDlg.ShowDialog() == DialogResult.OK)
            {
                Picture = new Bitmap(ofDlg.FileName);
                CreatePictureRegion();
            }
        }

        private void toolStripButtonMixed_Click(object sender, EventArgs e)
        {
            if (Picture == null) return;
            Random rand = new Random(Environment.TickCount);
            int r = 0;
            for (int i = 0; i < PB.Length; i++)
            {
                PB[i].Visible = true;
                r = rand.Next(0, PB.Length);
                Point ptR = PB[r].Location;
                Point ptI = PB[i].Location;
                PB[i].Location = ptR;
                PB[r].Location = ptI;
                PB[i].BorderStyle = BorderStyle.FixedSingle;
            }
            r = rand.Next(0, PB.Length);
            PB[r].Visible = false;
           
        }

        private void toolStripButtonRestore_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < PB.Length; i++)
            {
                Point pt = (Point)PB[i].Tag;
                PB[i].Location = pt;
                PB[i].Visible = true;
            }
        }

        private void toolStripButtonSetting_Click(object sender, EventArgs e)
        {
            SetDlg setDlg = new SetDlg();
            setDlg.LengthSides = LengthSides;
            if (setDlg.ShowDialog() == DialogResult.OK)
            {
                LengthSides = setDlg.LengthSides;

                
                CreatePictureRegion();
            }
        }

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            HelpDlg helpDlg = new HelpDlg();
            helpDlg.ImageDuplicate = Picture;
            helpDlg.ShowDialog();
        }



        
    }
}

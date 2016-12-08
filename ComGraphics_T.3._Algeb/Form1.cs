using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComGraphics_T._3._Algeb
{
        public partial class Form1 : Form
        {
            public Form1()
            {
                InitializeComponent();
            }
            private int [,] arrPix;

                struct Complex
            {
                public double x;
                public double y;

            };
        private static int W = 999;
        private static int H = 699;
        private Bitmap fractal = new Bitmap(W, H);
        private float[] poly = new float[] { -1,0,0,0,0,1,0};
        
        private Complex comp(double x, double y)
        {
            Complex z;
            z.x = x;
            z.y = y;
            return z;
        }

        private double Re(Complex z)
        {
            return z.x;
        }

        private double Im(Complex z)
        {
            return z.y;
        }

        private double mod(Complex z)
        {
            return Math.Sqrt(Math.Pow(z.x, 2) + Math.Pow(z.y, 2));
        }

        private Complex cadd(Complex z1, Complex z2)
        {
            Complex z;
            z.x = z1.x + z2.x;
            z.y = z1.y + z2.y;
            return z;
        }

        private Complex csub(Complex z1, Complex z2)
        {
            Complex z;
            z.x = z1.x - z2.x;
            z.y = z1.y - z2.y;
            return z;
        }

        private Complex crmult(double x, Complex z1)
        {
            Complex z;
            z.x = z1.x * x;
            z.y = z1.y * x;
            return z;
        }

        private Complex cmult(Complex z1, Complex z2)
        {
            Complex z;
            z.x = z1.x * z2.x - z1.y * z2.y;
            z.y = z1.x * z2.y + z1.y * z2.x;
            return z;
        }


        private Complex cdiv(Complex z1, Complex z2)
        {
            Complex z;
            z.x = (z1.x * z2.x + z1.y * z2.y) / (z2.x * z2.x + z2.y * z2.y);
            z.y = (-z1.x * z2.y + z1.y * z2.x) / (z2.x * z2.x + z2.y * z2.y);
            return z;
        }

        private Complex f(Complex z, int deg)
        {
            int i;
            Complex f;
            f = comp(poly[deg], 0);
            for (i = deg - 1; i >= 0; i--)
                f = cadd(cmult(f, z), comp(poly[i], 0));
            return f;
        }
        Complex df(Complex z, int deg)
        {
            int i;
            Complex df;
            df = comp(deg * poly[deg], 0);
            for (i = deg - 1; i > 0; i--)
                df = cadd(cmult(df, z), comp(i * poly[i], 0));

            return df;
        }

        const int iter = 100000;
            const double min = 1e-80;
            const double max = 1e-10;


            public void Newton_fractal()
            {
                arrPix = new int[W, H];
                int n, mx, my;
                Complex z, w = new Complex();
                Complex fz, dfz = new Complex();
                int deg = 5;
                mx = W / 2;
                my = H / 2;

                for (int y = -my; y < my; y++)
                    for (int x = -mx; x < mx; x++)
                    {
                        n = 0;
                        z = comp((x) * 0.004, (y) * 0.004);
                        w = z;
                    for (int k = 1; k <= iter; k++)
                    {
                        n = k;
                        fz = f(z, deg);
                        dfz = df(z, deg);
                        if (mod(dfz) <= min) { break; }
                        w = z;
                        z = csub(z, cdiv(fz, dfz)); 
                        if (mod(csub(z, w)) <= max) break;
                    }
                    arrPix[mx + x, my + y] = n;
                    }
            }


            private void PictureBox1_Paint(object sender, PaintEventArgs e)
            {
                Newton_fractal();
                Pen myPen = new Pen(Color.Black, 1);
                Graphics fid;
                fid = Graphics.FromImage(fractal);
                for (int i=1; i < W; i++)
                {
                     for (int j=1;j< H; j++)
                     {
                          int y = arrPix[i, j];
                          myPen.Color = Color.FromArgb(255, (y) % 255, (y) % 255, (y*7) % 255); 
                          e.Graphics.DrawRectangle(myPen, i, j, 1, 1);
                     }
                }
           // pictureBox1.Image = fractal;
            //fractal.Save("D://fgsfd.jpg");
            }
        }   
}

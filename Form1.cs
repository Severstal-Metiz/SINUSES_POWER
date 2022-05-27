using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Math;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        double period = 1; //period v sec
        int iterations = 1000;
        double dt;

        double cnstValue = 1;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dt = period / iterations;
            DataPointCollection gr1 = chart1.Series["Series1"].Points;
            DataPointCollection gr2 = chart1.Series["Series2"].Points;
            DataPointCollection gr3 = chart1.Series["Series3"].Points;
            DataPointCollection gr4 = chart1.Series["Series4"].Points;
            DataPointCollection gr5 = chart1.Series["Series5"].Points;
            DataPointCollection gr6 = chart1.Series["Series6"].Points;
            fun(gr1, 6, 0, 1,Sin);
            fun(gr2, 3, 0, 1,Sin);
            fun(gr3, 1, 0, 1,Sin);
            PhaseInvertor(gr4,gr1,gr3);
            funMul(gr5,gr4,gr2);
            funMul(gr6, gr1, gr2);
            //fun(gr3, 2, 0, 1, Sin);
            //funDiode(gr3);
            //funMost(gr3);
            //fun(gr3, 2, 0, 1, Sin);



            //tbI.Text = Integral(gr3,gr4).ToString();
            //tbM.Text = Max(gr3).ToString();
            //tbD.Text = Sqrt(Integral(gr3)).ToString();


            chart1.Series["Series1"].Enabled = false;
            chart1.Series["Series2"].Enabled = false;
            chart1.Series["Series3"].Enabled = false;
            chart1.Series["Series4"].Enabled = false;
            //chart1.Series["Series5"].Enabled = false;
            //chart1.Series["Series6"].Enabled = false;

        }

        void PhaseInvertor(DataPointCollection resul,DataPointCollection points, DataPointCollection pointsInvertFun)
        {
            double res, f;
            for (int i = 0; i < iterations; i++)
            {
                f = pointsInvertFun[i].YValues[0];
                if (f < 0)
                {
                    res = points[i].YValues[0] * -1;
                }else if (f > 0)
                {
                    res = points[i].YValues[0];
                }else res = 0;
                
                resul.AddXY(i * dt, res);
            }
        }




        double Integral(DataPointCollection points, DataPointCollection resul = null)
        {
            double res=0;
            for (int i = 0; i < iterations; i++)
            {
                res +=  dt * points[i].YValues[0];
                if (resul != null) resul.AddXY(i * dt, res / period);
            }
            return res/period;
        }

        double Max(DataPointCollection points, DataPointCollection resul = null)
        {
            double res = 0;
            for (int i = 0; i < iterations; i++)
            {
                //res points[i].YValues[0];
                if (Abs(res) < Abs(points[i].YValues[0])) res = points[i].YValues[0];
                if (resul != null) resul.AddXY(i * dt, res);
            }
            return res;
        }

        double Cnst(double _)
        {
            return cnstValue;
        }

        void funAdd(DataPointCollection res, DataPointCollection pointsA, DataPointCollection pointsB)
        {
            for (int i = 0; i < iterations; i++)
            {
                res.AddXY((double)i * dt, pointsA[i].YValues[0] + pointsB[i].YValues[0]);
            }
        }

        void funMul(DataPointCollection res, DataPointCollection pointsA, DataPointCollection pointsB)
        {
            for (int i = 0; i < iterations; i++)
            {
                res.AddXY((double)i * dt, pointsA[i].YValues[0] * pointsB[i].YValues[0]);
            }
        }

        void funDiode(DataPointCollection points)
        {
            for (int i = 0; i < iterations; i++)
            {
                if (points[i].YValues[0] < 0) points[i].YValues[0] = 0;
            }
        }

        void funMost(DataPointCollection points)
        {
            for (int i = 0; i < iterations; i++)
            {
                if (points[i].YValues[0] < 0) points[i].YValues[0] = points[i].YValues[0] * -1;
            }
        }

        void fun(DataPointCollection points, double freq, double phazaG, double amplitude, Func<double,double> func)
        {
            double w;
            double x, y;
            double phaza = phazaG / 360 * 2 * PI;
            for (double t = 0; t <= period; t += dt)
            {
                x = t; w = 2 * PI * freq;
                //y = Sin((t + phaza) * w) * amplitude;
                y = func(t * w + phaza) * amplitude;
                points.AddXY(x, y);
            }
        }
       

       
    }
}

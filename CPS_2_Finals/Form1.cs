using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CPS_2_Finals
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics gg = CreateGraphics();
            SolidBrush red = new SolidBrush(Color.Red);
            SolidBrush blue = new SolidBrush(Color.Blue);
            SolidBrush green = new SolidBrush(Color.Green);

            Random obj = new Random();

            int size = 20, n_sweeps = 1;
            int[,] spinsystem = new int[size, size];
            double r;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    spinsystem[i, j] = 1; // all spins up
                    gg.FillEllipse(red, 100 + (float)i * 10, 100 + (float)j * 10, 5, 5);
                }
            }

            double E1, E2, E, Eflip, Pflip;
            double Eavg = 0;
            double[] Ealpha = new double[n_sweeps];

            for (double T = 0; T < 5; T += 0.1)
            {
                for (int sweeps = 0; sweeps < n_sweeps; sweeps++)
                {
                    E = 0;
                    for (int i = 1; i < size - 1; i++)
                    {
                        for (int j = 1; j < size - 1; j++)
                        {
                            E1 = (-spinsystem[i, j] * (spinsystem[i + 1, j] + spinsystem[i - 1, j]
                                + spinsystem[i, j + 1] + spinsystem[i, j - 1]));

                            spinsystem[i, j] *= -1;

                            E2 = (-spinsystem[i, j] * (spinsystem[i + 1, j] + spinsystem[i - 1, j]
                                + spinsystem[i, j + 1] + spinsystem[i, j - 1]));

                            Eflip = E2 - E1;
                            if (Eflip < 0)
                            {
                                if (spinsystem[i, j] == -1)
                                {
                                    gg.FillEllipse(blue, 100 + (float)i * 10, 100 + (float)j * 10, 5, 5); ;
                                }
                                else
                                {
                                    gg.FillEllipse(red, 100 + (float)i * 10, 100 + (float)j * 10, 5, 5);
                                }
                            }
                            else // Eflip > 0
                            {
                                Pflip = Math.Exp(-Eflip / (T));
                                r = obj.NextDouble();

                                if (Pflip > r)
                                {
                                    //accept flipping
                                    if (spinsystem[i, j] == -1)
                                    {
                                        gg.FillEllipse(blue, 100 + (float)i * 10, 100 + (float)j * 10, 5, 5); ;
                                    }
                                    else
                                    {
                                        gg.FillEllipse(red, 100 + (float)i * 10, 100 + (float)j * 10, 5, 5);
                                    }
                                }
                                else
                                {
                                    //reject fliping
                                    spinsystem[i, j] *= -1;
                                }

                                E = E - spinsystem[i, j] * (spinsystem[i - 1, j]
                                      + spinsystem[i + 1, j] + spinsystem[i, j - 1]
                                       + spinsystem[i, j + 1]);

                            }
                        }
                        Ealpha[sweeps] = E;
                        Eavg += E;


                    }//END OF SWEEPS LOOP
                     //calculate averages
                    Eavg /= n_sweeps;
                    double DelE = 0;
                    for (int sw = 0; sw < n_sweeps; sw++)
                    {
                        DelE += Math.Pow(Ealpha[sw] - Eavg, 2);
                    }
                    DelE /= n_sweeps;
                    double C;
                    C= Math.Abs(DelE / (T * T));
                    C /= (size * size - 4 * size + 4);
                    gg.FillEllipse(blue, 650 + (float)T * 30, 300 - (float)C * 30, 5, 5);
                }
            }
        }
    }
}

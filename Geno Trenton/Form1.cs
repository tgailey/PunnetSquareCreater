using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geno_Trenton
{

    public partial class Form1 : Form
    {
        private String[] possibilitiesDad;
        private String[] possibilitiesMom;
        private String addedWord = "";
        private int enteredNum;
        private int enteredCombos;
        private double totalCombos;

        private DataTable data;
        private DataRow row;

        private int[] phenoTypeRatios;
        private bool[] phenotypeRatioBools;

        public Form1()
        {
            data = new DataTable();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Phenotype Ratio: ";
            if (textBox2.Text.Length != textBox3.Text.Length)
            {
                label1.Text = "Please Enter An Equal Amount of Alleles For Each Parent";
            }
            else
            {
                addedWord = "";
                possibilitiesDad = new String[1];
                possibilitiesMom = new String[1];
                enteredCombos = 0;

                String dadText = textBox2.Text;
                enteredNum = dadText.Length / 2;
                String[,] genoDad = new String[enteredNum, 2];
                for (int seperation = 0; seperation < dadText.Length; seperation++)
                {
                    genoDad[seperation / 2, seperation % 2] = dadText.Substring(seperation, 1);
                }

                String momText = textBox3.Text;
                String[,] genoMom = new String[enteredNum, 2];
                for (int seperation = 0; seperation < momText.Length; seperation++)
                {
                    genoMom[seperation / 2, seperation % 2] = momText.Substring(seperation, 1);
                }

                totalCombos = Math.Pow(2, enteredNum);

                GeneratePossibilities(genoDad);
                possibilitiesDad = addedWord.Split(' ');
                textBox1.Text = addedWord;
                addedWord = "";

                GeneratePossibilities(genoMom);
                possibilitiesMom = addedWord.Split(' ');
                textBox4.Text = addedWord;
                addedWord = "";


                data.Columns.Clear();
                data.Rows.Clear();
                data.Clear();

                phenoTypeRatios = new int[(int)Math.Pow(2,momText.Length/2)];
                phenotypeRatioBools = new bool[(int)Math.Pow(2, momText.Length / 2)];

                data.Columns.Add(" ", typeof(String));
                for (int fatherPossibilities = 0; fatherPossibilities < possibilitiesDad.Length; fatherPossibilities++)
                {
                    DataColumn dc = new DataColumn();
                    dc.DataType = typeof(String);
                    data.Columns.Add(dc);
                }
                row = data.NewRow();
                row[" "] = "";
                for (int fatherPossibilities = 0; fatherPossibilities < possibilitiesDad.Length; fatherPossibilities++)
                {
                    row[fatherPossibilities + 1] = possibilitiesDad[fatherPossibilities];
                }
                data.Rows.Add(row);
                label1.Text = "Phenotype Ratio: ";
                for (int MP = 0; MP < possibilitiesMom.Length; MP++)
                {
                    row = data.NewRow();
                    row[" "] = possibilitiesMom[MP];
                    for (int fatherPossibilities = 0; fatherPossibilities < possibilitiesDad.Length; fatherPossibilities++)
                    {
                        String temp3 = "";
                        for (int i = 0; i < enteredNum; i++)
                        {
                            if (possibilitiesDad[fatherPossibilities].Substring(i, 1).CompareTo(possibilitiesMom[MP].Substring(i, 1)) >= 0)
                            {
                                temp3 += possibilitiesDad[fatherPossibilities].Substring(i, 1);
                                temp3 += possibilitiesMom[MP].Substring(i, 1);
                            }
                            else
                            {
                                temp3 += possibilitiesMom[MP].Substring(i, 1);
                                temp3 += possibilitiesDad[fatherPossibilities].Substring(i, 1);
                            }
                        }
                        for (int phenotypes = 0; phenotypes < phenoTypeRatios.Length; phenotypes++)
                        {
                            phenotypeRatioBools[phenotypes] = true;
                        }
                        for (int phenotypes = 0; phenotypes < temp3.Length/2; phenotypes++)
                        {
                            if (temp3.Substring(phenotypes * 2, 1).CompareTo(temp3.Substring(phenotypes * 2, 1).ToLower()) > 0)
                            {
                                for (int i = 0; i < phenoTypeRatios.Length; i++)
                                {
                                    if ((int)((i) / (Math.Pow(2, temp3.Length / 2 - 1 - phenotypes)))%2 == 1)
                                    {
                                        phenotypeRatioBools[i] = false;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < phenoTypeRatios.Length; i++)
                                {
                                    if ((int)((i) / (Math.Pow(2, temp3.Length / 2 - 1 - phenotypes))) % 2 == 0)
                                    {
                                        phenotypeRatioBools[i] = false;
                                    }
                                }
                            }
                        }

                        for (int phenotypes = 0; phenotypes < phenoTypeRatios.Length; phenotypes++)
                        {
                            if (phenotypeRatioBools[phenotypes])
                            {
                                phenoTypeRatios[phenotypes]++;
                            }
                        }
                            row[fatherPossibilities + 1] = temp3;
                    }
                    data.Rows.Add(row);
                }
                dataGridView1.DataSource = data;
                for (int c = 0; c < phenoTypeRatios.Length; c++)
                { 
                    label1.Text += phenoTypeRatios[c].ToString();
                    if (c != phenoTypeRatios.Length-1)
                    {
                        label1.Text += " : ";
                    }
                }
            }
        }

        private void GeneratePossibilities(String[,] ge)
        {
            for (int i = 0; i < totalCombos; i++)
            {
                for (int c = 0; c < enteredNum; c++)
                {
                    if ((int)(enteredCombos / (Math.Pow(2, enteredNum-1-c))) % 2 == 1)
                    {
                        addedWord += ge[c, 1];
                    }
                    else
                    {
                        addedWord += ge[c, 0];
                    }
                }
                if (i != totalCombos - 1)
                {
                    addedWord += " ";
                }
                enteredCombos++;
            }
        }
    }
}

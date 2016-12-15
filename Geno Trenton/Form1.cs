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

        DataTable data;
        DataRow row;

        public Form1()
        {
            data = new DataTable();
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "";
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

                String temp = textBox2.Text;
                enteredNum = temp.Length / 2;
                String[,] genoDad = new String[enteredNum, 2];
                for (int seperation = 0; seperation < temp.Length; seperation++)
                {
                    genoDad[seperation / 2, seperation % 2] = temp.Substring(seperation, 1);
                }

                String temp2 = textBox3.Text;
                String[,] genoMom = new String[enteredNum, 2];
                for (int seperation = 0; seperation < temp2.Length; seperation++)
                {
                    genoMom[seperation / 2, seperation % 2] = temp2.Substring(seperation, 1);
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

                //DataTable data = new DataTable();
                data.Columns.Clear();
                data.Rows.Clear();
                data.Clear();


                data.Columns.Add(" ", typeof(String));
                for (int fatherPossibilities = 0; fatherPossibilities < possibilitiesDad.Length; fatherPossibilities++)
                {
                    try
                    {
                        //data.Columns.Add(possibilitiesDad[fatherPossibilities], typeof(String));
                        DataColumn dc = new DataColumn();
                        dc.DataType = typeof(String);
                        data.Columns.Add(dc);
                    }
                    catch (DuplicateNameException ex)
                    {

                    }
                }
                row = data.NewRow();
                row[" "] = "";
                for (int fatherPossibilities = 0; fatherPossibilities < possibilitiesDad.Length; fatherPossibilities++)
                {
                    row[fatherPossibilities + 1] = possibilitiesDad[fatherPossibilities];
                }
                data.Rows.Add(row);
                label1.Text = "";
                for (int MP = 0; MP < possibilitiesMom.Length; MP++)
                {
                    row = data.NewRow();
                    row[" "] = possibilitiesMom[MP];
                    for (int fatherPossibilities = 0; fatherPossibilities < possibilitiesDad.Length; fatherPossibilities++)
                    {
                        String temp3 = "";
                        for (int i = 0; i < enteredNum; i++)
                        {
                            temp3 += possibilitiesDad[fatherPossibilities].Substring(i, 1);
                            temp3 += possibilitiesMom[MP].Substring(i, 1);
                        }
                        //label1.Text += temp3;
                        //textBox4.Text += possibilitiesDad.Length;
                        //label1.Text += possibilitiesDad[fatherPossibilities];
                        //row[possibilitiesDad[fatherPossibilities]] = label1.Text;
                        row[fatherPossibilities + 1] = temp3;
                    }
                    data.Rows.Add(row);
                }
                dataGridView1.DataSource = data;
                //data.Rows.Add("Father Possibilities", typeof(String));
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

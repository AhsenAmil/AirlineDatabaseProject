using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Airline
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
 

        private void Form3_Load(object sender, EventArgs e)
        {
            label2.Text = Form1.SetValueForText1;
            label3.Text = Form1.SetValueForText2;
            label4.Text = Form1.SetValueForText3;
            label5.Text = Form1.SetValueForText4;
            label6.Text = Form1.SetValueForText5;
            label7.Text = Form1.SetValueForText6;

            label1.Text = Form1.SetValueForText7;
            label15.Text = Form1.SetValueForText8;


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void label1_Click(object sender, EventArgs e)
        {

       
        }
        
    }
}

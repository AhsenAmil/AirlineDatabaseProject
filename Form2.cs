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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=1234";
            string ticket_number = (string)textBox1.Text;
            string name = textBox2.Text;
            string surname = textBox3.Text;
            string email = textBox4.Text;
            string phone = (string)textBox9.Text;
            string Query = "DELETE FROM airline_data.Passenger WHERE first_name='" + name + "' AND last_name='"+surname + "' AND email_address='"+email+"' AND phone_number=" + phone +";";           
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);        
            MySqlDataReader myReader;

            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();
                MessageBox.Show("YOUR TICKET HAS BEEN REFUNDED");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

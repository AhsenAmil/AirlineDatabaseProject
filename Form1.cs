using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Airline
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fillComboBoxFrom();
            fillComboBoxTo();
        
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    void fillComboBoxFrom()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=1234";
            string Query = "select * from airline_data.Flight;";
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;
            
            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();
                while (myReader.Read())
                {
                    string sName = myReader.GetString("departure_airport");
                    comboBox1.Items.Add(sName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void fillComboBoxTo()
        {
            string constring = "datasource=localhost;port=3306;username=root;password=1234";
            string Query = "select * from airline_data.Flight;";
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;
            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();
                while (myReader.Read())
                {
                    string sName = myReader.GetString("destination_airport");
                    comboBox2.Items.Add(sName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    
        //for SEARCH
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string constring = "datasource=localhost;port=3306;username=root;password=1234";
            string fromSelected = comboBox1.SelectedItem.ToString();
            string toSelected = comboBox2.SelectedItem.ToString();
            string Query = "select arrival_date,departure_date,airplane_reg_number from airline_data.Flight WHERE destination_airport='" + toSelected + "' AND departure_airport='" + fromSelected + "';";
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;
            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();
                while (myReader.Read())
                {
                    listBox1.Items.Add(myReader["departure_date"] + " - " + myReader["arrival_date"]);
                    fillComboBoxSeat((string)myReader["airplane_reg_number"]);
                   
                
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //for BUY TICKET
        private void button2_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=1234";          
            string name = textBox3.Text;
            string surname = textBox4.Text;
            string email = textBox5.Text;
            string tel = (string)textBox2.Text;
            string seat1 = comboBox3.SelectedItem.ToString();
            string seat2 = comboBox4.SelectedItem.ToString();
            Object selectedFlight = listBox1.SelectedItem;
             //selectedFlight.GetType().GetProperty("registration_number").GetValue(selectedFlight, null);
            string Query = "use airline_data; INSERT INTO Passenger(first_name,last_name,email_address,phone_number) VALUES('" 
                + name + "' , '" + surname
                    + "' , '" +
                 email + "' , '" + tel + "'); " +
                 "INSERT INTO Ticket(is_refundable, is_available, seat_number_p1, seat_number_p2, airplane_reg_number, passenger_id, flight_number) VALUE " +
                 "(TRUE, FALSE, " + "'" + seat1 + "', '" + seat2 + "', '" + 
                 "(SELECT registration_number FROM Airplane WHERE Airplane.registration_number =" + selectedFlight.GetType().GetProperty("airplane_reg_number").GetValue(selectedFlight, null) + "),"+
                 "(SELECT passenger_id FROM Passenger WHERE Passenger.email_address =" + email + ")," +
                 "(SELECT flight_number FROM Flight WHERE Fligt.flight_number = " + selectedFlight.GetType().GetProperty("flight_number").GetValue(selectedFlight, null) + ")); ";

            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;
                
                try
                {
                    conDatabase.Open();
                    
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("ok");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Please provide your information!");
                }
            
        }
        void fillComboBoxSeat(string registration)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=1234";
            string Query = "select * from airline_data.Seat WHERE Seat.airplane_reg_number= " + registration + ";";
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;

            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();
                while (myReader.Read())
                {
                    string part1 = (string)myReader["seat_number_p1"];
                    comboBox3.Items.Add(part1);
                    string part2 = (string)myReader["seat_number_p2"];
                    comboBox4.Items.Add(part2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2();
            form2.ShowDialog();
            
        }
    }
}

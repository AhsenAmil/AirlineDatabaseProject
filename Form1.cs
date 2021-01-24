using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            label1.Text = DateTime.Now.ToLongDateString();
        }

    void fillComboBoxFrom()
        {
            
            string constring = "datasource=localhost;port=3306;username=root;password=1234";
            string Query = "select distinct departure_airport from airline_data.Flight;";
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
            string Query = "select  distinct destination_airport from airline_data.Flight;";
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
            string Query = "select arrival_date,departure_date,airplane_reg_number,flight_number, price from airline_data.Flight WHERE destination_airport='" + toSelected + "' AND departure_airport='" + fromSelected + "';";
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;
            
            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();
                while (myReader.Read())
                {
                    listBox1.Items.Add(myReader["departure_date"] + " - " + myReader["arrival_date"] + " - " + myReader["airplane_reg_number"] + " - " + myReader["flight_number"] + " - " + myReader["price"]);               
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
        public static string SetValueForText1 = "";
        public static string SetValueForText2 = "";
        public static string SetValueForText3 = "";
        public static string SetValueForText4 = "";
        public static string SetValueForText5 = "";
        public static string SetValueForText6 = "";
        public static string flight_number = "";
        public static string SetValueForText7 = "";
        public static string SetValueForText8 = "";
        

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
            string selectedFlight = (string)listBox1.SelectedItem;
            string[] parts = selectedFlight.Split('-');
            string airplane_reg_number = parts[parts.Length - 3];
            flight_number = parts[parts.Length - 2];           
            string price = parts[parts.Length - 1];         
            string Query = " INSERT INTO Passenger(first_name,last_name,email_address,phone_number) VALUES('"
                + name + "' , '" + surname
                    + "' , '" +
                 email + "' , '" + tel + "'); ";
            string Query2=
                 "INSERT INTO Ticket(is_refundable, is_available, seat_number_p1, seat_number_p2, airplane_reg_number, passenger_id, flight_number) VALUES(TRUE, FALSE, (SELECT seat_number_p1 FROM Seat WHERE seat_number_p1 = " + seat1 + " AND seat_number_p2 = '" + seat2 + "' AND Seat.airplane_reg_number = '" + airplane_reg_number + "')," +
                 "(SELECT seat_number_p2 FROM Seat WHERE seat_number_p1 =" + seat1 + " AND seat_number_p2 = '" + seat2 + "' AND Seat.airplane_reg_number = '" + airplane_reg_number + "')," +
                 "(SELECT registration_number FROM Airplane WHERE registration_number = '" + airplane_reg_number + "')," +
                 "(SELECT passenger_id FROM Passenger WHERE email_address='" + email +"' )," +
                 "(SELECT flight_number FROM Flight WHERE flight_number = '" + flight_number + "')); ";
            string Query3 = "SELECT ticket_number, gate_number FROM (Ticket JOIN Passenger ON Ticket.passenger_id = Passenger.passenger_id)JOIN Flight ON Ticket.flight_number = Flight.flight_number WHERE email_address = '" + email + "' AND Flight.flight_number =  " + flight_number + "; ";
            string Query4 = "SELECT ticket_number FROM Ticket WHERE flight_number = '" + flight_number + "' AND seat_number_p1 =" + seat1 + " AND seat_number_p2 = '" + seat2 + "'; ";
            string Query5 = "use airline_data; SELECT passenger_id FROM Passenger WHERE email_address = '" + email + "'; ";
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);  //insert passenger
            MySqlCommand cmdDatabase2 = new MySqlCommand(Query2, conDatabase);  //insert ticket
            MySqlCommand cmdDatabase3 = new MySqlCommand(Query3, conDatabase);   //form3 için
            MySqlCommand cmdDatabase4 = new MySqlCommand(Query4, conDatabase);  //ticket kontrol
            MySqlCommand cmdDatabase5 = new MySqlCommand(Query5, conDatabase); //passenger kontrol
            MySqlDataReader myReader;

            try
            {
               
             
                    conDatabase.Open();
                    myReader = cmdDatabase5.ExecuteReader();
                    if (myReader.Read())
                    {
                        conDatabase.Close();
                        conDatabase.Open();
                        myReader = cmdDatabase4.ExecuteReader();
                        if (myReader.Read())
                        {
                            MessageBox.Show("The seat already taken");
                        }
                        else
                        {
                            conDatabase.Close();
                            conDatabase.Open();
                            myReader = cmdDatabase2.ExecuteReader();

                            SetValueForText1 = textBox3.Text;
                            SetValueForText2 = textBox4.Text;
                            SetValueForText3 = textBox5.Text;
                            SetValueForText4 = textBox2.Text;
                            SetValueForText5 = comboBox3.Text;
                            SetValueForText6 = comboBox4.Text;

                            conDatabase.Close();
                            conDatabase.Open();
                            myReader = cmdDatabase3.ExecuteReader();
                            while (myReader.Read())
                            {
                                SetValueForText7 = myReader["ticket_number"].ToString();
                                SetValueForText8 = myReader["gate_number"].ToString();
                            }

                            Form3 form3 = new Form3();
                            form3.ShowDialog();
                        }


                    }
                    else
                    {
                        conDatabase.Close();
                        conDatabase.Open();
                        myReader = cmdDatabase4.ExecuteReader();
                        if (myReader.Read())
                        {
                            MessageBox.Show("The seat already taken");
                        }
                        else
                        {
                            conDatabase.Close();
                            conDatabase.Open();
                            myReader = cmdDatabase.ExecuteReader();
                            conDatabase.Close();
                            conDatabase.Open();
                            myReader = cmdDatabase2.ExecuteReader();

                            SetValueForText1 = textBox3.Text;
                            SetValueForText2 = textBox4.Text;
                            SetValueForText3 = textBox5.Text;
                            SetValueForText4 = textBox2.Text;
                            SetValueForText5 = comboBox3.Text;
                            SetValueForText6 = comboBox4.Text;

                            conDatabase.Close();
                            conDatabase.Open();
                            myReader = cmdDatabase3.ExecuteReader();
                            while (myReader.Read())
                            {
                                SetValueForText7 = myReader["ticket_number"].ToString();
                                SetValueForText8 = myReader["gate_number"].ToString();
                            }

                            Form3 form3 = new Form3();
                            form3.ShowDialog();
                        }

                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Please provide your information!");
            }
          

        }
        void fillComboBoxSeat()
        {
            
            string selectedFlight = (string)listBox1.SelectedItem;
            string[] parts = selectedFlight.Split('-');
            string airplane_reg_number = parts[parts.Length - 2];
            string constring = "datasource=localhost;port=3306;username=root;password=1234";
            string Query = "SELECT DISTINCT seat_number_p1 FROM airline_data.Seat WHERE airplane_reg_number= '" + airplane_reg_number + "';";
            string Query2 = "SELECT DISTINCT seat_number_p2 FROM airline_data.Seat WHERE airplane_reg_number= '" + airplane_reg_number + "';";
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlCommand cmdDatabase2 = new MySqlCommand(Query2, conDatabase);
            MySqlDataReader myReader;


            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();
                comboBox3.Items.Clear();
                while (myReader.Read())
                {
                    string part1 = myReader.GetString("seat_number_p1");
                    //string part1 = (string)myReader["seat_number_p1"];
                    comboBox3.Items.Add(part1);
                    
                }
                conDatabase.Close();
                conDatabase.Open();
                myReader = cmdDatabase2.ExecuteReader();
                comboBox4.Items.Clear();
                while (myReader.Read())
                {                
                    string part2 = myReader.GetString("seat_number_p2");
                    //string part2 = (string)myReader["seat_number_p2"];
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.ResetText();
            comboBox4.ResetText();
            fillComboBoxSeat();           
        }
        

    }
}

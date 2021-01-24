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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root;password=1234";
            string email = textBox1.Text;
            string Query = "use airline_data; SELECT departure_airport, destination_airport, departure_date, arrival_date, ticket_number " +
                "FROM (Flight JOIN Ticket ON Flight.flight_number = Ticket.flight_number) JOIN Passenger ON Ticket.passenger_id = Passenger.passenger_id WHERE email_address = '" + email + "';";
            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;
            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();

                while (myReader.Read())
                {

                    listBox1.Items.Add("From: "+ myReader["departure_airport"] + " To: " + myReader["destination_airport"] + " At: " + myReader["departure_date"] + myReader["arrival_date"] + " - " + myReader["ticket_number"]);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Bunifu.Framework.UI;
using Guna.UI2.WinForms;

namespace ProjectDesktop_app_Logement_
{
    public partial class Form1 : Form
    {
        public object JsonSerializer { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private async void Seconnecter_Click(object sender, EventArgs e)
        {
            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Define the API endpoint URL
                string url = "https://localhost:7194/api/login";

                // Define the request parameters as a dictionary
                var data = new Dictionary<string, string>
                {
                    { "email", email.Text },
                    { "password", Password.Text }
                };

                // Serialize the dictionary to a JSON string
                string jsonData = JsonConvert.SerializeObject(data);

                // Create a new StringContent object with the JSON string and the "application/json" media type
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the POST request to the API endpoint
                HttpResponseMessage response = await client.PostAsync(url, content);
                // Check if the response was successful
                if (response.IsSuccessStatusCode)
                {
                    // Get the response content as a string
                    string result = await response.Content.ReadAsStringAsync();

                    // Display the result in a message box  

                    MessageBox.Show("Connexion réussie!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open the next form if the authentication is successful
                    Form3 form3 = new Form3(result);
                    Hide();
                    form3.Show();
                }
                else
                {
                    // Display an error message if the authentication fails
                    MessageBox.Show("Authentication failed. Please check your email and password and try again.");
                    
                }
            }
        }


        private void Password_TextChanged(object sender, EventArgs e)
        {

        }

        private void email_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.imageHover;
            pictureBox1.Cursor = Cursors.Hand;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.normalImage;
            pictureBox1.Cursor = Cursors.Default;
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            Hide();
            form4.Show();
        }
    }
}

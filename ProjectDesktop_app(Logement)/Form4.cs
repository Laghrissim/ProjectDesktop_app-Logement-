using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDesktop_app_Logement_
{
    public partial class Form4 : Form
    {
        private string selectedImagePath= "C:/Users/HP/OneDrive/Bureau/Videos .Net/imagescrypto/user-2935527.png";
        public Form4()
        {
            InitializeComponent();
        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
        // Event handler for the button click event
        private void VerifyEmailAndPassword()
        {
            string email = guna2TextBox2.Text;
            string password = guna2TextBox3.Text;
            string confirmPassword = guna2TextBox4.Text;

            // Verify email pattern using regular expression
            if (!VerifyEmailPattern(email))
            {
                MessageBox.Show("Invalid email format. Please enter a valid email address.");
                return;
            }

            // Compare password and password confirmation
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please make sure the passwords match.");
                return;
            }

            // Email and password validation passed
            // Proceed with further actions or validations
            // ...
        }

        // Verify email pattern using regular expression
        private bool VerifyEmailPattern(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Create a regular expression object
            Regex regex = new Regex(pattern);

            // Check if the email matches the pattern
            return regex.IsMatch(email);
        }

        private async void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            VerifyEmailAndPassword();
            string name = guna2TextBox1.Text; // Replace with the actual name
            string email = guna2TextBox2.Text; // Replace with the actual email
            string password=guna2TextBox3.Text; // Replace with the actual password
            byte[] userImageBytes = null;
            using (FileStream fileStream = new FileStream(selectedImagePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    userImageBytes = reader.ReadBytes((int)fileStream.Length);
                }
            }

            // Create the request payload as an anonymous object
            var payload = new
            {
                name = name,
                email = email,
                password = password,
                Image = Convert.ToBase64String(userImageBytes) // Convert the byte array to a base64 string
            };

            // Serialize the payload to a JSON string
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Define the API endpoint URL
                string url = "https://localhost:7194/api/register";

                // Create a new StringContent object with the JSON string and the "application/json" media type
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    // Send the POST request to the API endpoint
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Check if the response was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Registration successful
                        // You can handle the success case here
                        MessageBox.Show("Inscription réussie!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1 form3 = new Form1();
                        Hide();
                        form3.Show();
                    }
                    else
                    {
                        // Registration failed
                        // You can handle the failure case here
                        MessageBox.Show("Inscription non valide!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Exception occurred during the request
                    // You can handle the exception case here
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            Form1 form3 = new Form1();
            Hide();
            form3.Show();
        }
       
        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;
                guna2CirclePictureBox1.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;
                guna2CirclePictureBox1.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void guna2CirclePictureBox1_MouseHover(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.BackColor = System.Drawing.Color.Transparent;
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.imageHover;
            pictureBox1.Cursor = Cursors.Hand;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

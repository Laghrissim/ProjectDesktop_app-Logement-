using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Bunifu.Framework.UI;
using System.Net.Http;

namespace ProjectDesktop_app_Logement_
{
   
    public partial class Form5 : Form

    {
        private string selectedImagePath = "C:/Users/HP/OneDrive/Bureau/Videos .Net/imagescrypto/big-house-13.png";
        public Form5()
        {
            InitializeComponent();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;
                guna2PictureBox1.Image = Image.FromFile(selectedImagePath);
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
                guna2PictureBox1.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel7_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {

        }


        //Map
    
        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private string GetSelectedDropdownItem(BunifuDropdown dropdown)
        {
            return dropdown.Text;
        }





        private async void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            Hide();
            form3.Show();
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

        private void bunifuCustomLabel6_MouseHover(object sender, EventArgs e)
        {
            bunifuCustomLabel6.ForeColor = Color.Gray;
        }

        private void bunifuCustomLabel6_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            Hide();
            form3.Show();
        }

        private void bunifuThinButton22_Click_1(object sender, EventArgs e)
        {

            Form3 form3 = new Form3();
            Hide();
            form3.Show();

        }

        private async void bunifuThinButton21_Click_1(object sender, EventArgs e)
        {
            // Get the input values

            string title = guna2TextBox1.Text;
            string description = guna2TextBox2.Text;
            string category = GetSelectedDropdownItem(bunifuDropdown1);
            string prix = guna2TextBox3.Text;
            string roomCount = GetSelectedDropdownItem(bunifuDropdown2);
            string bathroomCount = GetSelectedDropdownItem(bunifuDropdown3);
            string guestCount = GetSelectedDropdownItem(bunifuDropdown4);
            // Replace with the actual path
            string location = guna2TextBox4.Text;

            // Create the request payload as an anonymous object
            var payload = new
            {
                title = title,
                description = description,
                imageSrc = selectedImagePath,
                createdAt = DateTime.Now,
                category = category,
                bathroomCount = bathroomCount,
                roomCount = roomCount,
                guestCount = guestCount,
                locationValue = "string", // Replace with the actual location value
                userId = new { timestamp = 0 }, // Replace with the actual user ID
                price = prix
            };

            // Serialize the payload to a JSON string
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Define the API endpoint URL
                string url = "https://localhost:7194/Listings";

                // Create a new StringContent object with the JSON string and the "application/json" media type
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    // Send the POST request to the API endpoint
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Check if the response was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Offer added successfully
                        // You can handle the success case here
                        Console.WriteLine("Offer added successfully!");
                    }
                    else
                    {
                        // Offer addition failed
                        // You can handle the failure case here
                        Console.WriteLine("Failed to add offer. Error: " + response.StatusCode);
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

        private void guna2PictureBox1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;
                guna2PictureBox1.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void bunifuThinButton23_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;
                guna2PictureBox1.Image = Image.FromFile(selectedImagePath);
            }
        }
    }
   }


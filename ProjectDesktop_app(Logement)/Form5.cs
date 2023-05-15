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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Configuration;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using MongoDB.Bson;

namespace ProjectDesktop_app_Logement_
{
   
    public partial class Form5 : Form

    {   private readonly IConfiguration _configuration;
        private string selectedImagePath = "C:/Users/HP/OneDrive/Bureau/Videos .Net/imagescrypto/big-house-13.png";
        private string token;
        public Form5()
        {
            InitializeComponent();

        }
        public Form5(string token)
        {
            InitializeComponent();
            this.token = token;
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

            string titleText = guna2TextBox1.Text;
            string descriptionText = guna2TextBox2.Text;
            string categoryText = bunifuDropdown1.selectedValue.ToString();
            int prixText = int.Parse( guna2TextBox3.Text);
            string roomCount = bunifuDropdown2.selectedValue.ToString();
            string bathroomCount = bunifuDropdown3.selectedValue.ToString();
            string guestCount = bunifuDropdown4.selectedValue.ToString();
            // Replace with the actual path
            string locationText = guna2TextBox4.Text;
            byte[] userImageBytes = null;
            using (FileStream fileStream = new FileStream(selectedImagePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    userImageBytes = reader.ReadBytes((int)fileStream.Length);
                }
            }



            int roomCountText;
            int bathroomCountText;
            int guestCountText;

            // Method 1: Using int.Parse()
            bool roomCountSuccess = int.TryParse(roomCount, out roomCountText);
            bool bathroomCountSuccess = int.TryParse(bathroomCount, out bathroomCountText);
            bool guestCountSuccess = int.TryParse(guestCount, out guestCountText);

           
            var basePath = "C:/Users/HP/source/repos/ProjectDesktop_app(Logement)/ProjectDesktop_app(Logement)/";
            // Inject IConfiguration into your class or retrieve it from the DI container
            var configuration = new ConfigurationBuilder()
      .SetBasePath(basePath)
      .AddJsonFile("appsettings.json")
      .Build();

            // Retrieve the Jwt:Secret value from configuration
            var jwtSecret = configuration["Jwt:Secret"];

            // Use jwtSecret in your code
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            string tokenJson = token;
            JObject tokenObject = JObject.Parse(tokenJson);
            string jwtToken = tokenObject.Value<string>("token");

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);
            var userIdText = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
            ObjectId userIdValue;
            bool parseSuccess = ObjectId.TryParse(userIdText, out userIdValue);

            if (!parseSuccess)
            {
                // Handle the case where the parsing fails
                MessageBox.Show("Failed to parse userIdText as ObjectId.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // or handle the error accordingly
            }


            // Create the request payload as an anonymous object
            var payload = new
            {
                
                title = titleText,
                description = descriptionText,
                imageSrc = Convert.ToBase64String(userImageBytes),
                createdAt = DateTime.Now,
                category = categoryText,
                bathroomCount = bathroomCountText,
                roomCount = roomCountText,
                guestCount = guestCountText,
                locationValue = locationText, // Replace with the actual location value
                // Replace with the actual user ID
                price = prixText,
                
            };




            // Serialize the payload to a JSON string
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Define the API endpoint URL
                string url = "https://localhost:7194/Listings";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

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
                        MessageBox.Show("Ajout réussie!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Offer addition failed
                        // You can handle the failure case here
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(errorMessage);
                        Console.Write(errorMessage);
                        MessageBox.Show("Failed to add offer. Error: " + errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
   }


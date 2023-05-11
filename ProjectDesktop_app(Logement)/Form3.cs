using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net.Http.Headers;

namespace ProjectDesktop_app_Logement_
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.Load += Form_Load;
        }

        private void guna2HtmlLabel6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.imageHover;
            pictureBox1.Cursor = Cursors.Hand;
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.normalImage;
            pictureBox1.Cursor = Cursors.Default;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void bunifuCards1_Paint(object sender, PaintEventArgs e)
        {

        }


        // ...

        private async Task<IEnumerable<Listing>> GetListings()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://localhost:7194/Listings";
                client.DefaultRequestHeaders.Add("accept", "text/plain");

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                IEnumerable<Listing> listings = JsonConvert.DeserializeObject<IEnumerable<Listing>>(responseBody);

                return listings;
            }
        }

        private async Task PopulateListings()
        {
            IEnumerable<Listing> listings = await GetListings();

            foreach (Listing listing in listings)
            {
                // Create an instance of the bunifuCards1 control
                Bunifu.Framework.UI.BunifuCards card = new Bunifu.Framework.UI.BunifuCards();
                card.Size = new System.Drawing.Size(300, 350); // Adjust the size as needed
                card.BackColor = System.Drawing.Color.White; // Set the background color
                card.Margin = new Padding(30);

                // Create a TableLayoutPanel for the card content
                TableLayoutPanel contentPanel = new TableLayoutPanel();
                contentPanel.Dock = DockStyle.Fill; // Fill the remaining space
                card.Controls.Add(contentPanel);

                // Create a PictureBox control and set its properties
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new System.Drawing.Size(300, 150); // Adjust the size as needed
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                // Download the webp image from the URL
                using (WebClient webClient = new WebClient())
                {
                    byte[] imageData = webClient.DownloadData(listing.imageSrc);

                    // Load the webp image using ImageSharp
                    using (MemoryStream memoryStream = new MemoryStream(imageData))
                    {
                        using (SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(memoryStream.ToArray()))
                        {
                            // Convert ImageSharp image to System.Drawing.Image
                            using (var memoryStream2 = new MemoryStream())
                            {
                                image.Save(memoryStream2, new SixLabors.ImageSharp.Formats.Png.PngEncoder());

                                System.Drawing.Image convertedImage = System.Drawing.Image.FromStream(memoryStream2);

                                // Set the image in the PictureBox
                                pictureBox.Image = convertedImage;
                            }
                        }
                    }
                }


                pictureBox.Padding = new Padding(5);
                contentPanel.Controls.Add(pictureBox, 0, 0); // Add to the first row

                // Create a Label control for the title and set its properties
                Label titleLabel = new Label();
                titleLabel.Text = listing.title;
                titleLabel.AutoSize = true; // Adjust width based on content
                titleLabel.Font = new Font("Yu Gothic UI", 12, FontStyle.Bold); // Set the font and style
                titleLabel.Anchor = AnchorStyles.Top; // Anchor to the top
                titleLabel.TextAlign = ContentAlignment.MiddleCenter; // Center the text horizontally
                titleLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(titleLabel, 0, 1); // Add to the second row

                // Create a Label control for the description and set its properties
                Label descriptionLabel = new Label();
                descriptionLabel.Text = listing.description;
                descriptionLabel.AutoSize = true; // Adjust width based on content
                descriptionLabel.Font = new Font("Yu Gothic UI", 10); // Set the font
                descriptionLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(descriptionLabel, 0, 2); // Add to the third row

                // Create a Label control for the price and set its properties
                Label priceLabel = new Label();
                priceLabel.Text = "Price " + listing.price.ToString("0.00")+ " $"; // Format the price
                priceLabel.AutoSize = true; // Adjust width based on content
                priceLabel.Font = new Font("Yu Gothic UI", 10,FontStyle.Bold); // Set the font
                priceLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(priceLabel, 0, 3); // Add to the fourth row


                // Create a Label control for the address and set its properties
                Label addressLabel = new Label();
                addressLabel.Text = listing.category;
                addressLabel.AutoSize = true; // Adjust width based on content
                addressLabel.Font = new Font("Yu Gothic UI", 10); // Set the font
                addressLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(addressLabel, 0, 4); // Add to the fifth row

                // Adjust the height of the card based on the last control's bottom position
                card.Height = addressLabel.Bottom + 10;

                // Add the card to the FlowLayoutPanel
                flowLayoutPanel1.Controls.Add(card);

            }
        }

        private async Task SelectByCategory(string category)
        {
            using (var client = new HttpClient())
            {
                // Add authentication headers if required by the API
                // Replace "YourAuthenticationToken" with the actual token or credentials
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourAuthenticationToken");

                // Make the GET request to the correct URL for selecting listings
                var response = await client.GetAsync($"https://localhost:7194/Listings?category={category}");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse the response content into an object
                    flowLayoutPanel1.Controls.Clear();
                    IEnumerable<Listing> listings = JsonConvert.DeserializeObject<IEnumerable<Listing>>(responseContent);
                    foreach (dynamic listing in listings)
                    {
                        if(listing.category == category) { 
                        // Create an instance of the bunifuCards1 control
                        Bunifu.Framework.UI.BunifuCards card = new Bunifu.Framework.UI.BunifuCards();
                        card.Size = new System.Drawing.Size(320, 350); // Adjust the size as needed
                        card.BackColor = System.Drawing.Color.White; // Set the background color
                        card.Margin = new Padding(30);

                        // Create a TableLayoutPanel for the card content
                        TableLayoutPanel contentPanel = new TableLayoutPanel();
                        contentPanel.Dock = DockStyle.Fill; // Fill the remaining space
                        card.Controls.Add(contentPanel);

                        // Create a PictureBox control and set its properties
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Size = new System.Drawing.Size(300, 150); // Adjust the size as needed
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        // Download the webp image from the URL
                        using (WebClient webClient = new WebClient())
                        {
                            byte[] imageData = webClient.DownloadData(listing.imageSrc);

                            // Load the webp image using ImageSharp
                            using (MemoryStream memoryStream = new MemoryStream(imageData))
                            {
                                using (SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(memoryStream.ToArray()))
                                {
                                    // Convert ImageSharp image to System.Drawing.Image
                                    using (var memoryStream2 = new MemoryStream())
                                    {
                                        image.Save(memoryStream2, new SixLabors.ImageSharp.Formats.Png.PngEncoder());

                                        System.Drawing.Image convertedImage = System.Drawing.Image.FromStream(memoryStream2);

                                        // Set the image in the PictureBox
                                        pictureBox.Image = convertedImage;
                                    }
                                }
                            }
                        }


                        pictureBox.Padding = new Padding(5);
                        contentPanel.Controls.Add(pictureBox, 0, 0); // Add to the first row

                        // Create a Label control for the title and set its properties
                        Label titleLabel = new Label();
                        titleLabel.Text = listing.title;
                        titleLabel.AutoSize = true; // Adjust width based on content
                        titleLabel.Font = new Font("Yu Gothic UI", 12, FontStyle.Bold); // Set the font and style
                        titleLabel.Anchor = AnchorStyles.Top; // Anchor to the top
                            // Center the text horizontally
                        titleLabel.Padding = new Padding(5);
                        contentPanel.Controls.Add(titleLabel, 0, 1); // Add to the second row

                        // Create a Label control for the description and set its properties
                        Label descriptionLabel = new Label();
                        descriptionLabel.Text = listing.description;
                        //descriptionLabel.AutoScroll = true; // Adjust width based on content
                        descriptionLabel.Font = new Font("Yu Gothic UI", 10); // Set the font
                        descriptionLabel.Padding = new Padding(5);
                        contentPanel.Controls.Add(descriptionLabel, 0, 2); // Add to the third row

                        // Create a Label control for the price and set its properties
                        Label priceLabel = new Label();
                        priceLabel.Text = "Price " + listing.price.ToString("0.00") + " $"; // Format the price
                        priceLabel.AutoSize = true; // Adjust width based on content
                        priceLabel.Font = new Font("Yu Gothic UI", 10, FontStyle.Bold); // Set the font
                        priceLabel.Padding = new Padding(5);
                        contentPanel.Controls.Add(priceLabel, 0, 3); // Add to the fourth row


                        // Create a Label control for the address and set its properties
                        Label addressLabel = new Label();
                        addressLabel.Text = listing.category;
                        addressLabel.AutoSize = true; // Adjust width based on content
                        addressLabel.Font = new Font("Yu Gothic UI", 10); // Set the font
                        addressLabel.Padding = new Padding(5);
                        contentPanel.Controls.Add(addressLabel, 0, 4); // Add to the fifth row

                        // Adjust the height of the card based on the last control's bottom position
                        card.Height = addressLabel.Bottom + 10;

                        // Add the card to the FlowLayoutPanel
                        flowLayoutPanel1.Controls.Add(card);
                        }
                    }
                    // Process the response object as needed
                    // For example, you can update the UI or display a message
                    MessageBox.Show("Category selected and GET request successful!");
                }
                else
                {
                    // Display an error message if the request fails
                    MessageBox.Show("Error: GET request failed!");
                }
            }
        }



        private async void Form_Load(object sender, EventArgs e)
        {
            await PopulateListings();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void PictureBox7_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.imageHover;
            pictureBox1.Cursor = Cursors.Default;
        }

        private void PictureBox7_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.normalImage;
            pictureBox1.Cursor = Cursors.Default;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void pictureBox7_MouseEnter_1(object sender, EventArgs e)
        {
            pictureBox7.Image = Properties.Resources.imageHover;
            pictureBox7.Cursor = Cursors.Hand;
        }

        private void pictureBox7_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox7.Image = Properties.Resources.normalImage;
            pictureBox7.Cursor = Cursors.Hand;
        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private async void pictureBox2_ClickAsync(object sender, EventArgs e)
        {
            await SelectByCategory("Modern");
        }

        private async void label1_Click(object sender, EventArgs e)
        {
            await SelectByCategory("Modern");
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox2.BackColor = System.Drawing.Color.Gray;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BorderStyle = BorderStyle.None;
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.ForeColor = System.Drawing.Color.Gray;
        }
        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = System.Drawing.Color.Black;
        }

       

  

       

        

   
        

        private async void pictureBox8_Click(object sender, EventArgs e)
        {
            await SelectByCategory("Beach");
        }

        private async void label2_Click_1(object sender, EventArgs e)
        {
            await SelectByCategory("Beach");
        }

        private void pictureBox8_MouseEnter_1(object sender, EventArgs e)
        {
            pictureBox8.BorderStyle = BorderStyle.FixedSingle;
            pictureBox8.BackColor = System.Drawing.Color.Gray;
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {
            pictureBox8.BorderStyle = BorderStyle.None;
            pictureBox8.BackColor = System.Drawing.Color.Transparent;
        }

        private void label2_Enter(object sender, EventArgs e)
        {
            label2.ForeColor = System.Drawing.Color.Gray;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = System.Drawing.Color.Black;
        }
    }
}

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
using Color = System.Drawing.Color;
using System.Web.UI;
using Control = System.Windows.Forms.Control;
using Newtonsoft.Json.Linq;
using Bunifu.Framework.UI;
using System.Web.UI.WebControls;
using Button = System.Windows.Forms.Button;
using Label = System.Windows.Forms.Label;
using BorderStyle = System.Windows.Forms.BorderStyle;
using System.Linq;
using TextBox = System.Windows.Forms.TextBox;

namespace ProjectDesktop_app_Logement_
{
    
    public partial class Form3 : Form
    {
        private string result;
        private object contentPanel1;
        


        public Form3(string result)
        {
            InitializeComponent();
            this.result = result;
            this.Load += Form_Load;
            ApplyMouseEventsToPanels(flowLayoutPanel2);
            ApplyMouseEventsToPanels2(flowLayoutPanel18);
            ApplyMouseEventsToPanels2(guna2CustomGradientPanel1);
        }
        public Form3()
        {
            InitializeComponent();
            this.Load += Form_Load;
            ApplyMouseEventsToPanels(flowLayoutPanel2);
            ApplyMouseEventsToPanels2(flowLayoutPanel18);
            ApplyMouseEventsToPanels2(guna2CustomGradientPanel1);
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

        private async void label7_Click(object sender, EventArgs e)
        {
            await PopulateListings();
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void bunifuCards1_Paint(object sender, PaintEventArgs e)
        {

        }


        // ...

        private async Task<IEnumerable<Listing>> GetListings2()
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

        private async Task<IEnumerable<Listing>> GetListings2(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://localhost:7194/Listings/MesListings";
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.result}");
                client.DefaultRequestHeaders.Add("accept", "application/json");

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                IEnumerable<Listing> listings = JsonConvert.DeserializeObject<IEnumerable<Listing>>(responseBody);

                return listings;
            }
        }


        private async Task PopulateListings()
        {
            IEnumerable<Listing> listings = await GetListings2();
            flowLayoutPanel1.Controls.Clear();
            foreach (Listing listing in listings)
            {
                // Create an instance of the bunifuCards1 control
                Bunifu.Framework.UI.BunifuCards card = new Bunifu.Framework.UI.BunifuCards();
                card.Size = new System.Drawing.Size(300, 850); // Adjust the size as needed
                
                card.BackColor = System.Drawing.Color.White; // Set the background color
                card.Margin = new Padding(30);

                // Create a TableLayoutPanel for the card content
                TableLayoutPanel contentPanel = new TableLayoutPanel();
                contentPanel.Dock = DockStyle.Fill; // Fill the remaining space
                contentPanel.ColumnCount = 2;
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
                titleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left; ; // Anchor to the top
                // Center the text horizontally
                titleLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(titleLabel, 0, 1); // Add to the second row

                Label descriptionLabel = new Label();
                descriptionLabel.Text = listing.description;
                descriptionLabel.AutoSize = false;
                descriptionLabel.Font = new Font("Yu Gothic UI", 10);
                descriptionLabel.Padding = new Padding(5);

                // Measure the width of the text
                int textWidth = TextRenderer.MeasureText(descriptionLabel.Text, descriptionLabel.Font).Width;

                // Set the width of the label to accommodate the text
                descriptionLabel.Width = textWidth;

                // Check if the width exceeds the threshold
                if (textWidth > 0)
                {
                    descriptionLabel.AutoSize = true; // Enable auto-size to handle line breaks
                    descriptionLabel.MaximumSize = new System.Drawing.Size(300, int.MaxValue); // Set the maximum width to 320
                }

                contentPanel.Controls.Add(descriptionLabel, 0, 2);

                


                // Create a Label control for the address and set its properties
                Label addressLabel1 = new Label();
                addressLabel1.Text = listing.locationValue;
                addressLabel1.AutoSize = true; // Adjust width based on content
                addressLabel1.Font = new Font("Yu Gothic UI", 10); // Set the font
                addressLabel1.Padding = new Padding(5);
                contentPanel.Controls.Add(addressLabel1, 0, 3); // Add to the fifth row




                addressLabel1 = new Label();
                addressLabel1.AutoSize = true;
                addressLabel1.Location = new System.Drawing.Point(10, 10); // Adjust the location as needed
                contentPanel.Controls.Add(addressLabel1);

                

                // Initialize the add to favorites button
                

                // Create a Label control for the price and set its properties
                Label priceLabel = new Label();
                priceLabel.Text = "Price " + listing.price.ToString("0.00") + " $"; // Format the price
                priceLabel.AutoSize = true; // Adjust width based on content
                priceLabel.Font = new Font("Yu Gothic UI", 10, FontStyle.Bold); // Set the font
                priceLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(priceLabel, 0, 4);


                // Create a Label control for the address and set its properties
                Label addressLabel = new Label();
                addressLabel.Text = listing.category;
                addressLabel.AutoSize = true; // Adjust width based on content
                addressLabel.Font = new Font("Yu Gothic UI", 10); // Set the font
                addressLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(addressLabel, 0,5); // Add to the fifth row
                Button addToFavoritesButton;
                 Button reservationButton;
        addToFavoritesButton = new Button();
                addToFavoritesButton.Text = "Add to Favorites";
                addToFavoritesButton.Width= 100;
                addToFavoritesButton.BackColor = Color.White;
                addToFavoritesButton.ForeColor = Color.DeepPink;
                addToFavoritesButton.Location = new System.Drawing.Point(10, 40);
                addToFavoritesButton.Tag = listing.id;
                addToFavoritesButton.Click += (sender, e) => AddToFavoritesButton_Click(sender, e, addToFavoritesButton);
                contentPanel.Controls.Add(addToFavoritesButton,0,6);

                // Initialize the reservation button
                reservationButton = new Button();
                reservationButton.Width = 270;
                reservationButton.Text = "Make Reservation";
                reservationButton.Location = new System.Drawing.Point(10, 70); // Adjust the location as needed
                reservationButton.Click += (sender, e) => ReservationButton_Click(sender, e, addToFavoritesButton, listing);
                contentPanel.Controls.Add(reservationButton,0,7);




                // Adjust the height of the card based on the last control's bottom position
                card.Height = reservationButton.Bottom + 10;

                // Add the card to the FlowLayoutPanel
                flowLayoutPanel1.Controls.Add(card);

            }
        }







        private async Task PopulateListings2()
        {
            IEnumerable<Listing> listings = await GetListings2();
            flowLayoutPanel1.Controls.Clear();
            foreach (Listing listing in listings)
            {
                // Create an instance of the bunifuCards1 control
                Bunifu.Framework.UI.BunifuCards card = new Bunifu.Framework.UI.BunifuCards();
                card.Size = new System.Drawing.Size(300, 850); // Adjust the size as needed

                card.BackColor = System.Drawing.Color.White; // Set the background color
                card.Margin = new Padding(30);

                // Create a TableLayoutPanel for the card content
                TableLayoutPanel contentPanel = new TableLayoutPanel();
                contentPanel.Dock = DockStyle.Fill; // Fill the remaining space
                contentPanel.ColumnCount = 2;
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
                titleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left; ; // Anchor to the top
                // Center the text horizontally
                titleLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(titleLabel, 0, 1); // Add to the second row

                Label descriptionLabel = new Label();
                descriptionLabel.Text = listing.description;
                descriptionLabel.AutoSize = false;
                descriptionLabel.Font = new Font("Yu Gothic UI", 10);
                descriptionLabel.Padding = new Padding(5);

                // Measure the width of the text
                int textWidth = TextRenderer.MeasureText(descriptionLabel.Text, descriptionLabel.Font).Width;

                // Set the width of the label to accommodate the text
                descriptionLabel.Width = textWidth;

                // Check if the width exceeds the threshold
                if (textWidth > 0)
                {
                    descriptionLabel.AutoSize = true; // Enable auto-size to handle line breaks
                    descriptionLabel.MaximumSize = new System.Drawing.Size(300, int.MaxValue); // Set the maximum width to 320
                }

                contentPanel.Controls.Add(descriptionLabel, 0, 2);




                // Create a Label control for the address and set its properties
                Label addressLabel1 = new Label();
                addressLabel1.Text = listing.locationValue;
                addressLabel1.AutoSize = true; // Adjust width based on content
                addressLabel1.Font = new Font("Yu Gothic UI", 10); // Set the font
                addressLabel1.Padding = new Padding(5);
                contentPanel.Controls.Add(addressLabel1, 0, 3); // Add to the fifth row




                addressLabel1 = new Label();
                addressLabel1.AutoSize = true;
                addressLabel1.Location = new System.Drawing.Point(10, 10); // Adjust the location as needed
                contentPanel.Controls.Add(addressLabel1);



                // Initialize the add to favorites button


                // Create a Label control for the price and set its properties
                Label priceLabel = new Label();
                priceLabel.Text = "Price " + listing.price.ToString("0.00") + " $"; // Format the price
                priceLabel.AutoSize = true; // Adjust width based on content
                priceLabel.Font = new Font("Yu Gothic UI", 10, FontStyle.Bold); // Set the font
                priceLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(priceLabel, 0, 4);


                // Create a Label control for the address and set its properties
                Label addressLabel = new Label();
                addressLabel.Text = listing.category;
                addressLabel.AutoSize = true; // Adjust width based on content
                addressLabel.Font = new Font("Yu Gothic UI", 10); // Set the font
                addressLabel.Padding = new Padding(5);
                contentPanel.Controls.Add(addressLabel, 0, 5); // Add to the fifth row
                Button addToFavoritesButton;
                Button reservationButton;
                addToFavoritesButton = new Button();
                addToFavoritesButton.Text = "Add to Favorites";
                addToFavoritesButton.Width = 100;
                addToFavoritesButton.BackColor = Color.White;
                addToFavoritesButton.ForeColor = Color.DeepPink;
                addToFavoritesButton.Location = new System.Drawing.Point(10, 40);
                addToFavoritesButton.Tag = listing.id;
                addToFavoritesButton.Click += (sender, e) => AddToFavoritesButton_Click(sender, e, addToFavoritesButton);
                contentPanel.Controls.Add(addToFavoritesButton, 0, 6);

                // Initialize the reservation button
                reservationButton = new Button();
                reservationButton.Width = 270;
                reservationButton.Text = "Make Reservation";
                reservationButton.Location = new System.Drawing.Point(10, 70); // Adjust the location as needed
                reservationButton.Click += (sender, e) => ReservationButton_Click(sender, e, addToFavoritesButton, listing);
                contentPanel.Controls.Add(reservationButton, 0, 7);




                // Adjust the height of the card based on the last control's bottom position
                card.Height = reservationButton.Bottom + 10;

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
                            card.Size = new System.Drawing.Size(300, 850); // Adjust the size as needed

                            card.BackColor = System.Drawing.Color.White; // Set the background color
                            card.Margin = new Padding(30);

                            // Create a TableLayoutPanel for the card content
                            TableLayoutPanel contentPanel = new TableLayoutPanel();
                            contentPanel.Dock = DockStyle.Fill; // Fill the remaining space
                            contentPanel.ColumnCount = 2;
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
                            titleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left; ; // Anchor to the top
                                                                                        // Center the text horizontally
                            titleLabel.Padding = new Padding(5);
                            contentPanel.Controls.Add(titleLabel, 0, 1); // Add to the second row

                            Label descriptionLabel = new Label();
                            descriptionLabel.Text = listing.description;
                            descriptionLabel.AutoSize = false;
                            descriptionLabel.Font = new Font("Yu Gothic UI", 10);
                            descriptionLabel.Padding = new Padding(5);

                            // Measure the width of the text
                            int textWidth = TextRenderer.MeasureText(descriptionLabel.Text, descriptionLabel.Font).Width;

                            // Set the width of the label to accommodate the text
                            descriptionLabel.Width = textWidth;

                            // Check if the width exceeds the threshold
                            if (textWidth > 0)
                            {
                                descriptionLabel.AutoSize = true; // Enable auto-size to handle line breaks
                                descriptionLabel.MaximumSize = new System.Drawing.Size(300, int.MaxValue); // Set the maximum width to 320
                            }

                            contentPanel.Controls.Add(descriptionLabel, 0, 2);




                            // Create a Label control for the address and set its properties
                            Label addressLabel1 = new Label();
                            addressLabel1.Text = listing.locationValue;
                            addressLabel1.AutoSize = true; // Adjust width based on content
                            addressLabel1.Font = new Font("Yu Gothic UI", 10); // Set the font
                            addressLabel1.Padding = new Padding(5);
                            contentPanel.Controls.Add(addressLabel1, 0, 3); // Add to the fifth row




                            addressLabel1 = new Label();
                            addressLabel1.AutoSize = true;
                            addressLabel1.Location = new System.Drawing.Point(10, 10); // Adjust the location as needed
                            contentPanel.Controls.Add(addressLabel1);



                            // Initialize the add to favorites button


                            // Create a Label control for the price and set its properties
                            Label priceLabel = new Label();
                            priceLabel.Text = "Price " + listing.price.ToString("0.00") + " $"; // Format the price
                            priceLabel.AutoSize = true; // Adjust width based on content
                            priceLabel.Font = new Font("Yu Gothic UI", 10, FontStyle.Bold); // Set the font
                            priceLabel.Padding = new Padding(5);
                            contentPanel.Controls.Add(priceLabel, 0, 4);


                            // Create a Label control for the address and set its properties
                            Label addressLabel = new Label();
                            addressLabel.Text = listing.category;
                            addressLabel.AutoSize = true; // Adjust width based on content
                            addressLabel.Font = new Font("Yu Gothic UI", 10); // Set the font
                            addressLabel.Padding = new Padding(5);
                            contentPanel.Controls.Add(addressLabel, 0, 5); // Add to the fifth row
                            Button addToFavoritesButton;
                            Button reservationButton;
                            addToFavoritesButton = new Button();
                            addToFavoritesButton.Text = "Add to Favorites";
                            addToFavoritesButton.Width = 100;
                            addToFavoritesButton.BackColor = Color.White;
                            addToFavoritesButton.ForeColor = Color.DeepPink;
                            addToFavoritesButton.Location = new System.Drawing.Point(10, 40);
                            addToFavoritesButton.Tag = listing.id;
                            addToFavoritesButton.Click += (sender, e) => AddToFavoritesButton_Click(sender, e, addToFavoritesButton);
                            contentPanel.Controls.Add(addToFavoritesButton, 0, 6);

                            // Initialize the reservation button
                            reservationButton = new Button();
                            reservationButton.Width = 270;
                            reservationButton.Text = "Make Reservation";
                            reservationButton.Location = new System.Drawing.Point(10, 70); // Adjust the location as needed
                            reservationButton.Click += (sender, e) => ReservationButton_Click(sender, e, addToFavoritesButton, listing);
                            contentPanel.Controls.Add(reservationButton, 0, 7);




                            // Adjust the height of the card based on the last control's bottom position
                            card.Height = reservationButton.Bottom + 10;

                            // Add the card to the FlowLayoutPanel
                            flowLayoutPanel1.Controls.Add(card);
                        }
                    }
                    // Process the response object as needed
                    // For example, you can update the UI or display a message
                    
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

        

       

   
        

  

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await PopulateListings();
        }




        private void ApplyMouseEventsToControls(FlowLayoutPanel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Click += pictureBox_ClickAsync;
                    pictureBox.MouseEnter += pictureBox_MouseEnter;
                    pictureBox.MouseLeave += pictureBox_MouseLeave;
                }
                else if (control is Label label)
                {
                    label.Click += label_ClickAsync;
                    label.MouseEnter += label_MouseEnter;
                    label.MouseLeave += label_MouseLeave;
                }
            }
        }

        private async void pictureBox_ClickAsync(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            if (pictureBox.Tag != null)
            {
                await SelectByCategory(pictureBox.Tag.ToString());
            }
        }

        private async void label_ClickAsync(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (label.Tag != null) { 
            await SelectByCategory(label.Tag.ToString());
            }
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.BackColor = System.Drawing.Color.Gray;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BorderStyle = BorderStyle.None;
            pictureBox.BackColor = System.Drawing.Color.Transparent;
        }

        private void label_MouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = Color.Gray;
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = Color.Black;
        }


        private void ApplyMouseEventsToPanels(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                if (childControl is PictureBox pictureBox)
                {
                    pictureBox.Click += pictureBox_ClickAsync;
                    pictureBox.MouseHover += pictureBox_MouseEnter;
                    pictureBox.MouseLeave += pictureBox_MouseLeave;
                    foreach (Control childControl1 in childControl.Controls)
                    {
                        if (childControl1 is Label label)
                        {
                            if (pictureBox.Tag.ToString()==label.Tag.ToString())
                            { 
                             label.Click += label_ClickAsync;
                            label.MouseHover += label_MouseEnter;
                            label.MouseLeave += label_MouseLeave;
                            }
                        }

                    }


                }
                else if (childControl is Label label)
                {
                    label.Click += label_ClickAsync;
                    label.MouseHover += label_MouseEnter;
                    label.MouseLeave += label_MouseLeave;
                    foreach (Control childControl1 in childControl.Controls)
                    {
                        if (childControl1 is PictureBox pictureBox1)
                        {
                            if (pictureBox1.Tag.ToString() == label.Tag.ToString())
                            {
                                label.Click += label_ClickAsync;
                                label.MouseHover += label_MouseEnter;
                                label.MouseLeave += label_MouseLeave;
                            }
                        }

                    }
                }
                else if (childControl is FlowLayoutPanel flowLayoutPanel)
                {
                    ApplyMouseEventsToPanels(flowLayoutPanel); // Recursively apply mouse events to inner FlowLayoutPanel controls
                }
            }
        }




        private void ApplyMouseEventsToPanels2(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                if (childControl is PictureBox pictureBox)
                {
                    pictureBox.MouseHover += pictureBox_MouseEnter2;
                    pictureBox.MouseLeave += pictureBox_MouseLeave2;
                    foreach (Control childControl1 in childControl.Controls)
                    {
                        if (childControl1 is Label label)
                        {
                            if (pictureBox.Tag.ToString() == label.Tag.ToString())
                            {
                              
                                label.MouseHover += label_MouseEnter2;
                                label.MouseLeave += label_MouseLeave2;
                            }
                        }

                    }


                }
                else if (childControl is Label label)
                {
                    
                    label.MouseHover += label_MouseEnter2;
                    label.MouseLeave += label_MouseLeave2;
                    foreach (Control childControl1 in childControl.Controls)
                    {
                        if (childControl1 is PictureBox pictureBox1)
                        {
                            if (pictureBox1.Tag.ToString() == label.Tag.ToString())
                            {
                               
                                label.MouseHover += label_MouseEnter2;
                                label.MouseLeave += label_MouseLeave2;
                            }
                        }

                    }
                }
                else if (childControl is FlowLayoutPanel flowLayoutPanel)
                {
                    ApplyMouseEventsToPanels(flowLayoutPanel); // Recursively apply mouse events to inner FlowLayoutPanel controls
                }
            }
        }



        private void pictureBox_MouseEnter2(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.BackColor = System.Drawing.Color.Transparent;
        }

        private void pictureBox_MouseLeave2(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            
            
        }

        private void label_MouseEnter2(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = Color.White;
        }

        private void label_MouseLeave2(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            
        }






        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form5 form3 = new Form5();
            Hide();
            form3.Show();
        }

        private async void label2_Click(object sender, EventArgs e)
        {
            await PopulateListings2();
        }

        private async void pictureBox8_Click(object sender, EventArgs e)
        {
            await PopulateListings2();
        }

        



        private async static void AddToFavoritesButton_Click(object sender, EventArgs e, Button addToFavoritesButton)
        {
            // Get the selected listing ID
            JObject listingId = (JObject)addToFavoritesButton.Tag; // Replace with your logic to get the listing ID

            // Send the listing ID to the API to add it to favorites
            // Implement the API call here
            // Example:
            HttpClient client = new HttpClient();

            // Set the base address of the API
            client.BaseAddress = new Uri("https://localhost:7194/");

            // Create a new Listing object
            Listing listing = new Listing
            {
                id = listingId,
               
            };

            // Convert the Listing object to JSON
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(listing);

            // Create a StringContent object with the JSON data
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send a POST request to the API endpoint
            HttpResponseMessage response = await client.PostAsync("Listings", content);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Listing added to favorites.");
            }
            else
            {
                Console.WriteLine("Failed to add listing to favorites. Error: " + response.StatusCode);
            }
        

        // Update the button appearance
        addToFavoritesButton.BackColor = Color.DeepPink;
        addToFavoritesButton.ForeColor = Color.White;
        addToFavoritesButton.Text = "Favorie";
        }

        // Event handler for the reservation button click event
        private void ReservationButton_Click(object sender, EventArgs e, Button addToFavoritesButton, Listing listing)
        {
            Listing listing1 = new Listing();
            listing1 = listing;
            // Clear the flow layout panel
            flowLayoutPanel1.Controls.Clear();
            Button button = (Button)sender; // Cast the sender object to a Button
                                            // Get the parent BunifuCards control

            // Access and modify properties of the parent card

            Control parentControl = button.Parent;
            BunifuCards parentCard = null;

            while (parentControl != null)
            {
                if (parentControl is BunifuCards)
                {
                    parentCard = (BunifuCards)parentControl;
                    break;
                }

                parentControl = parentControl.Parent;
            }

            // Check if the parentCard is found
            if (parentCard != null)
            {
               
                parentCard.Size = new System.Drawing.Size(1063, 691);
                Button button1 = parentCard.Controls.Find("addToFavoritesButton", true).FirstOrDefault() as Button;

                // Make the parentCard fill its parent container

                // Example of making all elements within the parentCard fill the available space
                foreach (Control control in parentCard.Controls)
                {
                    //control.Size = new System.Drawing.Size(500, 500);// Set the Dock property of each control to Fill
                }



                flowLayoutPanel1.Controls.Add(parentCard);

                // Get the selected listing ID


                TextBox startDateTextBox = new TextBox();
                TextBox endDateTextBox = new TextBox();

                // Set the properties of the TextBox controls
                startDateTextBox.Name = "startDateTextBox";
                startDateTextBox.Width = 200;

                endDateTextBox.Name = "endDateTextBox";
                endDateTextBox.Width = 200;



                // Attach event handlers to the TextBox controls' LostFocus events
                startDateTextBox.LostFocus += DateTextBox_LostFocus;
                endDateTextBox.LostFocus += DateTextBox_LostFocus;
                // Assuming you have a TextBox named textBoxDate

                // Set the placeholder text
                string placeholder = "yyyy-MM-ddTHH:mm:ss.fffzzz";

                // Set the initial text and color
                startDateTextBox.Text = placeholder;
                startDateTextBox.ForeColor = System.Drawing.SystemColors.GrayText;

                endDateTextBox.Text = placeholder;
                endDateTextBox.ForeColor = System.Drawing.SystemColors.GrayText;

                // Attach the Enter and Leave event handlers
                startDateTextBox.Enter += TextBoxDate_Enter;
                startDateTextBox.Leave += TextBoxDate_Leave;

                endDateTextBox.Enter += TextBoxDate_Enter;
                endDateTextBox.Leave += TextBoxDate_Leave;
                // Add the TextBox controls to the panel
                parentCard.Controls.Add(startDateTextBox);
                parentCard.Controls.Add(endDateTextBox);
                if (listing != null && startDateTextBox != null && endDateTextBox != null)
                {
                    // Attach the event handler to the button
                    if (button1 != null)
                    {
                        button1.Click += (btnSender, btnEvent) => Button1_Click(btnSender, btnEvent, listing, startDateTextBox, endDateTextBox);
                        parentCard.Controls.Add(button1);
                    }
                    else
                    {
                        Console.WriteLine("Button1 not found!");
                    }
                }
                else
                {
                    Console.WriteLine("One or more objects are null!");
                }




            }
        }

            private static async void Button1_Click(object sender, EventArgs e,Listing listing,TextBox startDate,TextBox endDate)
        {
            // Handle the button click event here
            // You can perform actions or write code that will be executed when the button is clicked
            var payload = new
            {
                id = "string",
                userId = "string",
                listingId = listing.id,
                startDate = startDate.Text,
                endDate = endDate.Text,
                totalPrice = 0,
                createdAt = DateTime.UtcNow
            };

            // Convert the Reservation object to JSON
            string json = JsonConvert.SerializeObject(payload);

            // Set the base address of the API
            string apiBaseUrl = "https://localhost:7194/";

            using (HttpClient client = new HttpClient())
            {
                // Set the API endpoint URL
                string apiUrl = apiBaseUrl + "Reservation";

                // Create a StringContent object with the JSON data
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    // Send a POST request to the API endpoint
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Reservation added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add reservation. Error: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        private static void TextBoxDate_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Check if the current text is empty
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                // Set the placeholder text and change the text color
                textBox.Text = textBox.Tag.ToString();
                textBox.ForeColor = System.Drawing.SystemColors.GrayText;
            }
        }

        private static void TextBoxDate_Enter(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Check if the current text is the placeholder
            if (textBox.Text == textBox.Tag.ToString())
            {
                // Clear the placeholder text and change the text color
                textBox.Text = "";
                textBox.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private static void DateTextBox_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Parse the date entered in the TextBox
            if (DateTime.TryParse(textBox.Text, out DateTime selectedDate))
            {
                // Set the selected date with the desired format
                textBox.Text = selectedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
            }
            else
            {
                // Clear the TextBox if the entered date is invalid
                textBox.Clear();
            }
        }

        
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Transparent;
        }

        private void label7_MouseHover(object sender, EventArgs e)
        {
            label7.ForeColor= Color.DeepPink;
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Transparent;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Form1 form4 = new Form1();
            Hide();
            form4.Show();
        }

        private void label8_MouseHover(object sender, EventArgs e)
        {
            label8.ForeColor= Color.DeepPink;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Form1 form4 = new Form1();
            Hide();
            form4.Show();
        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click_1(object sender, EventArgs e)
        {

        }
    }
}

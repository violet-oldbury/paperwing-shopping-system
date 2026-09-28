using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace paperwing
{
    public partial class cart_menu : Form
    {
        public string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_usernpass.csv");
        public string filename_usrnm = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_sign_in_usrnm.csv");
        public string filename_yorn = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_sign_in_yorn.csv");
        public string filename_address = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_address.csv");
        public string filename_reciept = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "paperwing_reciept.txt");
        public static bool sign_in_yorn;
        public string fileyorn;
        public string fileusrnm;
        public string cartcontents;
        private bool ismax;

        public cart_menu()
        {
            InitializeComponent();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        int itemsordered;
        double totalcost;
        const int numberofproducts = 23;
        product[] products = new product[numberofproducts];


        //creates/checks that certain text files exists
        private void cart_menu_Load(object sender, EventArgs e)
        {

            products[0] = new product("Mistborn book 1: The Final Empire", 1140);
            products[1] = new product("Mistborn book 2: The Well of Ascension ", 974);
            products[2] = new product("Mistborn book 3: The Hero of Ages", 983);
            products[3] = new product("Moby Dick", 2897);
            products[4] = new product("Frankenstien", 2619);
            products[5] = new product("Bram Stoker's Dracula", 4914);
            products[6] = new product("The Iliad", 3987);
            products[7] = new product("1984", 3914);
            products[8] = new product("Fahrenheit 451", 764);
            products[9] = new product("Project Hail Mary", 314);
            products[10] = new product("The Lord of The Rings", 3890);
            products[11] = new product("Carmilla", 3914);
            products[12] = new product("The Art of War", 2876);
            products[13] = new product("The Shining", 4908);
            products[14] = new product("Dune", 8124);
            products[15] = new product("Dune Messiah", 3968);
            products[16] = new product("Children of Dune", 2034);
            products[17] = new product("God Emperor of Dune", 982);
            products[18] = new product("Heretics of Dune", 703);
            products[19] = new product("Chapterhouse: Dune", 1634);
            products[20] = new product("Hunters of Dune", 55);
            products[21] = new product("Sandworms of Dune", 37);
            products[22] = new product("The Very Hungry Caterpillar", 8075);
            for (int i = 0; i < numberofproducts; i++)
            {
                productlist.Items.Add(products[i].description);
            }
            productlist.SelectedIndex = 0;

            if (signed_in(fileyorn))
            {
                string[] lines = File.ReadAllLines(filename_usrnm);
                foreach (string line in lines)
                {
                    string[] credentials = line.Split(',');
                    if (credentials.Length == 1)
                    {
                        fileusrnm = credentials[0];

                    }
                }
                signinbutton.Text = fileusrnm;
            }
        }

        //read filename_yorn, if 1 then return true, if else then return false
        private bool signed_in(string yorn)
        {
            string[] lines = File.ReadAllLines(filename_yorn);

            foreach (string line in lines)
            {
                string[] credentials = line.Split(',');
                if (credentials.Length == 1)
                {
                    string fileyorn = credentials[0];


                    if (fileyorn == "1")
                    {
                        sign_in_yorn = true;
                        return true;
                    }
                    else if (fileyorn == "0")
                    {
                        sign_in_yorn = false;
                        return false;
                    }
                }
            }
            return false;
        }

        private void signinbutton_Click(object sender, EventArgs e)
        {
            if (signed_in(fileyorn))
            {
                Hide();
                profile_menu profile = new profile_menu();
                profile.ShowDialog();
                Close();
            }
            else
            {
                Hide();
                sign_in_menu sign_in = new sign_in_menu();
                sign_in.ShowDialog();
                Close();
            }
        }

        private void homebutton_Click(object sender, EventArgs e)
        {
            Hide();
            home_menu home = new home_menu();
            home.ShowDialog();
            Close();
        }

        private void searchbutton_Click(object sender, EventArgs e)
        {
            Hide();
            search_menu search = new search_menu();
            search.ShowDialog();
            Close();
        }

        private void genrebutton_Click(object sender, EventArgs e)
        {
            Hide();
            genre_menu genres = new genre_menu();
            genres.ShowDialog();
            Close();
        }

        private void popularbutton_Click(object sender, EventArgs e)
        {
            Hide();
            popular_menu popular = new popular_menu();
            popular.ShowDialog();
            Close();
        }

        private void aboutbutton_Click(object sender, EventArgs e)
        {
            Hide();
            about_menu about = new about_menu();
            about.ShowDialog();
            Close();
        }

        private void move_button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void minimise_button_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void maximise_button_Click(object sender, EventArgs e)
        {
            if (ismax == false)
            {
                this.WindowState = FormWindowState.Maximized;
                ismax = true;
            }

            else if (ismax == true)
            {
                this.WindowState = FormWindowState.Normal;
                ismax = false;
            }
        }

        private void close_button_Click(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter(filename_usrnm, append: false))
            {
                writer.Close();
            }

            using (StreamWriter writer = new StreamWriter(filename_yorn, append: false))
            {
                writer.Close();
            }

            this.Close();
        }

        private void cartbutton_Click(object sender, EventArgs e){}

        private void clearcart_Click(object sender, EventArgs e)
        {
            addresstextbox.Text = "";
            itemsordered = 0;
            totalcost = 0;
            totalitemslabel.Text = "Total Items: 0";

            for (int i = 0; i < numberofproducts; i++)
            {
                products[i].numberordered = 0;
            }
            productlist.SelectedIndex = 0;
            shoppingcartlist.Items.Clear();
            totalcostlabel.Text = "Total Cost: £0";
            addresstextbox.Text = "";
            
        }

        private void productlist_SelectedIndexChanged(object sender, EventArgs e){}

        //when the tab index is changed to mailing label, check if there is any items ordered. if not, show messagebox.
        //when the tab index is changed to payment information, check if there are any adresses inputted, if not then show messagebox
        private void shoppinginfotabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (shoppinginfotabs.SelectedIndex)
            {
                case 1:

                    if (itemsordered == 0)
                    {
                        MessageBox.Show("no items were ordered", "invalid order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        shoppinginfotabs.SelectedIndex = 0;
                    }
                break;

                case 2:
                    if (addresstextbox.Text == "")
                    {
                        MessageBox.Show("you did not fill out your mailing address", "invalid order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        shoppinginfotabs.SelectedIndex = 0;
                        addresstextbox.Focus();
                    }
                    else
                    {
                        string address = this.addresstextbox.Text;
                    }
                break;
            }
        }

        //when the add to cart button is clicked, add the relevant amount of numbers to how many items have been added. as well as this, up the cost relative to the prices of what have been put in the cart. 
        //after this, write the numbers to their respective labels.
        private void addtocart_Click(object sender, EventArgs e)
        {


            products[productlist.SelectedIndex].numberordered++;
            totalcost = totalcost + products[productlist.SelectedIndex].cost;
            itemsordered++;
            
            totalitemslabel.Text = "Total Items: " + itemsordered.ToString();
            totalcostlabel.Text = "Total Cost: £" + totalcost.ToString();

            shoppingcartlist.Items.Clear();
            for (int i = 0; i < numberofproducts; i++)
            {
                if (products[i].numberordered != 0)
                {
                    shoppingcartlist.Items.Add(products[i].numberordered.ToString() + " " + products[i].description);
                }
            }
        }

        //when the text in address is changed, then write it to filename_address
        private void mailinglabel_TextChanged(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter(filename_address, append: false))
            {
                writer.Write(addresstextbox.Text);
                writer.Close();
            }
        }

        private void purchasebutton_Click(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter(filename_reciept, append: false))
            {
                writer.Write("order:"+ "\n");
                for (int i = 0; i < numberofproducts; i++)
                {
                    if (products[i].numberordered != 0)
                    {
                        writer.Write(products[i].numberordered.ToString() + " " + products[i].description + "\n");
                    }
                }
                writer.Write("Cost: £"+totalcost.ToString() + "\n");
                writer.Write("delivering to:"+addresstextbox.Text);
                writer.Close();
            }
        }
    }
}

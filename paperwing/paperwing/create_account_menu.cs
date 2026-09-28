using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paperwing
{
    public partial class create_account_menu : Form
    {
        public string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_usernpass.csv");
        public string filename_usrnm = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_sign_in_usrnm.csv");
        public string filename_yorn = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_sign_in_yorn.csv");
        public static bool sign_in_yorn;
        public string fileyorn;
        public string fileusrnm;
        private bool ismax;
        public create_account_menu()
        {
            InitializeComponent();
        }
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        //takes user to sign_in_menu form
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

        //takes user to home_menu form
        private void homebutton_Click(object sender, EventArgs e)
        {
            Hide();
            home_menu home = new home_menu();
            home.ShowDialog();
            Close();
        }

        //takes user to search_menu form
        private void searchbutton_Click(object sender, EventArgs e)
        {
            Hide();
            search_menu search = new search_menu();
            search.ShowDialog();
            Close();
        }

        //takes user to genre_menu form
        private void genrebutton_Click(object sender, EventArgs e)
        {
            Hide();
            genre_menu genres = new genre_menu();
            genres.ShowDialog();
            Close();
        }

        //takes user to popular_menu form
        private void popularbutton_Click(object sender, EventArgs e)
        {
            Hide();
            popular_menu popular = new popular_menu();
            popular.ShowDialog();
            Close();
        }

        //takes user to about_menu form
        private void aboutbutton_Click(object sender, EventArgs e)
        {
            Hide();
            about_menu about = new about_menu();
            about.ShowDialog();
            Close();
        }

        //wipes paperwing_sign_in_usrnm.csv and paperwing_cafe_sign_in_yorn.csv then closes program
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

        //maximises software
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

        //minimises software
        private void minimise_button_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        //moves form to mouse
        private void move_button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        //creates/checks that certain text files exists
        private void create_account_menu_Load(object sender, EventArgs e)
        {
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

        //if the first two text boxes are not empty then write then to filename, show messagebox and then goes to sign in form. if they are empty then show messagebox
        private void createaccountbutton_Click(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter(filename, append: true))
            {
                if (usernameinput.Text != "" && passwordinput.Text != "")
                {

                    writer.Write(usernameinput.Text + "," + passwordinput.Text + "\n");

                    MessageBox.Show("created sign in");
                    writer.Close();

                    Hide();
                    sign_in_menu sign_in = new sign_in_menu();
                    sign_in.ShowDialog();
                    Close();
                }

                else
                {
                    MessageBox.Show("please input text into the username, password and email boxes please");
                }
            }
        }

        //when the box is ticked or unticked, reveal or hide the password
        private void showpasswordtickbox_CheckedChanged(object sender, EventArgs e)
        {
            passwordinput.UseSystemPasswordChar = !showpasswordtickbox.Checked;
        }

        //takes user to cart if you are signed in. if not, shows messagebox
        private void cartbutton_Click(object sender, EventArgs e)
        {
            if (signed_in(fileyorn))
            {
                Hide();
                cart_menu shoppingcart = new cart_menu();
                shoppingcart.ShowDialog();
                Close();
            }
            else
            {
                MessageBox.Show("sign in to access this feature");
            }
        }
    }
}

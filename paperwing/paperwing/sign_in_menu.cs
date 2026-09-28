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
    public partial class sign_in_menu : Form
    {
        public string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_usernpass.csv");
        public string filename_usrnm = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_sign_in_usrnm.csv");
        public string filename_yorn = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_sign_in_yorn.csv");
        public static bool sign_in_yorn;
        public string usrnm;
        public string psswrd;
        public string fileyorn;
        public string fileusrnm;
        private bool ismax;
        public sign_in_menu()
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

        //moves form about
        private void move_button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        //shows/hides password
        private void showpasswordtickbox_CheckedChanged(object sender, EventArgs e)
        {
            passwordinput.UseSystemPasswordChar = !showpasswordtickbox.Checked;
        }

        //if validatecredentials is true then show messagebox then write username to filename_usrnm and a 1 to filename_yorn then open sign in form
        //else then show messagebox then write 0 to filename_yorn
        private void thesigninbutton_Click(object sender, EventArgs e)
        {
            if (validatecredentials(usrnm, psswrd))
            {
                MessageBox.Show("signed in");

                using (StreamWriter writer = new StreamWriter(filename_usrnm, append: false))
                {
                    writer.Write(usernameinput.Text + "\n");
                    writer.Close();
                }

                using (StreamWriter writer = new StreamWriter(filename_yorn, append: false))
                {
                    writer.Write("1" + "\n");
                    writer.Close();
                }

                Hide();
                home_menu home = new home_menu();
                home.ShowDialog();
                Close();
            }

            else
            {
                MessageBox.Show("invalid credentials");
                using (StreamWriter writer = new StreamWriter(filename_yorn, append: false))
                {
                    writer.Write("0" + "\n");
                    writer.Close();
                }
            }
        }

        // try read filename and if has signed in then sign in yorn=true and return true. if not able to then show messagebox
        private bool validatecredentials(string username1, string password1)
        {
            try
            {
                string[] lines = File.ReadAllLines(filename);

                foreach (string line in lines)
                {
                    string[] credentials = line.Split(',');
                    if (credentials.Length == 2)
                    {
                        string fileUsername = credentials[0];
                        string filePassword = credentials[1];

                        if (fileUsername == username1 && filePassword == password1)
                        {
                            sign_in_yorn = true;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file location not found");
                return false;
            }
            catch (IOException)
            {
                MessageBox.Show("cannot access the file because it is being used by another process.");
                return false;
            }
        }

        //saves the users username to usrnm
        private void usernameinput_TextChanged(object sender, EventArgs e)
        {
            usrnm = usernameinput.Text;
        }

        //saves the users password to psswrd
        private void passwordinput_TextChanged(object sender, EventArgs e)
        {
            psswrd = passwordinput.Text;
        }

        //takes user to create_account form
        private void createaccountbutton_Click(object sender, EventArgs e)
        {
            Hide();
            create_account_menu create_Account = new create_account_menu();
            create_Account.ShowDialog();
            Close();
        }

        //creates/checks that certain text files exists
        private void sign_in_menu_Load(object sender, EventArgs e)
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

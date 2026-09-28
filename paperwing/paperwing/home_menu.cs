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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace paperwing
{
    public partial class home_menu : Form
    {
        public string filename_usrnm = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_sign_in_usrnm.csv");
        public string filename_yorn = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "paperwing_sign_in_yorn.csv");
        public static bool sign_in_yorn;
        public string fileyorn;
        public string fileusrnm;
        private bool ismax;
        public home_menu()
        {
            InitializeComponent();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        //creates/checks that certain text files exists
        private void home_menu_Load(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename_usrnm, append: true))
                {
                    writer.Close();
                }

                using (StreamWriter writer = new StreamWriter(filename_yorn, append: true))
                {
                    writer.Close();
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("neccessary file location not found");
                this.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("cannot access neccessary file because it is being used by another process.");
                this.Close();
            }

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

        //tales user to profile_menu form
        private void profile_Click(object sender, EventArgs e)
        {
            Hide();
            profile_menu profile = new profile_menu();
            profile.ShowDialog();
            Close();
        }

        private void homebutton_Click(object sender, EventArgs e){}

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

        //moves form
        private void move_button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e){}

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
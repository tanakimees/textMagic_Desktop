using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextmagicRest;
using TextmagicRest.Model;

namespace textMagic_Desktop
{
    public partial class Form1 : Form
    {
        //Local data
        bool fadeInOrOut = false;
        bool saveData = false;

        //Imports necessary DLL file for drawing rounded corners.
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        //InitializeComponent()
        public Form1()
        {
            InitializeComponent();
        }

        //Loads the form in.
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateRoundedCorners(this, 30);
            CreateRoundedCorners(pictureBox1, 20);
            CreateRoundedCorners(pictureBox2, 20);
            CreateRoundedCorners(panel2, 20);
            CreateRoundedCorners(panel4, 60);
            CreateRoundedCorners(panel3, 80);
            CreateRoundedCorners(textBox1, 5);
            CreateRoundedCorners(textBox2, 5);
            CreateRoundedCorners(label1, 5);
            CreateRoundedCorners(label2, 5);
            CreateRoundedCorners(label3, 10);
            CreateRoundedCorners(label4, 10);
            CreateRoundedCorners(label5, 10);
            fadeForm.Start();

            if(File.Exists(@"C:\temp\TextMagicLogin.txt"))
            {
                StreamReader sr = new StreamReader(@"C:\temp\TextMagicLogin.txt");
                string username1 = sr.ReadLine();
                string apikey = sr.ReadLine();

                textBox1.Text = username1;
                textBox2.Text = apikey;

                panel4.BackColor = Color.White;
                saveData = true;

                sr.Close();
            }          
        }

        //Optimizes the creation of rounded corners.
        void CreateRoundedCorners(Control c, int x)
        {
            c.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, c.Width, c.Height, x, x));
        }

        //Allows the borderless form to be moveable.
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        //Fades the form in or out, dependent on the 'fadeInOrOut' boolean.
        private void fadeForm_Tick(object sender, EventArgs e)
        {
            switch(fadeInOrOut)
            {
                case false:
                    this.Opacity += 0.01;
                    if(this.Opacity == 1)
                    {
                        fadeForm.Stop();
                    }
                    break;
                case true:
                    this.Opacity -= 0.01;
                    if (this.Opacity == 0)
                    {
                        this.Close();
                        Application.Exit();
                        fadeForm.Stop();
                    }
                    break;
            }
        }

        //Close button MouseEnter animation.
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(42, 135, 185);
        }

        //Close button MouseLeave animation.
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(0,92,138);
        }

        // Close button click handler.
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            fadeForm.Stop();
            fadeInOrOut = true;
            fadeForm.Start();
        }

        //Minimize to tray button MouseEnter animation,
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(42, 135, 185);
        }

        //Minimize to tray button MouseLeave animation.
        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(0, 92, 138);
        }

        //Minimize to tray button click handler.
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Login button MouseEnter animation.
        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.BackColor = Color.FromArgb(62, 155, 205);
        }

        //Login button MouseLeave animation.
        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.BackColor = Color.FromArgb(42, 135, 185);
        }

        //Login button click.
        private void label3_Click(object sender, EventArgs e)
        {
            var client = new Client(textBox1.Text, textBox2.Text);
            var auth = client.Ping();

            if(auth.Success)
            {
                MessageBox.Show("success");
            }
            else
            {
                MessageBox.Show("fail");
            }
        }

        //Create API key button MouseEnter animation.
        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(62, 155, 205);
        }

        //Create API key button MouseLeave animation.
        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(42, 135, 185);
        }

        //Create API key button click.
        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://my.textmagic.com/online/api/rest-api/keys-trial");
        }

        //Save login data button MouseEnter animation.
        private void panel3_MouseEnter(object sender, EventArgs e)
        {
            panel4.BackColor = Color.White;
        }

        //Save login data button MouseLeave animation.
        private void panel3_MouseLeave(object sender, EventArgs e)
        {
            if(saveData == true)
            {
                panel4.BackColor = Color.White;
            }
            else
            {
                panel4.BackColor = Color.FromArgb(0, 92, 138);
            }
        }

        //Save login data button MouseEnter animation.
        private void panel4_MouseEnter(object sender, EventArgs e)
        {
            panel4.BackColor = Color.White;
        }

        //Save login data button click.
        private void panel3_Click(object sender, EventArgs e)
        {
            if(saveData == false)
            {
                saveData = true;
                saveLogin();
                panel4.BackColor = Color.White;
            }
            else
            {
                saveData = false;
                saveLogin();
            }
        }

        //Save login data button click.
        private void panel4_Click(object sender, EventArgs e)
        {
            if (saveData == false)
            {
                saveData = true;
                saveLogin();
                panel4.BackColor = Color.White;
            }
            else
            {
                saveData = false;
                saveLogin();
            }
        }

        //Save login data logic.
        void saveLogin()
        {
            switch(saveData)
            {
                case false:
                    if(File.Exists(@"C:\temp\TextMagicLogin.txt"))
                    {
                        File.Delete(@"C:\temp\TextMagicLogin.txt");
                    }
                    break;
                case true:
                    if (!File.Exists(@"C:\temp\TextMagicLogin.txt"))
                    {
                        using(StreamWriter sw = File.CreateText(@"C:\temp\TextMagicLogin.txt"))
                        {
                            sw.WriteLine(textBox1.Text);
                            sw.WriteLine(textBox2.Text);

                            sw.Dispose();
                        }
                    }
                    break;
            }
        }
    }
}

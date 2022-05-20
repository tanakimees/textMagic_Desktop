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
using TextMagicClient;
using TextMagicClient.Api;
using TextMagicClient.Client;
using TextMagicClient.Model;

namespace textMagic_Desktop
{
    public partial class Form1 : Form
    {
        //Local data
        bool fadeInOrOut = false;
        bool saveData = false;

        TextMagicApi api = new TextMagicApi();

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
            CreateRoundedCorners(panel5, 20);
            CreateRoundedCorners(panel4, 60);
            CreateRoundedCorners(panel3, 80);
            CreateRoundedCorners(textBox1, 5);
            CreateRoundedCorners(textBox2, 5);
            CreateRoundedCorners(textBox3, 5);
            CreateRoundedCorners(textBox4, 5);
            CreateRoundedCorners(label1, 5);
            CreateRoundedCorners(label2, 5);
            CreateRoundedCorners(label3, 10);
            CreateRoundedCorners(label4, 10);
            CreateRoundedCorners(label5, 10);
            CreateRoundedCorners(label9, 10);
            CreateRoundedCorners(label12, 10);
            CreateRoundedCorners(label14, 10);
            CreateRoundedCorners(label16, 10);
            CreateRoundedCorners(label18, 10);
            CreateRoundedCorners(label19, 10);
            CreateRoundedCorners(label20, 10);
            CreateRoundedCorners(dataGridView1, 20);
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
            Configuration.Default.Username = textBox1.Text;
            Configuration.Default.Password = textBox2.Text;
            
            try
            {
                var result = api.Ping();
                panel5.Visible = true;
                panel5.BringToFront();

                label10.Text = "Balance: " + api.GetCurrentUser().Balance.ToString() + api.GetCurrentUser().Currency.UnicodeSymbol.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void label9_MouseEnter(object sender, EventArgs e)
        {
            label9.BackColor = Color.FromArgb(62, 155, 205);
        }

        private void label9_MouseLeave(object sender, EventArgs e)
        {
            label9.BackColor = Color.FromArgb(42, 135, 185);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            string[] phonenr = textBox3.Text.Split();

            for(int i = phonenr.Length; i >= 0; i--)
            {
                try
                {
                    var sendmMessageInputObject = new SendMessageInputObject
                    {
                        Text = textBox4.Text,
                        Phones = phonenr[i],
                    };

                    Configuration.Default.Username = textBox1.Text;
                    Configuration.Default.Password = textBox2.Text;

                    var result = api.SendMessage(sendmMessageInputObject);
                    label10.Text = "Balance: " + api.GetCurrentUser().Balance.ToString() + api.GetCurrentUser().Currency.UnicodeSymbol.ToString();
                    MessageBox.Show("Message(s) were succesfully sent.");

                    if(File.Exists(@"C:\temp\TextMagicPreviouslySentSMSnumber.txt"))
                    {
                        File.Delete(@"C:\temp\TextMagicPreviouslySentSMSnumber.txt");
                    }

                    if (File.Exists(@"C:\temp\TextMagicPreviouslySentSMSmessage.txt"))
                    {
                        File.Delete(@"C:\temp\TextMagicPreviouslySentSMSmessage.txt");
                    }

                    using (StreamWriter sw = File.CreateText(@"C:\temp\TextMagicPreviouslySentSMSnumber.txt"))
                    {
                        sw.WriteLine(textBox3.Text);
                        sw.Dispose();
                    }

                    using (StreamWriter sw1 = File.CreateText(@"C:\temp\TextMagicPreviouslySentSMSmessage.txt"))
                    {
                        sw1.WriteLine(textBox4.Text);
                        sw1.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    
                }
            }         
        }

        private void textBox4_MouseLeave(object sender, EventArgs e)
        {
            string[] phonenr = textBox3.Text.Split();

            int lengthofnrs = phonenr.Length;

            try
            {
                label11.Text = "Cost: " + api.GetMessagePrice(text: textBox4.Text, phones: phonenr[0], contacts: phonenr[0]).Total.Value * lengthofnrs + api.GetCurrentUser().Currency.UnicodeSymbol.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void textBox3_MouseEnter(object sender, EventArgs e)
        {
            string[] phonenr = textBox3.Text.Split();
            var apiInstance = new TextMagicApi();

            int lengthofnrs = phonenr.Length;

            try
            {
                label11.Text = "Cost: " + apiInstance.GetMessagePrice(text: textBox4.Text, phones: phonenr[0], contacts: phonenr[0]).Total.Value * lengthofnrs + apiInstance.GetCurrentUser().Currency.UnicodeSymbol.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void label12_MouseEnter(object sender, EventArgs e)
        {
            label12.BackColor = Color.FromArgb(62, 155, 205);
        }

        private void label12_MouseLeave(object sender, EventArgs e)
        {
            label12.BackColor = Color.FromArgb(42, 135, 185);
        }

        private void label12_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select a .txt file.";
            ofd.Filter = "Text files (*.txt) | *.txt";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                textBox3.Text = sr.ReadToEnd();
            }
        }

        private void label14_MouseEnter(object sender, EventArgs e)
        {
            label14.BackColor = Color.FromArgb(62, 155, 205);
        }

        private void label14_MouseLeave(object sender, EventArgs e)
        {
            label14.BackColor = Color.FromArgb(42, 135, 185);
        }

        private void label14_Click(object sender, EventArgs e)
        {
            StreamReader sr1 = new StreamReader(@"C:\temp\TextMagicPreviouslySentSMSnumber.txt");
            StreamReader sr2 = new StreamReader(@"C:\temp\TextMagicPreviouslySentSMSmessage.txt");

            textBox3.Text = sr1.ReadToEnd();
            textBox4.Text = sr2.ReadToEnd();

            sr1.Dispose();
            sr2.Dispose();
        }

        private void label16_MouseEnter(object sender, EventArgs e)
        {
            label16.BackColor = Color.FromArgb(62, 155, 205);
        }

        private void label16_MouseLeave(object sender, EventArgs e)
        {
            label16.BackColor = Color.FromArgb(42, 135, 185);
        }

        private void label16_Click(object sender, EventArgs e)
        {
            panel6.Visible = true;

            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "First name";
            dataGridView1.Columns[1].Name = "Last name";
            dataGridView1.Columns[2].Name = "Phone";
            dataGridView1.Columns[3].Name = "Country";

            int rowsfordatagrid = api.GetContacts().Resources.Count;

            for(int i = rowsfordatagrid - 1; i >= 0; i--)
            {
                string[] row =
                {
                    api.GetContacts().Resources[index:i].FirstName,
                    api.GetContacts().Resources[index:i].LastName,
                    api.GetContacts().Resources[index:i].Phone,
                    api.GetContacts().Resources[index:i].Country.Name
                };

                dataGridView1.Rows.Add(row);
            }
        }

        private void label18_MouseEnter(object sender, EventArgs e)
        {
            label18.BackColor = Color.FromArgb(62, 155, 205);
        }

        private void label18_MouseLeave(object sender, EventArgs e)
        {
            label18.BackColor = Color.FromArgb(42, 135, 185);
        }

        private void label18_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            panel6.Visible = false;
        }

        private void label19_MouseEnter(object sender, EventArgs e)
        {
            label19.BackColor = Color.FromArgb(62, 155, 205);
        }

        private void label19_MouseLeave(object sender, EventArgs e)
        {
            label19.BackColor = Color.FromArgb(42, 135, 185);
        }

        private void label19_Click(object sender, EventArgs e)
        {
            int rowsfordatagrid = api.GetContacts().Resources.Count;

            textBox3.Text = "";
            for(int i = rowsfordatagrid - 1; i >= 0; i--)
            {
                textBox3.Text += "+" + api.GetContacts().Resources[index: i].Phone + Environment.NewLine;
            }
        }

        private void label20_MouseEnter(object sender, EventArgs e)
        {
            label20.BackColor = Color.FromArgb(62, 155, 205);
        }

        private void label20_MouseLeave(object sender, EventArgs e)
        {
            label20.BackColor = Color.FromArgb(42, 135, 185);
        }

        private void label20_Click(object sender, EventArgs e)
        {
            int rowsfordatagrid = api.GetContacts().Resources.Count;

            textBox3.Text = "";
            for (int i = rowsfordatagrid; i >= 0; i--)
            {
                if(dataGridView1.Rows[i].Selected)
                {
                    textBox3.Text += "+" + dataGridView1.Rows[i].Cells[2].Value.ToString() + Environment.NewLine;
                }
            }
        }
    }
}

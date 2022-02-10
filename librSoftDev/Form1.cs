using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace librSoftDev
{
    public partial class Form1 : Form
    {
        //SQL Database connection configuration
        MySqlConnection con = new MySqlConnection(@"Data Source=localhost;port=3306;Initial Catalog=library;User id=root;password=''");

        //Print Variable
        Bitmap bitmap;

        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //EXIT BUTTON CONFIGS
            DialogResult iExit;

            try 
            {

             iExit = MessageBox.Show("Haluatko kirjautua ulos", "MySql Connector",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (iExit == DialogResult.Yes)
            {
                Application.Exit();
            }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //RESET BUTTON CONFIGS
            try
            {
                foreach (Control c in panel4.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).Clear();

                }
                //This will clear the searchbox
                textBox5.Text = "";

            }

            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);

            }



        }

        private void button6_Click(object sender, EventArgs e)
        {
            //PRINT BUTTON CONFIGS
            try
            {
                //This will take picture of the datagridview and makes pdf out of it
                int height = dataGridView1.Height;
                dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height * 2;
                bitmap = new Bitmap(dataGridView1.Width, dataGridView1.Height);
                dataGridView1.DrawToBitmap(bitmap, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
                printPreviewDialog1.PrintPreviewControl.Zoom = 1;
                printPreviewDialog1.ShowDialog();
                dataGridView1.Height = height;

            }

            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);

            }


        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Print datagridview pdf document configs
            try
            {   
                e.Graphics.DrawImage(bitmap, 0, 0);
            }
            catch  (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Every startup this loader will load data to database and show it up in datagridview
            con.Open();

            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT * FROM book";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            da.Fill(dt);

            dataGridView1.DataSource = dt;

            con.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Add new element to db button
            try
            {
                //Move this out try catch??
                con.Open();

                MySqlCommand cmd = con.CreateCommand();

                cmd.CommandType = CommandType.Text;

                //The box4 and box3 is twisted because there is some order issue in GUI. Use this code to get values correct places in db
                cmd.CommandText = "insert into book (Kirja, Kirjoittaja, Vuosi, Koodi) values ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox4.Text + "', '" + textBox3.Text + "')";

                cmd.ExecuteNonQuery();

                con.Close();

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                //Here we need fuction to call reload data again in db so, it gets updated everytime when new element is added
                con.Close();
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
            //This is update button. This code will refresh the data in datagridview
            try
            {
                //Move this out of try catch?
                con.Open();

                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "SELECT * FROM book";

                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                con.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                con.Close();
            }
            */
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
            
            {

                cmd.CommandText = "UPDATE book SET Kirja = @Kirja, Kirjoittaja = @Kirjoittaja, Vuosi = @Vuosi, Koodi = @Koodi";

                cmd.Parameters.AddWithValue("@Kirja", textBox1.Text);
                cmd.Parameters.AddWithValue("@Kirjoittaja", textBox2.Text);
                cmd.Parameters.AddWithValue("@Vuosi", textBox3.Text);
                cmd.Parameters.AddWithValue("@Koodi", textBox4.Text);

            }

            cmd.ExecuteNonQuery();
            con.Close();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Delete Button configs. WARNING! DO NOT USE. ATM DELETES ALL INFORMATION IN TABLE as
            try
            {
                con.Open();

                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "DELETE FROM book WHERE Koodi = Kirja=Kirja";
                cmd.ExecuteNonQuery();
                //con.Close();

                foreach(DataGridViewRow item in this.dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.RemoveAt(item.Index);
                }


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                //con.Open();

                //Put this inside a function. See the notes below
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "SELECT * FROM book";

                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                //con.Close();
                con.Close();
            }

            //NOTES
            //Make new upLoadData(); named function which reloads the data again on db and call it over here OR inside finally?
            //Get rid of try catch or test if it does work

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            //SearchBox configuration
            try
            {
                con.Open();

                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "SELECT * FROM book";

                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                DataView dv = dt.DefaultView;

                dv.RowFilter = String.Format("Kirja like'%{0}%'", textBox5.Text);

                dataGridView1.DataSource = dv.ToTable();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                con.Close();
            }


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //This will bring the data back to textboxes
            try 
            { 

            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            }

            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            
            }



        }
    }
}

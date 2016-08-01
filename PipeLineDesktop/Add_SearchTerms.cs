using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace PipeLineDesktop
{
    public partial class Add_SearchTerms : Form
    {
        private SQLiteConnection _connection;
        private BindingSource bindingSource=null;
        private SQLiteDataAdapter adapter=null;
        private string _selectedID;
        private DataTable dataTable=null;
        private SQLiteCommandBuilder sqlCommandBuilder = null;

        public Add_SearchTerms(SQLiteConnection connection)
        {
            InitializeComponent();
            this._connection=connection;           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if(button1.Text=="Add")
            //{
            //    foreach(string s in Regex.Split(textBox1.Text, ","))
            //    {
            //        ExecuteQuery(string.Format("insert into search(UserID,Term) values(1,'{0}')", s));
            //    }
            //    MessageBox.Show("Value(s) Inserted Successfully");
            //}
            //else
            //{
            //    ExecuteQuery(string.Format("update search set Term = '{0}' where ID = {1}",textBox1.Text.Replace("'","''"),_selectedID));
            //    MessageBox.Show("Value Updated Successfully");
            //}
            //this.button1.Text="Add";
            //this.textBox1.Text="";
            //FillGridView();

            try
            {
               int i=  adapter.Update(dataTable);

                MessageBox.Show("Search Term prefrence updated successfully");

                this.Close();
            }
            catch
            {

            }
        }

        private void FillGridView()
        {
            adapter=new SQLiteDataAdapter(string.Format("SELECT * FROM search;", new object[0]), _connection);

            DataSet dataSet=new DataSet();
            dataTable=new DataTable();

            sqlCommandBuilder=new SQLiteCommandBuilder(adapter);
            adapter.Fill(dataTable);
            //dataGridOpp.DataSource=dataSet.Tables[0];

            bindingSource=new BindingSource();
            bindingSource.DataSource=dataTable;

            dataGridOpp.DataSource=bindingSource;

            this.dataGridOpp.Columns["UserId"].Visible=false;
            this.dataGridOpp.Columns["ID"].Visible=false;            
        }

        private void dataGridOpp_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
               // _selectedID=dataGridOpp.SelectedCells[0].Value.ToString();
               // this.button1.Text="Update";
                //this.textBox1.Text=dataGridOpp.SelectedRows[0].Cells[2].Value.ToString();
            }
            catch
            {
                //
            }
        }

        private void Add_SearchTerms_Load(object sender, EventArgs e)
        {
            FillGridView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridOpp_KeyUp(object sender, KeyEventArgs e)
        {
            // check for delete key press
            if(e.KeyValue!=46)
                return;
            try
            {
                if(MessageBox.Show("Are you sure? You want to delete search term", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.Cancel)
                    return;
                //ExecuteQuery(string.Format("delete from search where ID = {0}",_selectedID));
                dataGridOpp.Rows.RemoveAt(dataGridOpp.SelectedRows[0].Index);
                //dataGridOpp.Refresh();
                //button3_Click(sender, e);
                adapter.Update(dataTable);
            }
            catch
            {
                //
            }
        }

        private void ExecuteQuery(string text)
        {
            try
            {
                SQLiteCommand cmd=new SQLiteCommand(text, _connection);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridOpp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                _selectedID=dataGridOpp.SelectedCells[0].Value.ToString();
               // this.button1.Text="Update";
                //this.textBox1.Text=dataGridOpp.SelectedRows[0].Cells[2].Value.ToString();
            }
            catch
            {
                //
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text="";
            this.button1.Text="Add";
        }

    }
}

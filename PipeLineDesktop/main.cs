using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using mshtml;
using LeadHarvest;
using LeadHarvest.Database;
using LeadHarvest.Entities;
using LeadHarvest.Providers;

namespace PipeLineDesktop
{
    public partial class main : Form
    {
        private List<string> _lSocial = new List<string>(new string[] { 
                "twitter.com", "http://facebook.com/", "youtube.com", "plus.google.com/+", "http://www.linkedin.com/company" }); 
        
        private int _referneceURL;
        private int _oppid;
        private string _selectedID ;
        private string _selectedText;

        private HtmlElement _html;
        bool skipSelectionChagned = true;
        MySqlConnection _connection = new MySqlConnection();

        public main()
        {
            InitializeComponent();
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void main_Load(object sender, EventArgs e)
        {

            WindowState = FormWindowState.Maximized;
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.ContextMenuStrip = this.contextMenuStrip1;

            var server = "localhost";
            var database = "pipeline";
            var uid = "root";
            var password = "9414";

            _connection.ConnectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3}",server, database, uid, password);           
            _connection.Open();

            string query = String.Format("SELECT * FROM leads;");
            MySqlDataAdapter dbAdapter = new MySqlDataAdapter(query, _connection);
            DataSet ds = new DataSet();
            dbAdapter.Fill(ds);
            dataGridOpp.DataSource = ds.Tables[0];
            try
            {
                // Set field indexes
                _referneceURL=this.dataGridOpp.Columns["ResponseUri"].Index;
                _oppid=this.dataGridOpp.Columns["oppid"].Index;

                _selectedID=dataGridOpp.SelectedCells[_oppid].Value.ToString();

                this.dataGridOpp.Columns["searchid"].Visible=false;
                this.dataGridOpp.Columns["oppid"].Visible=false;
                this.dataGridOpp.Columns["orgid"].Visible=false;
            }
            catch
            {
                // No Data Error Catch
            }

            toolStripStatusLabel2.Text = String.Format("Total={0}", this.dataGridOpp.Rows.Count.ToString());
            skipSelectionChagned = false;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!this.skipSelectionChagned){
                try
                {
                    Uri url = new Uri(dataGridOpp.SelectedCells[_referneceURL].Value.ToString());
                    this.webBrowser1.Url = url;
                    this.textBox1.Text = url.ToString();

                    _selectedID = dataGridOpp.SelectedCells[_oppid].Value.ToString();

                    string query = string.Format("SELECT Title, City, State, DatePosted, Created, Snippet, ResponseUri FROM opportunity WHERE id={0};", _selectedID);
                    MySqlDataAdapter dbAdapter = new MySqlDataAdapter(query, _connection);
                    DataSet ds = new DataSet();
                    dbAdapter.Fill(ds);
                    dataGridDetail.DataSource = ds.Tables[0];

                }
                catch (Exception ex) { }
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("(c) " + DateTime.Today.ToString("yyyy") + " All rights researved. \n Pipeline is a trademark of\n Sevenbrook Consulting Inc.", "Sevenbrook Consulting Inc.");
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            //! Hide all menu items
            for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items[i].Visible = false;
            }

            // ADD THESE MENU ITEMS EVERY TIME
            searchToolStripMenuItem.Visible = true;
            viewSourceToolStripMenuItem.Visible = true;

            //there are a number of things the user could have been on when they right clicked
            // a social icon/link >> find the social URL and update the database without displaying a context
            // they could have selected text >> here we have to display the menu and ask them what value to set >> or they may want to search google with the value
            // they could just be some randome locaion on the page >> view page content

            // What did they click on ?
            Point ScreenCoord = new Point(MousePosition.X, MousePosition.Y);
            Point BrowserCoord = webBrowser1.PointToClient(ScreenCoord);
            HtmlElement elem = webBrowser1.Document.GetElementFromPoint(BrowserCoord);
            
            // Did the user select somthing before right click ?
            IHTMLDocument2 html = webBrowser1.Document.DomDocument as IHTMLDocument2;
            IHTMLSelectionObject currentSelection = html.selection;
            if (currentSelection != null)
            {
                IHTMLTxtRange range = currentSelection.createRange() as IHTMLTxtRange;

                if (range != null)
                {
                    setValueToolStripMenuItem.Visible = true;
                    // RANGE.TEXT should be added to the DB for the context menu item selected
                   _selectedText = range.text;
                }
            }

            // you have to loop parents to see that they click
            // need to do a better way of doing this
            string innerHTML = string.Empty;
            bool found = false;
            if (elem.Parent.TagName == "A")
            {
                innerHTML = elem.Parent.InnerHtml; 
                found = true;;
            }
            else
            {
                if (elem.Parent.Parent.TagName == "A")
                {
                    innerHTML = elem.Parent.Parent.InnerHtml; 
                    found = true; ;
                }
                else 
                { 
                    if (elem.Parent.Parent.Parent.TagName == "A")
                    {
                        setValueToolStripMenuItem.Visible = true;
                        innerHTML = elem.Parent.Parent.Parent.InnerHtml;
                        found = true;
                    }
                }
            }

            if(found)
            {
                // does the innerHTML have a social url?
                if (_lSocial.Any(s=>innerHTML.Contains(s)))
                {
                    // update database with url 
                    var url = innerHTML;
                }

            }
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
            this.textBox1.Text = webBrowser1.Url.ToString();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uri url = new Uri(dataGridOpp.SelectedCells[_referneceURL].Value.ToString());
            this.webBrowser1.Url = url;
            this.textBox1.Text = url.ToString();
            this.textBox1.Text = webBrowser1.Url.ToString();
        }

        private void fowardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
            this.textBox1.Text = webBrowser1.Url.ToString();
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            _html = this.webBrowser1.Document.Body;
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            string query = "SELECT ID, Name, Description, Linkedin, Facebook, Twitter, GooglePlus FROM organization;";
            MySqlDataAdapter dbAdapter = new MySqlDataAdapter(query, _connection);
            DataSet ds = new DataSet();
            dbAdapter.Fill(ds);
            dataGridOrg.DataSource = ds.Tables[0];
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void goToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uri url = new Uri(this.textBox1.Text);
            this.webBrowser1.Url = url;
        }

        private void dataGridOpp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Delete)
            {
                // delete
                string query = string.Format("DELETE FROM opportunity WHERE id={0};",_selectedID);
                MySqlDataAdapter dbAdapter = new MySqlDataAdapter(query, _connection);

                //refreash
                query = String.Format("SELECT * FROM leads;");
                dbAdapter = new MySqlDataAdapter(query, _connection);
                DataSet ds = new DataSet();
                dbAdapter.Fill(ds);
                dataGridOpp.DataSource = ds.Tables[0];
                dataGridOpp.Refresh();
            }
        }

        private void sourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure... you want to start update","Confirm",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
            new Thread(new ThreadStart(Update)).Start();
        }
        bool Updating=false;
        public void Update()
        {
            bool Update=false;
            base.Invoke(new Action(() =>
            {
                Update=Updating;
                if(!Updating) Updating=true;
            }));

            if(Update) { MessageBox.Show("Update already running.. will show you new jobs once we complete updating"); return; }

            DateTime dtStart=DateTime.Now;
            LeadHarvesterExternal lhe=new LeadHarvesterExternal();
            lhe.HarvestLead();
            
            base.Invoke(new Action(() =>
            {
                string query=String.Format("SELECT * FROM leads where created between '"+dtStart+"' and '"+DateTime.Now+"';");
                MySqlDataAdapter dbAdapter=new MySqlDataAdapter(query, _connection);
                DataSet ds=new DataSet();
                dbAdapter.Fill(ds);
                Updating=false;
                new New_Jobs(ds.Tables[0]).Show();
            }));
        }
        private void main_Shown(object sender, EventArgs e)
        {
            if(MessageBox.Show("Application Require to update Data... Please click Yes to start Update..", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)==DialogResult.Yes)
                new Thread(new ThreadStart(Update)).Start();
            else
            {
                MessageBox.Show("You can update application by clicking update menu on the top.", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

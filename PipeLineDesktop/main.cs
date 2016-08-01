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
using mshtml;
using System.Data.SQLite;
using LeadHarvest;
using LeadHarvest.SqliteDal;
using LeadHarvest.Entities;
using LeadHarvest.Providers;
using System.Data.Common;

namespace PipeLineDesktop
{
    public partial class main : Form
    {
        private List<string> _lSocial=new List<string>(new string[] { 
                "twitter.com", "http://facebook.com/", "youtube.com", "plus.google.com/+", "http://www.linkedin.com/company" });

        private int _referneceURL;
        private int _oppid;
        private string _selectedID;
        private string _selectedText;

        private HtmlElement _html;
        bool skipSelectionChagned=true;
        SQLiteConnection _connection=new SQLiteConnection();

        #region Initial
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

            WindowState=FormWindowState.Maximized;
            webBrowser1.ScriptErrorsSuppressed=true;
            webBrowser1.ContextMenuStrip=this.contextMenuStrip1;            
      
            _connection.ConnectionString=_connection.ConnectionString=String.Format("Data Source={0};version=3;Compress=True;", "pipeLine.db");
            _connection.Open();
            FillDG();
            toolStripStatusLabel2.Text=String.Format("Total={0}", this.dataGridOpp.Rows.Count.ToString());
            toolStripStatusLabel1.Visible = false;
            skipSelectionChagned=false;
        }
        #endregion
        private void FillDG()
        {
            SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(string.Format("select * from leads order by modified desc;", new object[0]), this._connection);
            DataSet dataSet=new DataSet();
            sqLiteDataAdapter.Fill(dataSet);

            this.dataGridOpp.DataSource=null;
            this.dataGridOpp.Rows.Clear();
            this.dataGridOpp.Columns.Clear();

            this.dataGridOpp.DataSource=(object)dataSet.Tables[0];
            try
            {
                this._referneceURL=this.dataGridOpp.Columns["ResponseUri"].Index;
                this._oppid=this.dataGridOpp.Columns["oppid"].Index;
                this._selectedID=this.dataGridOpp.SelectedCells[this._oppid].Value.ToString();
                this.dataGridOpp.Columns["searchid"].Visible=false;
                this.dataGridOpp.Columns["oppid"].Visible=true;
                this.dataGridOpp.Columns["orgid"].Visible=false;
                this.dataGridOpp.Columns["SearchTerm"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                AddCheckboxColumn();
                //AddUpdateColumn();
                AddColumn();
                MarkUpdated();
                InitializeDataGridView(dataGridOpp);
            }
            catch
            {
            }
        }

        #region GridViewUpdateColumn *
        private void AddCheckboxColumn()
        {
            DataGridViewCheckBoxColumn col1=new DataGridViewCheckBoxColumn();
            col1.HeaderText="";
            col1.Name="Chkbx";
            col1.Width=20;
            this.dataGridOpp.Columns.Insert(0, col1);
            this.dataGridOpp.Refresh();
            this.dataGridOpp.Columns["Chkbx"].DisplayIndex=0;
            this.dataGridOpp.Refresh();

            //this._referneceURL=this.dataGridOpp.Columns["ResponseUri"].Index;
            //this._oppid=this.dataGridOpp.Columns["oppid"].Index;
            //this._selectedID=this.dataGridOpp.SelectedCells[this._oppid].Value.ToString();

          //  InitializeDataGridView(dataGridOpp);
        }
        private void AddUpdateColumn()
        {
            bool added=false;
            foreach(DataGridViewColumn c in dataGridOpp.Columns)
            {
                if(c.HeaderText=="Updated") added=true;
            }
            if(!added)
            {
                AddColumn();
            }

            dataGridOpp["RecentUpdated", dataGridOpp.SelectedRows[0].Index].Value="\u221A";
            dataGridOpp["RecentUpdated", dataGridOpp.SelectedRows[0].Index].Style.SelectionForeColor=Color.Black;
            dataGridOpp["RecentUpdated", dataGridOpp.SelectedRows[0].Index].Style.Font=new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold);
            dataGridOpp["Modified", dataGridOpp.SelectedRows[0].Index].Value=DateTime.Now.ToString();
            this.dataGridOpp.Refresh();
        }

        private void MarkUpdated()
        {
            foreach(DataGridViewRow row in dataGridOpp.Rows)
            {
                if(!string.IsNullOrEmpty(row.Cells["modified"].Value.ToString()))
                {
                    row.Cells["RecentUpdated"].Value="\u221A";
                    row.Cells["RecentUpdated"].Style.SelectionForeColor=Color.Black;
                    row.Cells["RecentUpdated"].Style.Font=new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold);                    
                }
            }
            this.dataGridOpp.Refresh();
        }

        private void AddColumn()
        {
            DataGridViewTextBoxColumn col1=new DataGridViewTextBoxColumn();
            col1.HeaderText="Updated";
            col1.Name="RecentUpdated";
            col1.Width=20;
            col1.DefaultCellStyle.ForeColor=Color.Black;
            col1.DefaultCellStyle.Font= new Font(FontFamily.GenericSansSerif, 50,FontStyle.Bold);
            col1.DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleCenter;
            //col1.DefaultCellStyle.
            this.dataGridOpp.Columns.Insert(1, col1);
            this.dataGridOpp.Refresh();
            this.dataGridOpp.Columns["RecentUpdated"].DisplayIndex=1;
            this.dataGridOpp.Refresh();

            this._referneceURL=this.dataGridOpp.Columns["ResponseUri"].Index;
            this._oppid=this.dataGridOpp.Columns["oppid"].Index;
            this._selectedID=this.dataGridOpp.SelectedCells[this._oppid].Value.ToString();

            InitializeDataGridView(dataGridOpp);
        }

        #endregion

        #region InitializeDataGridView()
        private void InitializeDataGridView(DataGridView dg)
        {
            dg.ReadOnly=false;
            dg.AllowUserToResizeColumns=true;
            dg.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.DisplayedCells;

            int MaxWidth=150;
            foreach(DataGridViewColumn c in dg.Columns)
            {
                if(c.Width>MaxWidth)
                {
                    //c.AutoSizeMode=DataGridViewAutoSizeColumnMode.None;
                    c.Width=MaxWidth;
                }
                if(c.Name=="RecentUpdated")
                {
                    c.Width=50;
                    c.DefaultCellStyle.ForeColor=Color.Black;
                    c.DefaultCellStyle.Font=new Font(FontFamily.GenericSansSerif, 50, FontStyle.Bold);
                    c.DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleCenter;
                    
                }
                c.AutoSizeMode=DataGridViewAutoSizeColumnMode.None;
            }

            for(int i=0; i<dg.Rows.Count; i++)
            {
                for(int j=1; j<dg.Columns.Count; j++)
                {
                    dg.Columns[j].ReadOnly=true;
                }
            }
        }

        private void InitializeDataGridView1(DataGridView dg)
        {
            dg.ReadOnly=false;
            dg.AllowUserToResizeColumns=true;
            dg.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.DisplayedCells;

            int MaxWidth=150;
            foreach(DataGridViewColumn c in dg.Columns)
            {
                if(c.Width>MaxWidth)
                {
                    //c.AutoSizeMode=DataGridViewAutoSizeColumnMode.None;
                    c.Width=MaxWidth;
                }
                if(c.Name=="RecentUpdated")
                {
                    c.Width=50;
                    c.DefaultCellStyle.ForeColor=Color.Black;
                    c.DefaultCellStyle.Font=new Font(FontFamily.GenericSansSerif, 50, FontStyle.Bold);
                    c.DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleCenter;

                }
                c.AutoSizeMode=DataGridViewAutoSizeColumnMode.None;
            }

            for(int i=0; i<dg.Rows.Count; i++)
            {
                for(int j=0; j<dg.Columns.Count-1; j++)
                {
                    dg.Columns[j].ReadOnly=true;
                }
            }
        }
        #endregion

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
            if(!this.skipSelectionChagned)
            {
                try
                {
                    Uri url=new Uri(dataGridOpp.SelectedCells[_referneceURL].Value.ToString());
                    this.webBrowser1.Url=url;
                    this.textBox1.Text=url.ToString();

                    _selectedID=dataGridOpp.SelectedCells[_oppid].Value.ToString();

                    FillOppertunity();
                    FillOrganization();
                }
                catch(Exception ex) { }
            }
        }

        private void FillOppertunity()
        {
            string query=string.Format("SELECT Title, City, State, DatePosted, Created, Snippet, ResponseUri,Compensation FROM opportunity WHERE id={0};", _selectedID);
            this.dataGridOpp.AllowUserToAddRows = false;
            SQLiteDataAdapter dbAdapter=new SQLiteDataAdapter(query, _connection);
            DataSet ds=new DataSet();
            dbAdapter.Fill(ds);
            dataGridDetail.DataSource=ds.Tables[0];

            dataGridDetail.AutoResizeColumns();
            dataGridDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            this.dataGridDetail.Refresh();
           // InitializeDataGridView1(dataGridDetail);           
        }

        #region FillOrganization
        private void FillOrganization()
        {
            //string query=string.Format("SELECT Name, LinkedIn as LinkedInUrl, FaceBook as FaceBookUrl, Twitter as TwitterUrl, GooglePlus as GooglePlusUrl, Description FROM organization WHERE id={0};", _selectedID);
            string query=string.Format(@"SELECT Name, LinkedIn as LinkedInUrl, FaceBook as FaceBookUrl, Twitter as TwitterUrl, GooglePlus as GooglePlusUrl, Description,phone.Number FROM organization left join phone_organization on phone_organization.OrganizationID = organization.ID
                                        left join phone on phone.ID = phone_organization.PhoneID WHERE organization.id={0};", _selectedID);
            DataSet dataSet2=new DataSet();
            SQLiteDataAdapter dbAdapter=new SQLiteDataAdapter(query, _connection);
            dbAdapter.Fill(dataSet2);

            this.dataGridOrg.DataSource=null;
            this.dataGridOrg.Rows.Clear();
            this.dataGridOrg.Columns.Clear();
            this.dataGridOrg.AllowUserToAddRows=false;

            this.dataGridOrg.DataSource=dataSet2.Tables[0];

            this.dataGridOrg.Columns["FacebookUrl"].Visible=false;
            this.dataGridOrg.Columns["LinkedInUrl"].Visible=false;
            this.dataGridOrg.Columns["TwitterUrl"].Visible=false;
            this.dataGridOrg.Columns["GooglePlusUrl"].Visible=false;

            int count=3;
            if(!string.IsNullOrEmpty(dataSet2.Tables[0].Rows[0]["LinkedInUrl"].ToString()) && dataSet2.Tables[0].Rows[0]["LinkedInUrl"].ToString() != "NULL")
            {
                this.AddImageColumn("LinkedIn", "social_icons/linkedin.png", count, dataSet2.Tables[0].Rows[0]["LinkedInUrl"].ToString());
                count++;
            }
            if(!string.IsNullOrEmpty(dataSet2.Tables[0].Rows[0]["FacebookUrl"].ToString()) && dataSet2.Tables[0].Rows[0]["FacebookUrl"].ToString() != "NULL")
            {
                this.AddImageColumn("Facebook", "social_icons/facebook.png", count, dataSet2.Tables[0].Rows[0]["FacebookUrl"].ToString());
                count++;
            }
            if(!string.IsNullOrEmpty(dataSet2.Tables[0].Rows[0]["TwitterUrl"].ToString()) && dataSet2.Tables[0].Rows[0]["TwitterUrl"].ToString() != "NULL")
            {
                this.AddImageColumn("Twitter", "social_icons/twitter.png", count, dataSet2.Tables[0].Rows[0]["TwitterUrl"].ToString());
                count++;
            }
            if(!string.IsNullOrEmpty(dataSet2.Tables[0].Rows[0]["GooglePlusUrl"].ToString()) && dataSet2.Tables[0].Rows[0]["GooglePlusUrl"].ToString() != "NULL")
            {
                this.AddImageColumn("Google", "social_icons/google_plus.png", count, dataSet2.Tables[0].Rows[0]["GooglePlusUrl"].ToString());

            }
            this.dataGridOrg.Refresh();
            InitializeDataGridView(dataGridOrg);
        }
        private void AddImageColumn(string ColumnName, string ImageSource, int Index,string Url)
        {
            DataGridViewImageColumn gridViewImageColumn=new DataGridViewImageColumn();
            Image image=Image.FromFile(ImageSource);
            image=new Bitmap(image, 30, 30);
            gridViewImageColumn.Image=image;
            gridViewImageColumn.HeaderText=ColumnName;
            gridViewImageColumn.Name=ColumnName;
            gridViewImageColumn.ToolTipText=Url;
            gridViewImageColumn.Tag=Url;
            this.dataGridOrg.Columns.Insert(Index, gridViewImageColumn);
        }
        #endregion

        #region Browser Code
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("(c) "+DateTime.Today.ToString("yyyy")+" All rights researved. \n Pipeline is a trademark of\n Sevenbrook Consulting Inc.", "Sevenbrook Consulting Inc.");
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            //! Hide all menu items
            for(int i=0; i<contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items[i].Visible=false;
            }

            // ADD THESE MENU ITEMS EVERY TIME
            searchToolStripMenuItem.Visible=true;
            viewSourceToolStripMenuItem.Visible=true;
            socialToolStripMenuItem.Visible=true;
           // setValueToolStripMenuItem.Visible = true;
            //there are a number of things the user could have been on when they right clicked
            // a social icon/link >> find the social URL and update the database without displaying a context
            // they could have selected text >> here we have to display the menu and ask them what value to set >> or they may want to search google with the value
            // they could just be some randome locaion on the page >> view page content

            // What did they click on ?
           // return;
            Point ScreenCoord=new Point(MousePosition.X, MousePosition.Y);
            Point BrowserCoord=webBrowser1.PointToClient(ScreenCoord);
            HtmlElement elem=webBrowser1.Document.GetElementFromPoint(BrowserCoord);

            // Did the user select somthing before right click ?
            IHTMLDocument2 html=webBrowser1.Document.DomDocument as IHTMLDocument2;
            IHTMLSelectionObject currentSelection=html.selection;
            try
            {
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
            }
            catch
            {
                return;
            }

            // you have to loop parents to see that they click
            // need to do a better way of doing this
            string innerHTML=string.Empty;
            bool found=false;
            if(elem.Parent.TagName=="A")
            {
                innerHTML=elem.Parent.InnerHtml;
                found=true; ;
            }
            else
            {
                if(elem.Parent.Parent.TagName=="A")
                {
                    innerHTML=elem.Parent.Parent.InnerHtml;
                    found=true; ;
                }
                else
                {
                    if(elem.Parent.Parent.Parent.TagName=="A")
                    {
                        setValueToolStripMenuItem.Visible=true;
                        innerHTML=elem.Parent.Parent.Parent.InnerHtml;
                        found=true;
                    }
                }
            }

            if(found)
            {
                // does the innerHTML have a social url?
                if(_lSocial.Any(s => innerHTML.Contains(s)))
                {
                    // update database with url 
                    var url=innerHTML;
                }

            }
        }
        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
            this.textBox1.Text=webBrowser1.Url.ToString();
        }
        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uri url=new Uri(dataGridOpp.SelectedCells[_referneceURL].Value.ToString());
            this.webBrowser1.Url=url;
            this.textBox1.Text=url.ToString();
            this.textBox1.Text=webBrowser1.Url.ToString();
        }
        private void fowardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
            this.textBox1.Text=webBrowser1.Url.ToString();
        }
        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            _html=this.webBrowser1.Document.Body;
        }
        private void tabControl1_Click(object sender, EventArgs e)
        {
            //string query="SELECT ID, Name, Description, Linkedin, Facebook, Twitter, GooglePlus FROM organization;";
            ////MySqlDataAdapter dbAdapter = new MySqlDataAdapter(query, _connection);
            //SQLiteDataAdapter dbAdapter=new SQLiteDataAdapter(query, _connection);
            //DataSet ds=new DataSet();
            //dbAdapter.Fill(ds);
            //dataGridOrg.DataSource=ds.Tables[0];
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void goToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uri url=new Uri(this.textBox1.Text);
            this.webBrowser1.Url=url;
        }
        #endregion

        private void dataGridOpp_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyValue==(char)Keys.Delete)
            {
                // delete
                if(e.KeyValue!=46)
                    return;
                try
                {
                    if(dataGridOpp["RecentUpdated", dataGridOpp.SelectedRows[0].Index].Value.ToString()=="\u221A")
                    {
                        if(MessageBox.Show("You updated data for this opportunity, If you delete now.. your data will lost", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.Cancel)
                            return;
                    }
                }catch
                {
                    //
                }

                bool cont = true;
                List<DataGridViewRow> rows=new List<DataGridViewRow>();
                foreach(DataGridViewRow row in dataGridOpp.Rows)
                {
                    if(Convert.ToBoolean(row.Cells["Chkbx"].Value)==true)
                    {
                        string query=string.Format("UPDATE opportunity SET ActiveStatus = 1 WHERE ID={0};", row.Cells[_oppid].Value);
                        SQLiteDataAdapter dbAdapter=new SQLiteDataAdapter(query, _connection);
                        SQLiteCommand cmd=new SQLiteCommand(query, _connection);
                        cmd.ExecuteNonQuery();
                        cont=false;
                        rows.Add(row);
                       // dataGridOpp.Rows.Remove(row);
                    }
                }

                if(cont)
                {
                    string query1=string.Format("UPDATE opportunity SET ActiveStatus = 1 WHERE ID={0};", _selectedID);
                    SQLiteDataAdapter dbAdapter1=new SQLiteDataAdapter(query1, _connection);
                    SQLiteCommand cmd=new SQLiteCommand(query1, _connection);
                    cmd.ExecuteNonQuery();
                    dataGridOpp.Rows.RemoveAt(dataGridOpp.SelectedRows[0].Index);
                }
                else
                {
                    foreach(DataGridViewRow r in rows)
                    {
                        dataGridOpp.Rows.Remove(r);
                    }
                }               
                dataGridOpp.Refresh();                
            }
        }

        private void sourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region Update
        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure... you want to start update", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK)
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

            try
            {

                DateTime dtStart=DateTime.Now;
                LeadHarvesterExternal lhe=new LeadHarvesterExternal();
                lhe.HarvestLead("pipeLine.db");

                base.Invoke(new Action(() =>
                {
                    string query=String.Format("SELECT * FROM leads where created > datetime('"+dtStart.ToString("yyyy-MM-dd HH:mm:ss")+"');");                                        
                    SQLiteDataAdapter dbAdapter=new SQLiteDataAdapter(query, _connection);
                    DataSet ds=new DataSet();
                    dbAdapter.Fill(ds);
                    Updating=false;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        new New_Jobs(ds.Tables[0]).ShowDialog();
                        FillDG();
                    }
                    else
                        MessageBox.Show("DataBase Is Up to date. No New Records Found");
                }));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void main_Shown(object sender, EventArgs e)
        {
            //if(MessageBox.Show("Application Require to update Data... Please click Yes to start Update..", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)==DialogResult.Yes)
            //    new Thread(new ThreadStart(Update)).Start();
        }
        private void UpdateOppertunity(string Field)
        {
            mshtml.IHTMLDocument2 htmlDocument2=this.webBrowser1.Document.DomDocument as mshtml.IHTMLDocument2;
            IHTMLSelectionObject selection=htmlDocument2.selection;
            if(selection==null)
                return;
            IHTMLTxtRange htmlTxtRange=selection.createRange() as IHTMLTxtRange;
            if(!string.IsNullOrEmpty(htmlTxtRange.text))
            {
                if(MessageBox.Show("Are you sure... Update: Old Value: "+this.dataGridOpp.SelectedRows[0].Cells[Field].Value+" New Value: "+htmlTxtRange.text, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK)
                {
                    SQLiteCommand cmd=new SQLiteCommand(string.Format("UPDATE opportunity SET {2} = '{1}' WHERE ID={0};", this._selectedID, htmlTxtRange.text,Field));
                    cmd.Connection=_connection;
                    cmd.ExecuteNonQuery();
                    dataGridOpp[Field, dataGridOpp.SelectedRows[0].Index].Value = htmlTxtRange.text;
                }
                else
                {
                    //int num=(int)MessageBox.Show("");
                }
            }
            else
            {
                MessageBox.Show("Please select some text and try again.");
            }
        }
        private string getSelectedText()
        {
            mshtml.IHTMLDocument2 htmlDocument2=this.webBrowser1.Document.DomDocument as mshtml.IHTMLDocument2;
            IHTMLSelectionObject selection=htmlDocument2.selection;
            if(selection==null)
                return "";
            IHTMLTxtRange htmlTxtRange=selection.createRange() as IHTMLTxtRange;
            return htmlTxtRange.text;
        }
        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedText=this.getSelectedText();      
            if(!string.IsNullOrEmpty(selectedText))
            {
                MessageBox.Show("Please select some text and try again."); return;
            }
            if(MessageBox.Show("Are you sure... Update: Old Value: "+this.dataGridOpp.SelectedRows[0].Cells["snippet"].Value+" New Value: "+selectedText, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK)
            {
                SQLiteCommand cmd=new SQLiteCommand(string.Format("UPDATE opportunity SET snippet = '{1}' WHERE ID={0};", this._selectedID, selectedText));
                cmd.Connection=_connection;
                cmd.ExecuteNonQuery();
            }
            else
            {
                //
            }
        }
        private void phoneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string selectedText=this.getSelectedText();
            if(!string.IsNullOrEmpty(selectedText))
            {
                if(MessageBox.Show("Are you sure... set Phone No. to: "+selectedText, "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)!=DialogResult.OK)
                    return;

                int ii=this.CreatePhone(selectedText);
                if(ii==0) MessageBox.Show("some exception occur");
                SQLiteCommand cmd=new SQLiteCommand(string.Format("select exists(select 1 from phone_opportunity where PhoneId = {0});", ii), _connection);
                if(Convert.ToInt32(cmd.ExecuteScalar())==1)
                {
                    cmd=new SQLiteCommand(string.Format("update phone_opportunity set OpportunityID = {0} where PhoneId = {1} ;", _selectedID, ii), _connection);
                }
                else
                {
                    string query=String.Format("INSERT OR IGNORE INTO phone_opportunity(PhoneId,OpportunityID)VALUES({0},{1});", ii, _selectedID);
                    cmd=new SQLiteCommand(query, _connection);
                }
            }
            else
            {
                int num=(int)MessageBox.Show("Please select some text on web-browser, then try to update..");
            }
        }
        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedText=this.getSelectedText();
            if(!string.IsNullOrEmpty(selectedText))
            {
                if(MessageBox.Show("Are you sure... set email to: "+selectedText, "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)!=DialogResult.OK)
                    return;

                int ii = this.CreateEmail(selectedText);
                if(ii==0) MessageBox.Show("some exception occur");
                SQLiteCommand cmd = new SQLiteCommand(string.Format("select exists(select 1 from email_opportunity where emailId = {0});", ii),_connection);
                if( Convert.ToInt32(cmd.ExecuteScalar()) == 1)
                {
                    cmd=new SQLiteCommand(string.Format("update email_opportunity set OpportunityID = {0} where emailid = {1} ;",_selectedID,ii ), _connection);
                }
                else
                {
                    string query=String.Format("INSERT OR IGNORE INTO email_opportunity(EmailID,OpportunityID)VALUES({0},{1});", ii, _selectedID);
                     cmd=new SQLiteCommand(query, _connection);
                }
            }
            else
            {
                int num=(int)MessageBox.Show("Please select some text on web-browser, then try to update..");
            }
        }
        private void titleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpdateOppertunity(((ToolStripItem)sender).Text);
            FillOppertunity();
            AddUpdateColumn();
        }
        public int CreateEmail(string emailAddress)
        {
            try
            {
                string query=String.Format("INSERT OR IGNORE INTO email(Address)VALUES('{0}');SELECT ID FROM email WHERE Address='{0}';", emailAddress);
                SQLiteCommand cmd=new SQLiteCommand(query, _connection);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch(Exception ex) { return 0; }
        }
        public int CreatePhone(string phoneNumber)
        {
            string query=String.Format("INSERT OR IGNORE INTO phone(Number)VALUES('{0}');SELECT ID FROM phone WHERE Number='{0}';", phoneNumber);
            SQLiteCommand cmd=new SQLiteCommand(query, _connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        #region DataGridOrg_CellContentClick
        private void dataGridOrg_CellContentClick(object sender, DataGridViewCellEventArgs e)
          {
              
              if(e.ColumnIndex > -1 && e.RowIndex > -1)
              {
                  //if(dataGridOrg[e.ColumnIndex,e.RowIndex].Tag != null && !string.IsNullOrEmpty(dataGridOrg[e.ColumnIndex,e.RowIndex].Tag.ToString()))
                  //{
                  //    try
                  //    {
                  //        webBrowser1.Navigate(dataGridOrg[e.ColumnIndex, e.RowIndex].Tag.ToString());
                  //    }
                  //    catch
                  //    {
                  //        // TODO:  Log error....   Ankur
                  //    }
                  //}
                  string url="";
                  if(dataGridOrg[e.ColumnIndex,e.RowIndex].OwningColumn.HeaderText == "Facebook")
                  {
                      url=this.dataGridOrg["FacebookUrl", e.RowIndex].Value.ToString();                                                                 
                  }
                  else if(dataGridOrg[e.ColumnIndex, e.RowIndex].OwningColumn.HeaderText=="LinkedIn")
                  {
                      url=this.dataGridOrg["LinkedInUrl", e.RowIndex].Value.ToString();
                  }
                  else if(dataGridOrg[e.ColumnIndex, e.RowIndex].OwningColumn.HeaderText=="Twitter")
                  {
                      url=this.dataGridOrg["TwitterUrl", e.RowIndex].Value.ToString();                      
                  }
                  else if(dataGridOrg[e.ColumnIndex, e.RowIndex].OwningColumn.HeaderText=="Google")
                  {
                      url=this.dataGridOrg["GooglePlusUrl", e.RowIndex].Value.ToString();
                  }

                  if(!string.IsNullOrEmpty(url))
                  {
                    try
                    {
                        this.webBrowser1.Url = new Uri(url);
                        this.textBox1.Text = url.ToString();
                    }
                    catch { }
                  }
              }
          }
        #endregion

        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpdateOrganization(((ToolStripItem)sender).Text);
            FillOrganization();
            AddUpdateColumn();
        }

        private void UpdateOrganization(string Field)
        {
            mshtml.IHTMLDocument2 htmlDocument2=this.webBrowser1.Document.DomDocument as mshtml.IHTMLDocument2;
            IHTMLSelectionObject selection=htmlDocument2.selection;
            if(selection==null)
                return;
            IHTMLTxtRange htmlTxtRange=selection.createRange() as IHTMLTxtRange;
            if(htmlTxtRange.text!=null)
            {
                if(MessageBox.Show("Are you sure... Update: Old Value: "+this.dataGridOrg[Field,0].Value+"\n New Value: "+htmlTxtRange.text, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK)
                {
                    SQLiteCommand cmd=new SQLiteCommand(string.Format("UPDATE organization SET {2} = '{1}' WHERE ID={0};", this._selectedID, htmlTxtRange.text, Field));
                    cmd.Connection=_connection;
                    cmd.ExecuteNonQuery();
                    
                }
                else
                {
                    //int num=(int)MessageBox.Show("Please select some text on webbrowser, then try to update..");
                }
            }
            else
            {
                MessageBox.Show("Please select some text and try again."); return;
            }
        }

        private void linkedInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpdateSocial(((ToolStripItem)sender).Text);
            FillOrganization();
            AddUpdateColumn();
        }

        private void UpdateSocial(string Field)
        {
            if(Field=="Google+") Field="GooglePlus";

            mshtml.IHTMLDocument2 htmlDocument2=this.webBrowser1.Document.DomDocument as mshtml.IHTMLDocument2;
            IHTMLSelectionObject selection=htmlDocument2.selection;
            if(selection==null)
                return;
            IHTMLTxtRange htmlTxtRange=selection.createRange() as IHTMLTxtRange;
            if(htmlTxtRange.text!=null)
            {
                if(MessageBox.Show("Are you sure... Update: Old Value: "+this.dataGridOrg[Field+"Url", 0].Value+" New Value: "+htmlTxtRange.text, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK)
                {
                    SQLiteCommand cmd=new SQLiteCommand(string.Format("UPDATE organization SET {2} = '{1}' WHERE ID={0};", this._selectedID, htmlTxtRange.text, Field));
                    cmd.Connection=_connection;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    //int num=(int)MessageBox.Show("Please select some text on webbrowser, then try to update..");
                }
            }
            else
            {
                MessageBox.Show("Please select some text and try again."); return;
            }
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel=true;            
        }

        private void viewSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string html=webBrowser1.DocumentText;
        }

        #region Block PopUp
        private void InjectAlertBlocker()
        {
            HtmlElement head=webBrowser1.Document.GetElementsByTagName("head")[0];
            HtmlElement scriptEl=webBrowser1.Document.CreateElement("script");
            IHTMLScriptElement element=(IHTMLScriptElement)scriptEl.DomElement;
            string alertBlocker="window.alert = function () { }";
            element.text=alertBlocker;
            head.AppendChild(scriptEl);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            InjectAlertBlocker();
        }
        #endregion

    
        private void searchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_SearchTerms ast=new Add_SearchTerms(_connection);
            ast.ShowDialog();
        }

        private void phoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedText=this.getSelectedText();
            if(!string.IsNullOrEmpty(selectedText))
            {
                if(MessageBox.Show("Are you sure... set Phone No. to: "+selectedText, "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)!=DialogResult.OK)
                    return;

                int ii=this.CreatePhone(selectedText);
                if(ii==0) MessageBox.Show("some exception occur");
                SQLiteCommand cmd=new SQLiteCommand(string.Format("select exists(select 1 from phone_organization where PhoneId = {0});", ii), _connection);
                if(Convert.ToInt32(cmd.ExecuteScalar())==1)
                {
                    cmd=new SQLiteCommand(string.Format("update phone_organization set OrganizationID = {0} where PhoneId = {1} ;", _selectedID, ii), _connection);
                }
                else
                {
                    string query=String.Format("INSERT OR IGNORE INTO phone_organization(PhoneId,OrganizationID)VALUES({0},{1});", ii, _selectedID);
                    cmd=new SQLiteCommand(query, _connection);
                }
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                FillOrganization();
                AddUpdateColumn();
            }
            else
            {
                int num=(int)MessageBox.Show("Please select some text on web-browser, then try to update..");
            }
        }

        private void dataGridOpp_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //
        }

        private void dataGridOpp_Sorted(object sender, EventArgs e)
        {            
            MarkUpdated();
        }

        private void dataGridDetail_Leave(object sender, EventArgs e)
        {
           // MessageBox.Show("grid out");

        }

        private void dataGridDetail_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 7 && e.RowIndex == 0)
            {
                SQLiteCommand cmd=new SQLiteCommand(string.Format("UPDATE opportunity SET Compensation = '{1}' WHERE ID={0};", this._selectedID, dataGridDetail[e.ColumnIndex, e.RowIndex].EditedFormattedValue.ToString()));
                cmd.Connection=_connection;
                cmd.ExecuteNonQuery();
                //FillOppertunity();
                AddUpdateColumn();
              //  MessageBox.Show(dataGridDetail[e.ColumnIndex, e.RowIndex].EditedFormattedValue.ToString());

            }
        }

        private void PergeTable(string table)
        {
            SQLiteCommand cmd = new SQLiteCommand(string.Format("delete from {0};", table));
            cmd.Connection = _connection;
            cmd.ExecuteNonQuery();

            SQLiteCommand cmd2 = new SQLiteCommand(string.Format("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='{0}';", table));
            cmd2.Connection = _connection;
            cmd2.ExecuteNonQuery();
        }

        private void purgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to purge all data from the system ?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                PergeTable("email");
                PergeTable("email_opportunity");
                PergeTable("email_organization");
                PergeTable("opportunity");
                PergeTable("organization");
                PergeTable("phone");
                PergeTable("phone_opportunity");
                PergeTable("phone_organization");
                MessageBox.Show("Database data purged. Configurations not affected", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
        }
    }
}
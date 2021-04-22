using LimsApplication.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LimsApplication
{
    public partial class FmMain : Form
    {
        private DBType dbType = DBType.SqlServer;
        public FmMain()
        {
            InitializeComponent();
            string dbTypeConfig = ConfigurationManager.AppSettings["DBType"];
            if (dbTypeConfig == "oracle")
            {
                dbType = DBType.Oracle;
            }
            else
            {
                dbType = DBType.SqlServer;
            }
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            string sql = "select CATEGORYID,CATNAME  from LIMSAPPCATEGORIES";
            DataTable dataTable = null;
            if (dbType == DBType.SqlServer)
            {
                dataTable = SQLHelper.GetDataTable(sql);
            }
            else
            {
                dataTable = OracleHelper.ExecToSqlGetTable(sql);
            }
            foreach (DataRow dataRow in dataTable.Rows)
            {
                string CATEGORYID = dataRow["CATEGORYID"].ToString();
                string CATNAME = dataRow["CATNAME"].ToString();

            }
        }
        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tspb.Value++;
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { 
            btnExport.Enabled = true;
            tssl.Text = "导出完成";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = false;
            string sql = "select count(1) from LIMSAPPCATEGORIES ";
            int count = 1;
            if (dbType == DBType.SqlServer)
            {
                count = (int)SQLHelper.ExecuteScalar(sql);
            }
            else
            {
                count = int.Parse(OracleHelper.ExecToSqlGetTable(sql).Rows[0][0].ToString());
            }
            tspb.Maximum = count;
            //
            bgw.RunWorkerAsync();
        }
    }
}

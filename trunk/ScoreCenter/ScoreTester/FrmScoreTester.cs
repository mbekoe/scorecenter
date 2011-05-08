using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Plugin.ScoreCenter;
using System.Diagnostics;

namespace ScoreTester
{
    public partial class FrmScoreTester : Form
    {
        public FrmScoreTester()
        {
            InitializeComponent();
        }

        private void NormalTest()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // note create a fake ScoreCenterScore to use current values
                // instead of saved values
                GenericScore score = new GenericScore();
                score.Url = tbxUrl.Text;
                score.Encoding = tbxEncoding.Text;
                score.XPath = tbxXpath.Text;
                score.Sizes = tbxSizes.Text;
                score.Headers = tbxHeaders.Text;

                score.Element = tbxElement.Text;
                score.Skip = ReadInt(tbxSkip);
                score.MaxLines = ReadInt(tbxMaxLines);

                // read and parse the score
                string[][] lines = ScoreFactory.Instance.GetParser(score).Read(score, ckxReload.Checked, null);

                int nbColumns = 0;
                grdTest.Columns.Clear();
                if (lines != null)
                {
                    foreach (string[] ss in lines)
                    {
                        if (ss != null)
                        {
                            nbColumns = Math.Max(nbColumns, ss.Length);
                        }
                    }

                    grdTest.Columns.Clear();
                    for (int i = 0; i < Math.Min(20, nbColumns); i++)
                    {
                        grdTest.Columns.Add(i.ToString(), i.ToString());
                    }

                    foreach (string[] ss in lines)
                    {
                        if (ss != null)
                        {
                            grdTest.Rows.Add(ss);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            NormalTest();
        }
        private static int ReadInt(TextBox control)
        {
            int result = 0;
            if (int.TryParse(control.Text, out result))
            {
                return result;
            }

            return -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbxUrl.Text = @"http://www.lequipe.fr/Football/ligue-des-champions-resultats.html";
            tbxXpath.Text = @"//div[@class='ListeDeroulante']//option";
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            string result = String.Empty;
            for (int i = 0; i < grdTest.Columns.Count; i++)
            {
                int max = 0;
                foreach (DataGridViewRow row in grdTest.Rows)
                {
                    if (row.Cells.Count > i)
                    {
                        int v = (row.Cells[i].Value == null ? 0 : row.Cells[i].Value.ToString().Length);
                        max = Math.Max(max, v);
                    }
                }

                if (result.Length > 0) result += ",";
                result += max.ToString();
            }

            tbxSizes.Text = result;
        }

        private void btnOpenUrl_Click(object sender, EventArgs e)
        {
            if (tbxUrl.Text.Length > 0)
            {
                string url = Tools.ParseUrl(tbxUrl.Text, null);
                Process.Start(url);
            }
        }
    }
}

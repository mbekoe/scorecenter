using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Plugin.ScoreCenter;

namespace ScoreTester
{
    public partial class FrmScoreTester : Form
    {
        private ScoreParser m_parser;

        public FrmScoreTester()
        {
            InitializeComponent();

            m_parser = new ScoreParser(5);
        }

        private void NormalTest()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int[] colSizes = Tools.GetSizes(tbxSizes.Text);

                // note create a fake ScoreCenterScore to use current values
                // instead of saved values
                Score score = new Score();
                score.Url = tbxUrl.Text;
                score.Encoding = tbxEncoding.Text;
                score.XPath = tbxXpath.Text;
                score.Sizes = tbxSizes.Text;
                score.Headers = tbxHeaders.Text;

                score.Element = tbxElement.Text;
                score.Skip = ReadInt(tbxSkip);
                score.MaxLines = ReadInt(tbxMaxLines);

                // read and parse the score
                string[][] lines = m_parser.Read(score, ckxReload.Checked);

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
            if (checkBox1.Checked)
                DynamicTest();
            else
                NormalTest();
        }
        private void DynamicTest()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int[] colSizes = Tools.GetSizes(tbxSizes.Text);

                // note create a fake ScoreCenterScore to use current values
                // instead of saved values
                Score score = new Score();
                score.Url = tbxUrl.Text;
                score.Encoding = tbxEncoding.Text;
                score.XPath = tbxXpath.Text;
                score.Sizes = tbxSizes.Text;
                score.Headers = tbxHeaders.Text;

                score.Element = tbxElement.Text;
                score.Skip = ReadInt(tbxSkip);
                score.MaxLines = ReadInt(tbxMaxLines);

                // read and parse the score
                List<Score> scores = m_parser.ParseDynamicList(score, ckxReload.Checked);

                grdTest.Columns.Clear();
                if (scores.Count > 0)
                {
                    grdTest.Columns.Add("Name", "Name");
                    grdTest.Columns.Add("Url", "Url");
                    grdTest.Columns.Add("Xpath", "Xpath");

                    foreach (Score sc in scores)
                    {
                        grdTest.Rows.Add(sc.Name, sc.Url, sc.XPath);
                    }

                    grdTest.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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
    }
}

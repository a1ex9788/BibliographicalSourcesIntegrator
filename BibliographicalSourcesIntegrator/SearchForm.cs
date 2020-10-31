using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliographicalSourcesIntegrator
{
    public partial class SearchForm : Form
    {
        private Form homeForm;

        public SearchForm(Form homeForm)
        {
            InitializeComponent();

            this.homeForm = homeForm;
        }


        private async void SearchButtonClick(object sender, EventArgs e)
        {
            string author = textBoxAuthor.Text;
            string title = textBoxTitle.Text;
            int initialYear = (int) numericUpDownInitialYear.Value;
            int finalYear = (int) numericUpDownFinalYear.Value;
            bool searchArticle = checkBoxArticle.Checked;
            bool searchBook = checkBoxBook.Checked;
            bool searchCongress = checkBoxCongress.Checked;

            await RequestsManager.GetRequestsManager().SearchDataInWarehouse();

            //listViewResults.Items.Add();
        }

        private void CleanSearchButtonClick(object sender, EventArgs e)
        {
            textBoxAuthor.Text = "";
            textBoxTitle.Text = "";
            numericUpDownInitialYear.Text = "";
            numericUpDownFinalYear.Text = "";
            checkBoxArticle.Text = "";
            checkBoxBook.Text = "";
            checkBoxCongress.Text = "";
            listViewResults.Items.Clear();
        }


        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            homeForm.Show();
        }
    }
}

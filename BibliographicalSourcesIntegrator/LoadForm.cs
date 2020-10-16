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
    public partial class LoadForm : Form
    {
        private Form homeForm;

        public LoadForm(Form homeForm)
        {
            InitializeComponent();

            this.homeForm = homeForm;

            labelDBLPReferencesNumber.Text = "";
            labelIEEEXploreReferencesNumber.Text = "";
            labelGoogleScholarReferencesNumber.Text = "";
            labelTotalReferencesNumber.Text = "";
            richTextBoxResults.Text = "";
        }


        private void LoadDataButtonClick(object sender, EventArgs e)
        {
            bool loadDBLP = checkBoxDBLP.Checked;
            bool loadIEEEXplore = checkBoxIEEEXplore.Checked;
            bool loadGoogleScholar = checkBoxGoogleScholar.Checked;
            int initialYear = (int) numericUpDownInitialYear.Value;
            int finalYear = (int) numericUpDownFinalYear.Value;

            new RequestsManager().LoadDataFromDataSources();

            labelDBLPReferencesNumber.Text = "";
            labelIEEEXploreReferencesNumber.Text = "";
            labelGoogleScholarReferencesNumber.Text = "";
            labelTotalReferencesNumber.Text = "";
            richTextBoxResults.Text = "";
        }


        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            homeForm.Show();
        }
    }
}

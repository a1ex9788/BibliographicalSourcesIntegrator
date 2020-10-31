using BibliographicalSourcesIntegratorContracts;
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


        private async void LoadDataButtonClick(object sender, EventArgs e)
        {
            bool loadDBLP = checkBoxDBLP.Checked;
            bool loadIEEEXplore = checkBoxIEEEXplore.Checked;
            bool loadGoogleScholar = checkBoxGoogleScholar.Checked;
            int initialYear = (int) numericUpDownInitialYear.Value;
            int finalYear = (int) numericUpDownFinalYear.Value;

            if (!loadDBLP && !loadIEEEXplore && !loadGoogleScholar)
            {
                ShowError("Any source selected, please select one or more.");

                return;
            }

            LoadRequest loadRequest = new LoadRequest(loadDBLP, loadIEEEXplore, loadGoogleScholar, initialYear, finalYear);

            LoadAnswer loadAnswer = await RequestsManager.GetRequestsManager().LoadDataFromDataSources(loadRequest);

            if (loadAnswer == null)
            {
                return;
            }

            ResetErrorText();
            labelDBLPReferencesNumber.Text = "";
            labelIEEEXploreReferencesNumber.Text = "";
            labelGoogleScholarReferencesNumber.Text = "";
            labelTotalReferencesNumber.Text = "";
        }


        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            homeForm.Show();
        }


        private void ShowError(string errorMessage)
        {
            richTextBoxResults.ForeColor = Color.Red;
            richTextBoxResults.Text = errorMessage;
        }

        private void ResetErrorText()
        {
            richTextBoxResults.ForeColor = Color.DarkGray;
            richTextBoxResults.Text = "";
        }
    }
}

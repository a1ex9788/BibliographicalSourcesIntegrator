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

            labelDBLPReferencesNumber.Text = "0";
            labelIEEEXploreReferencesNumber.Text = "0";
            labelGoogleScholarReferencesNumber.Text = "0";
            labelTotalReferencesNumber.Text = "0";
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

            ShowResults(loadAnswer);
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

        private void ShowResults(LoadAnswer loadAnswer)
        {
            ResetErrorText();

            labelDBLPReferencesNumber.Text = loadAnswer.DBLPNumberOfResults.ToString();
            labelIEEEXploreReferencesNumber.Text = loadAnswer.IEEEXploreNumberOfResults.ToString();
            labelGoogleScholarReferencesNumber.Text = loadAnswer.GoogleScholarNumberOfResults.ToString();

            labelTotalReferencesNumber.Text = PrepareErrorText();


            void ResetErrorText()
            {
                richTextBoxResults.ForeColor = Color.DarkGray;
                richTextBoxResults.Text = "";
            }

            string PrepareErrorText()
            {
                string errors = "";

                errors += "DBLP:";
                foreach (string error in loadAnswer.DBLPErrors)
                {
                    errors += " - " + error + "\n";
                }

                errors += "IEEEXplore:";
                foreach (string error in loadAnswer.IEEEXploreErrors)
                {
                    errors += " - " + error + "\n";
                }

                errors += "Google Scholar:";
                foreach (string error in loadAnswer.GoogleScholarErrors)
                {
                    errors += " - " + error + "\n";
                }

                return errors.Substring(0, errors.Length - 1);
            }
        }
    }
}

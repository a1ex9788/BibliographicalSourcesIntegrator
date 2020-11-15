using BibliographicalSourcesIntegratorContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliographicalSourcesIntegrator
{
    public partial class LoadForm : Form
    {
        private Form homeForm;

        private int currentInitialYear, currentFinalYear;


        public LoadForm(Form homeForm)
        {
            InitializeComponent();

            this.homeForm = homeForm;

            labelDBLPReferencesNumber.Text = "0";
            labelIEEEXploreReferencesNumber.Text = "0";
            labelGoogleScholarReferencesNumber.Text = "0";
            labelTotalReferencesNumber.Text = "0";
            richTextBoxErrors.Text = "";

            currentInitialYear = Convert.ToInt32(numericUpDownInitialYear.Value);
            currentFinalYear = Convert.ToInt32(numericUpDownFinalYear.Value);
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
                richTextBoxErrors.Text = "Any source selected, please select at least one.";

                return;
            }

            LoadRequest loadRequest = new LoadRequest(loadDBLP, loadIEEEXplore, loadGoogleScholar, initialYear, finalYear);
            LoadAnswer loadAnswer;

            LoadingForm loadingForm = new LoadingForm();

            ShowLoadingForm();

            try
            {
                loadAnswer = await RequestsManager.GetRequestsManager().LoadDataFromDataSources(loadRequest);
            }
            catch (HttpRequestException)
            {
                richTextBoxErrors.Text = "It was not possible to connect to the warehouse.";

                return;
            }
            finally
            {
                CloseLoadingForm();
            }

            ShowResults(loadAnswer, loadRequest.LoadFromDBLP, loadRequest.LoadFromIEEEXplore, loadRequest.LoadFromGoogleScholar);


            void ShowLoadingForm()
            {
                new Task(() => loadingForm.ShowDialog()).Start();

                this.Enabled = false;
            }

            void CloseLoadingForm()
            {
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.Close();
                });

                this.Enabled = true;
            }
        }

        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            homeForm.Show();
        }

        private void ShowResults(LoadAnswer loadAnswer, bool loadDBLP, bool loadIEEEXplore, bool loadGoogleScholar)
        {
            int dblpNumberOfResults = loadAnswer.DBLPNumberOfResults;
            int ieeeXploreNumberOfResults = loadAnswer.IEEEXploreNumberOfResults;
            int googleScholarNumberOfResults = loadAnswer.GoogleScholarNumberOfResults;

            labelDBLPReferencesNumber.Text = dblpNumberOfResults.ToString();
            labelIEEEXploreReferencesNumber.Text = ieeeXploreNumberOfResults.ToString();
            labelGoogleScholarReferencesNumber.Text = googleScholarNumberOfResults.ToString();
            labelTotalReferencesNumber.Text = (dblpNumberOfResults + ieeeXploreNumberOfResults + googleScholarNumberOfResults).ToString();

            richTextBoxErrors.Text = PrepareErrorText();


            string PrepareErrorText()
            {
                string errors = "";

                if (loadDBLP && loadAnswer.DBLPErrors.Count > 0)
                {
                    errors += "DBLP:\n";
                    foreach (string error in loadAnswer.DBLPErrors)
                    {
                        errors += " - " + error + "\n";
                    }
                }

                if (loadIEEEXplore && loadAnswer.IEEEXploreErrors.Count > 0)
                {
                    errors += "IEEEXplore:\n";
                    foreach (string error in loadAnswer.IEEEXploreErrors)
                    {
                        errors += " - " + error + "\n";
                    }
                }

                if (loadGoogleScholar && loadAnswer.GoogleScholarErrors.Count > 0)
                {
                    errors += "Google Scholar:\n";
                    foreach (string error in loadAnswer.GoogleScholarErrors)
                    {
                        errors += " - " + error + "\n";
                    }
                }

                return errors;
            }
        }

        private void NumericUpDownInitialYearValueChanged(object sender, EventArgs e)
        {
            int newInitialYear = Convert.ToInt32(numericUpDownInitialYear.Value);

            if (newInitialYear > currentFinalYear)
            {
                currentFinalYear++;
                numericUpDownFinalYear.Value = currentFinalYear;
            }

            currentInitialYear = newInitialYear;
        }

        private void NumericUpDownFinalYearValueChanged(object sender, EventArgs e)
        {
            int newFinalYear = Convert.ToInt32(numericUpDownFinalYear.Value);

            if (newFinalYear < currentInitialYear)
            {
                currentInitialYear--;
                numericUpDownInitialYear.Value = currentInitialYear;
            }

            currentFinalYear = newFinalYear;
        }
    }
}

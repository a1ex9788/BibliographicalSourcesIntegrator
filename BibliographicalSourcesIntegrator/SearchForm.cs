﻿using BibliographicalSourcesIntegratorContracts;
using System;
using System.Net.Http;
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
            bool searchArticle = checkBoxArticle.Checked;
            bool searchBook = checkBoxBook.Checked;
            bool searchCongress = checkBoxCongressComunication.Checked;
            int initialYear = (int) numericUpDownInitialYear.Value;
            int finalYear = (int) numericUpDownFinalYear.Value;
            string author = textBoxAuthor.Text;
            string title = textBoxTitle.Text;

            if (!searchArticle && !searchBook && !searchCongress)
            {
                richTextBoxResults.Text = "Any publication type selected, please select at least one.";

                return;
            }

            SearchRequest searchRequest = new SearchRequest(searchArticle, searchBook, searchCongress, initialYear, finalYear, author, title);
            SearchAnswer searchAnswer;

            LoadingForm loadingForm = new LoadingForm();

            ShowLoadingForm();

            try
            {
                searchAnswer = await RequestsManager.GetRequestsManager().SearchDataInWarehouse(searchRequest);
            }
            catch (HttpRequestException)
            {
                richTextBoxResults.Text = "It was not possible to connect to the warehouse.";

                return;
            }
            finally
            {
                CloseLoadingForm();
            }

            //listViewResults.Items.Add();


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

        private void CleanSearchButtonClick(object sender, EventArgs e)
        {
            textBoxAuthor.Text = "";
            textBoxTitle.Text = "";
            numericUpDownInitialYear.Text = "";
            numericUpDownFinalYear.Text = "";
            checkBoxArticle.Text = "";
            checkBoxBook.Text = "";
            checkBoxCongressComunication.Text = "";
            richTextBoxResults.Text = "";
        }


        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            homeForm.Show();
        }
    }
}

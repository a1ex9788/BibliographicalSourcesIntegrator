using BibliographicalSourcesIntegratorContracts;
using BibliographicalSourcesIntegratorContracts.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliographicalSourcesIntegrator
{
    public partial class SearchForm : Form
    {
        private Form homeForm;

        private int currentInitialYear, currentFinalYear;

        List<TabPage> tabs = new List<TabPage>();


        public SearchForm(Form homeForm)
        {
            InitializeComponent();

            this.homeForm = homeForm;

            richTextBoxArticles.Text = "";

            foreach (TabPage tabPage in tabControlResults.TabPages)
            {
                tabs.Add(tabPage);
            }

            currentInitialYear = Convert.ToInt32(numericUpDownInitialYear.Value);
            currentFinalYear = Convert.ToInt32(numericUpDownFinalYear.Value);
        }


        private async void SearchButtonClick(object sender, EventArgs e)
        {
            richTextBoxArticles.Text = "";

            bool searchArticle = checkBoxArticle.Checked;
            bool searchBook = checkBoxBook.Checked;
            bool searchCongress = checkBoxCongressComunication.Checked;
            int initialYear = (int) numericUpDownInitialYear.Value;
            int finalYear = (int) numericUpDownFinalYear.Value;
            string author = textBoxAuthor.Text;
            string title = textBoxTitle.Text;

            if (!searchArticle && !searchBook && !searchCongress)
            {
                richTextBoxArticles.ForeColor = Color.Red;
                richTextBoxArticles.Text = "Any publication type selected, please select at least one.";
                richTextBoxBooks.ForeColor = Color.Red;
                richTextBoxBooks.Text = "Any publication type selected, please select at least one.";
                richTextBoxCongressComunications.ForeColor = Color.Red;
                richTextBoxCongressComunications.Text = "Any publication type selected, please select at least one.";

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
                richTextBoxArticles.ForeColor = Color.Red;
                richTextBoxArticles.Text = "It was not possible to connect to the warehouse.";
                richTextBoxBooks.ForeColor = Color.Red;
                richTextBoxBooks.Text = "It was not possible to connect to the warehouse.";
                richTextBoxCongressComunications.ForeColor = Color.Red;
                richTextBoxCongressComunications.Text = "It was not possible to connect to the warehouse.";

                CloseLoadingForm();

                return;
            }

            ShowResults(searchAnswer, searchArticle, searchBook, searchCongress);

            CloseLoadingForm();

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

        private void ShowResults(SearchAnswer searchAnswer, bool searchArticle, bool searchBook, bool searchCongress)
        {
            richTextBoxArticles.ForeColor = Color.Black;
            richTextBoxBooks.ForeColor = Color.Black;
            richTextBoxCongressComunications.ForeColor = Color.Black;

            if (searchArticle)
            {
                string text = "";

                for (int i = 0; i < searchAnswer.Articles.Count; i++)
                {
                    Article a = searchAnswer.Articles[i];

                    text += $"{i+1}.- {a.Title}\n";
                    text += $"Year: {a.Year}\n";
                    text += a.Url == null ? "" : $"Url: {a.Url}\n";
                    text += a.InitialPage == null && a.FinalPage == null ? "" : $"Pages: {a.InitialPage} - {a.FinalPage}\n";
                    text += a.Exemplar == null ? "" : $"Volume: {a.Exemplar.Volume}\n";
                    text += a.Exemplar == null ? "" : $"Number: {a.Exemplar.Number}\n";
                    text += a.Exemplar == null ? "" : $"Month: {a.Exemplar.Month}\n";
                    text += a.Exemplar == null ? "" : $"Journal: {a.Exemplar.Journal.Name}\n";
                    text += PrepareAuthors(a.People);
                    text += "\n";
                }

                richTextBoxArticles.Text = text == "" ? "No articles found." : text;

                if (tabControlResults.TabPages.Contains(tabs[0]))
                {
                    tabControlResults.TabPages.Remove(tabs[0]);
                }

                tabControlResults.TabPages.Add(tabs[0]);
            }
            else
            {
                tabControlResults.TabPages.Remove(tabs[0]);
            }

            if (searchBook)
            {
                string text = "";

                for (int i = 0; i < searchAnswer.Books.Count; i++)
                {
                    Book b = searchAnswer.Books[i];

                    text += $"{i + 1}.- {b.Title}\n";
                    text += $"Year: {b.Year}\n";
                    text += b.Url == null ? "" : $"Url: {b.Url}\n";
                    text += b.Editorial == null ? "" : $"Editorial: {b.Editorial}\n";
                    text += PrepareAuthors(b.People);
                    text += "\n";
                }

                richTextBoxBooks.Text = text == "" ? "No books found." : text;

                if (tabControlResults.TabPages.Contains(tabs[1]))
                {
                    tabControlResults.TabPages.Remove(tabs[1]);
                }

                tabControlResults.TabPages.Add(tabs[1]);
            }
            else
            {
                tabControlResults.TabPages.Remove(tabs[1]);
            }

            if (searchCongress)
            {
                string text = "";

                for (int i = 0; i < searchAnswer.CongressComunications.Count; i++)
                {
                    CongressComunication cc = searchAnswer.CongressComunications[i];

                    text += $"{i+1}.- {cc.Title}\n";
                    text += $"Year: {cc.Year}\n";
                    text += cc.Url == null ? "" : $"Url: {cc.Url}\n";
                    text += cc.Congress == null ? "" : $"Congress: {cc.Congress}\n";
                    text += cc.Edition == null ? "" : $"Edition: {cc.Edition}\n";
                    text += cc.Place == null ? "" : $"Place: {cc.Place}\n";
                    text += cc.InitialPage == null && cc.FinalPage == null ? "" : $"Pages: {cc.InitialPage} - {cc.FinalPage}\n";
                    text += PrepareAuthors(cc.People);
                    text += "\n";
                }

                richTextBoxCongressComunications.Text = text == "" ? "No congress comunications found." : text;

                if (tabControlResults.TabPages.Contains(tabs[2]))
                {
                    tabControlResults.TabPages.Remove(tabs[2]);
                }

                tabControlResults.TabPages.Add(tabs[2]);
            }
            else
            {
                tabControlResults.TabPages.Remove(tabs[2]);
            }


            string PrepareAuthors(ICollection<Person_Publication> people)
            {
                if (people.Count == 0)
                {
                    return "";
                }

                string text = "Authors: ";

                foreach (Person person in people.Select(p => p.Person))
                {
                    text += person.Name + " " + person.Surnames + ", ";
                }

                return text.Substring(0, text.Length - 2) + "\n";
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
            richTextBoxArticles.Text = "";
        }


        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            homeForm.Show();
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

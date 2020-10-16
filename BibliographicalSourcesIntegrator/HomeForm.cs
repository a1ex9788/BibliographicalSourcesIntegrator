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
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }


        private void SearchButtonClick(object sender, EventArgs e)
        {
            LoadNewForm(new SearchForm());
        }

        private void LoadButtonClick(object sender, EventArgs e)
        {
            LoadNewForm(new LoadForm());
        }


        private void LoadNewForm(Form form)
        {
            form.Show();

            this.Hide();
        }
    }
}

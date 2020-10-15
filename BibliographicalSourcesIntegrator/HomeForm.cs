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

        private void searchButtonClick(object sender, EventArgs e)
        {
            this.Hide();

            SearchForm sf = new SearchForm();

            sf.Show();
        }

        private void loadButtonClick(object sender, EventArgs e)
        {
            this.Hide();

            LoadForm sf = new LoadForm();

            sf.Show();
        }
    }
}

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


        private void SearchButtonClick(object sender, EventArgs e)
        {

        }

        private void CleanSearchButtonClick(object sender, EventArgs e)
        {

        }


        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            homeForm.Show();
        }
    }
}

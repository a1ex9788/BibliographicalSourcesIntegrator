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
        public SearchForm()
        {
            InitializeComponent();
        }


        private void SearchButtonClick(object sender, EventArgs e)
        {

        }

        private void CleanSearchButtonClick(object sender, EventArgs e)
        {

        }


        private void CloseApplication(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

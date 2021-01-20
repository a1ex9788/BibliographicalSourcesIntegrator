using System.Drawing;
using System.Windows.Forms;

namespace BibliographicalSourcesIntegrator
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();

            pictureBox.Image = Image.FromFile("loading.gif");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp1
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Image Files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp"; // Filtr plików [cite: 297]
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                originalImage = new Bitmap(file);
                pbOriginal.Image = originalImage; // Wyświetlenie wczytanego obrazu [cite: 310, 313]
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Najpierw wczytaj obraz!");
                return;
            }

            btnProcess.Enabled = false;

            [cite_start]// Tworzymy zadania równoległe (można również użyć klasy Thread [cite: 290])
            Task.Run(() =>
            {
                Bitmap bmp1 = null, bmp2 = null, bmp3 = null, bmp4 = null;

                // Użycie Parallel.Invoke do zrównoleglenia nakładania filtrów
                Parallel.Invoke(
                    () => bmp1 = ImageFilters.ApplyGrayscale(originalImage),
                    () => bmp2 = ImageFilters.ApplyNegative(originalImage),
                    () => bmp3 = ImageFilters.ApplyThreshold(originalImage),
                    () => bmp4 = ImageFilters.ApplyMirror(originalImage)
                );

                // Bezpieczne przypisanie wyników do kontrolek UI z wątku pobocznego
                Invoke((Action)(() =>
                {
                    pbFilter1.Image = bmp1;
                    pbFilter2.Image = bmp2;
                    pbFilter3.Image = bmp3;
                    pbFilter4.Image = bmp4;
                    btnProcess.Enabled = true;
                }));
            });
        }
    }
}
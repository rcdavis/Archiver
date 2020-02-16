using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Archiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ArchiveProject archiveProject = new ArchiveProject();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PAK files (*.pak)|*.pak";

            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                if (dialog.FileName.EndsWith(".pak"))
                {
                    using Stream stream = File.Create(dialog.FileName);
                    IArchiveExporter archiveExporter = new PAKArchiveExporter();
                    archiveExporter.Export(archiveProject, stream);
                }
            }
        }

        private void importBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

using System.Windows;

namespace Archiver
{
    /// <summary>
    /// Interaction logic for AddTreeFolderDialog.xaml
    /// </summary>
    public partial class AddTreeFolderDialog : Window
    {
        public string FolderName { get; set; }

        public AddTreeFolderDialog()
        {
            InitializeComponent();
            DataContext = this;

            FolderName = "Folder";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

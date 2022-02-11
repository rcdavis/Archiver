using System;
using System.Windows;
using System.Windows.Input;

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

            Activated += AddTreeFolderDialog_Activated;
        }

        private void AddTreeFolderDialog_Activated(object sender, EventArgs e)
        {
            Keyboard.Focus(nameTB);
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

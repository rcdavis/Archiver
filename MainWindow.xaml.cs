using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Archiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ArchiveProject archiveProject = new ArchiveProject();
        private readonly TreeViewItem root;

        public MainWindow()
        {
            InitializeComponent();

            root = new TreeViewItem()
            {
                Header = archiveProject.Root.Name
            };
            treeView.Items.Add(root);
        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "Export Archive",
                Filter = "PAK files (*.pak)|*.pak",
                DefaultExt = ".pak"
            };

            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                IArchiveExporter archiveExporter = GetArchiveExporter(dialog.FileName);

                if (archiveExporter != null)
                {
                    using Stream stream = File.Create(dialog.FileName);
                    archiveExporter.Export(archiveProject, stream);
                }
            }
        }

        private void importBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "File To Add To Archive",
                Filter = "All files (*.*)|*.*"
            };

            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                TreeViewItem selectedItem = root;
                if (treeView.SelectedItem != null)
                    selectedItem = treeView.SelectedItem as TreeViewItem;

                TreeViewItem item = new TreeViewItem()
                {
                    Header = dialog.SafeFileName
                };
                selectedItem.Items.Add(item);
                selectedItem.IsExpanded = true;
            }
        }

        private void RemoveNode_Click(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem != null)
            {
                TreeViewItem selectedItem = treeView.SelectedItem as TreeViewItem;
                selectedItem.Items.Clear();

                if (selectedItem != root)
                {
                    RecursiveRemoveNode(root, selectedItem);
                }
            }
        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            AddTreeFolderDialog dialog = new AddTreeFolderDialog();
            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                TreeViewItem selectedItem = root;
                if (treeView.SelectedItem != null)
                    selectedItem = treeView.SelectedItem as TreeViewItem;

                TreeViewItem item = new TreeViewItem()
                {
                    Header = dialog.FolderName
                };
                selectedItem.Items.Add(item);
                selectedItem.IsExpanded = true;
            }
        }

        private void RecursiveRemoveNode(TreeViewItem root, TreeViewItem itemToRemove)
        {
            if (root.Items.Contains(itemToRemove))
            {
                root.Items.Remove(itemToRemove);
            }
            else
            {
                foreach(TreeViewItem item in root.Items)
                {
                    RecursiveRemoveNode(item, itemToRemove);
                }
            }
        }

        private IArchiveExporter GetArchiveExporter(string fileName)
        {
            if (fileName.EndsWith(".pak"))
                return new PAKArchiveExporter();

            return null;
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (treeView.SelectedItem != null && treeView.SelectedItem != root)
            {
                TreeViewItem item = treeView.SelectedItem as TreeViewItem;
                AddTreeFolderDialog dialog = new AddTreeFolderDialog
                {
                    FolderName = item.Header.ToString()
                };

                if (dialog.ShowDialog().GetValueOrDefault(false))
                {
                    item.Header = dialog.FolderName;
                }
            }
        }
    }
}

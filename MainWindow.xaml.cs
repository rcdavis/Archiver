using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Globalization;

namespace Archiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArchiveProject archiveProject = new ArchiveProject();
        private TreeViewItem root;

        public MainWindow()
        {
            InitializeComponent();
            SetupTreeView(archiveProject);
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = Resource.export_dialog_title,
                Filter = "PAK files (*.pak)|*.pak",
                DefaultExt = "pak"
            };

            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                IArchiveExporter archiveExporter = GetArchiveExporter(dialog.FileName);

                if (archiveExporter != null)
                {
                    using FileStream stream = File.Create(dialog.FileName);
                    archiveExporter.Export(archiveProject, stream);
                }
            }
        }

        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = Resource.add_node_dialog_title,
                Filter = "All files (*.*)|*.*"
            };

            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                if (Directory.Exists(dialog.FileName))
                {
                    Console.WriteLine(string.Format("{0} is a directory and not a file.", dialog.FileName));
                    return;
                }
                TreeViewItem selectedItem = root;
                if (treeView.SelectedItem != null)
                    selectedItem = treeView.SelectedItem as TreeViewItem;

                ArchiveProjectEntry entry = new ArchiveProjectEntry
                {
                    Name = dialog.SafeFileName,
                    Path = dialog.FileName,
                    IsFile = true
                };
                ((ArchiveProjectEntry)selectedItem.Header).AddChild(entry);

                TreeViewItem item = new TreeViewItem()
                {
                    Header = entry
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
                ArchiveProjectEntry entry = selectedItem.Header as ArchiveProjectEntry;

                if (entry.Parent != null)
                {
                    entry.Parent.RemoveChild(entry);
                }
                else
                {
                    archiveProject.Root.RemoveChild(entry);
                }

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
                    selectedItem = (TreeViewItem)treeView.SelectedItem;

                ArchiveProjectEntry entry = new ArchiveProjectEntry
                {
                    Name = dialog.FolderName
                };
                ((ArchiveProjectEntry)selectedItem.Header).AddChild(entry);

                TreeViewItem item = new TreeViewItem()
                {
                    Header = entry
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
                    ArchiveProjectEntry entry = item.Header as ArchiveProjectEntry;
                    entry.Name = dialog.FolderName;
                    // HACK: Must force the Header to change in order for the text to
                    // display correctly in the TreeView.
                    item.Header = null;
                    item.Header = entry;
                }
            }
        }

        private void OpenProjectCmd(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = Resource.open_project_dialog_title,
                Filter = "Archive Project (*.arproj)|*.arproj"
            };

            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                try
                {
                    using FileStream stream = File.OpenRead(dialog.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(ArchiveProject));
                    archiveProject = (ArchiveProject)serializer.Deserialize(stream);
                    SetupTreeView(archiveProject);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Open Project Error");
                }
            }
        }

        private void SaveProjectCmd(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = Resource.save_project_dialog_title,
                Filter = "Archive Project (*.arproj)|*.arproj",
                DefaultExt = "arproj"
            };

            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                try
                {
                    using FileStream stream = File.OpenWrite(dialog.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(ArchiveProject));
                    serializer.Serialize(stream, archiveProject);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Save Project Error");
                }
            }
        }

        private void SetupTreeView(ArchiveProject project)
        {
            treeView.Items.Clear();
            root = new TreeViewItem()
            {
                Header = project.Root
            };
            treeView.Items.Add(root);

            project.Root.Children.ForEach(child => AddTreeNode(root, child));
            project.BuildParents();
        }

        private void AddTreeNode(TreeViewItem parent, ArchiveProjectEntry entry)
        {
            TreeViewItem node = new TreeViewItem()
            {
                Header = entry
            };
            parent.Items.Add(node);

            entry.Children.ForEach(child => AddTreeNode(node, child));
        }

        private void EnglishUS_Click(object sender, RoutedEventArgs e)
        {
            UpdateLanguage(new CultureInfo("en-US"));
        }

        private void Japanese_Click(object sender, RoutedEventArgs e)
        {
            UpdateLanguage(new CultureInfo("ja-JP"));
        }

        private void UpdateLanguage(CultureInfo culture)
        {
            Resource.Culture = culture;
            Title = Resource.archiver;
            menuFile.Header = Resource.file;
            menuOpen.Header = Resource.open;
            menuSave.Header = Resource.save;
            menuEdit.Header = Resource.edit;
            menuLanguage.Header = Resource.language;
            menuLanguageEnUS.Header = Resource.language_en_us;
            menuLanguageJP.Header = Resource.language_jp;
            exportBtn.Content = Resource.export;
        }
    }
}

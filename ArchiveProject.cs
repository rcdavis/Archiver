using System.Collections.Generic;

namespace Archiver
{
    /// <summary>
    /// Represents a tree structure for archives.
    /// </summary>
    public class ArchiveProject
    {
        public ArchiveProjectEntry Root { get; set; }

        public ArchiveProject()
        {
            Root = new ArchiveProjectEntry
            {
                Name = "Root"
            };
        }

        public ICollection<ArchiveProjectEntry> GetFiles()
        {
            ICollection<ArchiveProjectEntry> entries = new List<ArchiveProjectEntry>();

            RecursiveFileCapture(entries, Root);

            return entries;
        }

        private void RecursiveFileCapture(ICollection<ArchiveProjectEntry> entries, ArchiveProjectEntry curEntry)
        {
            if (curEntry.IsFile)
            {
                entries.Add(curEntry);
            }

            curEntry.Children.ForEach(child => RecursiveFileCapture(entries, child));
        }

        public void BuildParents()
        {
            BuildParents(Root);
        }

        private void BuildParents(ArchiveProjectEntry entry)
        {
            foreach (var child in entry.Children)
            {
                child.Parent = entry;
                BuildParents(child);
            }
        }
    }
}

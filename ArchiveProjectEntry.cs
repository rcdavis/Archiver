using System.Collections.Generic;

namespace Archiver
{
    /// <summary>
    /// Entry into an archive tree structure.
    /// </summary>
    class ArchiveProjectEntry
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsFile { get; set; }
        public ArchiveProjectEntry Parent { get; set; }
        public ICollection<ArchiveProjectEntry> Children { get; private set; }

        public ArchiveProjectEntry()
        {
            Children = new List<ArchiveProjectEntry>();
            IsFile = false;
        }

        public void AddChild(ArchiveProjectEntry entry)
        {
            entry.Parent = this;
            entry.Url = Url + entry.Url;
            Children.Add(entry);
        }

        public void RemoveChild(ArchiveProjectEntry entry)
        {
            if (Children.Contains(entry))
            {
                Children.Remove(entry);
                entry.Parent = null;
                entry.Url = entry.Name;
            }
        }
    }
}

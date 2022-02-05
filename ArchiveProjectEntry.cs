using System.Collections.Generic;
using System.Xml.Serialization;

namespace Archiver
{
    /// <summary>
    /// Entry in an archive tree structure.
    /// </summary>
    public class ArchiveProjectEntry
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string RelativePath
        {
            get
            {
                if (Parent != null)
                    return Parent.RelativePath + "\\" + Name;

                return Name;
            }
        }
        public bool IsFile { get; set; }
        [XmlIgnore]
        public ArchiveProjectEntry Parent { get; set; }
        public List<ArchiveProjectEntry> Children { get; private set; }

        public ArchiveProjectEntry()
        {
            Children = new List<ArchiveProjectEntry>();
            IsFile = false;
        }

        public void AddChild(ArchiveProjectEntry entry)
        {
            entry.Parent = this;
            Children.Add(entry);
        }

        public void RemoveChild(ArchiveProjectEntry entry)
        {
            if (Children.Contains(entry))
            {
                Children.Remove(entry);
                entry.Parent = null;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

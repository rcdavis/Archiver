using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Archiver
{
    /// <summary>
    /// Exporter for PAK file archives.
    /// </summary>
    class PAKArchiveExporter : IArchiveExporter
    {
        /// <summary>
        /// First Header within a PAK file. Contains signature and directory location.
        /// </summary>
        class PAKMainHeader
        {
            public const int SIZE_OF = 12;
            public const int SIGNATURE_LENGTH = 4;

            public char[] Signature { get; private set; }
            public int DirectoryOffset { get; set; }
            public int DirectoryLength { get; set; }

            public int NumFiles
            {
                get { return DirectoryLength / PAKFileHeader.SIZE_OF; }
            }

            public PAKMainHeader()
            {
                Signature = new char[SIGNATURE_LENGTH];
                Signature[0] = 'P';
                Signature[1] = 'A';
                Signature[2] = 'C';
                Signature[3] = 'K';

                DirectoryOffset = SIZE_OF;
                DirectoryLength = 0;
            }

            public void Read(BinaryReader reader)
            {
                Signature = reader.ReadChars(SIGNATURE_LENGTH);
                DirectoryOffset = reader.ReadInt32();
                DirectoryLength = reader.ReadInt32();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(Signature, 0, SIGNATURE_LENGTH);
                writer.Write(DirectoryOffset);
                writer.Write(DirectoryLength);
            }
        }

        /// <summary>
        /// Header for every file entry within a PAK file.
        /// </summary>
        class PAKFileHeader
        {
            public const int SIZE_OF = 64;
            public const int FILE_NAME_LENGTH = 64;

            private char[] name = new char[FILE_NAME_LENGTH];

            public char[] Name
            {
                get { return name; }
                set
                {
                    value.CopyTo(name, 0);
                }
            }
            public int Offset { get; set; }
            public int Size { get; set; }

            public PAKFileHeader()
            {
                Offset = 0;
                Size = 0;
            }

            public void Read(BinaryReader reader)
            {
                Name = reader.ReadChars(FILE_NAME_LENGTH);
                Offset = reader.ReadInt32();
                Size = reader.ReadInt32();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(Name, 0, FILE_NAME_LENGTH);
                writer.Write(Offset);
                writer.Write(Size);
            }
        }

        public void Export(ArchiveProject project, Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);

            PAKMainHeader mainHeader = new PAKMainHeader();

            ICollection<ArchiveProjectEntry> entries = project.GetFiles();
            ICollection<PAKFileHeader> fileHeaders = new List<PAKFileHeader>();

            foreach(ArchiveProjectEntry entry in entries)
            {
                PAKFileHeader header = new PAKFileHeader
                {
                    Name = entry.Name.ToCharArray()
                };

                byte[] contents = File.ReadAllBytes(entry.Path);

                fileHeaders.Add(header);
            }
        }

        public void Import(ArchiveProject project, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}

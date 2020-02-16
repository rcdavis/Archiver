using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Archiver
{
    class PAKArchiveExporter : IArchiveExporter
    {
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

        class PAKFileHeader
        {
            public const int SIZE_OF = 64;
            public const int FILE_NAME_LENGTH = 64;

            public char[] Name { get; private set; }
            public int Offset { get; set; }
            public int Size { get; set; }

            public PAKFileHeader()
            {
                Name = new char[FILE_NAME_LENGTH];
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
            throw new NotImplementedException();
        }

        public void Import(ArchiveProject project, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}

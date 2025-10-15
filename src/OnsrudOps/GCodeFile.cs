using Microsoft.Win32;
using OnsrudOps.ReCut;
using OnsrudOps.Serial;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace OnsrudOps.src
{
    /// <summary>
    /// Class to represent a G Code file
    /// </summary>
    internal class GCodeFile : INotifyPropertyChanged, IFile
    {
        // the default g code extension
        private string extension = ".anc";
        private string _fileContents = string.Empty;
        private string _fileName = string.Empty;
        private string _fullName = string.Empty;
        private string _materialName = string.Empty;
        private bool _modified = false;

        // the counter of how many files have been created to use as ID
        private static int counter = 0;

        public static readonly GCodeFile EmptyFile = new();

        /// <summary>
        /// Create a new G Code file object from a file path or file contents
        /// </summary>
        /// <remarks>
        /// Constructor determines whether it was passed G Code text or a filepath
        /// </remarks>
        /// <param name="text"></param>
        public GCodeFile(string text)
        {
            // constructor was handed a filepath
            if (File.Exists(text))
            {
                _fileName = text[(text.LastIndexOf('\\') + 1)..];
                _fullName = text;
                _fileContents = File.ReadAllText(text);
                _materialName = GetMaterialName();
            }
            else
            {
                _fileContents = text;
                _materialName = GetMaterialName();
                _modified = true;
                _fileName = GetProgramName();
            }
            ID = ++counter;
        }

        public GCodeFile() { }

        /// <summary>
        /// ID to identify a file
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// The file name without the full path
        /// </summary>
        public string FileName
        {
            get => _fileName;
            set { _fileName = value; }
        }

        /// <summary>
        /// The full path of the file
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set { _fullName = value; }
        }

        /// <summary>
        /// The material used in this program
        /// </summary>
        public string MaterialName => _materialName;

        /// <summary>
        /// The contents of the G Code file
        /// </summary>
        public string FileContents
        {
            get { return _fileContents; }
            set
            {
                if (_fileContents != value)
                {
                    _fileContents = value;
                    _modified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Marks whether the file has been modified.
        /// </summary>
        public bool Modified
        {
            get => _modified;
        }

        /// <summary>
        /// Save this file back to the same directory
        /// </summary>
        public async Task<bool> SaveAsync()
        {
            try
            {
                // if the file was created inside of the application
                if (!File.Exists(_fullName))
                {
                    SaveFileDialog sfd = new()
                    {
                        Title = "Save G Code File",
                        DefaultDirectory = $"C:\\users\\{Environment.UserName}\\DropBox\\",
                        AddExtension = true,
                        DefaultExt = ".anc",
                        FileName = $"{_fileName}.anc"
                    };
                    if (sfd.ShowDialog() == true)
                    {
                        _fullName = sfd.FileName;
                    }
                    else
                    {
                        return false;
                    }
                }
                using FileStream fs = File.Create(_fullName);
                await fs.WriteAsync(UTF8Encoding.UTF8.GetBytes(_fileContents));
                _modified = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file {_fullName}!\r\r{ex.Message}");
                return false;
            }
        }

        private string GetMaterialName()
        {
            //Regex regex = new(@"MATERIAL: (.+)$");
            //Match m = regex.Match(FileContents);
            //return m.Groups[0].Value == "" ? "No Material Found" : m.Groups[0].Value;
            string searchString = "// MATERIAL: ";
            foreach (string line in _fileContents.Split("\r\n"))
            {
                if (line.StartsWith(searchString))
                {
                    return line[searchString.Length..];
                }
            }
            return "No Material Found";
        }

        private string GetProgramName()
        {
            //Regex regex = new(@"MATERIAL: (.+)$");
            //Match m = regex.Match(FileContents);
            //return m.Groups[0].Value == "" ? "No Material Found" : m.Groups[0].Value;
            string searchString = "P";
            foreach (string line in _fileContents.Split("\r\n"))
            {
                if (line.StartsWith(searchString))
                {
                    return line[searchString.Length..];
                }
            }
            return "No Name";
        }

        public override string ToString()
        {
            return FileName;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

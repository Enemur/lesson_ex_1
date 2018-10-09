using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ex_1.Annotations;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ex_1
{
    public class AppMV : INotifyPropertyChanged
    {
        public ObservableCollection<string> CurrentFiles { get; set; }
        private string _textBoxString;
        private FolderBrowserDialog _openDialog;
        private ICommand _searchButtonClick;
        
        public ICommand SearchButtonClick => _searchButtonClick;

        public string TextBoxString
        {
            get => _textBoxString;
            set
            {
                _textBoxString = value;
                OnPropertyChanged("TextBoxString");
            }
        }

        public AppMV()
        {
            CurrentFiles = new ObservableCollection<string>();

            _searchButtonClick = new RelayCommand(obj =>
            {
                if (_textBoxString == string.Empty)
                    return;

                CurrentFiles.Clear();

                if (_openDialog.ShowDialog() == DialogResult.OK)
                {
                    var dirPath = _openDialog.SelectedPath;
                    if (dirPath == null)
                        return;
                    var result = SearchCurrentWord(dirPath, _textBoxString);

                    foreach (var item in result)
                        CurrentFiles.Add(item);
                }
            });

            _openDialog = new FolderBrowserDialog();
        }

        private IEnumerable<string> SearchCurrentWord(string dirPath, string word)
        {
            var result = new List<string>();

            var files = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);

            foreach (var filePath in files)
            {
                var line = GetCountLineOfWordInFile(word, filePath);
                if (line != 0)
                    result.Add($"{filePath}, line: {line}");
            }

            return result;
        }

        private int GetCountLineOfWordInFile(string word, string filePath)
        {
            var indexOfWord = 0;

            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    indexOfWord++;
                    var line = reader.ReadLine();

                    var words = line.Split(' ').ToList();
                    if (words.Contains(word))
                        break;
                }
            }

            return indexOfWord;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

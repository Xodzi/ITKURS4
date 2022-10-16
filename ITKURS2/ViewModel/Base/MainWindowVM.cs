using ITKURS2.Infrastructure;
using ITKURS2.Views;
using Microsoft.Win32;
using MyRSA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ITKURS2.ViewModel.Base
{
    class MainWindowVM: ViewModel
    {
        #region Поля
        private string _newName;
        private string _openText;

        private string _closeText;

        // static public List<RSA> AllKeys { get; protected set; } заменил на ObservableCollection чтобы в DataGrid обновлялось

        private static ObservableCollection<RSA> _ObKeys = new ObservableCollection<RSA>();

        private static RSA _currentKey;

        private int _publickey;

        #endregion

        #region Свойства
        public ObservableCollection<RSA> ObKeys { get { return _ObKeys; } }


        public int Publickey { get { return _publickey;} set
            {
                Set(ref _publickey, value);
            } }
        public RSA CurrentKey
        {
            get => _currentKey; set
            {
                Set(ref _currentKey, value);
                //OnPropertyChanged();
            }
        }

        public string OpenText
        {
            get => _openText; set
            {
                Set(ref _openText, value);
               // OnPropertyChanged();
            }
        }

        public string CloseText
        {
            get => _closeText; set
            {
                Set(ref _closeText, value);
               //OnPropertyChanged();
            }
        }
        public string NewName
        {
            get => _newName; set
            {
                Set(ref _newName, value);
                //OnPropertyChanged();
            }
        }

        #endregion


        #region AllCommands


        #region EncrypteCommand
        public ICommand EncrypteCommand { get; }

        private void OnEncrypteCommandExecuted(object p) 
        {
            CloseText = CurrentKey.Encryption2(OpenText);
            OnPropertyChanged("CloseText");
        }
        private bool CanEncrypteCommandExecute(object p)
        {
            if (_openText != null & CurrentKey != null & _openText!= "") return true;
            return false;
        }
        #endregion

        #region DecrypteCommand
        public ICommand DecrypteCommand { get; }

        private void OnDecrypteCommandExecuted(object p)
        {
            OpenText = CurrentKey.Decryption2(CloseText);
            OnPropertyChanged("OpenText");
        }
        private bool CanDecrypteCommandExecute(object p)
        {
            if (_closeText != null & CurrentKey != null & _closeText != "") return true;
            return false;
        }
        #endregion

        #region KeysMgCommand
        public ICommand KeysMgCommand { get; }

        private void OnKeysMgExecuted(object p)
        {
            KeysMngmt TempWindow = new KeysMngmt();
            //KeysMngmtVM TempModel = new KeysMngmtVM();
            if(CurrentKey != null)
            {
                foreach (var key in _ObKeys)
                {
                    key.Selected = false;
                }
                _currentKey.Selected = true;
            }
            TempWindow.ShowDialog();

            //var temp = new Window();
            //temp.DataContext = 
            //temp.Show();
        }
        private bool CanKeysMgExecute(object p) => true;

        #endregion

        #region AddNewKeyCommand
        public ICommand AddNewKeyCommand { get; }

        private void OnAddNewKeyExecuted(object p)
        {
            RSA NewKey = new RSA(NewName);
            // AllKeys.Add(NewKey);
            ObKeys.Add(NewKey);
            OnPropertyChanged("");
        }
        private bool CanAddnewKeyExecute(object p)
        {
            if (NewName != null) return true;
            return true;
        }
        #endregion

        #region OpenFileDialog

        public ICommand OpenFileDialog { get; }

        private void OnOpenFileDialogExecuted(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) OpenText = File.ReadAllText(openFileDialog.FileName);
        }
        private bool CanOpenFileDialogExecute(object p)
        {
            return true;
        }
        #endregion

        #region SaveFileDialog
        public ICommand SaveFileDialog { get; }

        private void OnSaveFileDialogExecuted(object p)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog();
            if (openFileDialog.ShowDialog() == true) OpenText = File.ReadAllText(openFileDialog.FileName);
        }
        private bool CanSaveFileDialogExecute(object p)
        {
            return true;
        }

        #endregion

        #endregion




        public MainWindowVM()
        {
            EncrypteCommand = new LambdaCommand(OnEncrypteCommandExecuted, CanEncrypteCommandExecute);
            KeysMgCommand = new LambdaCommand(OnKeysMgExecuted, CanKeysMgExecute);
            DecrypteCommand = new LambdaCommand(OnDecrypteCommandExecuted, CanDecrypteCommandExecute);
            AddNewKeyCommand = new LambdaCommand(OnAddNewKeyExecuted, CanAddnewKeyExecute);
            OpenFileDialog = new LambdaCommand(OnOpenFileDialogExecuted, CanOpenFileDialogExecute);
        }
    }
}

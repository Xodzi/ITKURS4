using System;
using System.Collections.Generic;
using System.Linq;
using MyRSA;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ITKURS2.Infrastructure;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;

namespace ITKURS2.ViewModel.Base
{
    class KeysMngmtVM : MainWindowVM
    {

        private string _newName;
        //public RSA TempKey
        //{
        //    get => CurrentKey; set
        //    {
        //        if (value == null) return;
        //        if (CurrentKey == value) return;
        //        _currentKey = value;
        //    }
        //}

        public string NewName
        {
            get => _newName; set
            {
                Set(ref _newName, value);
                OnPropertyChanged();
            }
        }


        #region AddNewKey

        public ICommand AddNewKeyCommand { get; }

        private void OnAddNewKeyExecuted(object p)
        {
            RSA NewKey = new RSA(NewName);
           // AllKeys.Add(NewKey);
            ObKeys.Add(NewKey);
           // OnPropertyChanged("");
        }
        private bool CanAddnewKeyExecute(object p)
        {
            if (NewName != null) return true;
            return false;
        }

        #endregion

        public KeysMngmtVM()
        {
            AddNewKeyCommand = new LambdaCommand(OnAddNewKeyExecuted, CanAddnewKeyExecute);
        }

    }
}

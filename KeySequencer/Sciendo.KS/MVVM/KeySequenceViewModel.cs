using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Sciendo.KeySequencer.Repository;

namespace Sciendo.KS.MVVM
{
    internal class KeySequenceViewModel:ViewModelBase
    {
        private readonly IKeyDataProvider _keyDataProvider;

        public KeySequenceViewModel()
        {
            _keyDataProvider = new KeyDataProvider();
            FromKeys=new ObservableCollection<string>();
            FromKeys = FromKeys.AddKeys(_keyDataProvider.GetAllKeys());

        }
        private ObservableCollection<string> _fromKeys;
        private string _selectedFromKey;

        public ObservableCollection<string> FromKeys
        {
            get { return _fromKeys; }
            set
            {
                if (_fromKeys != value)
                {
                    _fromKeys = value;
                    RaisePropertyChanged(() => FromKeys);
                }
            }
        }

        public string SuperToKey { get; private set; }

        public string HighToKey { get; private set; }

        public string MediumToKey { get; private set; }

        public string Low1ToKey { get; set; }

        public string Low2ToKey { get; private set; }
        private void PerformClose()
        {
            Application.Current.Shutdown();
        }
        public ICommand Close { get; private set; }

        public void InitalizeView()
        {
            LoadToKeys(new string[] { "Low...", "Low...","Medium...", "High...", "Super..." });
            Close = new RelayCommand(PerformClose);
        }


        public string SelectedFromKey
        {
            get { return _selectedFromKey; }
            set
            {
                if (value != _selectedFromKey)
                {
                    _selectedFromKey = value;
                    LoadToKeys(_keyDataProvider.GetAllKeySequencesFrom(_selectedFromKey).Select(k => k.Key).ToArray());
                    RaisePropertyChanged(()=>SuperToKey);
                    RaisePropertyChanged(()=>HighToKey);
                    RaisePropertyChanged(()=>MediumToKey);
                    RaisePropertyChanged(()=>Low2ToKey);
                    RaisePropertyChanged(()=>Low1ToKey);

                }
            }
        }
        private void LoadToKeys(string[] toKeys)
        {
            SuperToKey = toKeys[4];
            HighToKey = toKeys[3];
            MediumToKey = toKeys[2];
            Low1ToKey = toKeys[1];
            Low2ToKey = toKeys[0];
        }
    }
}

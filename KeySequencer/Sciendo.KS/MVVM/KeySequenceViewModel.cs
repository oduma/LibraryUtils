using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
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
        private static void PerformClose()
        {
            Application.Current.Shutdown();
        }
        public ICommand Close { get; private set; }

        public ICommand SelectSuper { get; private set; }

        public ICommand SelectHigh { get; private set; }

        public ICommand SelectMedium { get; private set; }

        public ICommand SelectLow1 { get; private set; }

        public ICommand SelectLow2 { get; private set; }

        public void InitalizeView()
        {
            LoadToKeys(new[] { "Low...", "Low...","Medium...", "High...", "Super..." });
            Close = new RelayCommand(PerformClose);
            SelectSuper= new RelayCommand(PerformSelectSuper);
            SelectHigh=new RelayCommand(PerformSelectHigh);
            SelectMedium = new RelayCommand(PerformSelectMedium);
            SelectLow1=new RelayCommand(PerformSelectLow1);
            SelectLow2=new RelayCommand(PerformSelectLow2);
        }

        private void PerformSelectLow2()
        {
            SelectedFromKey = Low2ToKey;
        }

        private void PerformSelectLow1()
        {
            SelectedFromKey = Low1ToKey;
        }

        private void PerformSelectMedium()
        {
            SelectedFromKey = MediumToKey;
        }

        private void PerformSelectHigh()
        {
            SelectedFromKey = HighToKey;
        }

        private void PerformSelectSuper()
        {
            SelectedFromKey = SuperToKey;
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
                    RaisePropertyChanged(()=>SelectedFromKey);

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

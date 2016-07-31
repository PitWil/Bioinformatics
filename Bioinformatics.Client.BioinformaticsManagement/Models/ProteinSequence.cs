using GalaSoft.MvvmLight;

namespace Bioinformatics.Client.BioinformaticsManagement.Models
{
    public class ProteinSequence : ViewModelBase
    {
        private bool _isChecked;

        public ProteinSequence(string sequence)
        {
            Sequence = sequence;
        }

        public string Sequence { get; }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;

                RaisePropertyChanged("IsChecked");
            }
        }
    }
}
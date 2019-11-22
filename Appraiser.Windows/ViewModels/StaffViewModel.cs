using Appraiser.DTOs;

namespace Appraiser.Windows.ViewModels
{
    public class StaffViewModel : ViewModelBase
    {
        private readonly StaffDTO _model;

        public int Id
        {
            get => _model.Id;
            set
            {
                _model.Id = value;
                OnPropertyChanged();
            }
        }

        public string Logon
        {
            get => _model.Logon;
            set
            {
                _model.Logon = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _model.Name;
            set
            {
                _model.Name = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _model.Email;
            set
            {
                _model.Email = value;
                OnPropertyChanged();
            }
        }

        public string Manager => _model.Manager?.Name;

        public StaffViewModel(StaffDTO model)
        {
            _model = model;
        }
    }
}
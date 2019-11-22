using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Appraiser.Client;

namespace Appraiser.Windows.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ApiClient _client;

        public ObservableCollection<AppraisalViewModel> Appraisals { get; } = new ObservableCollection<AppraisalViewModel>();

        public MainViewModel(ApiClient client)
        {
            _client = client;
        }

        public async Task Load()
        {
            var appraisals = await _client.AppraisalsGetAsync();

            foreach (var a in appraisals)
                Appraisals.Add(new AppraisalViewModel(a));
        }
    }
}

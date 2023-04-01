using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using CyberSiberiaApp.Views;
using System.ComponentModel;

namespace CyberSiberiaApp.ViewModels
{
    public class FlatsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        INavigation _navigator;

        private Flat _selectedFlat;
        public int FacilityId { get; set; }

        public List<Flat> Flats { get; set; } 

        public Flat SelectedFlat
        {
            get { return _selectedFlat; }
            set
            {
                _selectedFlat = value;
                if (_selectedFlat != null)
                {
                    int id = _selectedFlat.Id;
                    _navigator.PushAsync(new DefectsPage(id));
                    UpdateFlats();
                }
                Notify("SelectedFlat");
            }
        }
        public FlatsViewModel()
        {
            UpdateFlats();
        }

        public void UpdateFlats()
        {
            using(Context context = new Context())
            {
                Flats = (from flat in context.Flats
                         where flat.FacilityId == FacilityId
                         select flat).ToList();
            }
            Notify("Flats");
        }

        public void GetNavigation(INavigation navigation)
        {
            _navigator = navigation;
        }

        public void Notify(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ButtonCommand AddFlat
        {
            get
            {
                return new ButtonCommand(async () =>
                {
                    await _navigator.PushAsync(new AddFlatPage(this));

                    UpdateFlats();
                });
            }
        }
    }
}

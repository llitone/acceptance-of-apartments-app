using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using CyberSiberiaApp.Views;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace CyberSiberiaApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        INavigation _navigator;
        Facility _selectedFacility;

        public List<Facility> Facilities { get; set; }

        public Facility SelectedFacility
        {
            get { return _selectedFacility; }
            set
            {
                _selectedFacility = value;
                if (_selectedFacility != null)
                {
                    int id = _selectedFacility.Id;
                    _navigator.PushAsync(new FlatsPage(this, id));
                    UpdateFacilities();
                }
                Notify("SelectedFacility");
            }
        }

        public MainViewModel()
        {
            UpdateFacilities();
        }

        public async void UpdateFacilities()
        {
            using (Context context = new Context())
            {
                Facilities = await context.Facilities.ToListAsync();
            }
            Notify("Facilities");
        }

        public void GetNavigation(INavigation navigation)
        {
            _navigator = navigation;
        }

        public void Notify(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ButtonCommand AddFacility
        {
            get
            {
                return new ButtonCommand(async () =>
                {
                    await _navigator.PushAsync(new AddFacilityPage(this));

                    UpdateFacilities();
                });
            }
        }
    }
}

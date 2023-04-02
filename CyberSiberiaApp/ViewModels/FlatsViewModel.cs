using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using CyberSiberiaApp.Views;
using System.ComponentModel;

namespace CyberSiberiaApp.ViewModels
{
    public class FlatsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action Close;
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
                    _navigator.PushAsync(new DefectsPage(this, id));
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

        public ButtonCommand DeleteFacility
        {
            get
            {
                return new ButtonCommand( () =>
                {
                    using(Context context = new())
                    {
                        List<Flat> deletedFlats = (from facility in context.Facilities
                                                  where facility.Id == FacilityId
                                                  join flat in context.Flats on
                                                  facility.Id equals flat.FacilityId
                                                  select flat).ToList();

                        List<Defect> deletedDefects = (from flat in deletedFlats
                                                      join defect in context.Defects on
                                                      flat.Id equals defect.FlatId
                                                      select defect).ToList();

                        List<Model.DB.EntityModels.Image> deletedImages = (from defect in deletedDefects
                                                     join img in context.Images on
                                                     defect.Id equals img.DefectId
                                                     select img).ToList();

                        Facility deleted = context.Facilities.Where(x => x.Id == FacilityId)
                                                             .FirstOrDefault();

                        context.Images.RemoveRange(deletedImages);
                        context.Defects.RemoveRange(deletedDefects);
                        context.Flats.RemoveRange(deletedFlats);
                        context.Facilities.Remove(deleted);

                        context.SaveChanges();
                    }
                    Close?.Invoke();
                });
            }
        }

    }
}

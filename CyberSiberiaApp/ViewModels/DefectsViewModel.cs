using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using CyberSiberiaApp.Views;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CyberSiberiaApp.ViewModels
{
    public class DefectsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action Close;

        private INavigation _navigator;
        private Defect _selectedDefect;
        private int _flatId;

        public string FlatNumber { get; set; }
        public int FlatId
        {
            get { return _flatId; }
            set
            {
                _flatId = value;
                using (Context context = new())
                {
                    FlatNumber = (from flat in context.Flats
                                  where flat.Id == _flatId
                                  select flat).FirstOrDefault().Number;
                }
                Notify("FlatId");
                Notify("FlatNumber");
            }
        }
        public List<Defect> Defects { get; set; }

        public Defect SelectedDefect
        {
            get { return _selectedDefect; }
            set
            {
                _selectedDefect = value;
                if (_selectedDefect != null)
                {
                    int id = _selectedDefect.Id;
                    _navigator.PushAsync(new DefectPage(this, id));
                    UpdateDefects();
                }
                Notify("SelectedDefect");
            }
        }
        public DefectsViewModel() 
        {
            
        }

        public void UpdateDefects()
        {
            using(Context context = new())
            {
                Defects = (from defect in context.Defects
                           where defect.FlatId == FlatId
                           select defect).ToList();
                Defects.Reverse();
            }
            Notify("Defects");
        }

        public void GetNavigation(INavigation navigation)
        {
            _navigator = navigation;
        }

        public void Notify(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ButtonCommand AddDefect
        {
            get
            {
                return new ButtonCommand(async () =>
                {
                    await _navigator.PushAsync(new AddDefectPage(this));

                    UpdateDefects();
                });
            }
        }

        public ButtonCommand DeleteFlat
        {
            get
            {
                return new ButtonCommand(() =>
                {
                    using (Context context = new())
                    {
                        List<Defect> deletedDefects = (from flat in context.Flats
                                                       where flat.Id == FlatId
                                                       join defect in context.Defects on
                                                       flat.Id equals defect.FlatId
                                                       select defect).ToList();

                        List<Model.DB.EntityModels.Image> deletedImages = 
                                                    (from defect in deletedDefects
                                                     join img in context.Images on
                                                     defect.Id equals img.Id
                                                     select img).ToList();

                        Flat deleted = context.Flats.Where(x => x.Id == FlatId)
                                                             .FirstOrDefault();

                        context.Images.RemoveRange(deletedImages);
                        context.Defects.RemoveRange(deletedDefects);
                        context.Flats.Remove(deleted);

                        context.SaveChanges();
                    }
                    Close?.Invoke();
                });
            }
        }
    }
}

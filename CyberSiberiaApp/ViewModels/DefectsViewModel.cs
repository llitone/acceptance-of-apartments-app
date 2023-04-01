using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using CyberSiberiaApp.Views;
using System.ComponentModel;

namespace CyberSiberiaApp.ViewModels
{
    public class DefectsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private INavigation _navigator;
        public int FlatId { get; set; }
        public List<Defect> Defects { get; set; }
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
                    await _navigator.PushAsync(new AddDefectPage(FlatId));

                    UpdateDefects();
                });
            }
        }
    }
}

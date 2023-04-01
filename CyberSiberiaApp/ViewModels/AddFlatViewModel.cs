using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using System.ComponentModel;
using System.Net;

namespace CyberSiberiaApp.ViewModels
{
    public class AddFlatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action Close;

        private string _number;

        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                Notify("Number");
            }
        }

        public int FacilityId { get; set; }

        public AddFlatViewModel()
        {

        }

        public void Notify(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ButtonCommand AddFlat
        {
            get
            {
                return new ButtonCommand(() =>
                {
                    using (Context context = new())
                    {
                        context.Flats.Add(new Flat()
                        {
                            FacilityId = FacilityId,
                            Number = Number
                        });
                        context.SaveChanges();
                    }
                    Close?.Invoke();
                });
            }
        }
    }
}

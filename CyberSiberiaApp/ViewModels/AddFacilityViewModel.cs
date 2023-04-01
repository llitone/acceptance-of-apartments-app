using CyberSiberiaApp.Model.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSiberiaApp.ViewModels
{
    public class AddFacilityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Action Close;

        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                Notify("Address");
            }
        }

        public void Notify(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ButtonCommand AddFacility
        {
            get
            {
                return new ButtonCommand(() =>
                {
                    using(Context context = new())
                    {
                        context.Facilities.Add(new Model.DB.EntityModels.Facility()
                        {
                            Address = _address
                        });
                        context.SaveChanges();
                    }
                    Close?.Invoke();
                });
            }
        }
    }
}

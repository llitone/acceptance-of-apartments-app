using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CyberSiberiaApp.ViewModels
{
    public class AddDefectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action Close;

        private Category _selectedCategory;
        private string _description;
        private string _gost;

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                Notify("SelectedCategory");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                Notify("Description");
            }
        }

        public string Gost
        {
            get { return _gost; }
            set
            {
                _gost = value;
                Notify("Gost");
            }
        }

        public List<Category> Categories { get; set; }

        public ObservableCollection<Model.DB.EntityModels.Image> Images { get; set; }

        public int FlatId { get; set; }

        public AddDefectViewModel()
        {
            GetCategories();
            Images = new ObservableCollection<Model.DB.EntityModels.Image>();
        }

        public void Notify(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void GetCategories()
        {
            using(Context context = new())
            {
                Categories = context.Categories.ToList();
            }
            Notify("Categories");
        }

        public ButtonCommand AddImage
        {
            get
            {
                return new ButtonCommand( async () =>
                {
                    try
                    {
                        FileResult result = await MediaPicker.PickPhotoAsync();
                        Images.Add(new Model.DB.EntityModels.Image()
                        {
                            Path = result.FullPath
                        });
                        Notify("Images");
                    }
                    catch
                    {
                        Notify("Images");
                    }
                });
            }
        }

        public ButtonCommand AddDefect
        {
            get
            {
                return new ButtonCommand(() =>
                {
                    using (Context context = new())
                    {
                        context.Defects.Add(new Defect()
                        {
                            FlatId = FlatId,
                            Category = context.Categories
                                .Where(x => x.Id == _selectedCategory.Id)
                                .FirstOrDefault(),
                            Description = _description,
                            Gost = _gost
                        });
                        context.SaveChanges();
                        int defectId = context.Defects.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        foreach(var img in Images)
                        {
                            img.DefectId = defectId;
                            context.Images.Add(img);
                        }
                        context.SaveChanges();
                    }
                    Close?.Invoke();
                });
            }
        }
    }
}

using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using System.ComponentModel;

namespace CyberSiberiaApp.ViewModels
{
    public class DefectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Defect Defect { get; set; }

        public List<Model.DB.EntityModels.Image> Images { get; set; }

        public int DefectId { get; set; }

        public void UpdateDefect()
        {
            using(Context context = new())
            {
                Defect = (from defect in context.Defects
                          where defect.Id == DefectId
                          select defect).FirstOrDefault();

                Images = (from image in context.Images
                          where image.DefectId == DefectId
                          select image).ToList();
            }
            Notify("Defect");
            Notify("Images");
        }

        public void Notify(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CyberSiberiaApp.ViewModels
{
    public class DefectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action Close;

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

                Defect.Category = (from category in context.Categories
                                   where category.Id == Defect.CategoryId
                                   select category).FirstOrDefault();

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

        public ButtonCommand DeleteDefect
        {
            get
            {
                return new ButtonCommand(() =>
                {
                    using (Context context = new())
                    {
                        List<Model.DB.EntityModels.Image> deletedImages =
                            (from defect in context.Defects
                             where defect.Id == DefectId
                             join img in context.Images on
                             defect.Id equals img.DefectId
                             select img).ToList();

                        Defect deleted = context.Defects.Where(x => x.Id == DefectId)
                                                        .FirstOrDefault();

                        context.Images.RemoveRange(deletedImages);
                        context.Defects.Remove(deleted);

                        context.SaveChanges();
                    }
                    Close?.Invoke();
                });
            }
        }
    }
}

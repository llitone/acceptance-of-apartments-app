using CyberSiberiaApp.Model.DB.EntityModels;

namespace CyberSiberiaApp.Model.DB
{
    public static class CategoriesList
    {
        public static List<Category> Categories = new List<Category>()
        {
            new Category() { Name = "Оконные конструкции" },
            new Category() { Name = "Витражное остекление" },
            new Category() { Name = "Входная дверь" },
            new Category() { Name = "Стены" },
            new Category() { Name = "Потолки" },
            new Category() { Name = "Полы" },
            new Category() { Name = "Электромонтажные/слаботочные работы" },
            new Category() { Name = "Отопление" },
            new Category() { Name = "Стояки ГХВС/канализация" },
            new Category() { Name = "Вентиляция" }
        };
    }
}

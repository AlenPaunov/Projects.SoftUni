namespace ProjectsSoftuni.Web.ViewModels.Settings
{
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Mapping;

    public class SettingViewModel : IMapFrom<Setting>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}

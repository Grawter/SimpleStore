
namespace SimpleStore.ViewModels.Supporting_tools
{
    public class FilterViewModel
    {
        public string SelectedName { get; private set; }    // введенное имя

        public string SelectedAvailability { get; private set; }    // введенная доступность
        
        public FilterViewModel(string name, string availability)
        {
            SelectedName = name;
            SelectedAvailability = availability;
        }

    }
}
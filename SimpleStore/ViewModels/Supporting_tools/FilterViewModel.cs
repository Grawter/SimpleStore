
namespace SimpleStore.ViewModels.Supporting_tools
{
    public class FilterViewModel
    {
        public int SelectedId { get; private set; } // введенное Id
        public string SelectedNameOrEmail { get; private set; } // введенное имя или email
        public string SelectedAvailability { get; private set; } // введенная доступность
        public string SelectedStatus { get; private set; } // введенный статус

        // Для модерирования магазина и админки
        public FilterViewModel(string NameOrEmail)
        {
            SelectedNameOrEmail = NameOrEmail;
        }

        // Для магазина
        public FilterViewModel(string name, string availability)
        {
            SelectedNameOrEmail = name;
            SelectedAvailability = availability;
        }

        // Для заказов
        public FilterViewModel(int? Id, string email, string status)
        {
            SelectedId = (int)Id;
            SelectedNameOrEmail = email;
            SelectedStatus = status;
        }

    }
}
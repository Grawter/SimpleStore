namespace SimpleStore.ViewModels.Supporting_tools
{
    public enum SortState
    {
        // Для магазина
        NameAsc, // по названию по возрастанию
        NameDesc, // по названию по убыванию
        CompanyAsc, // по компании по возрастанию
        CompanyDesc, // по компании по убыванию
        PriceAsc, // по цене по возрастанию
        PriceDesc, // по цене по убыванию
        CapacityAsc, // по ёмкости по возрастанию
        CapacityDesc, // по ёмкости по убыванию

        // Для админки
        NamesAsc,
        NamesDesc,
        SurnameAsc,
        SurnameDesc,
        EmailAsc,
        EmailDesc,
        PhoneAsc,
        PhoneDesc
    }

    public class SortViewModel
    {
        // Для магазина
        public SortState NameSort { get; private set; } // значение для сортировки по названию
        public SortState CompanySort { get; private set; } // значение для сортировки по компании
        public SortState PriceSort { get; private set; } // значение для сортировки цене
        public SortState CapacitySort { get; private set; } // значение для сортировки по ёмкости

        // Для админки
        public SortState NamesSort { get; private set; } // значение для сортировки по имени
        public SortState SurnameSort { get; private set; } // значение для сортировки по фамилии
        public SortState EmailSort { get; private set; } // значение для сортировки по эл. адресу
        public SortState PhoneSort { get; private set; } // значение для сортировки по телефону

        public SortState Current { get; private set; } // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            // Для магазина
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            CompanySort = sortOrder == SortState.CompanyAsc ? SortState.CompanyDesc : SortState.CompanyAsc;
            PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            CapacitySort = sortOrder == SortState.CapacityAsc ? SortState.CapacityDesc : SortState.CapacityAsc;

            // Для админки
            NamesSort = sortOrder == SortState.NamesAsc ? SortState.NamesDesc : SortState.NamesAsc;
            SurnameSort = sortOrder == SortState.SurnameAsc ? SortState.SurnameDesc : SortState.SurnameAsc;
            EmailSort = sortOrder == SortState.EmailAsc ? SortState.EmailDesc : SortState.EmailAsc;
            PhoneSort = sortOrder == SortState.PhoneAsc ? SortState.PhoneDesc : SortState.PhoneAsc;

            Current = sortOrder; 
        }
    }
}
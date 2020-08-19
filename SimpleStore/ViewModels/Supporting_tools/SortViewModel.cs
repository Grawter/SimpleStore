namespace SimpleStore.ViewModels.Supporting_tools
{
    public enum SortState
    {
        // Для магазина и заказа
        NameAsc, // по названию по возрастанию
        NameDesc, // по названию по убыванию
        CompanyAsc, // по компании по возрастанию
        CompanyDesc, // по компании по убыванию
        PriceAsc, // по цене по возрастанию
        PriceDesc, // по цене по убыванию
        CapacityAsc, // по ёмкости по возрастанию
        CapacityDesc, // по ёмкости по убыванию

        // Для админки и заказа
        NamesAsc, // по имени по возрастанию
        NamesDesc, // по имени по убыванию 
        SurnameAsc, // по фамилии по возрастанию
        SurnameDesc, // по фамилии по убыванию
        EmailAsc, // по эл.почте по возрастанию
        EmailDesc, // по эл. почте по убыванию
        PhoneAsc, // по телефону по возрастанию
        PhoneDesc, // по телефону по убыванию

        // Только для заказа
        IdAsc, // по номеру заказа по возрастанию
        IdDesc, // по номеру заказа по убыванию
        IdProductAsc, // по номеру товара по возрастанию
        IdProductDesc, // по номеру товара по убыванию
        CountAsc, // по количеству товара по возрастани
        CountDesc, // по количеству по убыванию
        AddressAsc, // по адресу по возрастани
        AddressDesc, // по адресу по убыванию
        StatusAsc, // по статусу по возрастани
        StatusDesc // по статусу по убыванию
    }

    public class SortViewModel
    {
        // Для магазина и заказа
        public SortState NameSort { get; private set; } // значение для сортировки по названию
        public SortState CompanySort { get; private set; } // значение для сортировки по компании
        public SortState PriceSort { get; private set; } // значение для сортировки цене
        public SortState CapacitySort { get; private set; } // значение для сортировки по ёмкости

        // Для админки и заказа
        public SortState NamesSort { get; private set; } // значение для сортировки по имени
        public SortState SurnameSort { get; private set; } // значение для сортировки по фамилии
        public SortState EmailSort { get; private set; } // значение для сортировки по эл. адресу
        public SortState PhoneSort { get; private set; } // значение для сортировки по телефону

        // Только для заказа
        public SortState IdSort { get; private set; } // значение для сортировки по номеру заказа
        public SortState IdProductSort { get; private set; } // значение для сортировки по номеру продукта
        public SortState CountSort { get; private set; } // значение для сортировки по количеству
        public SortState AddressSort { get; private set; } // значение для сортировки по адресу
        public SortState StatusSort { get; private set; } // значение для сортировки по статусу

        public SortState Current { get; private set; } // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            // Для магазина и заказа
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            CompanySort = sortOrder == SortState.CompanyAsc ? SortState.CompanyDesc : SortState.CompanyAsc;
            PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            CapacitySort = sortOrder == SortState.CapacityAsc ? SortState.CapacityDesc : SortState.CapacityAsc;

            // Для админки и заказа
            NamesSort = sortOrder == SortState.NamesAsc ? SortState.NamesDesc : SortState.NamesAsc;
            SurnameSort = sortOrder == SortState.SurnameAsc ? SortState.SurnameDesc : SortState.SurnameAsc;
            EmailSort = sortOrder == SortState.EmailAsc ? SortState.EmailDesc : SortState.EmailAsc;
            PhoneSort = sortOrder == SortState.PhoneAsc ? SortState.PhoneDesc : SortState.PhoneAsc;

            // Только для заказа
            IdSort = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
            IdProductSort = sortOrder == SortState.IdProductAsc ? SortState.IdProductDesc : SortState.IdProductAsc;
            CountSort = sortOrder == SortState.CountAsc ? SortState.CountDesc : SortState.CountAsc;
            AddressSort = sortOrder == SortState.AddressAsc ? SortState.AddressDesc : SortState.AddressAsc;
            StatusSort = sortOrder == SortState.StatusAsc ? SortState.StatusDesc : SortState.StatusAsc;

            Current = sortOrder; 
        }
    }
}
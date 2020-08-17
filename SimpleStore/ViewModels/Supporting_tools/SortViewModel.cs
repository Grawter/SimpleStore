namespace SimpleStore.ViewModels.Supporting_tools
{
    public enum SortState
    {
        NameAsc, // по имени по возрастанию
        NameDesc, // по имени по убыванию
        CompanyAsc, // по компании по возрастанию
        CompanyDesc, // по компании по убыванию
        PriceAsc, // по цене по возрастанию
        PriceDesc, // по цене по убыванию
        CapacityAsc, // по ёмкости по возрастанию
        CapacityDesc // по ёмкости по убыванию
    }

    public class SortViewModel
    {
        public SortState NameSort { get; private set; } // значение для сортировки по имени
        public SortState CompanySort { get; private set; } // значение для сортировки по компании
        public SortState PriceSort { get; private set; } // значение для сортировки цене
        public SortState CapacitySort { get; private set; } // значение для сортировки по ёмкости
        public SortState Current { get; private set; } // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            CompanySort = sortOrder == SortState.CompanyAsc ? SortState.CompanyDesc : SortState.CompanyAsc;
            PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            CapacitySort = sortOrder == SortState.CapacityAsc ? SortState.CapacityDesc : SortState.CapacityAsc;
            Current = sortOrder;
        }
    }
}
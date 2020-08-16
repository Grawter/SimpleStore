using SimpleStore.Models.Shop;
using System.Collections.Generic;

namespace SimpleStore.ViewModels.Supporting_tools
{
    public class IndexViewModel
    {
        public IEnumerable<Phone> Phones { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}

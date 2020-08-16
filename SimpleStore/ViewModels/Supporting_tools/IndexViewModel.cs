using SimpleStore.Models.Shop;
using System.Collections.Generic;

namespace SimpleStore.ViewModels.Supporting_tools
{
    public class IndexViewModel
    {
        public IEnumerable<Case> Cases { get; set; }

        public IEnumerable<Headphone> Headphones { get; set; }

        public IEnumerable<Phone> Phones { get; set; }

        public IEnumerable<Powerbank> Powerbankss { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}

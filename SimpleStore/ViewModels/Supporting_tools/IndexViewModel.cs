using System.Collections.Generic;
using SimpleStore.Models;

namespace SimpleStore.ViewModels.Supporting_tools
{
    public class IndexViewModel
    {
        public IEnumerable<SimpleStore.Models.User> Users { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}

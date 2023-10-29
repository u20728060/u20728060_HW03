using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u20728060_HW03.Models
{
    public class HomeViewModel
    {
        public IPagedList<student> students { get; set; }

        public IPagedList<book> books { get; set; }
    }
}
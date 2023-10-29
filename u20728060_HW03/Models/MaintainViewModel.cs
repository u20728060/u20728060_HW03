﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u20728060_HW03.Models
{
    public class MaintainViewModel
    {
        public IPagedList<author> authors { get; set; }

        public IPagedList<type> types { get; set; }

        public IPagedList<borrow> borrows { get; set; }
    }
}
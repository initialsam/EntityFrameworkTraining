﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTraining
{
    public interface IModificationHistory
    {
        DateTime DateModified { get; set; }
        DateTime DateCreated { get; set; }
      
    }
}

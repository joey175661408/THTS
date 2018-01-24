using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THTS.MVVM
{
    public interface ISequencedObject
    {
        int SequenceNumber { get; set; }

    }
}

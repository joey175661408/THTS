using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace THTS.MVVM
{ 
    public static class SequencingService
    {
       
        public static ObservableCollection<T> SetCollectionSequence<T>(ObservableCollection<T> targetCollection) where T : ISequencedObject
        {
            var sequenceNumber = 1;

            foreach (ISequencedObject sequencedObject in targetCollection)
            {
                sequencedObject.SequenceNumber = sequenceNumber;
                sequenceNumber++;
            }

            return targetCollection;
        }

        public static ICollection<T> SetCollectionSequence<T>(ICollection<T> targetCollection) where T : ISequencedObject
        {
            var sequenceNumber = 1;

            foreach (ISequencedObject sequencedObject in targetCollection)
            {
                sequencedObject.SequenceNumber = sequenceNumber;
                sequenceNumber++;
            }

            return targetCollection;
        }
    }
}

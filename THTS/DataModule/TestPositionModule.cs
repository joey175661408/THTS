using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.DataAccess;

namespace THTS.DataModule
{
    public class TestPositionModule
    {
        public string TestPositionName { get; set; }
        public ObservableCollection<Sensor> SensorsList { get; set; }
        public Sensor TestPositionID { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineBox
{
    public class Config
    {
        public static readonly int Port = Convert.ToInt32(ConfigurationManager.AppSettings["port"] ?? "9999");
    }
}

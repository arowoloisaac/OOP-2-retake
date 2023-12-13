using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arowolo_Project_2.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum IncomeType
    {
        Salary = 1,
        Scholarship = 2,
        Wages = 3,
        Others = 4,
    }
}

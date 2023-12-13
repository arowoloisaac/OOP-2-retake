using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arowolo_Project_2.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExpenseType
    {
        food = 1,
        restaurants = 2,
        medicine = 3,
        sport = 4,
        taxi = 5,
        rent = 6,
        investments = 7,
        clothes = 8,
        fun = 9,
        other = 10,
    }
}

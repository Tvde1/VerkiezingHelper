using System;
using System.Data;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Helpers.DAL
{
    public class ObjectFactory
    {
        public static Election CreateElection(DataRow row)
        {
            if (row == null) return null;
            var seats = row.IsNull("AmountOfSeats") ? null : (int?) row["AmountOfSeats"];
            var date = row.IsNull("Date") ? null : (DateTime?) row["Date"];
            return new Election((int)row["ElectionPk"], (string)row["Name"], seats, date);
        }
    }
}
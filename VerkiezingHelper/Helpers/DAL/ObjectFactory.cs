using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Helpers.DAL
{
    public static class ObjectFactory
    {
        public static Election CreateElection(DataRow row)
        {
            if (row == null) return null;
            var seats = row.IsNull("AmountOfSeats") ? null : (int?) row["AmountOfSeats"];
            var date = row.IsNull("Date") ? null : (DateTime?) row["Date"];
            return new Election((int)row["ElectionPk"], (string)row["Name"], seats, date);
        }

        public static Coalition CreateCoalition(DataRow row)
        {
            return row == null ? null : new Coalition((int)row["CoalitionPk"], (string)row[""]);
        }

        public static Party CreateParty(DataRow row)
        {
            return row == null ? null : new Party((int)row["PartyPk"], (string)row["Name"], (string)row["LeadCandidate"], (int)row["AmountOfVotes"]);
        }

        public static List<T> CreateList<T>(DataTable data, Func<DataRow, T> func)
        {
            return (from DataRow dataRow in data.Rows select func(dataRow)).ToList();
        }
    }
}
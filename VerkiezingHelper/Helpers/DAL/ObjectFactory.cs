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
            return new Election((int) row["ElectionPk"], (string) row["Name"], seats, date);
        }

        public static Coalition CreateCoalition(DataRow row)
        {
            var repo = new Repository();
            if (row == null) return null;
            var president = repo.GetParty((int) row["PresidentFk"], (int) row["ElectionFk"], false);
            return new Coalition((int) row["CoalitionPk"], (string) row["Name"], president);
        }

        public static Party CreateParty(DataRow row)
        {
            if (row == null) return null;

            var votes = row.Table.Columns.Contains("AmountOfVotes") && !row.IsNull("AmountOfVotes")
                ? (int) row["AmountOfVotes"]
                : 0;
            var percent = row.Table.Columns.Contains("PercentOfVotes") && !row.IsNull("PercentOfVotes")
                ? (double) row["PercentOfVotes"]
                : 0;
            var seats = row.Table.Columns.Contains("AmountOfSeats") && !row.IsNull("AmountOfSeats")
                ? (int) row["AmountOfSeats"]
                : 0;
            return new Party((int) row["PartyPk"], (string) row["Name"], (string) row["LeadCandidate"], votes, percent,
                seats);
        }

        public static List<T> CreateList<T>(DataTable data, Func<DataRow, T> func)
        {
            return (from DataRow dataRow in data.Rows select func(dataRow)).ToList();
        }
    }
}
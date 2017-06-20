﻿using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Helpers.DAL.Contexts
{
    internal interface IDatabaseContext
    {
        void Save(Party party);
        void Save(Coalition coalition);
        void Save(Election election);
        void Delete(Party party);
        void Delete(Coalition coalition);
        void Delete(Election election);
    }
}
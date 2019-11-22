using System.Collections.Generic;
using Appraiser.Data.Models;

namespace Appraiser.Mapping
{
    public abstract class Mapper<TD, TM>
    {
        protected readonly ChangeChecker<TD> _checker;

        public virtual List<Change> Changes => _checker.Changes;

        protected Mapper(ChangeChecker<TD> checker)
        {
            _checker = checker;
        }

        public abstract TM ToModel(TD from, TM to);
        public abstract TD ToDTO(TM from, TD to);
    }
}
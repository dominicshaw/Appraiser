using System;
using System.Collections.Generic;
using Appraiser.Data;
using Appraiser.Data.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Appraiser.Mapping
{
    [UsedImplicitly]
    public class ChangeChecker<TDTO>
    {
        private readonly string _table;
        private readonly Func<TDTO, int> _keySelector;
        private readonly AppraiserContext _context;
        private readonly string _username;

        public List<Change> Changes { get; } = new List<Change>();

        public ChangeChecker(IHttpContextAccessor httpContextAccessor, string table, Func<TDTO, int> keySelector, AppraiserContext context)
        {
            _table       = table;
            _keySelector = keySelector;
            _context     = context;
            _username    = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        public void Check<TValue, TModel>(string field,
            Func<TDTO, TValue> fromSelector,
            Func<TModel, TValue> toSelector,
            Action<TDTO, TModel> setter,
            TDTO from,
            TModel to)
        {
            var fromValue = fromSelector(from);
            var toValue = toSelector(to);

            if (fromValue is null && toValue is null) // both null, no change required
                return;

            if (!(fromValue is null) && fromValue.Equals(toValue)) // from is not null and is already set to the target value, no change required
                return;

            setter.Invoke(from, to);

            var change = new Change
            {
                Table    = _table,
                Field    = field,
                KeyId    = _keySelector(from),
                Old      = toValue?.ToString() ?? "null",
                New      = fromValue?.ToString() ?? "null",
                Username = _username,
                When     = DateTime.UtcNow
            };

            Changes.Add(change);
            _context.Add(change);
        }
    }
}
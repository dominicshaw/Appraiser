using Appraiser.Data.Models;
using Appraiser.DTOs;

namespace Appraiser.Mapping
{
    public class ChangeMapper
    {
        public static Change ToModel(ChangeDTO from, Change to)
        {
            to.Id       = from.Id       ;
            to.When     = from.When     ;
            to.Username = from.Username ;
            to.Table    = from.Table    ;
            to.Field    = from.Field    ;
            to.KeyId    = from.KeyId    ;
            to.Old      = from.Old      ;
            to.New      = from.New      ;

            return to;
        }

        public static ChangeDTO ToDTO(Change from, ChangeDTO to)
        {
            to.Id       = from.Id       ;
            to.When     = from.When     ;
            to.Username = from.Username ;
            to.Table    = from.Table    ;
            to.Field    = from.Field    ;
            to.KeyId    = from.KeyId    ;
            to.Old      = from.Old      ;
            to.New      = from.New      ;

            return to;
        }
    }
}
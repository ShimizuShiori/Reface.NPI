using System;

namespace Reface.NPI
{
    public class Constant
    {
        public static readonly Type TYPE_INPIDAO = typeof(INpiDao<>);

        public const string RESULT_FIELD_NAME_COUNT = "C";

        public const string PARAMETER_NAME_BEGIN_ROW_NUMBER = "BEGINRN";

        public const string PARAMETER_NAME_END_ROW_NUMBER = "ENDRN";

        public const string PARAMETER_SUFFIX_BETWEEN_BEGIN = "_BEGIN";
        public const string PARAMETER_SUFFIX_BETWEEN_END = "_END";

        public const string MACHINE_NAME_SELECT = "Select";
        public const string MACHINE_NAME_INSERT = "Insert";
        public const string MACHINE_NAME_UPDATE = "Update";
        public const string MACHINE_NAME_DELETE = "Delete";
        public const string MACHINE_NAME_COUNT = "Count";
    }
}

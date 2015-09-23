using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services
{
    public class VacancyFilterParams
    {
        private int _page;
        private int _pageSize;
        private string _sortColumn;
        private bool _reverceSort;
        private string _filter;
        private string _pool;
        private string _status;
        private string _addedBy;
        private string[] _pools = new string[0];
        private string[] _statuses = new string[0];
        private string[] _addedByArray = new string[0];
        public int Page { get { return _page; } set { _page = value; } }
        public int PageSize { get { return _pageSize; } set { _pageSize = value; } }
        public string SortColumn { get { return _sortColumn; } set { _sortColumn = TrimExtraChars(value); } }
        public bool ReverceSort { get { return _reverceSort; } set { _reverceSort = value; } }
        public string Filter { get { return _filter; } set { _filter = TrimExtraChars(value); } }
        public string Pool
        {
            get { return _pool; }
            set
            {
                _pool = TrimExtraChars(value);
                _pools = SplitParams(_pool);
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = TrimExtraChars(value);
                _statuses = SplitParams(_status);
            }
        }
        public string AddedBy
        {
            get { return _addedBy; }
            set
            {
                _addedBy = TrimExtraChars(value);
                _addedByArray = SplitParams(_addedBy);
            }
        }
        public string[] Pools { get { return _pools; } }
        public string[] Statuses { get { return _statuses; } }
        public string[] AddedByArray { get { return _addedByArray; } }
        private String TrimExtraChars(string value)
        {
            var _quote = '"';
            return value.Trim(_quote).Trim();
        }
        private string[] SplitParams(string value)
        {
            if (value != string.Empty)
            {
                if (value.IndexOf(',') != -1)
                    return value.Split(',');
                else
                    return new string[] { value };
            }
            else return new string[0];
        }
    }
}
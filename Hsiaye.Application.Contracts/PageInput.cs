using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class PageInput
    {
        private int pageIndex;

        public int PageIndex
        {
            get
            {
                if (pageIndex < 1)
                {
                    return 1;
                }
                else
                {
                    return pageIndex;
                }
            }
            set
            {
                pageIndex = value;
            }
        }

        private int pageSize;

        public int PageSize
        {
            get
            {
                if (pageSize > 100)
                {
                    return 100;
                }
                else
                {
                    return pageSize;
                }
            }
            set
            {
                pageSize = value;
            }
        }
    }
}

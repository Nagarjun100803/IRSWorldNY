using System;
using System.Collections.Generic;
using System.Text;

namespace IRSClassLib
{
    public interface IHeaderRecord
    {
        bool isHeader();
        bool isHeader(String buff);
        String toHeader();
        bool check_header(out String message);
        bool HasDate { get; }
        DateTime FileDate { get; }
    }
}

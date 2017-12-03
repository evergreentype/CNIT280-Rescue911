﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rescue_911
{
    public class Base_Station_Records
    {
        private string Record;
        private DateTime Date;

        public Base_Station_Records(string xRecord, DateTime xDate)
        {
            Record = xRecord;
            Date = xDate;
        }

        public Base_Station_Records()
        {
        }

        public String GetRecord()
        {
            return Record;
        }
        
        public DateTime GetDate()
        {
            return Date;
        }
        public void SetDate(DateTime xDate)
        {
            this.Date = xDate;
        }
        public void SetRecord(string xRecord)
        {
            this.Record = xRecord;
        }
    }
}

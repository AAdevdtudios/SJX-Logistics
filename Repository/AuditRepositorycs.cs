using SjxLogistics.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Repository
{
    public interface IAuditRepository
        {
            void InsertAuditLogs(Audit objauditmodel);
        }
    }



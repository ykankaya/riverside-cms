using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Data
{
    public class CmsDatabaseManagerFactory : DatabaseManagerFactory
    {
        private IOptions<DbOptions> _options;

        private static string _connectionString;

        public CmsDatabaseManagerFactory(IOptions<DbOptions> options)
        {
            _options = options;
        }

        protected override string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                    _connectionString = _options.Value.CmsConnectionString;
                return _connectionString;
            }
        }
    }
}

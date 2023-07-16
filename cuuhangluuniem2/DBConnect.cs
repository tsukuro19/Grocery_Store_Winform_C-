using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuuhangluuniem2
{
    class DBConnect
    {
        private string connect;
        
        public string myConnect()
        {
            connect = @"Data Source=DESKTOP-1DI3VRS;Initial Catalog=DBPoS;Integrated Security=True";
            return connect;
        }
    }
}

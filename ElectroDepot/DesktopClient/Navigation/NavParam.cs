using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Navigation
{
    public enum NavOperation
    {
        Add,
        Preview
    }

    public class NavParam
    {
        private NavOperation _operation;
        private object _payload;

        public NavOperation Operation
        {
            get
            {
                return _operation;
            }
        }

        public object Payload
        {
            get
            {
                return _payload;
            }
        }

        public NavParam(NavOperation operation, object payload)
        {
            _operation = operation;
            _payload = payload;
        }

        public static NavParam Create(NavOperation operation, object payload)
        {
            return new NavParam(operation, payload);
        }
    }
}

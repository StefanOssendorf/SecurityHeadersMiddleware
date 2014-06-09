using System.Collections.Generic;
using System.Threading.Tasks;

namespace OwinContrib.Security {
    public delegate Task AppFunc(IDictionary<string, object> env);

    public delegate AppFunc MidFunc(AppFunc next);

    public delegate void BuildFunc(MidFunc builder);
}
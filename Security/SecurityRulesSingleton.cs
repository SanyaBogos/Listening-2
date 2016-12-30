using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Security
{
    public class SecurityRulesSingleton
    {
        internal SecurityRules Rules { get; private set; }
        private static SecurityRulesSingleton _instance;
        public static SecurityRulesSingleton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SecurityRulesSingleton();
                return _instance;
            }
        }

        private SecurityRulesSingleton()
        {
            using (var sr = File.OpenText("security.json"))
            {
                var text = sr.ReadToEnd();
                Rules = JsonConvert.DeserializeObject<SecurityRules>(text);
            }
        }
    }
}

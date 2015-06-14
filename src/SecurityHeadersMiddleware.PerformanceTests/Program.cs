using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SecurityHeadersMiddleware.PerformanceTests {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Beginn {0}", DateTime.Now);
            //PerfConfigToHeaderValue();
            PerfAddHostToConfig();
            Console.WriteLine("Ende {0}", DateTime.Now);
        }
        const int iterations = 100000;

        private static void PerfAddHostToConfig() {
            var config = new ContentSecurityPolicyConfiguration();
            var uriList = new List<string>();

            for (int i = 0; i < iterations; i++) {
                uriList.Add(string.Format("https://www.example{0}.org/abcd{0}/", i));
            }
            PrepareTest(() => config.ScriptSrc.AddHost("https://www.example.org/abcd/"));

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++) {
                config.ScriptSrc.AddHost(uriList[i]);
            }
            sw.Stop();
            Console.WriteLine("PerfAddHostToConfig");
            PrintTime(sw);
        }

        private static void PerfConfigToHeaderValue() {
            var config = new ContentSecurityPolicyConfiguration();
            config.StyleSrc.AddKeyword(SourceListKeyword.Self);
            config.Sandbox.SetToEmptyValue();
            config.ConnectSrc.AddScheme("https");
            config.PluginTypes.AddMediaType("application/xml");
            config.BaseUri.AddScheme("https");
            config.BaseUri.AddHost("www.example.org");

            PrepareTest(() => config.ToHeaderValue());

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < iterations; i++) {
                config.ToHeaderValue();
                Trace.Write(i);
            }
            sw.Stop();

            Console.WriteLine("PerfConfigToHeaderValue");
            PrintTime(sw);
        }

        private static void PrintTime(Stopwatch sw) {
            Console.WriteLine("Whole Time {0}", sw.Elapsed);
            Console.WriteLine("Average Time {0}", TimeSpan.FromMilliseconds((double) sw.ElapsedMilliseconds/iterations));
            Console.ReadLine();
        }

        private static void PrepareTest(Action action) {
            for (int i = 0; i < 20; i++) {
                action();
            }
        }
    }
}

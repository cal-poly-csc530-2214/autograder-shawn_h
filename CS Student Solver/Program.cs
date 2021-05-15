using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace CS
{
    public delegate List<int> SolverDelegate(List<int> li);
    public delegate bool GraderRuleDelegate(MemberInfo info, out MemberInfo nextInfo, out string err);

    public class Student1
    {
        // Figure 2 - program a
        public List<int> Solve(List<int> poly)
        {
            var deriv = new List<int>();
            var zero = 0;

            if (poly.Count() == 1)
                return deriv;

            for (var e = 0; e < poly.Count(); e++)
            {
                if (poly[e] == 0)
                    zero += 1;
                else
                    deriv.Add(poly[e]*e);
            }

            return deriv;
        }
    }

    public class Student2
    {
        // Figure 2 - program b
        public List<int> Solve(List<int> poly)
        {
            var idx = 1;
            var deriv = new List<int>();
            var plen = poly.Count();

            while (idx <= plen)
            {
                var coeff = poly[0];
                poly.RemoveAt(0);
                deriv.Add(coeff * idx);
                idx = idx + 1;
                if (poly.Count() < 2)
                    return deriv;
            }

            return deriv;
        }
    }

    public class Student3
    {
        // Figure 2 - program c
        public List<int> Solve(List<int> poly)
        {
            var length = poly.Count() - 1;
            var i = length;
            var deriv = poly.GetRange(1, length);

            if (poly.Count() == 1)
            {
                deriv = new List<int>();
                deriv.Add(0);
                return deriv;
            }
            else
            {
                while (i >= 0)
                {
                    var newVal = poly[i] * i;
                    i -= 1;
                    deriv[i] = newVal;
                }
            }

            return deriv;
        }
    }

    public class StudentGrader
    {
        static void Main(string[] args)
        {
            var info = typeof(Student1);
            var validationRules = new Rule(Rule1);
            validationRules.AddSubRule(Rule2)
                .AddSubRule(Rule3)
                .AddSubRule(Rule4)
                .AddSubRule(Rule5);
            MemberInfo outInfo;
            String err;

            validationRules.Process(info, out outInfo, out err);
        }

        public static bool Rule1(MemberInfo info, out MemberInfo nextInfo, out string err)
        {
            nextInfo = info;
            err = "";
            return false;
        }

        public static bool Rule2(MemberInfo info, out MemberInfo nextInfo, out string err)
        {
            nextInfo = info;
            err = "";
            return false;
        }

        public static bool Rule3(MemberInfo info, out MemberInfo nextInfo, out string err)
        {
            nextInfo = info;
            err = "";
            return false;
        }

        public static bool Rule4(MemberInfo info, out MemberInfo nextInfo, out string err)
        {
            nextInfo = info;
            err = "";
            return false;
        }

        public static bool Rule5(MemberInfo info, out MemberInfo nextInfo, out string err)
        {
            nextInfo = info;
            err = "";
            return false;
        }
    }

    public class Rule
    {
        public Rule(GraderRuleDelegate impl)
        {
            subRules = new List<GraderRuleDelegate>();
            subRules.Add(impl);
        }

        public bool Process(MemberInfo info, out MemberInfo nextInfo, out string err)
        {
            nextInfo = info;
            err = "";

            foreach (var r in subRules)
            {
                var ret = ProcessRule(r, info, out nextInfo, out err);

                if (ret)
                {
                    info = nextInfo;
                }
                else
                {
                    Console.WriteLine(err);
                    return false;
                }
            }

            Console.WriteLine("All Tests Pass - 100% !!");
            return true;
        }

        private bool ProcessRule(GraderRuleDelegate ruleImpl, MemberInfo info, out MemberInfo nextInfo, out string err)
        {
            return ruleImpl(info, out nextInfo, out err);
        }

        public Rule AddSubRule(GraderRuleDelegate r)
        {
            subRules.Add(r);
            return this;
        }

        private List<GraderRuleDelegate> subRules;
    }
}

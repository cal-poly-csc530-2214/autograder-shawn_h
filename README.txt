Shawn Harris
Homework 5
Paper : Automated Feedback Generation for Introductory Programming Assignments

For this assigment I worked alone and not in a team.  The paper describes a system for automated 
grading of python programs in a cs program at MIT that goes beyond just checking correct inputs
and outputs. I decided to try and implement my own version in C# using reflection and standard C#.

The full code is shown below.  

Here is how it works: there is a primary class called
StudentGrader, this class contains Main and is the entry
point for the program.

There are a series of sample student programs also in the file 
named Student1, Student2, Student3
for this example these programs are in the same file but it 
would also be possible to open them each as individual files
and then use reflection to process them each.

The program contains a set of rules implemented as delegates (function pointers)
each delegate can examine an input program using Reflection, as such
a rule can test input and output test cases but also delve into the program
and example invidual properties of the program.  I have structured it so 
that the rules are edited in sequence and can effect the input of the next 
rule in the chain if they so choose.

For example a rule might receive the top level program info, if it just 
wants to test input and output cases it can then pass a reference to the
top level program to the next rule.  If the rule was a rule to example
the function signature it could pass a reference to the inner scope
info of the function to the next rule.  If a rule was looking for a 
varable declaration or something specific like a loop, it could pass
a reference to what comes after to the next rule.

Each rule is responsible for filing out an error text message if the 
rule fails to pass to give the student helpful information.  

I did not have time to implement actual rules haha. Reading the paper
and writing this much code took all my time, never the less I think
this program could at least do part of what was described in the paper.

Limitations, reflection can examine types and type info but I'm not sure
it can parse statements line by line, but this would not be hard to 
add to the workflow in my opinion, simply add a parser and an AST
to the workflow then the rules could also reason but the AST or 
sub tree of that AST which could easily contain additional info
like line numbers that would be useful in the student feedback.

-Shawn Harris


======================================================================
======================================================================

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

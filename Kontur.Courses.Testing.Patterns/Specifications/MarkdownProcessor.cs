using System;
using System.Globalization;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Kontur.Courses.Testing.Patterns.Specifications
{
	public class MarkdownProcessor
	{
		public string Render(string input)
		{
            var emReplacer = new Regex(@"([^\w\\]|^)_(.*?[^\\])_(\W|$)");
            var strongReplacer = new Regex(@"([^\w\\]|^)__(.*?[^\\])__(\W|$)");
            input = strongReplacer.Replace(input,
                    match => match.Groups[1].Value +
                            "<strong>" + match.Groups[2].Value + "</strong>" +
                            match.Groups[3].Value);
            input = emReplacer.Replace(input,
                    match => match.Groups[1].Value +
                            "<em>" + match.Groups[2].Value + "</em>" +
                            match.Groups[3].Value);
            input = input.Replace(@"\_", "_");
            return input;
		}
	}

	[TestFixture]
	public class MarkdownProcessor_should
	{
		private readonly MarkdownProcessor md = new MarkdownProcessor();

	    [Test]
	    public void without_underscore()
	    {
	        var str = "without underscore";
            var res = new MarkdownProcessor();

            Assert.AreEqual(str,res.Render(str));
	    }


        [TestCase("_a_", Result = @"<em>a</em>", TestName = "just em")]
        [TestCase("_a", Result = @"_a", TestName = "unpaired underscore")]
        [TestCase("_a_ b", Result = @"<em>a</em> b", TestName = "em and string")]
        [TestCase("a _b_", Result = @"a <em>b</em>", TestName = "string and em")]
        [TestCase("a_a_a", Result = @"a_a_a", TestName = "underscore in string")]
        [TestCase("_a_a_", Result = @"<em>a_a</em>", TestName = "em and underscore in string")]
        [TestCase("_a__", Result = @"<em>a_</em>", TestName = " underscore string and 2 underscores")]

        public string tag_em(string input)
        {
            var res = new MarkdownProcessor();
            return res.Render(input);
        }

        [TestCase("__a__", Result = @"<strong>a</strong>", TestName = "just strong")]
        [TestCase("__a", Result = @"__a", TestName = "unpaired underscores")]
        [TestCase("__a_", Result = @"<em>_a</em>", TestName = "2 underscores string and underscore")]
        [TestCase("__a__ b", Result = @"<strong>a</strong> b", TestName = "strong and string")]
        [TestCase("a __b__", Result = @"a <strong>b</strong>", TestName = "string and strong")]
        [TestCase("a__a__a", Result = @"a__a__a", TestName = "underscore in string")]
        [TestCase("__a__a__", Result = @"<strong>a__a</strong>", TestName = "strong and 2 underscores in string")]
        public string tag_strong(string input)
        {
            var res = new MarkdownProcessor();
            return res.Render(input);
        }

        [TestCase(@"\__a\__", Result = @"_<em>a</em>_", TestName = "tag em with 2 ecrans")]
        [TestCase(@"\__a_", Result = @"_<em>a</em>", TestName = "tag em with 2 ecrans")]
        [TestCase(@"_a _\_b__", Result = @"<em>a __b_</em>", TestName = "underscore and strong")]
        public string ecran(string input)
        {
            var res = new MarkdownProcessor();
            return res.Render(input);
        }
        //TODO see Markdown.txt
		
	}
}

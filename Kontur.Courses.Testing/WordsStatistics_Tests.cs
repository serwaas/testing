using System;
using System.Collections;
using System.Threading;
using Kontur.Courses.Testing.Implementations;
using NUnit.Framework;

namespace Kontur.Courses.Testing
{
    public class WordsStatistics_Tests
    {
        public Func<IWordsStatistics> createStat = () => new WordsStatistics_CorrectImplementation(); // меняется на разные реализации при запуске exe
        public IWordsStatistics stat;

        [SetUp]
        public void SetUp()
        {
            stat = createStat();
        }

        [Test]
        public void no_stats_if_no_words()
        {
            CollectionAssert.IsEmpty(stat.GetStatistics());
        }


        [Test]
        public void word_length_more_than_ten()
        {
            stat.AddWord("aAaAbBasdsddsad");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "aaaabbasds") }, stat.GetStatistics());
        }
        [Test]
        public void test_frequency()
        {
            stat.AddWord("hello");
            stat.AddWord("kontur");
            stat.AddWord("hello");
            stat.AddWord("ololo");
            stat.AddWord("lol");

            stat.AddWord("kontur");
            stat.AddWord("Kontur");
            stat.AddWord("kontur");
            CollectionAssert.AreEqual(new[] { Tuple.Create(4, "kontur"), 
                Tuple.Create(2, "hello"),Tuple.Create(1, "lol"),
                Tuple.Create(1, "ololo") }, stat.GetStatistics());
        }

        [Test]
        public void words_with_len_11()
        {

            stat.AddWord("AaaaaaaaaaB");
            stat.AddWord("aaaaaaaaaaC");

            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "aaaaaaaaaa") }, stat.GetStatistics());
        }

        [Test]
        public void word_with_zero_len()
        {
            stat.AddWord("Hell");

            stat.AddWord("");

            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hell") }, stat.GetStatistics());
        }
        [Test]
        public void add_word_in_other_stat()
        {
            var stat1 = createStat();
            stat1.AddWord("lol");
            stat.AddWord("ololo");



            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "ololo") }, stat.GetStatistics());
        }



        [Test, Timeout(2000)]
        public void count_of_words_equal_20000()
        {
            ArrayList res = new ArrayList();
            for (int i = 0; i < 20000; i++)
            {
                stat.AddWord(i.ToString());

                res.Add(Tuple.Create(1, i.ToString()));
            }
            res.Sort();


            CollectionAssert.AreEqual(res, stat.GetStatistics());
        }
        [Test]
        public void input_null_string()
        {


            stat.AddWord("Hell");

            stat.AddWord(null);

            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hell") }, stat.GetStatistics());
        }

    }
}
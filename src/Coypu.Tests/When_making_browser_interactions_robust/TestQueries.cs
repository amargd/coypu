using System;
using System.Diagnostics;
using Coypu.Queries;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class AlwaysSucceedsPredicateQuery : PredicateQuery
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private readonly bool actualResult;
        private readonly bool expecting;
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _retryInterval;

        public int Tries { get; set; }
        public long LastCall { get; set; }

        public AlwaysSucceedsPredicateQuery(bool actualResult, TimeSpan timeout, TimeSpan retryInterval)
        {
            _timeout = timeout;
            _retryInterval = retryInterval;
            this.actualResult = actualResult;
            stopWatch.Start();
        }

        public override bool Predicate()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            return actualResult;
        }

        public bool ExpectedResult
        {
            get { return expecting; }
        }

        public TimeSpan Timeout
        {
            get { return _timeout; }
        }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }

    public class AlwaysSucceedsQuery<T> : Query<T>
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private readonly T actualResult;
        private readonly T expecting;
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _retryInterval;

        public int Tries { get; set; }
        public long LastCall { get; set; }

        public AlwaysSucceedsQuery(T actualResult, TimeSpan timeout, TimeSpan retryInterval)
        {
            _timeout = timeout;
            _retryInterval = retryInterval;
            this.actualResult = actualResult;
            stopWatch.Start();
        }

        public AlwaysSucceedsQuery(T actualResult, T expecting, TimeSpan timeout, TimeSpan retryInterval)
            : this(actualResult,timeout,retryInterval)
        {
            this.expecting = expecting;
        }

        public T Run()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            return actualResult;
        }

        public T ExpectedResult
        {
            get { return expecting; }
        }

        public TimeSpan Timeout
        {
            get { return _timeout; }
        }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }

    public class ThrowsSecondTimeQuery<T> : Query<T>
    {
        private readonly T result;
        private readonly TimeSpan _retryInterval;
        public TimeSpan Timeout { get; set; }

        public ThrowsSecondTimeQuery(T result, TimeSpan timeout, TimeSpan retryInterval)
        {
            this.result = result;
            _retryInterval = retryInterval;
            Timeout = timeout;
        }

        public T Run()
        {
            Tries++;
            if (Tries == 1)
                throw new TestException("Fails first time");

            return result;
        }

        public T ExpectedResult
        {
            get { return default(T); }
        }

        public int Tries { get; set; }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }

    public class AlwaysThrowsQuery<TResult, TException> : Query<TResult> where TException : Exception
    {
        private readonly TimeSpan _retryInterval;
        private readonly Stopwatch stopWatch = new Stopwatch();

        public AlwaysThrowsQuery(TimeSpan timeout, TimeSpan retryInterval)
        {
            _retryInterval = retryInterval;
            Timeout = timeout;
            stopWatch.Start();
        }

        public TResult Run()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;
            throw (TException)Activator.CreateInstance(typeof(TException), "Test Exception");
        }

        public TResult ExpectedResult
        {
            get { return default(TResult); }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }

        public TimeSpan Timeout { get; set; }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }

    public class AlwaysThrowsPredicateQuery<TException> : PredicateQuery where TException : Exception
    {
        private readonly Stopwatch stopWatch = new Stopwatch();

        public AlwaysThrowsPredicateQuery(TimeSpan timeout, TimeSpan retryInterval) : base(new Options{Timeout = timeout,RetryInterval = retryInterval})
        {
            stopWatch.Start();
        }

        public override bool Predicate()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;
            throw (TException)Activator.CreateInstance(typeof(TException), "Test Exception");
        }

        public bool ExpectedResult
        {
            get { return false; }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }

    }

    public class ThrowsThenSubsequentlySucceedsQuery<T> : Query<T>
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private readonly T actualResult;
        private readonly T expectedResult;
        private readonly int throwsHowManyTimes;
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _retryInterval;

        public ThrowsThenSubsequentlySucceedsQuery(T actualResult, T expectedResult, int throwsHowManyTimes, TimeSpan timeout, TimeSpan retryInterval)
        {
            stopWatch.Start();
            this.actualResult = actualResult;
            this.expectedResult = expectedResult;
            this.throwsHowManyTimes = throwsHowManyTimes;
            _timeout = timeout;
            _retryInterval = retryInterval;
        }

        public T Run()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            if (Tries <= throwsHowManyTimes)
                throw new TestException("Fails first time");

            return actualResult;
        }

        public T ExpectedResult
        {
            get { return expectedResult; }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }


        public TimeSpan Timeout
        {
            get { return _timeout; }
        }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }

    public class ThrowsThenSubsequentlySucceedsPredicateQuery : PredicateQuery
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private readonly bool actualResult;
        private readonly bool expectedResult;
        private readonly int throwsHowManyTimes;
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _retryInterval;

        public ThrowsThenSubsequentlySucceedsPredicateQuery(bool actualResult, bool expectedResult, int throwsHowManyTimes, TimeSpan timeout, TimeSpan retryInterval)
        {
            stopWatch.Start();
            this.actualResult = actualResult;
            this.expectedResult = expectedResult;
            this.throwsHowManyTimes = throwsHowManyTimes;
            _timeout = timeout;
            _retryInterval = retryInterval;
        }

        public override bool Predicate()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            if (Tries <= throwsHowManyTimes)
                throw new TestException("Fails first time");

            return actualResult;
        }

        public bool ExpectedResult
        {
            get { return expectedResult; }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }


        public TimeSpan Timeout
        {
            get { return _timeout; }
        }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }
}
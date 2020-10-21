using System.Collections.Generic;

namespace Ellumination.Collections.Generic
{
    using Xunit;
    using Xunit.Abstractions;
    using Xwellbehaved;

    public class SingletonTests : TestFixtureBase
    {
        // TODO: TBD: would be better to refactor to base class, but for the berth of other classes that would also be impacted...
        /// <summary>
        /// Gets the OutputHelper for private use.
        /// </summary>
        private ITestOutputHelper OutputHelper { get; }

        public SingletonTests(ITestOutputHelper outputHelper)
        {
            this.OutputHelper = outputHelper;
        }

        /// <summary>
        /// Gets or Sets the Singleton for use throughout the tests.
        /// </summary>
        private BidirectionalSingleton<int> Singleton { get; set; }

        [Background]
        public void Background(int value, BidirectionalSingleton<int> singleton)
        {
            $"Initialize a nominal default {nameof(value)}".x(() => value = default);

            $"Initialize a {nameof(singleton)} using extension methods".x(() => singleton = value.ToBidirectionalSingleton());

            $"Hand the {nameof(singleton)} off to the property under test".x(() => this.Singleton = singleton);
        }

        [Scenario]
        public void Can_Evaluate_Get(int delta, ICollection<(int value, int result)> passes)
        {
            bool TryOnEvaluateGet(int value, out int result)
            {
                result = value + delta;

                var success = result > default(int);

                if (success)
                {
                    passes.AssertNotNull().Add((value, result));
                }

                return success;
            }

            $"Initialize a nominal {nameof(delta)}".x(() => delta = 1);

            $"Initialize the {nameof(passes)}".x(() => passes = new List<(int value, int result)>());

            $"Rewire the {nameof(this.Singleton)}.{nameof(this.Singleton.EvaluateGet)} event".x(() => this.Singleton.EvaluateGet += TryOnEvaluateGet);

            // In this scenario we do want to verify that the Get events raise accordingly.
            void TryGetSingletonValue() => this.Singleton.Value.AssertTrue(x => x > default(int));
            //                             ^^^^^^^^^^^^^^^^^^^^

            var baseStep = $"Try to get the {nameof(this.Singleton)}.{nameof(this.Singleton.Value)}";

            baseStep.x(TryGetSingletonValue);

            $"{baseStep} a second time".x(TryGetSingletonValue);

            $"Which should be two {nameof(passes)}".x(() => passes.AssertNotNull().AssertEqual(2, x => x.Count));
        }

        [Scenario]
        public void May_Reject_Evaluate_Get(int delta, ICollection<(int value, int result)> passes)
        {
            bool TryOnEvaluateGet(int value, out int result)
            {
                result = value - delta;
                passes.AssertNotNull().Add((value, result));
                return result > default(int);
            }

            $"Initialize a nominal {nameof(delta)}".x(() => delta = 1);

            $"Initialize the {nameof(passes)}".x(() => passes = new List<(int value, int result)>());

            $"Rewire the {nameof(this.Singleton)}.{nameof(this.Singleton.EvaluateGet)} event".x(() => this.Singleton.EvaluateGet += TryOnEvaluateGet);

            // In this scenario we do want to verify based on the Property clause.
            $"{nameof(this.Singleton)}.{nameof(this.Singleton.Value)} unchanged".x(() => this.Singleton.Value.AssertEqual(default, x => (int)x));
            //                                                                           ^^^^^^^^^^^^^^^^^^^^

            $"Verify the {nameof(passes)}".x(() => passes.AssertNotNull().AssertEqual(1, x => x.Count));

            // Xunit has insufficient knowledge to implicitly convert, so we must help it out a bit.
            $"And {nameof(this.Singleton)} should remain unaffected".x(() => this.Singleton.AssertEqual(default, x => (int)x));
            //                                                                                                        ^^^^^
        }

        [Scenario]
        public void Before_Get(int singletonValue, ICollection<(int old, int current, int value)> passes)
        {
            // We do not care what the old or current values were in this scenario.
            void OnBeforeGet(int old, int current, int value) => passes.AssertNotNull().Add((old, current, value));

            $"Initialize the {nameof(passes)}".x(() => passes = new List<(int old, int current, int value)>());

            $"Rewire the {nameof(this.Singleton)}.{nameof(this.Singleton.BeforeGet)} event".x(() => this.Singleton.BeforeGet += OnBeforeGet);

            // This time we actually do want to route through the Singleton Value property clause.
            void GetCurrentValue() => singletonValue = this.Singleton.Value;
            //                                         ^^^^^^^^^^^^^^^^^^^^

            // Yes, the positioning of steps is intentional in this scenario.
            $"Get the {nameof(singletonValue)}".x(GetCurrentValue);

            $"Get the {nameof(singletonValue)} a second time".x(GetCurrentValue);

            $"Verify the {nameof(passes)}".x(() => passes.AssertNotNull().AssertEqual(2, x => x.Count));
        }

        [Scenario]
        public void Before_Set(int singletonValue, int delta, ICollection<(int old, int current, int value)> passes)
        {
            void OnBeforeSet(int old, int current, int value)
            {
                old.AssertEqual(singletonValue - delta);
                value.AssertEqual(singletonValue);
                passes.AssertNotNull().Add((old, current, value));
            }

            $"Initialize a baseline {nameof(singletonValue)}".x(() => singletonValue = this.Singleton);

            $"Initialize a nominal {nameof(delta)}".x(() => delta = 1);

            $"Initialize the {nameof(passes)}".x(() => passes = new List<(int old, int current, int value)>());

            $"Rewire the {nameof(this.Singleton)}.{nameof(this.Singleton.BeforeSet)} event".x(() => this.Singleton.BeforeSet += OnBeforeSet);

            void SetValue() => this.Singleton.Value = singletonValue += delta;

            $"Set the {nameof(this.Singleton)}.{nameof(this.Singleton.Value)}".x(SetValue);

            // Aby time we are verifying the Singleton, we should leave the property clauses alone.
            void VerifyCurrentValue() => this.Singleton.AssertEqual(singletonValue, x => (int)x);
            //                                                                           ^^^^^

            $"Verify the {nameof(singletonValue)} value".x(VerifyCurrentValue);

            $"Set the {nameof(this.Singleton)}.{nameof(this.Singleton.Value)} a second time".x(SetValue);

            $"Verify the {nameof(singletonValue)} value a second time".x(VerifyCurrentValue);

            $"And verify two passes".x(() => passes.AssertNotNull().AssertEqual(2, x => x.Count));
        }

        [Scenario]
        public void Can_Evaluate_Set(int delta, ICollection<(int old, int value)> passes)
        {
            // Accepts only positive values.
            bool TryOnEvaluateSet(int old, int value)
            {
                passes.AssertNotNull().Add((old, value));
                return value > default(int);
            }

            $"Initialize a nominal {nameof(delta)}".x(() => delta = 1);

            $"Initialize the {nameof(passes)}".x(() => passes = new List<(int old, int value)>());

            $"Rewire the {nameof(this.Singleton)}.{nameof(this.Singleton.EvaluateSet)} event".x(() => this.Singleton.EvaluateSet += TryOnEvaluateSet);

            // This time we can implicitly convert the Singleton, as well as potentially effecting the property Set clause...
            $"Set the {nameof(this.Singleton)}.{nameof(this.Singleton.Value)}".x(() => this.Singleton.Value = this.Singleton + delta);
            //                                                                         ^^^^^^^^^^^^^^^^^^^^   ^^^^^^^^^^^^^^

            $"Verify the {nameof(passes)}".x(() => passes.AssertNotNull().AssertEqual(1, x => x.Count));

            // Xunit has insufficient knowledge to implicitly convert, so we must help it out a bit.
            $"And {nameof(this.Singleton)}.{nameof(this.Singleton.Value)} is correct".x(() => this.Singleton.AssertEqual(default(int) + delta, x => (int)x));
            //                                                                                                                                      ^^^^^
        }

        [Scenario]
        public void May_Reject_Evaluate_Set(int delta, ICollection<(int old, int value)> passes)
        {
            // Accepts only allows zero or positive values, therefore rejects negative values.
            bool TryOnEvaluateSet(int old, int value)
            {
                passes.AssertNotNull().Add((old, value));
                return value >= default(int);
            }

            $"Initialize a nominal {nameof(delta)}".x(() => delta = -1);

            $"Initialize the {nameof(passes)}".x(() => passes = new List<(int old, int value)>());

            $"Rewire the {nameof(this.Singleton)}.{nameof(this.Singleton.EvaluateSet)} event".x(() => this.Singleton.EvaluateSet += TryOnEvaluateSet);

            // This time we can implicitly convert the Singleton...
            $"Try to set the {nameof(this.Singleton)}.{nameof(this.Singleton.Value)}".x(() => this.Singleton.Value = this.Singleton + delta);
            //                                                                                                       ^^^^^^^^^^^^^^

            $"Verify the {nameof(passes)}".x(() => passes.AssertNotNull().AssertEqual(1, x => x.Count));

            // Xunit has insufficient knowledge to implicitly convert, so we must help it out a bit.
            $"And {nameof(this.Singleton)} should remain unaffected".x(() => this.Singleton.AssertEqual(default, x => (int)x));
            //                                                                                                        ^^^^^
        }
    }
}

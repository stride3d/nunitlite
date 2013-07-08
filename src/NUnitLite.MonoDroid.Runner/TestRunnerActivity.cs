using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;
using NUnitLite.Runner;

namespace NUnitLite.MonoDroid
{
    /// <summary>
    /// Derive from this activity to create a standard test runner activity in your app.
    /// </summary>
    public abstract class TestRunnerActivity : ListActivity
    {
        private TestResultsListAdapter _testResultsAdapter;

        /// <summary>
        /// Handles the creation of the activity
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _testResultsAdapter = new TestResultsListAdapter(this);
            ListAdapter = _testResultsAdapter;
        }

        protected override void OnResume()
        {
            base.OnResume();

            var testAssemblies = GetAssembliesForTest();
            var testAssemblyEnumerator = testAssemblies.GetEnumerator();
            var testRunner = new NUnit.Framework.Internal.NUnitLiteTestAssemblyRunner(new NUnitLiteTestAssemblyBuilder());

            // Clear the test result list
            TestRunContext.Current.TestResults.Clear();

            _testResultsAdapter.NotifyDataSetInvalidated();
            _testResultsAdapter.NotifyDataSetChanged();

            // Add a test listener for the test runner
            var listener = new UITestListener((TestResultsListAdapter)ListAdapter);

            // Start the test process in a background task
            Task.Factory.StartNew(() =>
            {
                var emptyFilter = new EmptyFilter();
                while (testAssemblyEnumerator.MoveNext())
                {
                    try
                    {
                        var assembly = testAssemblyEnumerator.Current;
                        if (!testRunner.Load(assembly, new Hashtable()))
                        {
                            AssemblyName assemblyName = AssemblyHelper.GetAssemblyName(assembly);
                            Console.WriteLine("No tests found in assembly {0}", assemblyName.Name);
                            return;
                        }

                        testRunner.Run(
                            listener,
                            emptyFilter);
                    }
                    catch (Exception ex)
                    {
                        ShowErrorDialog(ex);
                    }
                }
            });
        }

        /// <summary>
        /// Handles list item click
        /// </summary>
        /// <param name="l"></param>
        /// <param name="v"></param>
        /// <param name="position"></param>
        /// <param name="id"></param>
        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var testRunItem = TestRunContext.Current.TestResults[position];

            if (testRunItem.Running)
            {
                Toast.MakeText(this, "This test is still running.", ToastLength.Short).Show();
                return;
            }

            var intent = new Intent(this, GetDetailsActivityType);
            intent.PutExtra("TestCaseName", testRunItem.TestCaseName);

            StartActivity(intent);
        }

        /// <summary>
        /// Retrieves a list of assemblies that contain test cases to execute using the test runner activity.
        /// </summary>
        /// <returns>Returns the list of assemblies to test</returns>
        protected abstract IEnumerable<Assembly> GetAssembliesForTest();

        /// <summary>
        /// Gets the type of activity to use for displaying test details
        /// </summary>
        protected abstract Type GetDetailsActivityType { get; }

        /// <summary>
        /// Displays an error dialog in case a test run fails
        /// </summary>
        /// <param name="exception"></param>
        private void ShowErrorDialog(Exception exception)
        {
            RunOnUiThread(() =>
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetTitle("Failed to execute unit-test suite");
                builder.SetMessage(exception.ToString());

                var dialog = builder.Create();

                dialog.Show();
            });
        }

        /// <summary>
        /// Nested class provides an empty filter - one that always
        /// returns true when called, unless the test is marked explicit.
        /// </summary>
        [Serializable]
        private class EmptyFilter : TestFilter
        {
            public override bool Match(ITest test)
            {
                return test.RunState != RunState.Explicit;
            }

            public override bool Pass(ITest test)
            {
                return test.RunState != RunState.Explicit;
            }
        }
    }
}
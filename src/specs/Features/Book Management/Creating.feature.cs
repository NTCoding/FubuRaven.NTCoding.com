// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.5.0.0
//      Runtime Version:4.0.30319.237
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
namespace Specs.Features.BookManagement
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.5.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Creating new books")]
    public partial class CreatingNewBooksFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Creating.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Creating new books", "As the site owner\r\nI need to be able to create new books\r\nSo that I can show my o" +
                    "pinion of them", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Valid Creation")]
        [NUnit.Framework.TestCaseAttribute("Mega Book", "Swimming", "A bit rubbish", "Reviewed", "Your Mom, The Milkman")]
        public virtual void ValidCreation(string title, string genre, string description, string status, string authors)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Valid Creation", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
 testRunner.Given("I have navigated to the \"Create Book\" page");
#line 8
  testRunner.And(string.Format("I have specified a {0}", title));
#line 9
  testRunner.And(string.Format("I have specified a {0}", genre));
#line 10
  testRunner.And(string.Format("I have specified a {0}", description));
#line 11
  testRunner.And(string.Format("I have specified a {0}", status));
#line 12
  testRunner.And(string.Format("I have specified the {0}", authors));
#line 13
  testRunner.And("I have selected a cover image");
#line 14
 testRunner.When("I confirm creation");
#line 15
 testRunner.Then("I should be on the \"View Book\" page for the new book");
#line 16
  testRunner.And(string.Format("I should see the {0} I specified", title));
#line 17
  testRunner.And(string.Format("I should see the {0} I specified", genre));
#line 18
  testRunner.And(string.Format("I should see the {0} I specified", description));
#line 19
  testRunner.And(string.Format("I should the {0} I specified", status));
#line 20
  testRunner.And(string.Format("I should see the {0} I specified", authors));
#line 21
  testRunner.And("I should see the cover image I specified");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Invalid Input")]
        public virtual void InvalidInput()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Invalid Input", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 28
 testRunner.Given("I have navigated to the \"Create Book\" page");
#line 29
  testRunner.And("I have not entered any information");
#line 30
 testRunner.When("I confirm creation");
#line 31
 testRunner.Then("I should be on the \"Create Book\" page");
#line 32
  testRunner.And("I should see a failed validation message for each empty field");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Duplicate Book")]
        public virtual void DuplicateBook()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Duplicate Book", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line 35
 testRunner.Given("I have navigate to the \"Create Book\" page");
#line 36
  testRunner.And("I specify the title for the name of a book that already exists");
#line 37
 testRunner.When("I confirm creation");
#line 38
 testRunner.Then("I should be on the \"Create Book\"");
#line 39
  testRunner.And("I should see the message \"Book with this title already exists\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
    }
}
#endregion
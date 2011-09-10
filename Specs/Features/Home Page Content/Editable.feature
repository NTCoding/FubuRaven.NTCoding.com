Feature: Editable home page content	
	In order to be able to change my greeting to visitors of the site
	As the site owner
	I need to be able to edit the content of the home page online

Scenario: Happy Path
	Given I have navigated to the "Edit Home Page" page
		And I have specified the new content as "Welcome to NTCoding - now getting jiggy with fubu and Raven"
	When I confirm my new content
	Then I should be viewing the "Home Page"
		And I should see the following welcome content "Welcome to NTCoding - now getting jiggy with fubu and Raven"

Scenario: Html Content
	Given I have navigated to the "Edit Home Page" page
		And I have specified the new content as "<h1>Naughty</h1>"
	When I confirm my changes
	Then I should be viewing the "Edit Home Page" page
		And I should see the following validation message "Update failed - Html not allowed"

Scenario: Empty Content
	Given I have navigated to the "Edit Home Page" page
		And I have specified the new content as ""
	When I confirm my change
	Then I should be viewing the "Edit Home Page" page
		And I should see be prompted to confirm I want the content left empty


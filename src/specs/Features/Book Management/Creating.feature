Feature: Creating new books
	As the site owner
	I need to be able to create new books
	So that I can show my opinion of them

Scenario: Valid Creation
	Given I have navigated to the "Create Book" page
		And I have specified a title
		And I have specified a genre
		And I have specified a description
		And I have specified a status
		And I have specified at least one author
		And I have selected a cover image
	When I confirm creation
	Then I should be on the "View Book" page for the new book
		And I should see the title I entered
		And I should see the genre I entered
		And I should see the description I entered
		And I should the status I entered
		And I should see the author(s) I entered
		And I should see the cover image I selected

Scenario: Invalid Input
	Given I have navigated to the "Create Book" page
		And I have not entered any information
	When I confirm creation
	Then I should be on the "Create Book" page
		And I should see a failed validation message for each empty field
		
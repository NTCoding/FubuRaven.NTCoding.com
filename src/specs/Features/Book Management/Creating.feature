Feature: Creating new books
	As the site owner
	I need to be able to create new books
	So that I can show my opinion of them

Scenario Outline: Valid Creation
	Given I have navigated to the "Create Book" page
		And I have specified a <title>
		And I have specified a <genre>
		And I have specified a <description>
		And I have specified a <status>
		And I have specified the <authors>
		And I have selected a cover image
	When I confirm creation
	Then I should be on the "View Book" page for the new book
		And I should see the <title> I specified
		And I should see the <genre> I specified
		And I should see the <description> I specified
		And I should the <status> I specified
		And I should see the <authors> I specified
		And I should see the cover image I specified

		Examples:
		| title     | genre    | description   | status   | authors               |
		| Mega Book | Swimming | A bit rubbish | Reviewed | Your Mom, The Milkman |

Scenario: Invalid Input
	Given I have navigated to the "Create Book" page
		And I have not entered any information
	When I confirm creation
	Then I should be on the "Create Book" page
		And I should see a failed validation message for each empty field
		
Scenario: Duplicate Book
	Given I have navigate to the "Create Book" page
		And I specify the title for the name of a book that already exists
	When I confirm creation
	Then I should be on the "Create Book"
		And I should see the message "Book with this title already exists"
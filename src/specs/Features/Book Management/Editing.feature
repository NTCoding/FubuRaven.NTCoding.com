Feature: Editing Books
	As the site owner
	I need to be able to books
	So I change update their status and correct any errors

Scenario: Valid Edit
	Given I have navigate to the "Edit Book" page for a book
		And I update the details of the book as follows
	When I confirm my update
	Then I should be viewing the "View Book" page for the book
		And I should the updated details

Scenario: Invalid Edit
	Given I have navigated to the "Edit Book" page for a book
		And I set some fields to empty
	When I confirm my update
	Then I should be viewing the "Edit Book" page for the book
		And I should see validation failed messages for the empty fields


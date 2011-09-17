Feature: Deleting Books
	As the site owner
	I need to be able to delete books
	So that I can remove duplicates or errors

Scenario: Deleting a book
	Given I have navigated to the "Book Management List" page
		And I have clicked the delete link for a book
	When I have confir the deletion of the book
	Then I should be viewing the "Book Management List" page
		And I should not see the book I have deleted
		And I should see the message "Book deleted"
	

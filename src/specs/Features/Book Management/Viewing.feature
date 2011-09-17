Feature: Management View of Book
	As the site owner
	I need to be able view all the details of a book
	So that I can update it accordingly

Scenario: Example book
	Given the system contains a book with the following details
	When I navigate to the view page for this book
	Then I should see all the details for this book
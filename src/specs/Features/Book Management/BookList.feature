Feature: Book Management List
	As the site owner
	I need a list of all the books in the system
	So I can update their details

Scenario: Website has books
	Given the website has some books
	When I navigate to the "Book Management List" page
	Then I should all of the books in the system

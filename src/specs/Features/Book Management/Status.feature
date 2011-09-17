Feature: Book Status
	As the site owner
	I need to restrict the statuses books can be set to
	So that they can be shown on the relevant pages

Scenario: Good Statuses
	Given I have navigated to the "Create Book" page
		And I have filled in all the fields
		And I have set the <status>
	When I confirm creation
	Then the book should be created

	Examples:
	|      status       |
	| Wishlist          |
	| Currently Reading |
	| Reviewed          |


Scenario: Invalid Statuses
	Given I have navigated to the "Create Book" page
		And I have set the <status>
	When I confirm creation
	Then the book should not be created

	Examples:
	| status    |
	| Random    |
	| Who cares |

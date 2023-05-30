Feature: Add and remove classrooms

Background:
	Given I created a school

Scenario: Add classes to school
	When I Add classes to school
	And I Get school using id
	Then I should get school containing the class I added

Scenario: Remove class from school
	When I Add classes to school
	And I Get school using id
	And I remove the class from school
	And I Get school using id
	Then I should get school with the class removed
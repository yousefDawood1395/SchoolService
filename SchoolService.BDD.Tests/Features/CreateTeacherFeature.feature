Feature: Create a new teacher
cxlkcg clkxhhvkjxhbvxv

Scenario: Create a  new teacher success
	When I Create a new teacher with valid data
	And I get the teacher sing Id
	Then I should get the teacher

Scenario: Create a  new teacher not valid
	When I Create a new teacher using invalid data
	Then I should get data not valid response
# LinkedinFetcher

## Objective
Build an application that parses LinkedIn public profiles, stores results in structured manner in persistent layer and allows to perform search on stored results.
## Technical Details
The interaction with application performed via RESTful API. The API must contain at least 3 endpoints:
1.	Adding a public LinkedIn profile page 
2.	Searching for people that were previously added
3.	Searching for skills and viewing associated people

Once the page is parsed it must extract at minimum the following fields:
1.	Name of the person
2.	Current title 
3.	Current position
4.	Summary
5.	List of skills
6.	Experience
7.	Education

5 first fields are searchable through the API. Based on 3 last fields you will need to design and implement a scoring mechanism for the person’s profile. The definition of scoring mechanism is totally in your hand – it can be trivial (based on years of experience, amount of skills, etc.) or complex (NLP which analyzes the quality of English in all fields, educational institutions or employees ranking, etc.).

The application needs to be built with high-volume in mind:
1.	Assume that a lot of users will be adding profiles for parsing concurrently
2.	The persistent layer may eventually contain millions of results, searching through those results still needs to be effective

## Implementation requirements
1.	You can choose to implement on one of the following languages: Python, C#, NodeJS, Java or Ruby
2.	SOLID principles must be applied to delivered code
3.	There must be a bare minimum of unit tests 
4.	You are free to use any 3rd party libraries and services
5.	Proper exception handling, comments and documentation are bonus points
6.	The final result needs to be submitted to GIT repository (GitHub or similar)
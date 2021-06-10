Feature: Start a new project

@manager
Scenario: Create a new project from scratch
  Given there are no existing projects in the system
    And a user named Dave has logged into the system
   When the user chooses the "Create Project" option
    And supplies a project name of "Unit Test" 
    And supplies a project description of "Unit Test"
   Then a new project will be created
    And it will have the name "Unit Test"
    And it will have the description "Unit Test"
    And Dave will be registered as a team member with the "Manager" role on the project

# Manager

In our kanban system, the manager is responsible for creating the structure of the overall kanban board and setting up the workflow used to resolve the cards on the board. This means the manager will create the initial project and define the project boards that will be part of the project. Then, the manager will specify all the workflow rules needed to move the work cards across the board(s) to completion.

## Use Cases

### Setup and Maintain A Project Board

#### Start a new project

```Gherkin
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
    And Dave is the only team member in the new project

Scenario: Create a new project using an existing project as a template
  Given there is an existing project in the system called "Template"
    And a user named Dave has logged into the system
   When the user chooses the "Create Project" option
    And supplies the name "Unit Test"    
    And chooses the "use existing project as template" option
    And selects the project named "Template"
   Then a new project will be created
    And the boards defined in the new project will match the ones in the "Template" project
    And the columns defined in the new project will match the ones in the "Template" project
    And the workflows defined in the new project will match the ones in the "Template" project
    And the roles defined in the new project will match the ones in the "Template" project
    And Dave will be registered as a team member with the "Manager" role on the project
    And Dave is the only team member in the new project    
```

#### Invite a person to join your project

```Gherkin
Scenario: Invite a user to join by email address
  Given a project called "Unit Test" exists
    And a user named Dave is the project manager
   When Dave selects the "Invite Team Member" option
    And Dave selects the option to "Invite by Email"
    And Dave enters the email address "NewMember@FakeEmail.net"
   Then the system will create a new team member account for "NewMember"
    And the system will send an invitation email to "NewMember@FakeEmail.net"
    And the email will contain a single-use URL to activate the new account

Scenario: Invite a user of a different project to join
  Given a project called "Unit Test" exists
    And a project called "Other Team" exists
    And a user named Dave is a manager in the "Unit Test" project
    And a user named Bill is a worker in the "Other Team" project
   When Dave selects the "Invite Team Member" option
    And Dave selects the option to "Existing User" option
    And Dave selects "Bill" from the list of known users
    And Dave selects the "Worker" role for the new team member
   Then the system adds "Bill" to the "Unit Test" project
    And Bill is given the "Worker" role in the "Unit Test" project
```

#### Define roles that the members of your project team will perform

```Gherkin
Scenario: Define a role
  Given
    And
   When
    And
   Then
    And
    
Scenario: View the permissions of an existing role
  Given
    And
   When
    And
   Then
    And
```

#### Specify the role a project team member will perform on the project

```Gherkin
Scenario: Link a project team member to a role
  Given
    And
   When
    And
   Then
    And

Scenario: Link a project team member to a built-in role
  Given
    And
   When
    And
   Then
    And
```

#### Associate a worker role to a column on the project board

```Gherkin
Scenario: Specify a role as the worker for a column in the project
  Given
    And
   When
    And
   Then
    And
```

#### Associate a reviewer role to a column on the project board

```Gherkin
Scenario: Specify a role as the reviewer for a column in the project
  Given
    And
   When
    And
   Then
    And
```

#### Add a new project board to the project

```Gherkin
Scenario: Split a board into two boards
  Given
    And
   When
    And
   Then
    And

Scenario: Combine two boards into one board
  Given
    And
   When
    And
   Then
    And
```

#### Add column to a project board

```Gherkin
Scenario: Add a new column to a board
  Given
    And
   When
    And
   Then
    And

Scenario: Remove an existing column from a board
  Given
    And
   When
    And
   Then
    And
```

#### Define workflow rules that say which columns a card can move to based on which column it is in right now

```Gherkin
Scenario: Define a set of columns that a card can move to when it leaves a column
  Given
    And
   When
    And
   Then
    And

Scenario: Define a parallel process by putting one column under another
  Given
    And
   When
    And
   Then
    And

Scenario: Define an auto-move rule when a column has no reviewers
  Given
    And
   When
    And
   Then
    And
```

#### Define a workflow rule that says a card should move to a different board when it is approved in a particular column

```Gherkin
Scenario: Define a rule that moves a card to a column on a different board in the project
  Given
    And
   When
    And
   Then
    And

Scenario: Define a rule that moves a card to a column on in a different project
  Given
    And
   When
    And
   Then
    And
```

### Gather and Analyze Project Performance Metrics

#### Generate a report showing the project velocity for a given time period

```Gherkin
Scenario: View Velocity Report
  Given
    And
   When
    And
   Then
    And
```

#### Generate a report showing the task cycle time for a given time period

```Gherkin
Scenario: View Cycle Time Report
  Given
    And
   When
    And
   Then
    And
```

#### Generate a Work Breakdown Structure from the tasks in the system

```Gherkin
Scenario: View Work Breakdown Structure
  Given
    And
   When
    And
   Then
    And
```

#### Generate a report showing all tasks in a specific state

```Gherkin
Scenario: View Task List
  Given
    And
   When
    And
   Then
    And
```

#### Generate a report showing all tasks assigned to a particular worker

```Gherkin
Scenario: View Worker Activity Report
  Given
    And
   When
    And
   Then
    And
```

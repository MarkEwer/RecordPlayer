using System;

namespace ME.Kanban.Tests.Manager
{
    using System.Threading.Tasks;
    using Xunit;

    public class Setup_And_Maintain_A_Project_Board
    {
        [Fact]
        public void Create_A_New_Project_From_Scratch()
        {
            /*
             * Scenario: Create a new project from scratch
             *   Given there are no existing projects in the system
             *     And a user named Dave has logged into the system
             *    When the user chooses the "Create Project" option
             *     And supplies a project name of "Unit Test"
             *     And supplies a project description of "Unit Test"
             *    Then a new project will be created
             *     And it will have the name "Unit Test"
             *     And it will have the description "Unit Test"
             *     And Dave will be registered as a team member with the "Manager" role on the project
             *     And Dave is the only team member in the new project
             */

            Given_there_are_no_existing_projects_in_the_system();
            And_a_user_named_Dave_has_logged_into_the_system();
            When_the_user_chooses_the_Create_Project_option();
            And_supplies_a_project_name_of_Unit_Test();
            And_supplies_a_project_description_of_Unit_Test();
            Then_a_new_project_will_be_created();
            And_it_will_have_the_name_Unit_Test();
            And_it_will_have_the_description_Unit_Test();
            And_Dave_will_be_registered_as_a_team_member_with_the_Manager_role_on_the_project();
            And_Dave_is_the_only_team_member_in_the_new_project();

        }

        private async Task And_a_user_named_Dave_has_logged_into_the_system()
        {
        }

        private async Task And_Dave_is_the_only_team_member_in_the_new_project()
        {
        }

        private async Task And_Dave_will_be_registered_as_a_team_member_with_the_Manager_role_on_the_project()
        {
        }

        private async Task And_it_will_have_the_description_Unit_Test()
        {
        }

        private async Task And_it_will_have_the_name_Unit_Test()
        {
        }

        private async Task And_supplies_a_project_description_of_Unit_Test()
        {
        }

        private async Task And_supplies_a_project_name_of_Unit_Test()
        {
        }

        private async Task Given_there_are_no_existing_projects_in_the_system()
        {
        }

        private async Task Then_a_new_project_will_be_created()
        {
        }

        private async Task When_the_user_chooses_the_Create_Project_option()
        {
        }
    }
}
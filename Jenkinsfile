pipeline{
    agent any
    stages{
        stage("Restore"){
            when{
                anyOf{
                    branch "main"
                }
            }
            steps{
                bat "dotnet restore"
            }
        }
        stage("Build"){
            steps{
                bat "dotnet build --no-restore"
            }
        }
        stage("Test"){
            steps{
                bat "dotnet test --no-build --verbosity normal"
            }
        }
    }
}
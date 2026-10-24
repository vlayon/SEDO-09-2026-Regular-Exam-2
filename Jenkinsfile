
pipeline {
    agent any

    stages {
        stage("Restore") {
            when {
                branch "main"
            }
            steps {
                bat "dotnet restore"
            }
        }

        stage("Build") {
            when {
                branch "main"
            }
            steps {
                bat "dotnet build --no-restore"
            }
        }

        stage("Test") {
            when {
                branch "main"
            }
            steps {
                bat "dotnet test --no-build --no-restore --verbosity normal"
            }
        }
    }
}

// Jenkins does not see my .net6 installation
pipeline {
    agent any

    environment {
        DOTNET_ROOT = 'C:\\Program Files\\dotnet6'
        PATH = "C:\\Program Files\\dotnet6;${env.PATH}"
    }

    stages {
        stage("Restore") {
            when {
                branch "main"
            }
            steps {
                bat '"C:\\Program Files\\dotnet6\\dotnet.exe" restore Homies.sln'
            }
        }

        stage("Build") {
            when {
                branch "main"
            }
            steps {
                bat '"C:\\Program Files\\dotnet6\\dotnet.exe" build Homies.sln --no-restore'
            }
        }

        stage("Test") {
            when {
                branch "main"
            }
            steps {
                bat '"C:\\Program Files\\dotnet6\\dotnet.exe" test Homies.sln --no-build --no-restore --verbosity normal'
            }
        }
    }
}


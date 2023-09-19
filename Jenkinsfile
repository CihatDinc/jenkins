pipeline {
    agent any

    stages {
        stage('Docker Build') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'CodeBuild/github/token', accessKeyVariable: 'AWS_ACCESS_KEY_ID', secretKeyVariable: 'AWS_SECRET_ACCESS_KEY']]) {
                    script {
                        def secrets = sh(script: """#!/bin/bash
                        aws secretsmanager get-secret-value --secret-id CodeBuild/github/token | jq -r '.SecretString'
                        """, returnStdout: true).trim()
                        
                        def creds = readJSON text: secrets
                        
                        env.GITHUB_USERNAME = creds.GITHUB_USERNAME
                        env.GITHUB_PACKAGE_URL = creds.GITHUB_PACKAGE_URL
                        env.GITHUB_ACCESS_TOKEN = creds.GITHUB_ACCESS_TOKEN
                    }

                    sh """
                        docker build . \
                        --build-arg GITHUB_USERNAME=${GITHUB_USERNAME} \
                        --build-arg GITHUB_PACKAGE_URL=${GITHUB_PACKAGE_URL} \
                        --build-arg GITHUB_ACCESS_TOKEN=${GITHUB_ACCESS_TOKEN} \
                        -t cihatdinc/era:latest
                    """
                }
            }
        }
    }
}

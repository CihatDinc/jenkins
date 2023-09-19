pipeline {
 agent any
        environment {
            AWS_ACCOUNT_ID="212845026981"
            AWS_DEFAULT_REGION="eu-central-1" 
            IMAGE_REPO_NAME="jenkins-test-customer"
            IMAGE_TAG="latest"
            REPOSITORY_URI = "${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com/${IMAGE_REPO_NAME}"
            GITHUB_USERNAME="nebim-github-user"
            GITHUB_ACCESS_TOKEN="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"
            GITHUB_PACKAGE_URL="https://nuget.pkg.github.com/nebim-era/index.json"
        }

        stages {
       
            stage('Logging into AWS ECR') {
                steps {
                    script {
                        sh "aws ecr get-login-password --region ${AWS_DEFAULT_REGION} | docker login --username AWS --password-stdin ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com"
                    }       
                }
            }
       
            stage('Cloning Git') {
                steps {
                    checkout([$class: 'GitSCM', branches: [[name: '*/main']], doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], userRemoteConfigs: [[credentialsId: '', url: 'https://github.com/CihatDinc/jenkins.git']]]) 
                }
            }
       
        // Building Docker images
            stage('Building image') {
                steps{
                    script {
                        sh "docker build --build-arg GITHUB_USERNAME=${GITHUB_USERNAME} --build-arg GITHUB_ACCESS_TOKEN=${GITHUB_ACCESS_TOKEN} --build-arg GITHUB_PACKAGE_URL=${GITHUB_PACKAGE_URL} -t ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com/${IMAGE_REPO_NAME}:${IMAGE_TAG} ."
                    }
                }
            }
       
        // Uploading Docker images into AWS ECR
            stage('Pushing to ECR') {
                steps{ 
                    script {
                        sh "docker push ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com/${IMAGE_REPO_NAME}:${IMAGE_TAG}"
                    }
                }
            }
        }
}

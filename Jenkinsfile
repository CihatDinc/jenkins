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
            S3_BUCKET="nebim-era-plt-deployment-yamls/nebim-era-plt-comm-customer-deployment-yaml/nebim-era-plt-comm-customer-deployment.yaml"
            SERVICE_ACCOUNT_NAME="era-plt-service-account"
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
                    checkout scmGit(branches: [[name: '*/main']], extensions: [[$class: 'WipeWorkspace']], userRemoteConfigs: [[credentialsId: 'GithubConnection', url: 'https://github.com/CihatDinc/jenkins.git']])
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

         // Updating deployment.yaml           
            stage('Fetch and Modify deployment.yaml') {
                steps {
                    sh "aws s3 cp s3://${S3_BUCKET} ."
                    sh "cat nebim-era-plt-comm-customer-deployment.yaml"
                    sh "sed -i 's|CONTAINER_IMAGE|${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com/${IMAGE_REPO_NAME}:${IMAGE_TAG}|g' nebim-era-plt-comm-customer-deployment.yaml"
                    sh "sed -i 's|SERVICE_ACCOUNT_NAME|${SERVICE_ACCOUNT_NAME}|g' nebim-era-plt-comm-customer-deployment.yaml"
                    sh "cat nebim-era-plt-comm-customer-deployment.yaml"
                }
            }

            stage('K8S Deploy') {
                steps {
                    sh "kubectl apply -f nebim-era-plt-comm-customer-deployment.yaml"
                }
            }            
        }

}
